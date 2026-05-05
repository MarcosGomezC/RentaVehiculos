using Microsoft.Extensions.DependencyInjection;
using RentaVehiculo;
using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Facturas;

public partial class FacturaList : Form
{
    private readonly FacturaService _service;

    public FacturaList(FacturaService service)
    {
        InitializeComponent();
        _service = service;
    }

    private void FacturaList_Load(object sender, EventArgs e)
    {
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.Columns.Clear();
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id", DataPropertyName = "Id", MinimumWidth = 52, FillWeight = 35 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id renta", DataPropertyName = "IdRenta", MinimumWidth = 80, FillWeight = 55 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Número", DataPropertyName = "NumeroFactura", MinimumWidth = 120, FillWeight = 95 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fecha", DataPropertyName = "FechaEmision", MinimumWidth = 130, FillWeight = 90 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Subtotal", DataPropertyName = "Subtotal", MinimumWidth = 88, FillWeight = 65 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Impuestos", DataPropertyName = "Impuestos", MinimumWidth = 88, FillWeight = 65 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total", DataPropertyName = "Total", MinimumWidth = 88, FillWeight = 65 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Estado", DataPropertyName = "Estado", MinimumWidth = 72, FillWeight = 50 });
        ListFormLayout.ConfigureDataGrid(dataGridView1);
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            dataGridView1.DataSource = await _service.GetList(f => true);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (Program.ServiceProvider.GetRequiredService<FacturaForm>().ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private void btnModificar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not Factura entidad)
        {
            MessageBox.Show("Seleccione una factura.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (ActivatorUtilities.CreateInstance<FacturaForm>(Program.ServiceProvider, entidad).ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not Factura entidad)
            return;
        if (MessageBox.Show("¿Eliminar esta factura?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        _ = EliminarAsync(entidad.Id);
    }

    private async Task EliminarAsync(int id)
    {
        try
        {
            if (await _service.Eliminar(id))
            {
                MessageBox.Show("Eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
