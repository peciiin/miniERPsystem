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

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Storage> Storages { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
