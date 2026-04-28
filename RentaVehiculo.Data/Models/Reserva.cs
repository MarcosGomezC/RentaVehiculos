using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentaVehiculo.Data.Models;

[Index("IdCliente", Name = "IX_Reservas_IdCliente")]
[Index("IdVehiculo", Name = "IX_Reservas_IdVehiculo")]
[Index("SucursalEntrega", Name = "IX_Reservas_SucursalEntrega")]
[Index("SucursalRecogida", Name = "IX_Reservas_SucursalRecogida")]
public partial class Reserva
{
    [Key]
    public int Id { get; set; }

    public int IdCliente { get; set; }

    public int IdVehiculo { get; set; }

    public DateTime FechaReserva { get; set; }

    public DateTime FechaInicioReserva { get; set; }

    public DateTime FechaFinReserva { get; set; }

    public int Estado { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal MontoDeposito { get; set; }

    public bool DepositoPagado { get; set; }

    [StringLength(1000)]
    public string? Observaciones { get; set; }

    public int? SucursalRecogida { get; set; }

    public int? SucursalEntrega { get; set; }

    [ForeignKey("IdCliente")]
    [InverseProperty("Reservas")]
    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    [ForeignKey("IdVehiculo")]
    [InverseProperty("Reservas")]
    public virtual Vehiculo IdVehiculoNavigation { get; set; } = null!;

    [ForeignKey("SucursalEntrega")]
    [InverseProperty("ReservaSucursalEntregaNavigations")]
    public virtual Sucursale? SucursalEntregaNavigation { get; set; }

    [ForeignKey("SucursalRecogida")]
    [InverseProperty("ReservaSucursalRecogidaNavigations")]
    public virtual Sucursale? SucursalRecogidaNavigation { get; set; }
}
