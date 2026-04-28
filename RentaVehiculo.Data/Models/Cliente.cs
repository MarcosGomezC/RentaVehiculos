using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentaVehiculo.Data.Models;

[Index("Email", Name = "IX_Clientes_Email", IsUnique = true)]
[Index("IdUsuario", Name = "IX_Clientes_IdUsuario")]
[Index("Licencia", Name = "IX_Clientes_Licencia", IsUnique = true)]
public partial class Cliente
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [StringLength(100)]
    public string Apellido { get; set; } = null!;

    [StringLength(200)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string Telefono { get; set; } = null!;

    [StringLength(50)]
    public string Licencia { get; set; } = null!;

    public DateTime FechaVencimientoLicencia { get; set; }

    public DateTime FechaNacimiento { get; set; }

    [StringLength(500)]
    public string? Direccion { get; set; }

    [StringLength(100)]
    public string? Ciudad { get; set; }

    [StringLength(20)]
    public string? CodigoPostal { get; set; }

    public int TipoCliente { get; set; }

    public DateTime FechaRegistro { get; set; }

    public bool Activo { get; set; }

    public int? IdUsuario { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Clientes")]
    public virtual Usuario? IdUsuarioNavigation { get; set; }

    [InverseProperty("IdClienteNavigation")]
    public virtual ICollection<Renta> Renta { get; set; } = new List<Renta>();

    [InverseProperty("IdClienteNavigation")]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
