using System;
using System.Collections.Generic;
using CryptoTrading.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrading.Infrastructure.Data;

public partial class CryptoTradingDbContext : DbContext
{
    public CryptoTradingDbContext() { }

    public CryptoTradingDbContext(DbContextOptions<CryptoTradingDbContext> options)
        : base(options) { }

    public virtual DbSet<BinanceKline> BinanceKlines { get; set; }

    public virtual DbSet<TestTable> TestTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BinanceKline>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BinanceK__3214EC073D94591D");

            entity.ToTable("BinanceKline");

            entity
                .HasIndex(
                    e => new
                    {
                        e.Symbol,
                        e.Interval,
                        e.OpenTime,
                    },
                    "IX_BinanceKline_Symbol_Interval_OpenTime"
                )
                .IsUnique();

            entity.Property(e => e.ClosePrice).HasColumnType("decimal(18, 8)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.HighPrice).HasColumnType("decimal(18, 8)");
            entity.Property(e => e.Interval).HasMaxLength(10).IsUnicode(false);
            entity.Property(e => e.LowPrice).HasColumnType("decimal(18, 8)");
            entity.Property(e => e.OpenPrice).HasColumnType("decimal(18, 8)");
            entity.Property(e => e.Symbol).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.Volume).HasColumnType("decimal(18, 8)");
        });

        modelBuilder.Entity<TestTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestTabl__3214EC0795E68645");

            entity.ToTable("TestTable");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
