using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentaVehiculo.Data.Models;

public partial class Sucursale
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string Nombre { get; set; } = null!;

    [StringLength(500)]
    public string? Direccion { get; set; }

    [StringLength(100)]
    public string? Ciudad { get; set; }

    [StringLength(20)]
    public string? Telefono { get; set; }

    [StringLength(200)]
    public string? Email { get; set; }

    public TimeOnly HorarioApertura { get; set; }

    public TimeOnly HorarioCierre { get; set; }

    public bool Activa { get; set; }

    [InverseProperty("SucursalEntregaNavigation")]
    public virtual ICollection<Renta> RentaSucursalEntregaNavigations { get; set; } = new List<Renta>();

    [InverseProperty("SucursalRecogidaNavigation")]
    public virtual ICollection<Renta> RentaSucursalRecogidaNavigations { get; set; } = new List<Renta>();

    [InverseProperty("SucursalEntregaNavigation")]
    public virtual ICollection<Reserva> ReservaSucursalEntregaNavigations { get; set; } = new List<Reserva>();

    [InverseProperty("SucursalRecogidaNavigation")]
    public virtual ICollection<Reserva> ReservaSucursalRecogidaNavigations { get; set; } = new List<Reserva>();

    [InverseProperty("IdSucursalNavigation")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    [InverseProperty("IdSucursalNavigation")]
    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
