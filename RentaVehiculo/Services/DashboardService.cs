using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Data.Context;

namespace RentaVehiculo.UI.Services;

public enum DashboardAlertaTipo
{
    Info,
    Advertencia,
    Peligro
}

public record DashboardAlertaItem(
    string Titulo,
    string Detalle,
    string Plazo,
    DashboardAlertaTipo Tipo);

public record DashboardEstadisticas(
    int VehiculosActivos,
    int VehiculosAltaEsteMes,
    int RentasProximasAVencer,
    int MantenimientosAbiertos,
    IReadOnlyList<DashboardAlertaItem> Alertas);

/// <summary>
/// Indicadores y alertas para el panel principal (consultas reales sobre la BD).
/// </summary>
public class DashboardService(RentaVehiculosContext context)
{
    public async Task<DashboardEstadisticas> ObtenerAsync(CancellationToken cancellationToken = default)
    {
        var hoy = DateTime.Today;
        var finVentana = hoy.AddDays(1);
        var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

        var vehActivos = await context.Vehiculos.CountAsync(v => v.Activo, cancellationToken);
        var vehEsteMes = await context.Vehiculos.CountAsync(v => v.FechaRegistro >= inicioMes, cancellationToken);

        // Rentas aún abiertas con devolución prevista hoy o mañana
        var rentasProximas = await context.Rentas.CountAsync(r =>
            !r.FechaFinReal.HasValue
            && r.FechaFinProgramada.Date >= hoy
            && r.FechaFinProgramada.Date <= finVentana, cancellationToken);

        // Trabajos de mantenimiento sin cerrar (fecha fin nula)
        var mantAbiertos = await context.Mantenimientos.CountAsync(m => m.FechaFin == null, cancellationToken);

        var alertas = new List<DashboardAlertaItem>();

        var mantRows = await (
            from m in context.Mantenimientos.AsNoTracking()
            join v in context.Vehiculos.AsNoTracking() on m.IdVehiculo equals v.Id
            where m.FechaInicio >= hoy && m.FechaInicio < hoy.AddDays(8)
            orderby m.FechaInicio
            select new { m, v.Marca, v.Modelo, v.Placa }
        ).Take(3).ToListAsync(cancellationToken);

        foreach (var x in mantRows)
        {
            var desc = string.IsNullOrWhiteSpace(x.m.Descripcion)
                ? $"{x.Marca} {x.Modelo} ({x.Placa})"
                : $"{x.Marca} {x.Modelo} - {x.m.Descripcion}";
            var plazo = x.m.FechaInicio.Date == hoy.AddDays(1) ? "Mañana" : x.m.FechaInicio.ToString("dd/MM HH:mm");
            alertas.Add(new DashboardAlertaItem(
                "Mantenimiento programado",
                desc,
                plazo,
                DashboardAlertaTipo.Info));
        }

        var rentaRows = await (
            from r in context.Rentas.AsNoTracking()
            join c in context.Clientes.AsNoTracking() on r.IdCliente equals c.Id
            join v in context.Vehiculos.AsNoTracking() on r.IdVehiculo equals v.Id
            where !r.FechaFinReal.HasValue
                  && r.FechaFinProgramada >= DateTime.Now.AddHours(-4)
                  && r.FechaFinProgramada <= DateTime.Now.AddDays(1)
            orderby r.FechaFinProgramada
            select new { r.FechaFinProgramada, c.Nombre, c.Apellido, v.Marca, v.Modelo }
        ).Take(3).ToListAsync(cancellationToken);

        foreach (var x in rentaRows)
        {
            var cuando = x.FechaFinProgramada <= DateTime.Now ? "En curso / vence ya" :
                x.FechaFinProgramada.Date == hoy ? "Hoy" : "Mañana";
            alertas.Add(new DashboardAlertaItem(
                "Renta por vencer",
                $"{x.Marca} {x.Modelo} — Cliente: {x.Nombre} {x.Apellido}",
                cuando,
                DashboardAlertaTipo.Advertencia));
        }

        var vencidas = await (
            from r in context.Rentas.AsNoTracking()
            join c in context.Clientes.AsNoTracking() on r.IdCliente equals c.Id
            join v in context.Vehiculos.AsNoTracking() on r.IdVehiculo equals v.Id
            where !r.FechaFinReal.HasValue && r.FechaFinProgramada < hoy
            orderby r.FechaFinProgramada
            select new { r.FechaFinProgramada, c.Nombre, c.Apellido, v.Marca, v.Modelo }
        ).Take(2).ToListAsync(cancellationToken);

        foreach (var x in vencidas)
        {
            var dias = (hoy - x.FechaFinProgramada.Date).Days;
            alertas.Add(new DashboardAlertaItem(
                "Renta vencida",
                $"{x.Marca} {x.Modelo} — Cliente: {x.Nombre} {x.Apellido}",
                dias <= 0 ? "Hoy" : $"Hace {dias} día(s)",
                DashboardAlertaTipo.Peligro));
        }

        var ordenadas = alertas
            .OrderByDescending(a => a.Tipo == DashboardAlertaTipo.Peligro ? 2 : a.Tipo == DashboardAlertaTipo.Advertencia ? 1 : 0)
            .ThenBy(a => a.Titulo)
            .Take(8)
            .ToList();

        return new DashboardEstadisticas(
            vehActivos,
            vehEsteMes,
            rentasProximas,
            mantAbiertos,
            ordenadas);
    }
}
