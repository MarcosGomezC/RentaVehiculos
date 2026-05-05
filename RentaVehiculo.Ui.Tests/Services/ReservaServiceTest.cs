using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Ui.Tests.Infrastructure;
using RentaVehiculo.UI.Services;
using ReservaEntity = RentaVehiculo.Data.Models.Reserva;

namespace RentaVehiculo.Ui.Tests.Services;

public class ReservaServiceTest
{
    [Fact]
    public async Task Buscar_CuandoExisteReserva_RetornaEntidad()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Reservas.Add(CreateReserva(
                id: 1,
                idCliente: 1,
                idVehiculo: 1,
                fechaReserva: new DateTime(2024, 1, 15),
                fechaInicioReserva: new DateTime(2024, 1, 20),
                fechaFinReserva: new DateTime(2024, 1, 23),
                estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ReservaService(context);

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
    public async Task Buscar_CuandoNoExisteReserva_RetornaNull()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new ReservaService(context);

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
            seedContext.Reservas.AddRange(
                CreateReserva(id: 1, idCliente: 1, idVehiculo: 1, fechaReserva: new DateTime(2024, 1, 1), fechaInicioReserva: new DateTime(2024, 1, 2), fechaFinReserva: new DateTime(2024, 1, 3), estado: 1),
                CreateReserva(id: 2, idCliente: 2, idVehiculo: 1, fechaReserva: new DateTime(2024, 1, 1), fechaInicioReserva: new DateTime(2024, 1, 5), fechaFinReserva: new DateTime(2024, 1, 7), estado: 2),
                CreateReserva(id: 3, idCliente: 1, idVehiculo: 2, fechaReserva: new DateTime(2024, 1, 2), fechaInicioReserva: new DateTime(2024, 1, 8), fechaFinReserva: new DateTime(2024, 1, 9), estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ReservaService(context);

        // Act
        var result = await service.GetList(r => r.Estado == 1);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Id == 1);
        Assert.Contains(result, r => r.Id == 3);
        Assert.DoesNotContain(result, r => r.Id == 2);
    }

    [Fact]
    public async Task Insertar_CuandoFechaReservaEsDefault_GeneraFechaReservaYAlmacena()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new ReservaService(context);

        var reserva = CreateReserva(
            id: 10,
            idCliente: 3,
            idVehiculo: 2,
            fechaReserva: default,
            fechaInicioReserva: DateTime.Today.AddDays(2),
            fechaFinReserva: DateTime.Today.AddDays(3),
            estado: 1);
        var beforeInsert = DateTime.Now;

        // Act
        var wasInserted = await service.Insertar(reserva);

        // Assert
        Assert.True(wasInserted);
        Assert.True(reserva.FechaReserva >= beforeInsert);

        var saved = await context.Reservas.FirstOrDefaultAsync(r => r.Id == 10);
        Assert.NotNull(saved);
        Assert.Equal(3, saved!.IdCliente);
    }

    [Fact]
    public async Task Existe_CuandoExisteReserva_RetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Reservas.Add(CreateReserva(
                id: 5,
                idCliente: 1,
                idVehiculo: 1,
                fechaReserva: new DateTime(2024, 2, 1),
                fechaInicioReserva: new DateTime(2024, 2, 5),
                fechaFinReserva: new DateTime(2024, 2, 6),
                estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ReservaService(context);

        // Act
        var exists = await service.Existe(5);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task Existe_CuandoNoExisteReserva_RetornaFalse()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new ReservaService(context);

        // Act
        var exists = await service.Existe(77);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task Modificar_CuandoReservaExiste_ActualizaDatos()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Reservas.Add(CreateReserva(
                id: 20,
                idCliente: 1,
                idVehiculo: 1,
                fechaReserva: new DateTime(2024, 3, 1),
                fechaInicioReserva: new DateTime(2024, 3, 10),
                fechaFinReserva: new DateTime(2024, 3, 12),
                estado: 1,
                depositoPagado: false));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ReservaService(context);

        var updated = CreateReserva(
            id: 20,
            idCliente: 1,
            idVehiculo: 1,
            fechaReserva: new DateTime(2024, 3, 1),
            fechaInicioReserva: new DateTime(2024, 3, 10),
            fechaFinReserva: new DateTime(2024, 3, 12),
            estado: 2,
            depositoPagado: true);

        // Act
        var wasUpdated = await service.Modificar(updated);

        // Assert
        Assert.True(wasUpdated);
        var saved = await context.Reservas.FirstOrDefaultAsync(r => r.Id == 20);
        Assert.NotNull(saved);
        Assert.Equal(2, saved!.Estado);
        Assert.True(saved.DepositoPagado);
    }

    [Fact]
    public async Task Guardar_CuandoReservaNoExiste_InsertaYRetornaTrue()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new ReservaService(context);

        var newReserva = CreateReserva(
            id: 30,
            idCliente: 2,
            idVehiculo: 3,
            fechaReserva: default,
            fechaInicioReserva: DateTime.Today.AddDays(1),
            fechaFinReserva: DateTime.Today.AddDays(2),
            estado: 1);

        // Act
        var result = await service.Guardar(newReserva);

        // Assert
        Assert.True(result);
        var saved = await context.Reservas.FirstOrDefaultAsync(r => r.Id == 30);
        Assert.NotNull(saved);
        Assert.True(saved!.FechaReserva != default);
    }

    [Fact]
    public async Task Guardar_CuandoReservaExiste_ModificaYRetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Reservas.Add(CreateReserva(
                id: 40,
                idCliente: 1,
                idVehiculo: 1,
                fechaReserva: new DateTime(2024, 4, 1),
                fechaInicioReserva: new DateTime(2024, 4, 5),
                fechaFinReserva: new DateTime(2024, 4, 6),
                estado: 1,
                depositoPagado: false));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ReservaService(context);

        var updated = CreateReserva(
            id: 40,
            idCliente: 1,
            idVehiculo: 1,
            fechaReserva: new DateTime(2024, 4, 1),
            fechaInicioReserva: new DateTime(2024, 4, 5),
            fechaFinReserva: new DateTime(2024, 4, 6),
            estado: 2,
            depositoPagado: true);

        // Act
        var result = await service.Guardar(updated);

        // Assert
        Assert.True(result);
        var saved = await context.Reservas.FirstOrDefaultAsync(r => r.Id == 40);
        Assert.NotNull(saved);
        Assert.Equal(2, saved!.Estado);
        Assert.True(saved.DepositoPagado);
    }

    [Fact]
    public async Task Eliminar_CuandoExisteReserva_EliminaYRetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Reservas.Add(CreateReserva(
                id: 50,
                idCliente: 1,
                idVehiculo: 2,
                fechaReserva: new DateTime(2024, 5, 1),
                fechaInicioReserva: new DateTime(2024, 5, 10),
                fechaFinReserva: new DateTime(2024, 5, 11),
                estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ReservaService(context);

        // Act
        var result = await service.Eliminar(50);

        // Assert
        Assert.True(result);
        var saved = await context.Reservas.FirstOrDefaultAsync(r => r.Id == 50);
        Assert.Null(saved);
    }

    [Fact]
    public async Task Eliminar_CuandoNoExisteReserva_RetornaFalse()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new ReservaService(context);

        // Act
        var result = await service.Eliminar(999);

        // Assert
        Assert.False(result);
    }

    private static ReservaEntity CreateReserva(
        int id,
        int idCliente,
        int idVehiculo,
        DateTime fechaReserva,
        DateTime fechaInicioReserva,
        DateTime fechaFinReserva,
        int estado,
        decimal montoDeposito = 0m,
        bool depositoPagado = false,
        string? observaciones = null,
        int? sucursalRecogida = null,
        int? sucursalEntrega = null)
    {
        return new ReservaEntity
        {
            Id = id,
            IdCliente = idCliente,
            IdVehiculo = idVehiculo,
            FechaReserva = fechaReserva,
            FechaInicioReserva = fechaInicioReserva,
            FechaFinReserva = fechaFinReserva,
            Estado = estado,
            MontoDeposito = montoDeposito,
            DepositoPagado = depositoPagado,
            Observaciones = observaciones,
            SucursalRecogida = sucursalRecogida,
            SucursalEntrega = sucursalEntrega
        };
    }
}

