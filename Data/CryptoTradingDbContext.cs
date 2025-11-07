using System;
using System.Collections.Generic;
using CryptoTradingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoTradingAPI.Data;

public partial class CryptoTradingDbContext : DbContext
{
    public CryptoTradingDbContext() { }

    public CryptoTradingDbContext(DbContextOptions<CryptoTradingDbContext> options)
        : base(options) { }

    public virtual DbSet<TestTable> TestTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        =>
        optionsBuilder.UseSqlServer(
            "Server=localhost,1440;Database=Crypto;User Id=sa;Password=MyS3cur3@CryptoDB2025;Encrypt=False;TrustServerCertificate=True;"
        );

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
