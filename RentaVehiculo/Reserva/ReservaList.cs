using Microsoft.Extensions.DependencyInjection;
using RentaVehiculo;
using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Infrastructure;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Reservas;

public partial class ReservaList : Form
{
    private readonly ReservaService _service;

    public ReservaList(ReservaService service)
    {
        InitializeComponent();
        _service = service;
    }

    private void ReservaList_Load(object sender, EventArgs e)
    {
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.Columns.Clear();
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id", DataPropertyName = "Id", MinimumWidth = 52, FillWeight = 35 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id cliente", DataPropertyName = "IdCliente", MinimumWidth = 80, FillWeight = 55 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id vehículo", DataPropertyName = "IdVehiculo", MinimumWidth = 90, FillWeight = 60 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Inicio", DataPropertyName = "FechaInicioReserva", MinimumWidth = 130, FillWeight = 95 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fin", DataPropertyName = "FechaFinReserva", MinimumWidth = 130, FillWeight = 95 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Estado", DataPropertyName = "Estado", MinimumWidth = 72, FillWeight = 50 });
        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Depósito", DataPropertyName = "MontoDeposito", MinimumWidth = 88, FillWeight = 65 });
        dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "Dep. pagado", DataPropertyName = "DepositoPagado", MinimumWidth = 100, FillWeight = 65 });
        ListFormLayout.ConfigureDataGrid(dataGridView1);
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            dataGridView1.DataSource = await _service.GetList(r => true);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (Program.ServiceProvider.GetRequiredService<ReservaForm>().ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private void btnModificar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not Reserva entidad)
        {
            MessageBox.Show("Seleccione una reserva.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (ActivatorUtilities.CreateInstance<ReservaForm>(Program.ServiceProvider, entidad).ShowDialog(this) == DialogResult.OK)
            _ = LoadDataAsync();
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow?.DataBoundItem is not Reserva entidad)
            return;
        if (MessageBox.Show("¿Eliminar esta reserva?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
