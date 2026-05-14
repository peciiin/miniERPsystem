using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace miniERPsystem.Models;

public partial class MiniErpsystemContext : DbContext
{
    public MiniErpsystemContext()
    {
    }

    public MiniErpsystemContext(DbContextOptions<MiniErpsystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Finance> Finances { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Storage> Storages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=miniERPsystem;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Finance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Finance__3214EC07FB6C4E57");

            entity.ToTable("Finance");

            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .HasDefaultValue("CZK");
            entity.Property(e => e.Note).HasMaxLength(100);
            entity.Property(e => e.PricePerItem).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Type).HasMaxLength(30);

            entity.HasOne(d => d.Item).WithMany(p => p.Finances)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ForeignKey_finance");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId).HasName("PK__Recipe__FDD988B07A074423");

            entity.ToTable("Recipe");

            entity.Property(e => e.NeededMaterial)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(16, 2)");

            entity.HasOne(d => d.Material).WithMany(p => p.RecipeMaterials)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK__Recipe__Material__5070F446");

            entity.HasOne(d => d.Product).WithMany(p => p.RecipeProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Recipe__ProductI__4F7CD00D");
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Storage__727E838BC67A96BA");

            entity.ToTable("Storage");

            entity.Property(e => e.IsFinal)
                .HasDefaultValue(false)
                .HasColumnName("isFinal");
            entity.Property(e => e.ItemName).HasMaxLength(150);
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(16, 2)");
            entity.Property(e => e.Units)
                .HasMaxLength(10)
                .HasDefaultValue("ks");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
