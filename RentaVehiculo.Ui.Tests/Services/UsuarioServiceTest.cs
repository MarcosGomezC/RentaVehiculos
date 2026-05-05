using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Ui.Tests.Infrastructure;
using RentaVehiculo.UI.Services;
using UsuarioEntity = RentaVehiculo.Data.Models.Usuario;

namespace RentaVehiculo.Ui.Tests.Services;

public class UsuarioServiceTest
{
    [Fact]
    public async Task Buscar_CuandoExisteUsuario_RetornaEntidad()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Usuarios.Add(CreateUsuario(
                id: 1,
                nombre: "Marco",
                apellido: "Gomez",
                nombreUsuario: "marco",
                email: "marco@correo.com",
                rol: "Admin",
                activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new UsuarioService(context);

        // Act
        var result = await service.Buscar(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
        Assert.Equal("marco", result.NombreUsuario);
        Assert.Empty(context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task Buscar_CuandoNoExisteUsuario_RetornaNull()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new UsuarioService(context);

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
            seedContext.Usuarios.AddRange(
                CreateUsuario(id: 1, nombre: "A", apellido: "A", nombreUsuario: "a", email: "a@correo.com", rol: "Admin", activo: true),
                CreateUsuario(id: 2, nombre: "B", apellido: "B", nombreUsuario: "b", email: "b@correo.com", rol: "Empleado", activo: false),
                CreateUsuario(id: 3, nombre: "C", apellido: "C", nombreUsuario: "c", email: "c@correo.com", rol: "Empleado", activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new UsuarioService(context);

        // Act
        var result = await service.GetList(u => u.Activo);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.Id == 1);
        Assert.Contains(result, u => u.Id == 3);
        Assert.DoesNotContain(result, u => u.Id == 2);
    }

    [Fact]
    public async Task Insertar_CuandoUsuarioEsValido_GeneraFechaCreacionYAlmacena()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new UsuarioService(context);
        var usuario = CreateUsuario(
            id: 10,
            nombre: "Luis",
            apellido: "Perez",
            nombreUsuario: "lperez",
            email: "luis@correo.com",
            rol: "Empleado",
            activo: true,
            fechaCreacion: default);
        var beforeInsert = DateTime.Now;

        // Act
        var wasInserted = await service.Insertar(usuario);

        // Assert
        Assert.True(wasInserted);
        Assert.True(usuario.FechaCreacion >= beforeInsert);

        var saved = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == 10);
        Assert.NotNull(saved);
        Assert.Equal("lperez", saved!.NombreUsuario);
    }

    [Fact]
    public async Task Existe_CuandoExisteUsuario_RetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Usuarios.Add(CreateUsuario(
                id: 5,
                nombre: "Ana",
                apellido: "Diaz",
                nombreUsuario: "adiaz",
                email: "ana@correo.com",
                rol: "Admin",
                activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new UsuarioService(context);

        // Act
        var exists = await service.Existe(5);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task Existe_CuandoNoExisteUsuario_RetornaFalse()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new UsuarioService(context);

        // Act
        var exists = await service.Existe(77);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task Modificar_CuandoUsuarioExiste_ActualizaDatos()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Usuarios.Add(CreateUsuario(
                id: 20,
                nombre: "Juan",
                apellido: "Lopez",
                nombreUsuario: "jlopez",
                email: "juan@correo.com",
                rol: "Empleado",
                activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new UsuarioService(context);
        var updated = CreateUsuario(
            id: 20,
            nombre: "Juan Carlos",
            apellido: "Lopez",
            nombreUsuario: "jlopez",
            email: "juan@correo.com",
            rol: "Admin",
            activo: true,
            fechaCreacion: new DateTime(2020, 1, 1));

        // Act
        var wasUpdated = await service.Modificar(updated);

        // Assert
        Assert.True(wasUpdated);

        var saved = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == 20);
        Assert.NotNull(saved);
        Assert.Equal("Juan Carlos", saved!.Nombre);
        Assert.Equal("Admin", saved.Rol);
    }

    [Fact]
    public async Task Guardar_CuandoUsuarioNoExiste_InsertaYRetornaTrue()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new UsuarioService(context);
        var newUsuario = CreateUsuario(
            id: 30,
            nombre: "Marta",
            apellido: "Reyes",
            nombreUsuario: "mreyes",
            email: "marta@correo.com",
            rol: "Empleado",
            activo: true,
            fechaCreacion: default);

        // Act
        var result = await service.Guardar(newUsuario);

        // Assert
        Assert.True(result);

        var saved = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == 30);
        Assert.NotNull(saved);
        Assert.Equal("mreyes", saved!.NombreUsuario);
        Assert.True(saved.FechaCreacion != default);
    }

    [Fact]
    public async Task Guardar_CuandoUsuarioExiste_ModificaYRetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Usuarios.Add(CreateUsuario(
                id: 40,
                nombre: "Pedro",
                apellido: "Santos",
                nombreUsuario: "psantos",
                email: "pedro@correo.com",
                rol: "Empleado",
                activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new UsuarioService(context);
        var updated = CreateUsuario(
            id: 40,
            nombre: "Pedro",
            apellido: "Santos",
            nombreUsuario: "psantos",
            email: "pedro@correo.com",
            rol: "Admin",
            activo: false,
            fechaCreacion: new DateTime(2020, 1, 1));

        // Act
        var result = await service.Guardar(updated);

        // Assert
        Assert.True(result);

        var saved = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == 40);
        Assert.NotNull(saved);
        Assert.Equal("Admin", saved!.Rol);
        Assert.False(saved.Activo);
    }

    [Fact]
    public async Task Eliminar_CuandoExisteUsuario_EliminaYRetornaTrue()
    {
        // Arrange
        var dbName = TestDbContextFactory.NewDatabaseName();
        await using (var seedContext = TestDbContextFactory.CreateContext(dbName))
        {
            seedContext.Usuarios.Add(CreateUsuario(
                id: 50,
                nombre: "Sara",
                apellido: "Mena",
                nombreUsuario: "smena",
                email: "sara@correo.com",
                rol: "Empleado",
                activo: true));
            await seedContext.SaveChangesAsync();
        }

        await using var context = TestDbContextFactory.CreateContext(dbName);
        var service = new UsuarioService(context);

        // Act
        var result = await service.Eliminar(50);

        // Assert
        Assert.True(result);
        var saved = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == 50);
        Assert.Null(saved);
    }

    [Fact]
    public async Task Eliminar_CuandoNoExisteUsuario_RetornaFalse()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateContext(TestDbContextFactory.NewDatabaseName());
        var service = new UsuarioService(context);

        // Act
        var result = await service.Eliminar(999);

        // Assert
        Assert.False(result);
    }

    private static UsuarioEntity CreateUsuario(
        int id,
        string nombre,
        string apellido,
        string nombreUsuario,
        string email,
        string rol,
        bool activo,
        string? passwordHash = null,
        DateTime? fechaCreacion = null,
        int? idSucursal = null)
    {
        return new UsuarioEntity
        {
            Id = id,
            Nombre = nombre,
            Apellido = apellido,
            NombreUsuario = nombreUsuario,
            Email = email,
            PasswordHash = passwordHash ?? "hash",
            Rol = rol,
            Activo = activo,
            FechaCreacion = fechaCreacion ?? DateTime.Now,
            IdSucursal = idSucursal
        };
    }
}
