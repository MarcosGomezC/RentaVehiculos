using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentaVehiculo.Data.Models;

[Index("IdVehiculo", Name = "IX_Mantenimientos_IdVehiculo")]
public partial class Mantenimiento
{
    [Key]
    public int Id { get; set; }

    public int IdVehiculo { get; set; }

    public int TipoMantenimiento { get; set; }

    [StringLength(1000)]
    public string? Descripcion { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Costo { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public int KilometrajeMantenimiento { get; set; }

    public int ProximoMantenimiento { get; set; }

    [StringLength(200)]
    public string? Proveedor { get; set; }

    public int Estado { get; set; }

    [StringLength(2000)]
    public string? Observaciones { get; set; }

    [ForeignKey("IdVehiculo")]
    [InverseProperty("Mantenimientos")]
    public virtual Vehiculo IdVehiculoNavigation { get; set; } = null!;
}
