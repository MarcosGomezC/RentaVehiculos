using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentaVehiculo.Data.Models;

public partial class ConfiguracionRentum
{
    [Key]
    public int Id { get; set; }

    public int HorasGracia { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PorcentajeMultaPorHora { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal DepositoMinimo { get; set; }

    public int KmMaximosPorDia { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostoPorKmExtra { get; set; }

    public int DiasMaximoReserva { get; set; }

    public bool PermitirRentaSinDeposito { get; set; }

    public int EdadMinimaRenta { get; set; }

    public int AñosMinimosLicencia { get; set; }
}
