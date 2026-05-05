using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Mantenimientos;

public partial class MantenimientoForm : Form
{
    private readonly MantenimientoService _service;
    private Mantenimiento? _entidad;

    public MantenimientoForm(MantenimientoService service) : this(service, null) { }

    public MantenimientoForm(MantenimientoService service, Mantenimiento? entidad)
    {
        InitializeComponent();
        _service = service;
        _entidad = entidad;
        if (_entidad != null)
        {
            numIdVehiculo.Value = _entidad.IdVehiculo;
            numTipo.Value = _entidad.TipoMantenimiento;
            numCosto.Value = _entidad.Costo;
            dtpInicio.Value = _entidad.FechaInicio;
            if (_entidad.FechaFin.HasValue)
                dtpFin.Value = _entidad.FechaFin.Value;
            numKm.Value = _entidad.KilometrajeMantenimiento;
            numProx.Value = _entidad.ProximoMantenimiento;
            numEstado.Value = _entidad.Estado;
            txtProveedor.Text = _entidad.Proveedor ?? "";
        }
    }

    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        _entidad ??= new Mantenimiento();
        _entidad.IdVehiculo = (int)numIdVehiculo.Value;
        _entidad.TipoMantenimiento = (int)numTipo.Value;
        _entidad.Costo = numCosto.Value;
        _entidad.FechaInicio = dtpInicio.Value;
        _entidad.FechaFin = dtpFin.Value;
        _entidad.KilometrajeMantenimiento = (int)numKm.Value;
        _entidad.ProximoMantenimiento = (int)numProx.Value;
        _entidad.Estado = (int)numEstado.Value;
        _entidad.Proveedor = string.IsNullOrWhiteSpace(txtProveedor.Text) ? null : txtProveedor.Text.Trim();

        try
        {
            if (await _service.Guardar(_entidad))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
