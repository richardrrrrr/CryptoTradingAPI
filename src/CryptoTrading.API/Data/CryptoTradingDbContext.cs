using System;
using System.Collections.Generic;
using CryptoTrading.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrading.Data;

public partial class CryptoTradingDbContext : DbContext
{
    public CryptoTradingDbContext() { }

    public CryptoTradingDbContext(DbContextOptions<CryptoTradingDbContext> options)
        : base(options) { }

    public virtual DbSet<TestTable> TestTables { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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


