﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DoAn1;

public partial class MyShopContext : DbContext
{
    public MyShopContext()
    {
    }

    public MyShopContext(DbContextOptions<MyShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderBook> OrderBooks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\SqlExpress; Trusted_Connection=Yes; Initial Catalog=MyShop; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Book__3214EC07A651F723");

            entity.ToTable("Book");

            entity.Property(e => e.Author).HasMaxLength(255);
            entity.Property(e => e.CategoryId).HasColumnName("Category_Id");
            entity.Property(e => e.Cover).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PublishingCompany).HasMaxLength(255);

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Book_fk0");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC0787668DC1");

            entity.ToTable("Category");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Discount__3214EC07EABCAFF6");

            entity.ToTable("Discount");

            entity.Property(e => e.Code).HasMaxLength(255);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC07E4095C6C");

            entity.ToTable("Order");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CustomerName).HasMaxLength(255);
            entity.Property(e => e.DiscountId).HasColumnName("Discount_Id");

            entity.HasOne(d => d.Discount).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DiscountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Order_fk0");
        });

        modelBuilder.Entity<OrderBook>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.BookId }).HasName("PK_ORDER_BOOK");

            entity.ToTable("Order_Book");

            entity.Property(e => e.OrderId).HasColumnName("Order_Id");
            entity.Property(e => e.BookId).HasColumnName("Book_Id");

            entity.HasOne(d => d.Book).WithMany(p => p.OrderBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Order_Book_fk1");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderBooks)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Order_Book_fk0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
