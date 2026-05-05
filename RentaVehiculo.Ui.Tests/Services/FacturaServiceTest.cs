using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Ui.Tests.Infrastructure;
using RentaVehiculo.UI.Services;
using FacturaEntity = RentaVehiculo.Data.Models.Factura;

namespace RentaVehiculo.Ui.Tests.Services;

public class FacturaServiceTest
{
    [Fact]
    public async Task Buscar_CuandoExisteFactura_RetornaEntidad()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Facturas.Add(CreateFactura(
                id: 1,
                idRenta: 1,
                numeroFactura: "FAC-001",
                fechaEmision: new DateTime(2024, 1, 15),
                subtotal: 100.00m,
                impuestos: 20.00m,
                total: 120.00m,
                metodoPago: 1,
                estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new FacturaService(context);

        // Act
        var result = await service.Buscar(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
        Assert.Equal("FAC-001", result.NumeroFactura);
        Assert.Equal(120.00m, result.Total);
        Assert.Empty(context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task Buscar_CuandoNoExisteFactura_RetornaNull()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new FacturaService(context);

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
            seedContext.Facturas.AddRange(
                CreateFactura(id: 1, idRenta: 1, numeroFactura: "FAC-001", fechaEmision: new DateTime(2024, 1, 10), 
                    subtotal: 100m, impuestos: 20m, total: 120m, metodoPago: 1, estado: 1),
                CreateFactura(id: 2, idRenta: 2, numeroFactura: "FAC-002", fechaEmision: new DateTime(2024, 1, 11), 
                    subtotal: 150m, impuestos: 30m, total: 180m, metodoPago: 2, estado: 2),
                CreateFactura(id: 3, idRenta: 3, numeroFactura: "FAC-003", fechaEmision: new DateTime(2024, 1, 12), 
                    subtotal: 200m, impuestos: 40m, total: 240m, metodoPago: 1, estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new FacturaService(context);

        // Act
        var result = await service.GetList(f => f.Estado == 1);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, f => f.Id == 1);
        Assert.Contains(result, f => f.Id == 3);
        Assert.DoesNotContain(result, f => f.Id == 2);
    }

    [Fact]
    public async Task Insertar_CuandoFacturaEsValida_GeneraFechaEmisionYAlmacena()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new FacturaService(context);
        var factura = CreateFactura(
            id: 10,
            idRenta: 5,
            numeroFactura: "FAC-010",
            fechaEmision: default,
            subtotal: 250.00m,
            impuestos: 50.00m,
            total: 300.00m,
            metodoPago: 1,
            estado: 1);
        var beforeInsert = DateTime.Now;

        // Act
        var wasInserted = await service.Insertar(factura);

        // Assert
        Assert.True(wasInserted);
        Assert.True(factura.FechaEmision >= beforeInsert);

        var saved = await context.Facturas.FirstOrDefaultAsync(f => f.Id == 10);
        Assert.NotNull(saved);
        Assert.Equal("FAC-010", saved!.NumeroFactura);
        Assert.Equal(300.00m, saved.Total);
    }

    [Fact]
    public async Task Existe_CuandoExisteFactura_RetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Facturas.Add(CreateFactura(
                id: 5,
                idRenta: 2,
                numeroFactura: "FAC-005",
                fechaEmision: new DateTime(2024, 2, 1),
                subtotal: 75.00m,
                impuestos: 15.00m,
                total: 90.00m,
                metodoPago: 2,
                estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new FacturaService(context);

        // Act
        var exists = await service.Existe(5);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task Existe_CuandoNoExisteFactura_RetornaFalse()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new FacturaService(context);

        // Act
        var exists = await service.Existe(77);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task Modificar_CuandoFacturaExiste_ActualizaDatos()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Facturas.Add(CreateFactura(
                id: 20,
                idRenta: 4,
                numeroFactura: "FAC-020",
                fechaEmision: new DateTime(2024, 1, 20),
                subtotal: 500.00m,
                impuestos: 100.00m,
                total: 600.00m,
                metodoPago: 1,
                estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new FacturaService(context);
        var updated = CreateFactura(
            id: 20,
            idRenta: 4,
            numeroFactura: "FAC-020",
            fechaEmision: new DateTime(2024, 1, 20),
            subtotal: 500.00m,
            impuestos: 100.00m,
            total: 600.00m,
            metodoPago: 2,
            estado: 2);

        // Act
        var wasUpdated = await service.Modificar(updated);

        // Assert
        Assert.True(wasUpdated);

        var saved = await context.Facturas.FirstOrDefaultAsync(f => f.Id == 20);
        Assert.NotNull(saved);
        Assert.Equal(2, saved!.MetodoPago);
        Assert.Equal(2, saved.Estado);
    }

    [Fact]
    public async Task Guardar_CuandoFacturaNoExiste_InsertaYRetornaTrue()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new FacturaService(context);
        var newFactura = CreateFactura(
            id: 30,
            idRenta: 8,
            numeroFactura: "FAC-030",
            fechaEmision: default,
            subtotal: 350.00m,
            impuestos: 70.00m,
            total: 420.00m,
            metodoPago: 3,
            estado: 1);

        // Act
        var result = await service.Guardar(newFactura);

        // Assert
        Assert.True(result);

        var saved = await context.Facturas.FirstOrDefaultAsync(f => f.Id == 30);
        Assert.NotNull(saved);
        Assert.Equal("FAC-030", saved!.NumeroFactura);
        Assert.True(saved.FechaEmision != default);
    }

    [Fact]
    public async Task Guardar_CuandoFacturaExiste_ModificaYRetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Facturas.Add(CreateFactura(
                id: 40,
                idRenta: 6,
                numeroFactura: "FAC-040",
                fechaEmision: new DateTime(2024, 2, 5),
                subtotal: 450.00m,
                impuestos: 90.00m,
                total: 540.00m,
                metodoPago: 1,
                estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new FacturaService(context);
        var updated = CreateFactura(
            id: 40,
            idRenta: 6,
            numeroFactura: "FAC-040",
            fechaEmision: new DateTime(2024, 2, 5),
            subtotal: 450.00m,
            impuestos: 90.00m,
            total: 540.00m,
            metodoPago: 2,
            estado: 2);

        // Act
        var result = await service.Guardar(updated);

        // Assert
        Assert.True(result);

        var saved = await context.Facturas.FirstOrDefaultAsync(f => f.Id == 40);
        Assert.NotNull(saved);
        Assert.Equal(2, saved!.MetodoPago);
        Assert.Equal(2, saved.Estado);
    }

    [Fact]
    public async Task Eliminar_CuandoExisteFactura_EliminaYRetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Facturas.Add(CreateFactura(
                id: 50,
                idRenta: 7,
                numeroFactura: "FAC-050",
                fechaEmision: new DateTime(2024, 2, 10),
                subtotal: 200.00m,
                impuestos: 40.00m,
                total: 240.00m,
                metodoPago: 1,
                estado: 1));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new FacturaService(context);

        // Act
        var result = await service.Eliminar(50);

        // Assert
        Assert.True(result);
        var saved = await context.Facturas.FirstOrDefaultAsync(f => f.Id == 50);
        Assert.Null(saved);
    }

    [Fact]
    public async Task Eliminar_CuandoNoExisteFactura_RetornaFalse()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new FacturaService(context);

        // Act
        var result = await service.Eliminar(999);

        // Assert
        Assert.False(result);
    }

    private static FacturaEntity CreateFactura(
        int id,
        int idRenta,
        string numeroFactura,
        DateTime fechaEmision,
        decimal subtotal,
        decimal impuestos,
        decimal total,
        int metodoPago,
        int estado,
        string? urlPdf = null)
    {
        return new FacturaEntity
        {
            Id = id,
            IdRenta = idRenta,
            NumeroFactura = numeroFactura,
            FechaEmision = fechaEmision,
            Subtotal = subtotal,
            Impuestos = impuestos,
            Total = total,
            MetodoPago = metodoPago,
            Estado = estado,
            UrlPdf = urlPdf
        };
    }
}
