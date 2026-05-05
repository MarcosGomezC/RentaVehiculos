using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Ui.Tests.Infrastructure;
using RentaVehiculo.UI.Services;
using ClienteEntity = RentaVehiculo.Data.Models.Cliente;
using MantenimientoEntity = RentaVehiculo.Data.Models.Mantenimiento;
using RentaEntity = RentaVehiculo.Data.Models.Renta;
using VehiculoEntity = RentaVehiculo.Data.Models.Vehiculo;

namespace RentaVehiculo.Ui.Tests.Services;

public class DashboardServiceTest
{
    [Fact]
    public async Task ObtenerAsync_CalculaConteosBasicos()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        var hoy = DateTime.Today;
        var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Vehiculos.AddRange(
                CreateVehiculo(id: 1, placa: "A-1", activo: true, fechaRegistro: inicioMes.AddDays(1)),
                CreateVehiculo(id: 2, placa: "A-2", activo: true, fechaRegistro: inicioMes.AddDays(-1)),
                CreateVehiculo(id: 3, placa: "A-3", activo: false, fechaRegistro: inicioMes.AddDays(2)));

            seedContext.Mantenimientos.AddRange(
                CreateMantenimiento(id: 1, idVehiculo: 1, fechaInicio: hoy, fechaFin: null),
                CreateMantenimiento(id: 2, idVehiculo: 2, fechaInicio: hoy, fechaFin: hoy));

            seedContext.Rentas.AddRange(
                CreateRenta(id: 1, idCliente: 1, idVehiculo: 1, fechaFinProgramada: hoy, fechaFinReal: null),
                CreateRenta(id: 2, idCliente: 1, idVehiculo: 2, fechaFinProgramada: hoy.AddDays(1), fechaFinReal: null),
                CreateRenta(id: 3, idCliente: 1, idVehiculo: 2, fechaFinProgramada: hoy.AddDays(10), fechaFinReal: null),
                CreateRenta(id: 4, idCliente: 1, idVehiculo: 2, fechaFinProgramada: hoy, fechaFinReal: hoy));

            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new DashboardService(context);

        // Act
        var stats = await service.ObtenerAsync();

        // Assert
        Assert.Equal(2, stats.VehiculosActivos);
        Assert.Equal(2, stats.VehiculosAltaEsteMes);
        Assert.Equal(2, stats.RentasProximasAVencer);
        Assert.Equal(1, stats.MantenimientosAbiertos);
    }

    [Fact]
    public async Task ObtenerAsync_GeneraAlertasYPriorizaPeligro()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        var hoy = DateTime.Today;

        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Clientes.Add(CreateCliente(id: 1, nombre: "Marco", apellido: "Gomez"));
            seedContext.Vehiculos.AddRange(
                CreateVehiculo(id: 1, placa: "P-1", activo: true, fechaRegistro: hoy),
                CreateVehiculo(id: 2, placa: "P-2", activo: true, fechaRegistro: hoy));

            // Mantenimiento dentro de 7 días -> Info
            seedContext.Mantenimientos.Add(CreateMantenimiento(
                id: 1,
                idVehiculo: 1,
                fechaInicio: hoy.AddDays(2).AddHours(10),
                fechaFin: null,
                descripcion: "Aceite"));

            // Renta por vencer (hoy/mañana) -> Advertencia
            seedContext.Rentas.Add(CreateRenta(
                id: 1,
                idCliente: 1,
                idVehiculo: 2,
                fechaFinProgramada: DateTime.Now.AddHours(2),
                fechaFinReal: null));

            // Renta vencida (antes de hoy) -> Peligro
            seedContext.Rentas.Add(CreateRenta(
                id: 2,
                idCliente: 1,
                idVehiculo: 2,
                fechaFinProgramada: hoy.AddDays(-2).AddHours(12),
                fechaFinReal: null));

            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new DashboardService(context);

        // Act
        var stats = await service.ObtenerAsync();

        // Assert
        Assert.NotNull(stats.Alertas);
        Assert.Contains(stats.Alertas, a => a.Titulo == "Mantenimiento programado" && a.Tipo == DashboardAlertaTipo.Info);
        Assert.Contains(stats.Alertas, a => a.Titulo == "Renta por vencer" && a.Tipo == DashboardAlertaTipo.Advertencia);
        Assert.Contains(stats.Alertas, a => a.Titulo == "Renta vencida" && a.Tipo == DashboardAlertaTipo.Peligro);

        Assert.Equal(DashboardAlertaTipo.Peligro, stats.Alertas.First().Tipo);
    }

    private static VehiculoEntity CreateVehiculo(int id, string placa, bool activo, DateTime fechaRegistro)
    {
        return new VehiculoEntity
        {
            Id = id,
            Marca = "Marca",
            Modelo = "Modelo",
            Año = 2024,
            Placa = placa,
            Kilometraje = 0,
            TipoCombustible = 1,
            TipoTransmision = 1,
            NumeroAsientos = 5,
            CapacidadMaletero = 400,
            TipoVehiculo = 1,
            Estado = 1,
            PrecioPorDia = 50m,
            FechaRegistro = fechaRegistro,
            Activo = activo
        };
    }

    private static ClienteEntity CreateCliente(int id, string nombre, string apellido)
    {
        return new ClienteEntity
        {
            Id = id,
            Nombre = nombre,
            Apellido = apellido,
            Email = $"{id}@correo.com",
            Telefono = "000",
            Licencia = $"LIC{id}",
            FechaVencimientoLicencia = DateTime.Today.AddYears(1),
            FechaNacimiento = new DateTime(1990, 1, 1),
            TipoCliente = 1,
            FechaRegistro = DateTime.Today,
            Activo = true
        };
    }

    private static MantenimientoEntity CreateMantenimiento(
        int id,
        int idVehiculo,
        DateTime fechaInicio,
        DateTime? fechaFin,
        string? descripcion = null)
    {
        return new MantenimientoEntity
        {
            Id = id,
            IdVehiculo = idVehiculo,
            TipoMantenimiento = 1,
            Descripcion = descripcion,
            Costo = 0m,
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            KilometrajeMantenimiento = 0,
            ProximoMantenimiento = 0,
            Estado = 1
        };
    }

    private static RentaEntity CreateRenta(
        int id,
        int idCliente,
        int idVehiculo,
        DateTime fechaFinProgramada,
        DateTime? fechaFinReal)
    {
        return new RentaEntity
        {
            Id = id,
            IdCliente = idCliente,
            IdVehiculo = idVehiculo,
            FechaInicio = DateTime.Today.AddDays(-1),
            FechaFinProgramada = fechaFinProgramada,
            FechaFinReal = fechaFinReal,
            SucursalRecogida = 1,
            KilometrajeInicial = 0,
            Estado = 1,
            CostoDiario = 1m,
            DiasRentados = 1,
            CostoTotal = 1m,
            Deposito = 0m,
            DepositoDevuelto = false,
            Descuento = 0m,
            CostoAdicionales = 0m,
            ContratoFirmado = false,
            FechaCreacion = DateTime.Now
        };
    }
}

