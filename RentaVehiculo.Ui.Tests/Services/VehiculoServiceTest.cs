using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Ui.Tests.Infrastructure;
using RentaVehiculo.UI.Services;
using VehiculoEntity = RentaVehiculo.Data.Models.Vehiculo;

namespace RentaVehiculo.Ui.Tests.Services;

public class VehiculoServiceTest
{
    [Fact]
    public async Task Buscar_CuandoExisteVehiculo_RetornaEntidad()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Vehiculos.Add(CreateVehiculo(
                id: 1,
                marca: "Toyota",
                modelo: "Camry",
                año: 2023,
                placa: "ABC-123",
                color: "Plateado",
                numeroSerie: "SN12345",
                kilometraje: 5000,
                tipoCombustible: 1,
                tipoTransmision: 1,
                numeroAsientos: 5,
                capacidadMaletero: 450,
                tipoVehiculo: 1,
                estado: 1,
                precioPorDia: 50.00m,
                activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new VehiculoService(context);

        // Act
        var result = await service.Buscar(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
        Assert.Equal("ABC-123", result.Placa);
        Assert.Equal("Toyota", result.Marca);
        Assert.Equal(50.00m, result.PrecioPorDia);
        Assert.Empty(context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task Buscar_CuandoNoExisteVehiculo_RetornaNull()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new VehiculoService(context);

        // Act
        var result = await service.Buscar(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetList_CuandoSeFiltraPorActivo_RetornaCoincidencias()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Vehiculos.AddRange(
                CreateVehiculo(id: 1, marca: "Toyota", modelo: "Corolla", año: 2023, placa: "ABC-001", color: "Blanco",
                    numeroSerie: "SN001", kilometraje: 1000, tipoCombustible: 1, tipoTransmision: 1, numeroAsientos: 5,
                    capacidadMaletero: 400, tipoVehiculo: 1, estado: 1, precioPorDia: 40m, activo: true),
                CreateVehiculo(id: 2, marca: "Honda", modelo: "Civic", año: 2022, placa: "ABC-002", color: "Negro",
                    numeroSerie: "SN002", kilometraje: 15000, tipoCombustible: 1, tipoTransmision: 1, numeroAsientos: 5,
                    capacidadMaletero: 380, tipoVehiculo: 1, estado: 1, precioPorDia: 45m, activo: false),
                CreateVehiculo(id: 3, marca: "Ford", modelo: "Focus", año: 2023, placa: "ABC-003", color: "Rojo",
                    numeroSerie: "SN003", kilometraje: 2000, tipoCombustible: 1, tipoTransmision: 1, numeroAsientos: 5,
                    capacidadMaletero: 420, tipoVehiculo: 1, estado: 1, precioPorDia: 42m, activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new VehiculoService(context);

        // Act
        var result = await service.GetList(v => v.Activo);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, v => v.Id == 1);
        Assert.Contains(result, v => v.Id == 3);
        Assert.DoesNotContain(result, v => v.Id == 2);
    }

    [Fact]
    public async Task Insertar_CuandoVehiculoEsValido_GeneraFechaRegistroYAlmacena()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new VehiculoService(context);
        var vehiculo = CreateVehiculo(
            id: 10,
            marca: "Hyundai",
            modelo: "Elantra",
            año: 2024,
            placa: "XYZ-789",
            color: "Azul",
            numeroSerie: "SN010",
            kilometraje: 0,
            tipoCombustible: 1,
            tipoTransmision: 1,
            numeroAsientos: 5,
            capacidadMaletero: 410,
            tipoVehiculo: 1,
            estado: 1,
            precioPorDia: 48.00m,
            activo: true,
            fechaRegistro: default);
        var beforeInsert = DateTime.Now;

        // Act
        var wasInserted = await service.Insertar(vehiculo);

        // Assert
        Assert.True(wasInserted);
        Assert.True(vehiculo.FechaRegistro >= beforeInsert);

        var saved = await context.Vehiculos.FirstOrDefaultAsync(v => v.Id == 10);
        Assert.NotNull(saved);
        Assert.Equal("XYZ-789", saved!.Placa);
        Assert.Equal("Hyundai", saved.Marca);
    }

    [Fact]
    public async Task Existe_CuandoExisteVehiculo_RetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Vehiculos.Add(CreateVehiculo(
                id: 5,
                marca: "Mazda",
                modelo: "3",
                año: 2023,
                placa: "MZD-555",
                color: "Gris",
                numeroSerie: "SN005",
                kilometraje: 8000,
                tipoCombustible: 1,
                tipoTransmision: 1,
                numeroAsientos: 5,
                capacidadMaletero: 395,
                tipoVehiculo: 1,
                estado: 1,
                precioPorDia: 46.00m,
                activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new VehiculoService(context);

        // Act
        var exists = await service.Existe(5);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task Existe_CuandoNoExisteVehiculo_RetornaFalse()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new VehiculoService(context);

        // Act
        var exists = await service.Existe(77);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task Modificar_CuandoVehiculoExiste_ActualizaDatos()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Vehiculos.Add(CreateVehiculo(
                id: 20,
                marca: "Chevrolet",
                modelo: "Cruze",
                año: 2022,
                placa: "CHV-200",
                color: "Blanco",
                numeroSerie: "SN020",
                kilometraje: 25000,
                tipoCombustible: 1,
                tipoTransmision: 1,
                numeroAsientos: 5,
                capacidadMaletero: 430,
                tipoVehiculo: 1,
                estado: 1,
                precioPorDia: 44.00m,
                activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new VehiculoService(context);
        var updated = CreateVehiculo(
            id: 20,
            marca: "Chevrolet",
            modelo: "Cruze",
            año: 2022,
            placa: "CHV-200",
            color: "Negro",
            numeroSerie: "SN020",
            kilometraje: 26000,
            tipoCombustible: 1,
            tipoTransmision: 1,
            numeroAsientos: 5,
            capacidadMaletero: 430,
            tipoVehiculo: 1,
            estado: 2,
            precioPorDia: 44.00m,
            activo: true,
            fechaRegistro: new DateTime(2020, 1, 1));

        // Act
        var wasUpdated = await service.Modificar(updated);

        // Assert
        Assert.True(wasUpdated);

        var saved = await context.Vehiculos.FirstOrDefaultAsync(v => v.Id == 20);
        Assert.NotNull(saved);
        Assert.Equal("Negro", saved!.Color);
        Assert.Equal(26000, saved.Kilometraje);
        Assert.Equal(2, saved.Estado);
    }

    [Fact]
    public async Task Guardar_CuandoVehiculoNoExiste_InsertaYRetornaTrue()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new VehiculoService(context);
        var newVehiculo = CreateVehiculo(
            id: 30,
            marca: "Kia",
            modelo: "Forte",
            año: 2024,
            placa: "KIA-300",
            color: "Verde",
            numeroSerie: "SN030",
            kilometraje: 100,
            tipoCombustible: 1,
            tipoTransmision: 1,
            numeroAsientos: 5,
            capacidadMaletero: 415,
            tipoVehiculo: 1,
            estado: 1,
            precioPorDia: 47.00m,
            activo: true,
            fechaRegistro: default);

        // Act
        var result = await service.Guardar(newVehiculo);

        // Assert
        Assert.True(result);

        var saved = await context.Vehiculos.FirstOrDefaultAsync(v => v.Id == 30);
        Assert.NotNull(saved);
        Assert.Equal("KIA-300", saved!.Placa);
        Assert.True(saved.FechaRegistro != default);
    }

    [Fact]
    public async Task Guardar_CuandoVehiculoExiste_ModificaYRetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Vehiculos.Add(CreateVehiculo(
                id: 40,
                marca: "Nissan",
                modelo: "Sentra",
                año: 2023,
                placa: "NIS-400",
                color: "Plateado",
                numeroSerie: "SN040",
                kilometraje: 12000,
                tipoCombustible: 1,
                tipoTransmision: 1,
                numeroAsientos: 5,
                capacidadMaletero: 405,
                tipoVehiculo: 1,
                estado: 1,
                precioPorDia: 43.00m,
                activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new VehiculoService(context);
        var updated = CreateVehiculo(
            id: 40,
            marca: "Nissan",
            modelo: "Sentra",
            año: 2023,
            placa: "NIS-400",
            color: "Plateado",
            numeroSerie: "SN040",
            kilometraje: 13000,
            tipoCombustible: 1,
            tipoTransmision: 1,
            numeroAsientos: 5,
            capacidadMaletero: 405,
            tipoVehiculo: 1,
            estado: 2,
            precioPorDia: 43.00m,
            activo: false,
            fechaRegistro: new DateTime(2020, 1, 1));

        // Act
        var result = await service.Guardar(updated);

        // Assert
        Assert.True(result);

        var saved = await context.Vehiculos.FirstOrDefaultAsync(v => v.Id == 40);
        Assert.NotNull(saved);
        Assert.Equal(2, saved!.Estado);
        Assert.False(saved.Activo);
    }

    [Fact]
    public async Task Eliminar_CuandoExisteVehiculo_EliminaYRetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Vehiculos.Add(CreateVehiculo(
                id: 50,
                marca: "Volkswagen",
                modelo: "Jetta",
                año: 2022,
                placa: "VW-500",
                color: "Rojo",
                numeroSerie: "SN050",
                kilometraje: 30000,
                tipoCombustible: 1,
                tipoTransmision: 1,
                numeroAsientos: 5,
                capacidadMaletero: 425,
                tipoVehiculo: 1,
                estado: 1,
                precioPorDia: 45.00m,
                activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new VehiculoService(context);

        // Act
        var result = await service.Eliminar(50);

        // Assert
        Assert.True(result);
        var saved = await context.Vehiculos.FirstOrDefaultAsync(v => v.Id == 50);
        Assert.Null(saved);
    }

    [Fact]
    public async Task Eliminar_CuandoNoExisteVehiculo_RetornaFalse()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new VehiculoService(context);

        // Act
        var result = await service.Eliminar(999);

        // Assert
        Assert.False(result);
    }

    private static VehiculoEntity CreateVehiculo(
        int id,
        string marca,
        string modelo,
        int año,
        string placa,
        int kilometraje,
        int tipoCombustible,
        int tipoTransmision,
        int numeroAsientos,
        int capacidadMaletero,
        int tipoVehiculo,
        int estado,
        decimal precioPorDia,
        bool activo,
        string? color = null,
        string? numeroSerie = null,
        string? imagenPrincipal = null,
        string? descripcion = null,
        string? caracteristicasExtras = null,
        int? idSucursal = null,
        DateTime? fechaRegistro = null)
    {
        return new VehiculoEntity
        {
            Id = id,
            Marca = marca,
            Modelo = modelo,
            Año = año,
            Placa = placa,
            Color = color,
            NumeroSerie = numeroSerie,
            Kilometraje = kilometraje,
            TipoCombustible = tipoCombustible,
            TipoTransmision = tipoTransmision,
            NumeroAsientos = numeroAsientos,
            CapacidadMaletero = capacidadMaletero,
            TipoVehiculo = tipoVehiculo,
            Estado = estado,
            PrecioPorDia = precioPorDia,
            ImagenPrincipal = imagenPrincipal,
            Descripcion = descripcion,
            CaracteristicasExtras = caracteristicasExtras,
            IdSucursal = idSucursal,
            FechaRegistro = fechaRegistro ?? default,
            Activo = activo
        };
    }
}
