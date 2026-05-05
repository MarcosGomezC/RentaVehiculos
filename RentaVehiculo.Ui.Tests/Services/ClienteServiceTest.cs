using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Ui.Tests.Infrastructure;
using RentaVehiculo.UI.Services;
using ClienteEntity = RentaVehiculo.Data.Models.Cliente;

namespace RentaVehiculo.Ui.Tests.Services;

public class ClienteServiceTest
{
    [Fact]
    public async Task Buscar_CuandoExisteCliente_RetornaEntidad()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Clientes.Add(CreateCliente(
                id: 1,
                nombre: "Marco",
                apellido: "Gomez",
                email: "marco@correo.com",
                telefono: "1234567890",
                licencia: "ABC123",
                fechaVencimientoLicencia: new DateTime(2025, 12, 31),
                fechaNacimiento: new DateTime(1990, 5, 15),
                tipoCliente: 1,
                activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ClienteService(context);

        // Act
        var result = await service.Buscar(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
        Assert.Equal("marco@correo.com", result.Email);
        Assert.Empty(context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task Buscar_CuandoNoExisteCliente_RetornaNull()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new ClienteService(context);

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
            seedContext.Clientes.AddRange(
                CreateCliente(id: 1, nombre: "A", apellido: "A", email: "a@correo.com", telefono: "111", licencia: "AAA111", 
                    fechaVencimientoLicencia: new DateTime(2025, 12, 31), fechaNacimiento: new DateTime(1990, 1, 1), tipoCliente: 1, activo: true),
                CreateCliente(id: 2, nombre: "B", apellido: "B", email: "b@correo.com", telefono: "222", licencia: "BBB222", 
                    fechaVencimientoLicencia: new DateTime(2025, 12, 31), fechaNacimiento: new DateTime(1991, 2, 2), tipoCliente: 1, activo: false),
                CreateCliente(id: 3, nombre: "C", apellido: "C", email: "c@correo.com", telefono: "333", licencia: "CCC333", 
                    fechaVencimientoLicencia: new DateTime(2025, 12, 31), fechaNacimiento: new DateTime(1992, 3, 3), tipoCliente: 1, activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ClienteService(context);

        // Act
        var result = await service.GetList(c => c.Activo);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Id == 1);
        Assert.Contains(result, c => c.Id == 3);
        Assert.DoesNotContain(result, c => c.Id == 2);
    }

    [Fact]
    public async Task Insertar_CuandoClienteEsValido_GeneraFechaRegistroYAlmacena()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new ClienteService(context);
        var cliente = CreateCliente(
            id: 10,
            nombre: "Luis",
            apellido: "Perez",
            email: "luis@correo.com",
            telefono: "9876543210",
            licencia: "XYZ789",
            fechaVencimientoLicencia: new DateTime(2026, 6, 30),
            fechaNacimiento: new DateTime(1985, 7, 20),
            tipoCliente: 2,
            activo: true,
            fechaRegistro: default);
        var beforeInsert = DateTime.Now;

        // Act
        var wasInserted = await service.Insertar(cliente);

        // Assert
        Assert.True(wasInserted);
        Assert.True(cliente.FechaRegistro >= beforeInsert);

        var saved = await context.Clientes.FirstOrDefaultAsync(c => c.Id == 10);
        Assert.NotNull(saved);
        Assert.Equal("luis@correo.com", saved!.Email);
    }

    [Fact]
    public async Task Existe_CuandoExisteCliente_RetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Clientes.Add(CreateCliente(
                id: 5,
                nombre: "Ana",
                apellido: "Diaz",
                email: "ana@correo.com",
                telefono: "5555555555",
                licencia: "ANA555",
                fechaVencimientoLicencia: new DateTime(2025, 9, 30),
                fechaNacimiento: new DateTime(1988, 3, 10),
                tipoCliente: 1,
                activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ClienteService(context);

        // Act
        var exists = await service.Existe(5);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task Existe_CuandoNoExisteCliente_RetornaFalse()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new ClienteService(context);

        // Act
        var exists = await service.Existe(77);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task Modificar_CuandoClienteExiste_ActualizaDatos()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Clientes.Add(CreateCliente(
                id: 20,
                nombre: "Juan",
                apellido: "Lopez",
                email: "juan@correo.com",
                telefono: "1111111111",
                licencia: "JLO111",
                fechaVencimientoLicencia: new DateTime(2025, 8, 15),
                fechaNacimiento: new DateTime(1987, 4, 22),
                tipoCliente: 1,
                activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ClienteService(context);
        var updated = CreateCliente(
            id: 20,
            nombre: "Juan Carlos",
            apellido: "Lopez",
            email: "juan@correo.com",
            telefono: "1111111111",
            licencia: "JLO111",
            fechaVencimientoLicencia: new DateTime(2025, 8, 15),
            fechaNacimiento: new DateTime(1987, 4, 22),
            tipoCliente: 2,
            activo: true,
            fechaRegistro: new DateTime(2020, 1, 1));

        // Act
        var wasUpdated = await service.Modificar(updated);

        // Assert
        Assert.True(wasUpdated);

        var saved = await context.Clientes.FirstOrDefaultAsync(c => c.Id == 20);
        Assert.NotNull(saved);
        Assert.Equal("Juan Carlos", saved!.Nombre);
        Assert.Equal(2, saved.TipoCliente);
    }

    [Fact]
    public async Task Guardar_CuandoClienteNoExiste_InsertaYRetornaTrue()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new ClienteService(context);
        var newCliente = CreateCliente(
            id: 30,
            nombre: "Marta",
            apellido: "Reyes",
            email: "marta@correo.com",
            telefono: "3333333333",
            licencia: "MAR333",
            fechaVencimientoLicencia: new DateTime(2026, 1, 31),
            fechaNacimiento: new DateTime(1992, 6, 5),
            tipoCliente: 1,
            activo: true,
            fechaRegistro: default);

        // Act
        var result = await service.Guardar(newCliente);

        // Assert
        Assert.True(result);

        var saved = await context.Clientes.FirstOrDefaultAsync(c => c.Id == 30);
        Assert.NotNull(saved);
        Assert.Equal("marta@correo.com", saved!.Email);
        Assert.True(saved.FechaRegistro != default);
    }

    [Fact]
    public async Task Guardar_CuandoClienteExiste_ModificaYRetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Clientes.Add(CreateCliente(
                id: 40,
                nombre: "Pedro",
                apellido: "Santos",
                email: "pedro@correo.com",
                telefono: "4444444444",
                licencia: "PED444",
                fechaVencimientoLicencia: new DateTime(2025, 11, 30),
                fechaNacimiento: new DateTime(1989, 9, 12),
                tipoCliente: 1,
                activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ClienteService(context);
        var updated = CreateCliente(
            id: 40,
            nombre: "Pedro",
            apellido: "Santos",
            email: "pedro@correo.com",
            telefono: "4444444444",
            licencia: "PED444",
            fechaVencimientoLicencia: new DateTime(2025, 11, 30),
            fechaNacimiento: new DateTime(1989, 9, 12),
            tipoCliente: 2,
            activo: false,
            fechaRegistro: new DateTime(2020, 1, 1));

        // Act
        var result = await service.Guardar(updated);

        // Assert
        Assert.True(result);

        var saved = await context.Clientes.FirstOrDefaultAsync(c => c.Id == 40);
        Assert.NotNull(saved);
        Assert.Equal(2, saved!.TipoCliente);
        Assert.False(saved.Activo);
    }

    [Fact]
    public async Task Eliminar_CuandoExisteCliente_EliminaYRetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Clientes.Add(CreateCliente(
                id: 50,
                nombre: "Sara",
                apellido: "Mena",
                email: "sara@correo.com",
                telefono: "5555555555",
                licencia: "SAR555",
                fechaVencimientoLicencia: new DateTime(2026, 3, 15),
                fechaNacimiento: new DateTime(1994, 8, 8),
                tipoCliente: 1,
                activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new ClienteService(context);

        // Act
        var result = await service.Eliminar(50);

        // Assert
        Assert.True(result);
        var saved = await context.Clientes.FirstOrDefaultAsync(c => c.Id == 50);
        Assert.Null(saved);
    }

    [Fact]
    public async Task Eliminar_CuandoNoExisteCliente_RetornaFalse()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new ClienteService(context);

        // Act
        var result = await service.Eliminar(999);

        // Assert
        Assert.False(result);
    }

    private static ClienteEntity CreateCliente(
        int id,
        string nombre,
        string apellido,
        string email,
        string telefono,
        string licencia,
        DateTime fechaVencimientoLicencia,
        DateTime fechaNacimiento,
        int tipoCliente,
        bool activo,
        string? direccion = null,
        string? ciudad = null,
        string? codigoPostal = null,
        DateTime? fechaRegistro = null,
        int? idUsuario = null)
    {
        return new ClienteEntity
        {
            Id = id,
            Nombre = nombre,
            Apellido = apellido,
            Email = email,
            Telefono = telefono,
            Licencia = licencia,
            FechaVencimientoLicencia = fechaVencimientoLicencia,
            FechaNacimiento = fechaNacimiento,
            Direccion = direccion,
            Ciudad = ciudad,
            CodigoPostal = codigoPostal,
            TipoCliente = tipoCliente,
            FechaRegistro = fechaRegistro ?? default,
            Activo = activo
        };
    }
}
