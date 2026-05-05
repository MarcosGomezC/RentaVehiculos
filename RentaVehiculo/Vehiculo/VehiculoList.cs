using Microsoft.Extensions.DependencyInjection;
using RentaVehiculo;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;
using RentaVehiculo.Data.Models;

namespace RentaVehiculo.UI.Vehiculos;

public partial class VehiculoList : Form
{
    private readonly VehiculoService _service;

    public VehiculoList(VehiculoService service)
    {
        InitializeComponent();
        _service = service;
    }

    private void VehiculoList_Load(object sender, EventArgs e)
    {
        ConfigureColumns();
        _ = LoadDataAsync();
    }

    private void ConfigureColumns()
    {
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.Columns.Clear();
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id", DataPropertyName = "Id", MinimumWidth = 56, FillWeight = 40 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Marca", DataPropertyName = "Marca", MinimumWidth = 90, FillWeight = 90 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Modelo", DataPropertyName = "Modelo", MinimumWidth = 90, FillWeight = 90 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Año", DataPropertyName = "Año", MinimumWidth = 64, FillWeight = 50 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Placa", DataPropertyName = "Placa", MinimumWidth = 88, FillWeight = 70 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Km", DataPropertyName = "Kilometraje", MinimumWidth = 72, FillWeight = 55 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Precio/día", DataPropertyName = "PrecioPorDia", MinimumWidth = 88, FillWeight = 70 });
        dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "Activo", DataPropertyName = "Activo", MinimumWidth = 72, FillWeight = 55 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Estado", DataPropertyName = "Estado", MinimumWidth = 72, FillWeight = 50 });
        ListFormLayout.ConfigureDataGrid(dataGridView1);
    }

    private async Task LoadDataAsync()
    {
        try
        {
            var list = await _service.GetList(v => true);
            dataGridView1.DataSource = list;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar vehículos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        var form = Program.ServiceProvider.GetRequiredService<VehiculoForm>();
        if (form.ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private void btnModificar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not Vehiculo entidad)
        {
            MessageBox.Show("Seleccione un vehículo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var form = ActivatorUtilities.CreateInstance<VehiculoForm>(Program.ServiceProvider, entidad);
        if (form.ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not Vehiculo entidad)
        {
            MessageBox.Show("Seleccione un vehículo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show($"¿Eliminar vehículo {entidad.Placa}?", "Confirmar", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        _ = EliminarAsync(entidad.Id);
    }

    private async Task EliminarAsync(int id)
    {
        try
        {
            if (await _service.Eliminar(id))
            {
                MessageBox.Show("Eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync();
            }
            else
                MessageBox.Show("No se pudo eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
