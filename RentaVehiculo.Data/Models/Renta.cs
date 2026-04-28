using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentaVehiculo.Data.Models;

[Index("IdCliente", Name = "IX_Rentas_IdCliente")]
[Index("IdEmpleado", Name = "IX_Rentas_IdEmpleado")]
[Index("IdVehiculo", Name = "IX_Rentas_IdVehiculo")]
[Index("SucursalEntrega", Name = "IX_Rentas_SucursalEntrega")]
[Index("SucursalRecogida", Name = "IX_Rentas_SucursalRecogida")]
public partial class Renta
{
    [Key]
    public int Id { get; set; }

    public int IdCliente { get; set; }

    public int IdVehiculo { get; set; }

    public int? IdEmpleado { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFinProgramada { get; set; }

    public DateTime? FechaFinReal { get; set; }

    public int SucursalRecogida { get; set; }

    public int? SucursalEntrega { get; set; }

    public int KilometrajeInicial { get; set; }

    public int? KilometrajeFinal { get; set; }

    public int Estado { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostoDiario { get; set; }

    public int DiasRentados { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostoTotal { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Deposito { get; set; }

    public bool DepositoDevuelto { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Descuento { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostoAdicionales { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MultaPorRetraso { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MultaPorDaños { get; set; }

    [StringLength(2000)]
    public string? ObservacionesInicio { get; set; }

    [StringLength(2000)]
    public string? ObservacionesFin { get; set; }

    public bool ContratoFirmado { get; set; }

    [StringLength(500)]
    public string? UrlContrato { get; set; }

    public DateTime FechaCreacion { get; set; }

    [InverseProperty("IdRentaNavigation")]
    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    [ForeignKey("IdCliente")]
    [InverseProperty("Renta")]
    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    [ForeignKey("IdEmpleado")]
    [InverseProperty("Renta")]
    public virtual Usuario? IdEmpleadoNavigation { get; set; }

    [ForeignKey("IdVehiculo")]
    [InverseProperty("Renta")]
    public virtual Vehiculo IdVehiculoNavigation { get; set; } = null!;

    [InverseProperty("IdRentaNavigation")]
    public virtual ICollection<RentaExtra> RentaExtras { get; set; } = new List<RentaExtra>();

    [ForeignKey("SucursalEntrega")]
    [InverseProperty("RentaSucursalEntregaNavigations")]
    public virtual Sucursale? SucursalEntregaNavigation { get; set; }

    [ForeignKey("SucursalRecogida")]
    [InverseProperty("RentaSucursalRecogidaNavigations")]
    public virtual Sucursale SucursalRecogidaNavigation { get; set; } = null!;
}
