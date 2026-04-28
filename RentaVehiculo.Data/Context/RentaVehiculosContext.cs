using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RentaVehiculo.Data.Models;

namespace RentaVehiculo.Data.Context;

public partial class RentaVehiculosContext : DbContext
{
    public RentaVehiculosContext()
    {
    }

    public RentaVehiculosContext(DbContextOptions<RentaVehiculosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<ConfiguracionRentum> ConfiguracionRenta { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Mantenimiento> Mantenimientos { get; set; }

    public virtual DbSet<Renta> Rentas { get; set; }

    public virtual DbSet<RentaExtra> RentaExtras { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Sucursale> Sucursales { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    public virtual DbSet<VehiculoCaracteristica> VehiculoCaracteristicas { get; set; }

    public virtual DbSet<VehiculoImagene> VehiculoImagenes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MARCOSGOMEZ\\SQLEXPRESS;Database=RentaVehiculos;User Id=marcos;Password=marcos358;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Clientes).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasOne(d => d.IdRentaNavigation).WithMany(p => p.Facturas).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Mantenimiento>(entity =>
        {
            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Mantenimientos).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Renta>(entity =>
        {
            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Renta).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Renta).OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Renta).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.SucursalEntregaNavigation).WithMany(p => p.RentaSucursalEntregaNavigations).OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.SucursalRecogidaNavigation).WithMany(p => p.RentaSucursalRecogidaNavigations).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Reservas).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Reservas).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Usuarios).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasIndex(e => e.NumeroSerie, "IX_Vehiculos_NumeroSerie")
                .IsUnique()
                .HasFilter("([NumeroSerie] IS NOT NULL)");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Vehiculos).OnDelete(DeleteBehavior.SetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
