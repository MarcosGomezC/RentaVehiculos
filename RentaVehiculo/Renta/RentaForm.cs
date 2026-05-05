using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Rentas;

public partial class RentaForm : Form
{
    private readonly RentaService _service;
    private Renta? _entidad;

    public RentaForm(RentaService service) : this(service, null) { }

    public RentaForm(RentaService service, Renta? entidad)
    {
        InitializeComponent();
        _service = service;
        _entidad = entidad;
        if (_entidad != null)
        {
            numIdCliente.Value = _entidad.IdCliente;
            numIdVehiculo.Value = _entidad.IdVehiculo;
            numIdEmpleado.Value = _entidad.IdEmpleado ?? 0;
            numSucRec.Value = _entidad.SucursalRecogida;
            numSucEnt.Value = _entidad.SucursalEntrega ?? 0;
            dtpInicio.Value = _entidad.FechaInicio;
            dtpFin.Value = _entidad.FechaFinProgramada;
            numKmIni.Value = _entidad.KilometrajeInicial;
            numCostoDia.Value = _entidad.CostoDiario;
            numDias.Value = _entidad.DiasRentados;
            numCostoTot.Value = _entidad.CostoTotal;
            numDep.Value = _entidad.Deposito;
            numEstado.Value = _entidad.Estado;
        }
        else
        {
            dtpInicio.Value = DateTime.Now;
            dtpFin.Value = DateTime.Now.AddDays(1);
            numCostoDia.Value = 30;
            numDias.Value = 1;
            numCostoTot.Value = 30;
            numDep.Value = 100;
            numEstado.Value = 1;
        }
    }

    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        if (numSucRec.Value == 0)
        {
            MessageBox.Show("Indique sucursal de recogida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _entidad ??= new Renta();
        _entidad.IdCliente = (int)numIdCliente.Value;
        _entidad.IdVehiculo = (int)numIdVehiculo.Value;
        _entidad.IdEmpleado = numIdEmpleado.Value == 0 ? null : (int)numIdEmpleado.Value;
        _entidad.SucursalRecogida = (int)numSucRec.Value;
        _entidad.SucursalEntrega = numSucEnt.Value == 0 ? null : (int)numSucEnt.Value;
        _entidad.FechaInicio = dtpInicio.Value;
        _entidad.FechaFinProgramada = dtpFin.Value;
        _entidad.KilometrajeInicial = (int)numKmIni.Value;
        _entidad.CostoDiario = numCostoDia.Value;
        _entidad.DiasRentados = (int)numDias.Value;
        _entidad.CostoTotal = numCostoTot.Value;
        _entidad.Deposito = numDep.Value;
        _entidad.Estado = (int)numEstado.Value;
        if (_entidad.FechaCreacion == default)
            _entidad.FechaCreacion = DateTime.Now;
        if (_entidad.Descuento == 0 && _entidad.CostoAdicionales == 0)
        {
            _entidad.Descuento = 0;
            _entidad.CostoAdicionales = 0;
        }

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
