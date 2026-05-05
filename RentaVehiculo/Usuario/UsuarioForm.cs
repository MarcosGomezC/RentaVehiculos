using RentaVehiculo.Data.Models;
using RentaVehiculo.UI.Services;

namespace RentaVehiculo.UI.Usuarios;

public partial class UsuarioForm : Form
{
    private readonly UsuarioService _service;
    private Usuario? _entidad;

    public UsuarioForm(UsuarioService service) : this(service, null) { }

    public UsuarioForm(UsuarioService service, Usuario? entidad)
    {
        InitializeComponent();
        _service = service;
        _entidad = entidad;
        if (_entidad != null)
        {
            txtNombre.Text = _entidad.Nombre;
            txtApellido.Text = _entidad.Apellido;
            txtUsuario.Text = _entidad.NombreUsuario;
            txtEmail.Text = _entidad.Email;
            txtPassword.Text = "";
            txtPassword.PlaceholderText = "(sin cambiar)";
            txtRol.Text = _entidad.Rol;
            numSucursal.Value = _entidad.IdSucursal ?? 0;
            chkActivo.Checked = _entidad.Activo;
        }
        else
        {
            txtPassword.PlaceholderText = "obligatoria en alta";
            chkActivo.Checked = true;
            txtRol.Text = "Empleado";
        }
    }

    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtUsuario.Text))
        {
            MessageBox.Show("Nombre y nombre de usuario son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _entidad ??= new Usuario();
        _entidad.Nombre = txtNombre.Text.Trim();
        _entidad.Apellido = txtApellido.Text.Trim();
        _entidad.NombreUsuario = txtUsuario.Text.Trim();
        _entidad.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? $"{txtUsuario.Text.Trim()}@local" : txtEmail.Text.Trim();
        if (!string.IsNullOrWhiteSpace(txtPassword.Text))
            _entidad.PasswordHash = txtPassword.Text.Trim();
        else if (_entidad.Id == 0)
            _entidad.PasswordHash = "cambiar123";
        _entidad.Rol = string.IsNullOrWhiteSpace(txtRol.Text) ? "Empleado" : txtRol.Text.Trim();
        _entidad.Activo = chkActivo.Checked;
        _entidad.IdSucursal = numSucursal.Value == 0 ? null : (int)numSucursal.Value;

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
