using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentaVehiculo.Data.Models;

[Index("IdRenta", Name = "IX_RentaExtras_IdRenta")]
public partial class RentaExtra
{
    [Key]
    public int Id { get; set; }

    public int IdRenta { get; set; }

    [StringLength(200)]
    public string Concepto { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Costo { get; set; }

    public int Cantidad { get; set; }

    [ForeignKey("IdRenta")]
    [InverseProperty("RentaExtras")]
    public virtual Renta IdRentaNavigation { get; set; } = null!;
}
