using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APIRESIDENCIAS.Models;

public partial class Sistem21ResidenciaswebcaContext : DbContext
{
    public Sistem21ResidenciaswebcaContext()
    {
    }

    public Sistem21ResidenciaswebcaContext(DbContextOptions<Sistem21ResidenciaswebcaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Archivosenviados> Archivosenviados { get; set; }

    public virtual DbSet<Asignaciontareas> Asignaciontareas { get; set; }

    public virtual DbSet<Carrera> Carrera { get; set; }

    public virtual DbSet<Coordinadores> Coordinadores { get; set; }

    public virtual DbSet<Inciarsesion> Inciarsesion { get; set; }

    public virtual DbSet<Residentes> Residentes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=sistemas19.com;database=sistem21_residenciaswebca;user=sistem21_residenciasca;password=sistemas19_", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.20-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Archivosenviados>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("archivosenviados");

            entity.HasIndex(e => e.IdResidente, "fkResidente_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Estatus)
                .HasColumnType("int(1)")
                .HasColumnName("estatus");
            entity.Property(e => e.FechaEnvio)
                .HasColumnType("datetime")
                .HasColumnName("fecha_envio");
            entity.Property(e => e.IdResidente)
                .HasColumnType("int(11)")
                .HasColumnName("idResidente");
            entity.Property(e => e.NombreArchivo)
                .HasMaxLength(60)
                .HasColumnName("nombre_archivo");
            entity.Property(e => e.NumTarea)
                .HasColumnType("int(11)")
                .HasColumnName("numTarea");

            entity.HasOne(d => d.IdResidenteNavigation).WithMany(p => p.Archivosenviados)
                .HasForeignKey(d => d.IdResidente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkResidente");
        });

        modelBuilder.Entity<Asignaciontareas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("asignaciontareas");

            entity.HasIndex(e => e.Idcoordinador, "fkasignacion_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Idcoordinador)
                .HasColumnType("int(11)")
                .HasColumnName("idcoordinador");
            entity.Property(e => e.Intruccion)
                .HasColumnType("text")
                .HasColumnName("intruccion");
            entity.Property(e => e.NombreTarea).HasMaxLength(60);
            entity.Property(e => e.NumTarea)
                .HasColumnType("int(2)")
                .HasColumnName("numTarea");

            entity.HasOne(d => d.IdcoordinadorNavigation).WithMany(p => p.Asignaciontareas)
                .HasForeignKey(d => d.Idcoordinador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkasignacion");
        });

        modelBuilder.Entity<Carrera>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("carrera");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.NombreCarrera)
                .HasMaxLength(30)
                .HasColumnName("nombre_Carrera");
        });

        modelBuilder.Entity<Coordinadores>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("coordinadores");

            entity.HasIndex(e => e.IdCarrera, "fkCarrera_idx");

            entity.HasIndex(e => e.IdIniciarSesion, "fkliniciarSession_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.IdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("idCarrera");
            entity.Property(e => e.IdIniciarSesion)
                .HasColumnType("int(11)")
                .HasColumnName("idIniciar_Sesion");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(60)
                .HasColumnName("nombre_completo");

            entity.HasOne(d => d.IdCarreraNavigation).WithMany(p => p.Coordinadores)
                .HasForeignKey(d => d.IdCarrera)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkCarrera");

            entity.HasOne(d => d.IdIniciarSesionNavigation).WithMany(p => p.Coordinadores)
                .HasForeignKey(d => d.IdIniciarSesion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkliniciarSession");
        });

        modelBuilder.Entity<Inciarsesion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("inciarsesion");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(10)
                .HasColumnName("contrasena");
            entity.Property(e => e.Numcontrol)
                .HasMaxLength(10)
                .HasColumnName("numcontrol");
        });

        modelBuilder.Entity<Residentes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("residentes");

            entity.HasIndex(e => e.IdCarrera, "fkCarrera_idx");

            entity.HasIndex(e => e.IdIniciarSesion, "fkIniciarSesion_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Cooasesor)
                .HasMaxLength(60)
                .HasColumnName("cooasesor");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("idCarrera");
            entity.Property(e => e.IdIniciarSesion)
                .HasColumnType("int(11)")
                .HasColumnName("idIniciar_Sesion");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(60)
                .HasColumnName("nombre_completo");

            entity.HasOne(d => d.IdCarreraNavigation).WithMany(p => p.Residentes)
                .HasForeignKey(d => d.IdCarrera)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkcarreras");

            entity.HasOne(d => d.IdIniciarSesionNavigation).WithMany(p => p.Residentes)
                .HasForeignKey(d => d.IdIniciarSesion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkinicioSesion");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
