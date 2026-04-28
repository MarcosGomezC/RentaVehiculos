using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentaVehiculo.Data.Models;

[Index("IdSucursal", Name = "IX_Vehiculos_IdSucursal")]
[Index("Placa", Name = "IX_Vehiculos_Placa", IsUnique = true)]
public partial class Vehiculo
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Marca { get; set; } = null!;

    [StringLength(100)]
    public string Modelo { get; set; } = null!;

    public int Año { get; set; }

    [StringLength(20)]
    public string Placa { get; set; } = null!;

    [StringLength(50)]
    public string? Color { get; set; }

    [StringLength(100)]
    public string? NumeroSerie { get; set; }

    public int Kilometraje { get; set; }

    public int TipoCombustible { get; set; }

    public int TipoTransmision { get; set; }

    public int NumeroAsientos { get; set; }

    public int CapacidadMaletero { get; set; }

    public int TipoVehiculo { get; set; }

    public int Estado { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PrecioPorDia { get; set; }

    [StringLength(500)]
    public string? ImagenPrincipal { get; set; }

    [StringLength(1000)]
    public string? Descripcion { get; set; }

    [StringLength(2000)]
    public string? CaracteristicasExtras { get; set; }

    public int? IdSucursal { get; set; }

    public DateTime FechaRegistro { get; set; }

    public bool Activo { get; set; }

    [ForeignKey("IdSucursal")]
    [InverseProperty("Vehiculos")]
    public virtual Sucursale? IdSucursalNavigation { get; set; }

    [InverseProperty("IdVehiculoNavigation")]
    public virtual ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();

    [InverseProperty("IdVehiculoNavigation")]
    public virtual ICollection<Renta> Renta { get; set; } = new List<Renta>();

    [InverseProperty("IdVehiculoNavigation")]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    [InverseProperty("IdVehiculoNavigation")]
    public virtual ICollection<VehiculoCaracteristica> VehiculoCaracteristicas { get; set; } = new List<VehiculoCaracteristica>();

    [InverseProperty("IdVehiculoNavigation")]
    public virtual ICollection<VehiculoImagene> VehiculoImagenes { get; set; } = new List<VehiculoImagene>();
}
