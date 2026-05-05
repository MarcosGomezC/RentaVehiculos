using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Reservas;

public partial class ReservaForm : Form
{
    private readonly ReservaService _service;
    private Reserva? _entidad;

    public ReservaForm(ReservaService service) : this(service, null) { }

    public ReservaForm(ReservaService service, Reserva? entidad)
    {
        InitializeComponent();
        _service = service;
        _entidad = entidad;
        if (_entidad != null)
        {
            numIdCliente.Value = _entidad.IdCliente;
            numIdVehiculo.Value = _entidad.IdVehiculo;
            dtpInicio.Value = _entidad.FechaInicioReserva;
            dtpFin.Value = _entidad.FechaFinReserva;
            numMonto.Value = _entidad.MontoDeposito;
            numEstado.Value = _entidad.Estado;
            chkDepPagado.Checked = _entidad.DepositoPagado;
        }
        else
        {
            dtpInicio.Value = DateTime.Now.Date;
            dtpFin.Value = DateTime.Now.Date.AddDays(1);
            numMonto.Value = 50;
        }
    }

    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        _entidad ??= new Reserva();
        _entidad.IdCliente = (int)numIdCliente.Value;
        _entidad.IdVehiculo = (int)numIdVehiculo.Value;
        _entidad.FechaInicioReserva = dtpInicio.Value;
        _entidad.FechaFinReserva = dtpFin.Value;
        _entidad.MontoDeposito = numMonto.Value;
        _entidad.Estado = (int)numEstado.Value;
        _entidad.DepositoPagado = chkDepPagado.Checked;
        if (_entidad.FechaReserva == default)
            _entidad.FechaReserva = DateTime.Now;

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
