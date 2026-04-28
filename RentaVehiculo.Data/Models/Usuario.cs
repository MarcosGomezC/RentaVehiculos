using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentaVehiculo.Data.Models;

[Index("Email", Name = "IX_Usuarios_Email", IsUnique = true)]
[Index("IdSucursal", Name = "IX_Usuarios_IdSucursal")]
[Index("NombreUsuario", Name = "IX_Usuarios_NombreUsuario", IsUnique = true)]
public partial class Usuario
{
    [Key]
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string NombreUsuario { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public bool Activo { get; set; }

    public int? IdSucursal { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    [ForeignKey("IdSucursal")]
    [InverseProperty("Usuarios")]
    public virtual Sucursale? IdSucursalNavigation { get; set; }

    [InverseProperty("IdEmpleadoNavigation")]
    public virtual ICollection<Renta> Renta { get; set; } = new List<Renta>();
}
