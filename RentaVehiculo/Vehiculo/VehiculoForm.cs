using RentaVehiculo.UI.Services;
using RentaVehiculo.Data.Models;

namespace RentaVehiculo.UI.Vehiculos;

public partial class VehiculoForm : Form
{
    private readonly VehiculoService _service;
    private Vehiculo? _entidad;

    public VehiculoForm(VehiculoService service) : this(service, null)
    {
    }

    public VehiculoForm(VehiculoService service, Vehiculo? entidad)
    {
        InitializeComponent();
        _service = service;
        _entidad = entidad;
        if (_entidad != null)
            Cargar(_entidad);
        else
            chkActivo.Checked = true;
    }

    private void Cargar(Vehiculo v)
    {
        txtMarca.Text = v.Marca;
        txtModelo.Text = v.Modelo;
        numAño.Value = Math.Clamp(v.Año, (int)numAño.Minimum, (int)numAño.Maximum);
        txtPlaca.Text = v.Placa;
        numKm.Value = Math.Clamp(v.Kilometraje, 0, (int)numKm.Maximum);
        numPrecio.Value = Math.Clamp(v.PrecioPorDia, 0, numPrecio.Maximum);
        numEstado.Value = v.Estado;
        chkActivo.Checked = v.Activo;
        numIdSucursal.Value = v.IdSucursal ?? 0;
    }

    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMarca.Text) || string.IsNullOrWhiteSpace(txtPlaca.Text))
        {
            MessageBox.Show("Marca y placa son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var esNuevo = _entidad == null || _entidad.Id == 0;
        _entidad ??= new Vehiculo();

        _entidad.Marca = txtMarca.Text.Trim();
        _entidad.Modelo = txtModelo.Text.Trim();
        _entidad.Año = (int)numAño.Value;
        _entidad.Placa = txtPlaca.Text.Trim();
        _entidad.Kilometraje = (int)numKm.Value;
        _entidad.PrecioPorDia = numPrecio.Value;
        _entidad.Estado = (int)numEstado.Value;
        _entidad.Activo = chkActivo.Checked;
        _entidad.IdSucursal = numIdSucursal.Value == 0 ? null : (int)numIdSucursal.Value;

        if (_entidad.TipoCombustible == 0 && esNuevo)
            _entidad.TipoCombustible = 1;
        if (_entidad.TipoTransmision == 0 && esNuevo)
            _entidad.TipoTransmision = 1;
        if (_entidad.NumeroAsientos == 0 && esNuevo)
            _entidad.NumeroAsientos = 5;
        if (_entidad.CapacidadMaletero == 0 && esNuevo)
            _entidad.CapacidadMaletero = 300;
        if (_entidad.TipoVehiculo == 0 && esNuevo)
            _entidad.TipoVehiculo = 1;

        try
        {
            btnGuardar.Enabled = false;
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
        finally
        {
            btnGuardar.Enabled = true;
        }
    }
}
