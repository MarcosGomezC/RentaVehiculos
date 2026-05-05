using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Data.Context;
using RentaVehiculo.Data.Models;
using System.Linq.Expressions;

namespace RentaVehiculo.UI.Services;

public class VehiculoService(RentaVehiculosContext context) : IService<Vehiculo, int>
{
    public Task<Vehiculo?> Buscar(int id) =>
        context.Vehiculos.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

    public async Task<bool> Eliminar(int id)
    {
        var entity = await context.Vehiculos.FindAsync(id);
        if (entity is null)
            return false;
        context.Vehiculos.Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<List<Vehiculo>> GetList(Expression<Func<Vehiculo, bool>> criterio) =>
        await context.Vehiculos.AsNoTracking().Where(criterio).ToListAsync();

    public async Task<bool> Insertar(Vehiculo entity)
    {
        if (entity.FechaRegistro == default)
            entity.FechaRegistro = DateTime.Now;
        context.Vehiculos.Add(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Vehiculo entity) =>
        !await Existe(entity.Id) ? await Insertar(entity) : await Modificar(entity);

    public Task<bool> Existe(int id) => context.Vehiculos.AnyAsync(v => v.Id == id);

    public async Task<bool> Modificar(Vehiculo entity)
    {
        context.Vehiculos.Update(entity);
        return await context.SaveChangesAsync() > 0;
    }
}
