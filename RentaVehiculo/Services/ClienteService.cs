using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Data.Context;
using RentaVehiculo.Data.Models;
using System.Linq.Expressions;

namespace RentaVehiculo.UI.Services;

public class ClienteService(RentaVehiculosContext context) : IService<Cliente, int>
{
    public Task<Cliente?> Buscar(int id) =>
        context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> Eliminar(int id)
    {
        var entity = await context.Clientes.FindAsync(id);
        if (entity is null)
            return false;
        context.Clientes.Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<List<Cliente>> GetList(Expression<Func<Cliente, bool>> criterio) =>
        await context.Clientes.AsNoTracking().Where(criterio).ToListAsync();

    public async Task<bool> Insertar(Cliente entity)
    {
        if (entity.FechaRegistro == default)
            entity.FechaRegistro = DateTime.Now;
        context.Clientes.Add(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Cliente entity) =>
        !await Existe(entity.Id) ? await Insertar(entity) : await Modificar(entity);

    public Task<bool> Existe(int id) => context.Clientes.AnyAsync(c => c.Id == id);

    public async Task<bool> Modificar(Cliente entity)
    {
        context.Clientes.Update(entity);
        return await context.SaveChangesAsync() > 0;
    }
}
