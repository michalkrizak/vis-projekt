using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace api.Models;

public partial class VolejbalContext : DbContext
{
    public VolejbalContext()
    {
    }

    public VolejbalContext(DbContextOptions<VolejbalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Hrac> Hracs { get; set; }

    public virtual DbSet<SestavaZapasu> SestavaZapasus { get; set; }

    public virtual DbSet<Sezona> Sezonas { get; set; }

    public virtual DbSet<Tym> Tyms { get; set; }

    public virtual DbSet<Zapa> Zapas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Volejbal;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hrac>(entity =>
        {
            entity.HasKey(e => e.IdHrac).HasName("PK__Hrac__BF69FB21992E9205");

            entity.ToTable("Hrac");

            entity.Property(e => e.IdHrac)
                .ValueGeneratedNever()
                .HasColumnName("id_hrac");
            entity.Property(e => e.IdTym).HasColumnName("id_tym");
            entity.Property(e => e.Jmeno)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("jmeno");
            entity.Property(e => e.Pozice)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pozice");
            entity.Property(e => e.Prijmeni)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("prijmeni");

            entity.HasOne(d => d.IdTymNavigation).WithMany(p => p.Hracs)
                .HasForeignKey(d => d.IdTym)
                .HasConstraintName("FK__Hrac__id_tym__38996AB5");
        });

        modelBuilder.Entity<SestavaZapasu>(entity =>
        {
            entity.HasKey(e => new { e.IdZapas, e.IdHrac }).HasName("PK__SestavaZ__817274BCA49654DC");

            entity.ToTable("SestavaZapasu");

            entity.Property(e => e.IdZapas).HasColumnName("id_zapas");
            entity.Property(e => e.IdHrac).HasColumnName("id_hrac");
            entity.Property(e => e.Body).HasColumnName("body");
            entity.Property(e => e.Esa).HasColumnName("esa");
            entity.Property(e => e.IdTym).HasColumnName("id_tym");
            entity.Property(e => e.Pozice)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pozice");
            entity.Property(e => e.Sety).HasColumnName("sety");

            entity.HasOne(d => d.IdHracNavigation).WithMany(p => p.SestavaZapasus)
                .HasForeignKey(d => d.IdHrac)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SestavaZa__id_hr__47DBAE45");

            entity.HasOne(d => d.IdTymNavigation).WithMany(p => p.SestavaZapasus)
                .HasForeignKey(d => d.IdTym)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SestavaZa__id_ty__48CFD27E");

            entity.HasOne(d => d.IdZapasNavigation).WithMany(p => p.SestavaZapasus)
                .HasForeignKey(d => d.IdZapas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SestavaZa__id_za__46E78A0C");
        });

        modelBuilder.Entity<Sezona>(entity =>
        {
            entity.HasKey(e => e.IdSezona).HasName("PK__Sezona__94BFBBE47223E653");

            entity.ToTable("Sezona");

            entity.Property(e => e.IdSezona)
                .ValueGeneratedNever()
                .HasColumnName("id_sezona");
            entity.Property(e => e.Nazev)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nazev");
            entity.Property(e => e.Rok).HasColumnName("rok");
        });

        modelBuilder.Entity<Tym>(entity =>
        {
            entity.HasKey(e => e.IdTym).HasName("PK__Tym__6A2944D9B80985BB");

            entity.ToTable("Tym");

            entity.Property(e => e.IdTym)
                .ValueGeneratedNever()
                .HasColumnName("id_tym");
            entity.Property(e => e.DatumZalozeni).HasColumnName("datum_zalozeni");
            entity.Property(e => e.Nazev)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nazev");
            entity.Property(e => e.Stadion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("stadion");
            entity.Property(e => e.Trener)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("trener");

            entity.HasMany(d => d.IdSezonas).WithMany(p => p.IdTyms)
                .UsingEntity<Dictionary<string, object>>(
                    "TymSezona",
                    r => r.HasOne<Sezona>().WithMany()
                        .HasForeignKey("IdSezona")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Tym_Sezon__id_se__440B1D61"),
                    l => l.HasOne<Tym>().WithMany()
                        .HasForeignKey("IdTym")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Tym_Sezon__id_ty__4316F928"),
                    j =>
                    {
                        j.HasKey("IdTym", "IdSezona").HasName("PK__Tym_Sezo__3362BF671C34C67F");
                        j.ToTable("Tym_Sezona");
                        j.IndexerProperty<int>("IdTym").HasColumnName("id_tym");
                        j.IndexerProperty<int>("IdSezona").HasColumnName("id_sezona");
                    });
        });

        modelBuilder.Entity<Zapa>(entity =>
        {
            entity.HasKey(e => e.IdZapas).HasName("PK__Zapas__8A84EB0E8BB2FD14");

            entity.Property(e => e.IdZapas)
                .ValueGeneratedNever()
                .HasColumnName("id_zapas");
            entity.Property(e => e.Datum).HasColumnName("datum");
            entity.Property(e => e.IdSezona).HasColumnName("id_sezona");
            entity.Property(e => e.IdTym1).HasColumnName("id_tym1");
            entity.Property(e => e.IdTym2).HasColumnName("id_tym2");
            entity.Property(e => e.SkoreTym1).HasColumnName("skore_tym1");
            entity.Property(e => e.SkoreTym2).HasColumnName("skore_tym2");
            entity.Property(e => e.Vitez).HasColumnName("vitez");

            entity.HasOne(d => d.IdSezonaNavigation).WithMany(p => p.Zapas)
                .HasForeignKey(d => d.IdSezona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Zapas__id_sezona__3F466844");

            entity.HasOne(d => d.IdTym1Navigation).WithMany(p => p.ZapaIdTym1Navigations)
                .HasForeignKey(d => d.IdTym1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Zapas__id_tym1__3D5E1FD2");

            entity.HasOne(d => d.IdTym2Navigation).WithMany(p => p.ZapaIdTym2Navigations)
                .HasForeignKey(d => d.IdTym2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Zapas__id_tym2__3E52440B");

            entity.HasOne(d => d.VitezNavigation).WithMany(p => p.ZapaVitezNavigations)
                .HasForeignKey(d => d.Vitez)
                .HasConstraintName("FK__Zapas__vitez__403A8C7D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
