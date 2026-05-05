using Microsoft.Extensions.DependencyInjection;
using RentaVehiculo;
using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Mantenimientos;

public partial class MantenimientoList : Form
{
    private readonly MantenimientoService _service;

    public MantenimientoList(MantenimientoService service)
    {
        InitializeComponent();
        _service = service;
    }

    private void MantenimientoList_Load(object sender, EventArgs e)
    {
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.Columns.Clear();
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id", DataPropertyName = "Id", MinimumWidth = 52, FillWeight = 35 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id vehículo", DataPropertyName = "IdVehiculo", MinimumWidth = 96, FillWeight = 70 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tipo", DataPropertyName = "TipoMantenimiento", MinimumWidth = 72, FillWeight = 55 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Costo", DataPropertyName = "Costo", MinimumWidth = 88, FillWeight = 65 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Inicio", DataPropertyName = "FechaInicio", MinimumWidth = 140, FillWeight = 100 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Estado", DataPropertyName = "Estado", MinimumWidth = 72, FillWeight = 55 });
        ListFormLayout.ConfigureDataGrid(dataGridView1);
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            dataGridView1.DataSource = await _service.GetList(m => true);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (Program.ServiceProvider.GetRequiredService<MantenimientoForm>().ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private void btnModificar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not Mantenimiento entidad)
        {
            MessageBox.Show("Seleccione un registro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (ActivatorUtilities.CreateInstance<MantenimientoForm>(Program.ServiceProvider, entidad).ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not Mantenimiento entidad)
            return;
        if (MessageBox.Show("¿Eliminar este mantenimiento?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
