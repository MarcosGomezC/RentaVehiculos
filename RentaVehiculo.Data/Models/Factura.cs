using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentaVehiculo.Data.Models;

[Index("IdRenta", Name = "IX_Facturas_IdRenta")]
[Index("NumeroFactura", Name = "IX_Facturas_NumeroFactura", IsUnique = true)]
public partial class Factura
{
    [Key]
    public int Id { get; set; }

    public int IdRenta { get; set; }

    [StringLength(50)]
    public string NumeroFactura { get; set; } = null!;

    public DateTime FechaEmision { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Subtotal { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Impuestos { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Total { get; set; }

    public int MetodoPago { get; set; }

    public int Estado { get; set; }

    [Column("UrlPDF")]
    [StringLength(500)]
    public string? UrlPdf { get; set; }

    [ForeignKey("IdRenta")]
    [InverseProperty("Facturas")]
    public virtual Renta IdRentaNavigation { get; set; } = null!;
}
