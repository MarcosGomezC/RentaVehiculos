using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentaVehiculo.Data.Models;

[Index("IdVehiculo", Name = "IX_VehiculoImagenes_IdVehiculo")]
public partial class VehiculoImagene
{
    [Key]
    public int Id { get; set; }

    public int IdVehiculo { get; set; }

    [StringLength(500)]
    public string UrlImagen { get; set; } = null!;

    public bool EsPrincipal { get; set; }

    public int Orden { get; set; }

    [ForeignKey("IdVehiculo")]
    [InverseProperty("VehiculoImagenes")]
    public virtual Vehiculo IdVehiculoNavigation { get; set; } = null!;
}
