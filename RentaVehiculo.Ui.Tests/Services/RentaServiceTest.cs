using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Ui.Tests.Infrastructure;
using RentaVehiculo.UI.Services;
using RentaEntity = RentaVehiculo.Data.Models.Renta;

namespace RentaVehiculo.Ui.Tests.Services;

public class RentaServiceTest
{
    [Fact]
    public async Task Buscar_CuandoExisteRenta_RetornaEntidad()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Rentas.Add(CreateRenta(
                id: 1,
                idCliente: 1,
                idVehiculo: 1,
                fechaInicio: new DateTime(2024, 1, 1),
                fechaFinProgramada: new DateTime(2024, 1, 3),
                fechaCreacion: new DateTime(2024, 1, 1, 8, 0, 0),
                estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new RentaService(context);

        // Act
        var result = await service.Buscar(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
        Assert.Equal(1, result.IdCliente);
        Assert.Equal(1, result.IdVehiculo);
        Assert.Empty(context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task Buscar_CuandoNoExisteRenta_RetornaNull()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new RentaService(context);

        // Act
        var result = await service.Buscar(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetList_CuandoSeFiltraPorEstado_RetornaCoincidencias()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Rentas.AddRange(
                CreateRenta(id: 1, idCliente: 1, idVehiculo: 1, fechaInicio: new DateTime(2024, 1, 1), fechaFinProgramada: new DateTime(2024, 1, 2), fechaCreacion: new DateTime(2024, 1, 1), estado: 1),
                CreateRenta(id: 2, idCliente: 1, idVehiculo: 2, fechaInicio: new DateTime(2024, 1, 5), fechaFinProgramada: new DateTime(2024, 1, 6), fechaCreacion: new DateTime(2024, 1, 5), estado: 2),
                CreateRenta(id: 3, idCliente: 2, idVehiculo: 1, fechaInicio: new DateTime(2024, 1, 7), fechaFinProgramada: new DateTime(2024, 1, 9), fechaCreacion: new DateTime(2024, 1, 7), estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new RentaService(context);

        // Act
        var result = await service.GetList(r => r.Estado == 1);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Id == 1);
        Assert.Contains(result, r => r.Id == 3);
        Assert.DoesNotContain(result, r => r.Id == 2);
    }

    [Fact]
    public async Task Insertar_CuandoFechaCreacionEsDefault_GeneraFechaCreacionYAlmacena()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new RentaService(context);
        var renta = CreateRenta(
            id: 10,
            idCliente: 2,
            idVehiculo: 3,
            fechaInicio: DateTime.Today,
            fechaFinProgramada: DateTime.Today.AddDays(2),
            fechaCreacion: default,
            estado: 1);
        var beforeInsert = DateTime.Now;

        // Act
        var wasInserted = await service.Insertar(renta);

        // Assert
        Assert.True(wasInserted);
        Assert.True(renta.FechaCreacion >= beforeInsert);

        var saved = await context.Rentas.FirstOrDefaultAsync(r => r.Id == 10);
        Assert.NotNull(saved);
        Assert.Equal(2, saved!.IdCliente);
    }

    [Fact]
    public async Task Existe_CuandoExisteRenta_RetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Rentas.Add(CreateRenta(
                id: 5,
                idCliente: 1,
                idVehiculo: 1,
                fechaInicio: new DateTime(2024, 2, 1),
                fechaFinProgramada: new DateTime(2024, 2, 3),
                fechaCreacion: new DateTime(2024, 2, 1),
                estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new RentaService(context);

        // Act
        var exists = await service.Existe(5);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task Existe_CuandoNoExisteRenta_RetornaFalse()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new RentaService(context);

        // Act
        var exists = await service.Existe(77);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task Modificar_CuandoRentaExiste_ActualizaDatos()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Rentas.Add(CreateRenta(
                id: 20,
                idCliente: 1,
                idVehiculo: 1,
                fechaInicio: new DateTime(2024, 3, 1),
                fechaFinProgramada: new DateTime(2024, 3, 5),
                fechaCreacion: new DateTime(2024, 3, 1),
                estado: 1,
                costoTotal: 200m,
                diasRentados: 4));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new RentaService(context);

        var updated = CreateRenta(
            id: 20,
            idCliente: 1,
            idVehiculo: 1,
            fechaInicio: new DateTime(2024, 3, 1),
            fechaFinProgramada: new DateTime(2024, 3, 6),
            fechaCreacion: new DateTime(2024, 3, 1),
            estado: 2,
            costoTotal: 250m,
            diasRentados: 5);

        // Act
        var wasUpdated = await service.Modificar(updated);

        // Assert
        Assert.True(wasUpdated);
        var saved = await context.Rentas.FirstOrDefaultAsync(r => r.Id == 20);
        Assert.NotNull(saved);
        Assert.Equal(2, saved!.Estado);
        Assert.Equal(250m, saved.CostoTotal);
        Assert.Equal(new DateTime(2024, 3, 6), saved.FechaFinProgramada);
    }

    [Fact]
    public async Task Guardar_CuandoRentaNoExiste_InsertaYRetornaTrue()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new RentaService(context);

        var newRenta = CreateRenta(
            id: 30,
            idCliente: 3,
            idVehiculo: 2,
            fechaInicio: DateTime.Today,
            fechaFinProgramada: DateTime.Today.AddDays(1),
            fechaCreacion: default,
            estado: 1);

        // Act
        var result = await service.Guardar(newRenta);

        // Assert
        Assert.True(result);
        var saved = await context.Rentas.FirstOrDefaultAsync(r => r.Id == 30);
        Assert.NotNull(saved);
        Assert.True(saved!.FechaCreacion != default);
    }

    [Fact]
    public async Task Guardar_CuandoRentaExiste_ModificaYRetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Rentas.Add(CreateRenta(
                id: 40,
                idCliente: 1,
                idVehiculo: 1,
                fechaInicio: new DateTime(2024, 4, 1),
                fechaFinProgramada: new DateTime(2024, 4, 3),
                fechaCreacion: new DateTime(2024, 4, 1),
                estado: 1,
                depositoDevuelto: false));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new RentaService(context);

        var updated = CreateRenta(
            id: 40,
            idCliente: 1,
            idVehiculo: 1,
            fechaInicio: new DateTime(2024, 4, 1),
            fechaFinProgramada: new DateTime(2024, 4, 3),
            fechaCreacion: new DateTime(2024, 4, 1),
            estado: 2,
            depositoDevuelto: true);

        // Act
        var result = await service.Guardar(updated);

        // Assert
        Assert.True(result);
        var saved = await context.Rentas.FirstOrDefaultAsync(r => r.Id == 40);
        Assert.NotNull(saved);
        Assert.Equal(2, saved!.Estado);
        Assert.True(saved.DepositoDevuelto);
    }

    [Fact]
    public async Task Eliminar_CuandoExisteRenta_EliminaYRetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Rentas.Add(CreateRenta(
                id: 50,
                idCliente: 1,
                idVehiculo: 1,
                fechaInicio: new DateTime(2024, 5, 1),
                fechaFinProgramada: new DateTime(2024, 5, 2),
                fechaCreacion: new DateTime(2024, 5, 1),
                estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new RentaService(context);

        // Act
        var result = await service.Eliminar(50);

        // Assert
        Assert.True(result);
        var saved = await context.Rentas.FirstOrDefaultAsync(r => r.Id == 50);
        Assert.Null(saved);
    }

    [Fact]
    public async Task Eliminar_CuandoNoExisteRenta_RetornaFalse()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new RentaService(context);

        // Act
        var result = await service.Eliminar(999);

        // Assert
        Assert.False(result);
    }

    private static RentaEntity CreateRenta(
        int id,
        int idCliente,
        int idVehiculo,
        DateTime fechaInicio,
        DateTime fechaFinProgramada,
        DateTime fechaCreacion,
        int estado,
        int sucursalRecogida = 1,
        int? sucursalEntrega = null,
        int kilometrajeInicial = 0,
        int? kilometrajeFinal = null,
        decimal costoDiario = 50m,
        int diasRentados = 1,
        decimal costoTotal = 50m,
        decimal deposito = 0m,
        bool depositoDevuelto = false,
        decimal descuento = 0m,
        decimal costoAdicionales = 0m,
        decimal? multaPorRetraso = null,
        decimal? multaPorDaños = null,
        string? observacionesInicio = null,
        string? observacionesFin = null,
        bool contratoFirmado = false,
        string? urlContrato = null,
        int? idEmpleado = null,
        DateTime? fechaFinReal = null)
    {
        return new RentaEntity
        {
            Id = id,
            IdCliente = idCliente,
            IdVehiculo = idVehiculo,
            IdEmpleado = idEmpleado,
            FechaInicio = fechaInicio,
            FechaFinProgramada = fechaFinProgramada,
            FechaFinReal = fechaFinReal,
            SucursalRecogida = sucursalRecogida,
            SucursalEntrega = sucursalEntrega,
            KilometrajeInicial = kilometrajeInicial,
            KilometrajeFinal = kilometrajeFinal,
            Estado = estado,
            CostoDiario = costoDiario,
            DiasRentados = diasRentados,
            CostoTotal = costoTotal,
            Deposito = deposito,
            DepositoDevuelto = depositoDevuelto,
            Descuento = descuento,
            CostoAdicionales = costoAdicionales,
            MultaPorRetraso = multaPorRetraso,
            MultaPorDaños = multaPorDaños,
            ObservacionesInicio = observacionesInicio,
            ObservacionesFin = observacionesFin,
            ContratoFirmado = contratoFirmado,
            UrlContrato = urlContrato,
            FechaCreacion = fechaCreacion
        };
    }
}

