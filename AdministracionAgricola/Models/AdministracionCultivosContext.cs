using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdministracionAgricola.Models;

public partial class AdministracionCultivosContext : DbContext
{
    public AdministracionCultivosContext()
    {
    }

    public AdministracionCultivosContext(DbContextOptions<AdministracionCultivosContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<CategoriaPlantum> CategoriaPlanta { get; set; }

    public virtual DbSet<Clima> Climas { get; set; }

    public virtual DbSet<ControlPerdidum> ControlPerdida { get; set; }

    public virtual DbSet<Insumo> Insumos { get; set; }

    public virtual DbSet<Lote> Lotes { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Produccion> Produccions { get; set; }

    public virtual DbSet<RegistroPlantum> RegistroPlanta { get; set; }

    public virtual DbSet<Riego> Riegos { get; set; }

    public virtual DbSet<Semilla> Semillas { get; set; }

    public virtual DbSet<Siembra> Siembras { get; set; }

    public virtual DbSet<UnidadMedidum> UnidadMedida { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<CategoriaPlantum>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__CD54BC5A17D3D87D");

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.IdPersona).HasColumnName("id_persona");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre_categoria");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.CategoriaPlanta)
                .HasForeignKey(d => d.IdPersona)
                .HasConstraintName("FK_Persona");
        });

        modelBuilder.Entity<Clima>(entity =>
        {
            entity.HasKey(e => e.IdClima).HasName("PK__Clima__2573BC9F61EA05D4");

            entity.ToTable("Clima");

            entity.Property(e => e.IdClima).HasColumnName("id_clima");
            entity.Property(e => e.DescripcionCondiciones)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion_condiciones");
            entity.Property(e => e.EstacionAnio)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estacion_anio");
            entity.Property(e => e.IdUnidadMedida).HasColumnName("id_unidad_medida");
            entity.Property(e => e.Temperatura).HasColumnName("temperatura");

            entity.HasOne(d => d.IdUnidadMedidaNavigation).WithMany(p => p.Climas)
                .HasForeignKey(d => d.IdUnidadMedida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Clima__id_unidad__5AEE82B9");
        });

        modelBuilder.Entity<ControlPerdidum>(entity =>
        {
            entity.HasKey(e => e.IdControlPerdida).HasName("PK__Control___6044163FE1C9F1C5");

            entity.ToTable("Control_Perdida");

            entity.Property(e => e.IdControlPerdida).HasColumnName("id_control_perdida");
            entity.Property(e => e.CantidadPerdida).HasColumnName("cantidad_perdida");
            entity.Property(e => e.CausaPlaga)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("causa_plaga");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaPerdida)
                .HasColumnType("datetime")
                .HasColumnName("fecha_perdida");
            entity.Property(e => e.IdSiembra).HasColumnName("id_siembra");
            entity.Property(e => e.IdUnidadMedida).HasColumnName("id_unidad_medida");

            entity.HasOne(d => d.IdSiembraNavigation).WithMany(p => p.ControlPerdida)
                .HasForeignKey(d => d.IdSiembra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Control_P__id_si__66603565");

            entity.HasOne(d => d.IdUnidadMedidaNavigation).WithMany(p => p.ControlPerdida)
                .HasForeignKey(d => d.IdUnidadMedida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Control_P__id_un__6754599E");
        });

        modelBuilder.Entity<Insumo>(entity =>
        {
            entity.HasKey(e => e.IdInsumo).HasName("PK__Insumo__D4F202B1D4DCA9E8");

            entity.ToTable("Insumo");

            entity.Property(e => e.IdInsumo).HasColumnName("id_insumo");
            entity.Property(e => e.CantidadInsumo).HasColumnName("cantidad_insumo");
            entity.Property(e => e.FechaUso)
                .HasColumnType("datetime")
                .HasColumnName("fecha_uso");
            entity.Property(e => e.IdLote).HasColumnName("id_lote");
            entity.Property(e => e.IdUnidadMedida).HasColumnName("id_unidad_medida");
            entity.Property(e => e.NombreInsumo)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("nombre_insumo");
            entity.Property(e => e.PrecioTotal).HasColumnName("precio_total");
            entity.Property(e => e.PrecioUnitario).HasColumnName("precio_unitario");
            entity.Property(e => e.TipoInsumo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo_insumo");

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.Insumos)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Insumo__id_lote__71D1E811");

            entity.HasOne(d => d.IdUnidadMedidaNavigation).WithMany(p => p.Insumos)
                .HasForeignKey(d => d.IdUnidadMedida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Insumo__id_lote__70DDC3D8");
        });

        modelBuilder.Entity<Lote>(entity =>
        {
            entity.HasKey(e => e.IdLote).HasName("PK__Lote__9A000486443746BB");

            entity.ToTable("Lote");

            entity.Property(e => e.IdLote).HasColumnName("id_lote");
            entity.Property(e => e.Area).HasColumnName("area");
            entity.Property(e => e.CodigoUbicacion)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("codigo_ubicacion");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("PK__Persona__228148B08E8F3301");

            entity.ToTable("Persona");

            entity.Property(e => e.IdPersona).HasColumnName("id_persona");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Dni).HasColumnName("dni");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PkAspNetUsers)
                .HasMaxLength(450)
                .HasColumnName("PK_AspNetUsers");

            entity.HasOne(d => d.PkAspNetUsersNavigation).WithMany(p => p.Personas)
                .HasForeignKey(d => d.PkAspNetUsers)
                .HasConstraintName("FK__Persona__PK_AspN__31B762FC");
        });

        modelBuilder.Entity<Produccion>(entity =>
        {
            entity.HasKey(e => e.IdProduccion).HasName("PK__Producci__9EBBA43392CB4BF3");

            entity.ToTable("Produccion");

            entity.Property(e => e.IdProduccion).HasColumnName("id_produccion");
            entity.Property(e => e.CantidadProduccion).HasColumnName("cantidad_produccion");
            entity.Property(e => e.FechaCosecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha_cosecha");
            entity.Property(e => e.IdLote).HasColumnName("id_lote");
            entity.Property(e => e.IdUnidadMedida).HasColumnName("id_unidad_medida");
            entity.Property(e => e.TipoProducto)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo_producto");

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.Produccions)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Produccio__id_lo__6D0D32F4");

            entity.HasOne(d => d.IdUnidadMedidaNavigation).WithMany(p => p.Produccions)
                .HasForeignKey(d => d.IdUnidadMedida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Produccio__id_un__6E01572D");
        });

        modelBuilder.Entity<RegistroPlantum>(entity =>
        {
            entity.HasKey(e => e.IdRegistroPlanta).HasName("PK__Registro__29E8E417026AF64B");

            entity.Property(e => e.IdRegistroPlanta).HasColumnName("id_registro_planta");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.NombrePlanta)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre_planta");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.RegistroPlanta)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RegistroP__id_ca__5441852A");
        });

        modelBuilder.Entity<Riego>(entity =>
        {
            entity.HasKey(e => e.IdRiego).HasName("PK__Riego__41B02AA6A864A057");

            entity.ToTable("Riego");

            entity.Property(e => e.IdRiego).HasColumnName("id_riego");
            entity.Property(e => e.FechaRiego)
                .HasColumnType("datetime")
                .HasColumnName("fecha_riego");
            entity.Property(e => e.IdLote).HasColumnName("id_lote");
            entity.Property(e => e.TipoRiego)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("tipo_riego");

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.Riegos)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Riego__id_lote__6A30C649");
        });

        modelBuilder.Entity<Semilla>(entity =>
        {
            entity.HasKey(e => e.IdSemilla).HasName("PK__Semilla__9740208C3BD6224B");

            entity.ToTable("Semilla");

            entity.Property(e => e.IdSemilla).HasColumnName("id_semilla");
            entity.Property(e => e.CantidadSemilla).HasColumnName("cantidad_semilla");
            entity.Property(e => e.IdUnidadMedida).HasColumnName("id_unidad_medida");
            entity.Property(e => e.PrecioTotalSemilla).HasColumnName("precio_total_semilla");
            entity.Property(e => e.TipoSemilla)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo_semilla");

            entity.HasOne(d => d.IdUnidadMedidaNavigation).WithMany(p => p.Semillas)
                .HasForeignKey(d => d.IdUnidadMedida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Semilla__id_unid__5DCAEF64");
        });

        modelBuilder.Entity<Siembra>(entity =>
        {
            entity.HasKey(e => e.IdSiembra).HasName("PK__Siembra__361831DB05063CB0");

            entity.ToTable("Siembra");

            entity.Property(e => e.IdSiembra).HasColumnName("id_siembra");
            entity.Property(e => e.FechaSiembra)
                .HasColumnType("datetime")
                .HasColumnName("fecha_siembra");
            entity.Property(e => e.IdClima).HasColumnName("id_clima");
            entity.Property(e => e.IdLote).HasColumnName("id_lote");
            entity.Property(e => e.IdRegistroPlanta).HasColumnName("id_registro_planta");
            entity.Property(e => e.IdSemilla).HasColumnName("id_semilla");
            entity.Property(e => e.TipoSiembra)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("tipo_siembra");

            entity.HasOne(d => d.IdClimaNavigation).WithMany(p => p.Siembras)
                .HasForeignKey(d => d.IdClima)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Siembra__id_clim__628FA481");

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.Siembras)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Siembra__id_lote__60A75C0F");

            entity.HasOne(d => d.IdRegistroPlantaNavigation).WithMany(p => p.Siembras)
                .HasForeignKey(d => d.IdRegistroPlanta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Siembra__id_regi__619B8048");

            entity.HasOne(d => d.IdSemillaNavigation).WithMany(p => p.Siembras)
                .HasForeignKey(d => d.IdSemilla)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Siembra__id_semi__6383C8BA");
        });

        modelBuilder.Entity<UnidadMedidum>(entity =>
        {
            entity.HasKey(e => e.IdUnidadMedida).HasName("PK__Unidad_M__2F721BD3DAF1F064");

            entity.ToTable("Unidad_Medida");

            entity.Property(e => e.IdUnidadMedida).HasColumnName("id_unidad_medida");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.NombreMedida)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("nombre_medida");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
