using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentaVehiculo.Data.Models;

[Index("IdVehiculo", Name = "IX_VehiculoCaracteristicas_IdVehiculo")]
public partial class VehiculoCaracteristica
{
    [Key]
    public int Id { get; set; }

    public int IdVehiculo { get; set; }

    [StringLength(100)]
    public string Caracteristica { get; set; } = null!;

    [StringLength(50)]
    public string? Icono { get; set; }

    [ForeignKey("IdVehiculo")]
    [InverseProperty("VehiculoCaracteristicas")]
    public virtual Vehiculo IdVehiculoNavigation { get; set; } = null!;
}
