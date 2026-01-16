using Microsoft.EntityFrameworkCore;
using NayeliApi.Core.DTOs;
using NayeliApi.Core.Entities;

namespace NayeliApi.Infrastructure.Data;

public class BancoDbContext : DbContext
{
    public BancoDbContext(DbContextOptions<BancoDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<CuentaBancaria> CuentasBancarias { get; set; }
    public DbSet<Transaccion> Transacciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Sexo).IsRequired().HasMaxLength(50);
            entity.Property(e => e.FechaNacimiento).IsRequired();
            entity.Property(e => e.Ingresos).HasPrecision(18, 2);
        });

        modelBuilder.Entity<CuentaBancaria>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NumeroCuenta).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.NumeroCuenta).IsUnique(); //Creamos un indice unico para el numero de cuenta
            entity.Property(e => e.SaldoActual).HasPrecision(18, 2);

            entity.HasOne(e => e.Cliente)
                .WithMany(c => c.Cuentas)
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina un cliente, se eliminan sus cuentas bancarias
        });

        modelBuilder.Entity<Transaccion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Monto).HasPrecision(18, 2);
            entity.Property(e => e.SaldoDespues).HasPrecision(18, 2);
            entity.Property(e => e.Fecha).IsRequired();
            entity.Property(e => e.Tipo).IsRequired();

            entity.HasOne(e => e.CuentaBancaria)
                .WithMany(c => c.Transacciones)
                .HasForeignKey(e => e.CuentaBancariaId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
