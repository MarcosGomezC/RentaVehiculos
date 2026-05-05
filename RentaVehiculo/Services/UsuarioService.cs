using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Data.Context;
using System.Linq.Expressions;
using UsuarioEntity = RentaVehiculo.Data.Models.Usuario;

namespace RentaVehiculo.UI.Services;

public class UsuarioService(RentaVehiculosContext context) : IService<UsuarioEntity, int>
{
    public Task<UsuarioEntity?> Buscar(int id)
    {
        return context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> Eliminar(int id)
    {
        var usuario = await context.Usuarios.FindAsync(id);
        if (usuario is null)
            return false;

        context.Usuarios.Remove(usuario);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<List<UsuarioEntity>> GetList(Expression<Func<UsuarioEntity, bool>> criterio)
    {
        return await context.Usuarios
            .AsNoTracking()
            .Where(criterio)
            .ToListAsync();
    }

    public async Task<bool> Insertar(UsuarioEntity usuario)
    {
        usuario.FechaCreacion = DateTime.Now;
        context.Usuarios.Add(usuario);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(UsuarioEntity usuario)
    {
        if (!await Existe(usuario.Id))
            return await Insertar(usuario);
        else
            return await Modificar(usuario);
    }

    public async Task<bool> Existe(int id)
    {
        return await context.Usuarios.AnyAsync(u => u.Id == id);
    }

    public async Task<bool> Modificar(UsuarioEntity usuario)
    {
        context.Usuarios.Update(usuario);
        return await context.SaveChangesAsync() > 0;
    }
}
