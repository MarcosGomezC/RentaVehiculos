using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Ui.Tests.Infrastructure;
using RentaVehiculo.UI.Services;
using MantenimientoEntity = RentaVehiculo.Data.Models.Mantenimiento;

namespace RentaVehiculo.Ui.Tests.Services;

public class MantenimientoServiceTest
{
    [Fact]
    public async Task Buscar_CuandoExisteMantenimiento_RetornaEntidad()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Mantenimientos.Add(CreateMantenimiento(
                id: 1,
                idVehiculo: 1,
                tipoMantenimiento: 1,
                descripcion: "Cambio de aceite",
                costo: 25m,
                fechaInicio: new DateTime(2024, 1, 20),
                fechaFin: null,
                kilometrajeMantenimiento: 10000,
                proximoMantenimiento: 15000,
                proveedor: "Taller X",
                estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new MantenimientoService(context);

        // Act
        var result = await service.Buscar(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
        Assert.Equal(1, result.IdVehiculo);
        Assert.Equal(25m, result.Costo);
        Assert.Empty(context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task Buscar_CuandoNoExisteMantenimiento_RetornaNull()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new MantenimientoService(context);

        // Act
        var result = await service.Buscar(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetList_CuandoSeFiltraPorAbierto_RetornaCoincidencias()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Mantenimientos.AddRange(
                CreateMantenimiento(id: 1, idVehiculo: 1, tipoMantenimiento: 1, descripcion: "A", costo: 10m, fechaInicio: new DateTime(2024, 1, 1), fechaFin: null, kilometrajeMantenimiento: 100, proximoMantenimiento: 200, proveedor: "P", estado: 1),
                CreateMantenimiento(id: 2, idVehiculo: 1, tipoMantenimiento: 1, descripcion: "B", costo: 20m, fechaInicio: new DateTime(2024, 1, 2), fechaFin: new DateTime(2024, 1, 3), kilometrajeMantenimiento: 200, proximoMantenimiento: 300, proveedor: "P", estado: 2),
                CreateMantenimiento(id: 3, idVehiculo: 2, tipoMantenimiento: 2, descripcion: "C", costo: 30m, fechaInicio: new DateTime(2024, 1, 4), fechaFin: null, kilometrajeMantenimiento: 300, proximoMantenimiento: 400, proveedor: "P", estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new MantenimientoService(context);

        // Act
        var result = await service.GetList(m => m.FechaFin == null);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, m => m.Id == 1);
        Assert.Contains(result, m => m.Id == 3);
        Assert.DoesNotContain(result, m => m.Id == 2);
    }

    [Fact]
    public async Task Insertar_CuandoMantenimientoEsValido_Almacena()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new MantenimientoService(context);
        var mantenimiento = CreateMantenimiento(
            id: 10,
            idVehiculo: 3,
            tipoMantenimiento: 2,
            descripcion: "Frenos",
            costo: 120m,
            fechaInicio: new DateTime(2024, 2, 1),
            fechaFin: null,
            kilometrajeMantenimiento: 50000,
            proximoMantenimiento: 60000,
            proveedor: "Taller Y",
            estado: 1);

        // Act
        var wasInserted = await service.Insertar(mantenimiento);

        // Assert
        Assert.True(wasInserted);
        var saved = await context.Mantenimientos.FirstOrDefaultAsync(m => m.Id == 10);
        Assert.NotNull(saved);
        Assert.Equal(120m, saved!.Costo);
    }

    [Fact]
    public async Task Existe_CuandoExisteMantenimiento_RetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Mantenimientos.Add(CreateMantenimiento(
                id: 5,
                idVehiculo: 1,
                tipoMantenimiento: 1,
                descripcion: "A",
                costo: 10m,
                fechaInicio: new DateTime(2024, 1, 1),
                fechaFin: null,
                kilometrajeMantenimiento: 100,
                proximoMantenimiento: 200,
                proveedor: "P",
                estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new MantenimientoService(context);

        // Act
        var exists = await service.Existe(5);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task Existe_CuandoNoExisteMantenimiento_RetornaFalse()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new MantenimientoService(context);

        // Act
        var exists = await service.Existe(77);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task Modificar_CuandoMantenimientoExiste_ActualizaDatos()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Mantenimientos.Add(CreateMantenimiento(
                id: 20,
                idVehiculo: 1,
                tipoMantenimiento: 1,
                descripcion: "Inicial",
                costo: 50m,
                fechaInicio: new DateTime(2024, 3, 1),
                fechaFin: null,
                kilometrajeMantenimiento: 1000,
                proximoMantenimiento: 2000,
                proveedor: "P",
                estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new MantenimientoService(context);

        var updated = CreateMantenimiento(
            id: 20,
            idVehiculo: 1,
            tipoMantenimiento: 2,
            descripcion: "Actualizado",
            costo: 60m,
            fechaInicio: new DateTime(2024, 3, 1),
            fechaFin: new DateTime(2024, 3, 2),
            kilometrajeMantenimiento: 1100,
            proximoMantenimiento: 2100,
            proveedor: "P2",
            estado: 2);

        // Act
        var wasUpdated = await service.Modificar(updated);

        // Assert
        Assert.True(wasUpdated);
        var saved = await context.Mantenimientos.FirstOrDefaultAsync(m => m.Id == 20);
        Assert.NotNull(saved);
        Assert.Equal(2, saved!.TipoMantenimiento);
        Assert.Equal("Actualizado", saved.Descripcion);
        Assert.Equal(new DateTime(2024, 3, 2), saved.FechaFin);
    }

    [Fact]
    public async Task Guardar_CuandoMantenimientoNoExiste_InsertaYRetornaTrue()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new MantenimientoService(context);

        var newMantenimiento = CreateMantenimiento(
            id: 30,
            idVehiculo: 2,
            tipoMantenimiento: 1,
            descripcion: "Nuevo",
            costo: 80m,
            fechaInicio: new DateTime(2024, 4, 1),
            fechaFin: null,
            kilometrajeMantenimiento: 2000,
            proximoMantenimiento: 3000,
            proveedor: "P",
            estado: 1);

        // Act
        var result = await service.Guardar(newMantenimiento);

        // Assert
        Assert.True(result);
        var saved = await context.Mantenimientos.FirstOrDefaultAsync(m => m.Id == 30);
        Assert.NotNull(saved);
    }

    [Fact]
    public async Task Guardar_CuandoMantenimientoExiste_ModificaYRetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Mantenimientos.Add(CreateMantenimiento(
                id: 40,
                idVehiculo: 1,
                tipoMantenimiento: 1,
                descripcion: "Viejo",
                costo: 70m,
                fechaInicio: new DateTime(2024, 4, 10),
                fechaFin: null,
                kilometrajeMantenimiento: 3000,
                proximoMantenimiento: 4000,
                proveedor: "P",
                estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new MantenimientoService(context);

        var updated = CreateMantenimiento(
            id: 40,
            idVehiculo: 1,
            tipoMantenimiento: 1,
            descripcion: "Cerrado",
            costo: 75m,
            fechaInicio: new DateTime(2024, 4, 10),
            fechaFin: new DateTime(2024, 4, 11),
            kilometrajeMantenimiento: 3200,
            proximoMantenimiento: 4200,
            proveedor: "P",
            estado: 2);

        // Act
        var result = await service.Guardar(updated);

        // Assert
        Assert.True(result);
        var saved = await context.Mantenimientos.FirstOrDefaultAsync(m => m.Id == 40);
        Assert.NotNull(saved);
        Assert.Equal(new DateTime(2024, 4, 11), saved!.FechaFin);
        Assert.Equal(2, saved.Estado);
    }

    [Fact]
    public async Task Eliminar_CuandoExisteMantenimiento_EliminaYRetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Mantenimientos.Add(CreateMantenimiento(
                id: 50,
                idVehiculo: 1,
                tipoMantenimiento: 1,
                descripcion: "Borrar",
                costo: 10m,
                fechaInicio: new DateTime(2024, 5, 1),
                fechaFin: null,
                kilometrajeMantenimiento: 100,
                proximoMantenimiento: 200,
                proveedor: "P",
                estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new MantenimientoService(context);

        // Act
        var result = await service.Eliminar(50);

        // Assert
        Assert.True(result);
        var saved = await context.Mantenimientos.FirstOrDefaultAsync(m => m.Id == 50);
        Assert.Null(saved);
    }

    [Fact]
    public async Task Eliminar_CuandoNoExisteMantenimiento_RetornaFalse()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new MantenimientoService(context);

        // Act
        var result = await service.Eliminar(999);

        // Assert
        Assert.False(result);
    }

    private static MantenimientoEntity CreateMantenimiento(
        int id,
        int idVehiculo,
        int tipoMantenimiento,
        string? descripcion,
        decimal costo,
        DateTime fechaInicio,
        DateTime? fechaFin,
        int kilometrajeMantenimiento,
        int proximoMantenimiento,
        string? proveedor,
        int estado,
        string? observaciones = null)
    {
        return new MantenimientoEntity
        {
            Id = id,
            IdVehiculo = idVehiculo,
            TipoMantenimiento = tipoMantenimiento,
            Descripcion = descripcion,
            Costo = costo,
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            KilometrajeMantenimiento = kilometrajeMantenimiento,
            ProximoMantenimiento = proximoMantenimiento,
            Proveedor = proveedor,
            Estado = estado,
            Observaciones = observaciones
        };
    }
}

