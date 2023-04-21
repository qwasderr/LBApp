using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace LBApp.Models;

public partial class DblibraryContext : DbContext
{
    public DblibraryContext()
    {
    }

    public DblibraryContext(DbContextOptions<DblibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<AuthorsBook> AuthorsBooks { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<PublishingHouse> PublishingHouses { get; set; }

    public virtual DbSet<Reader> Readers { get; set; }

    public virtual DbSet<ReadersBook> ReadersBooks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server= DESKTOP-VQ4QIBS\\SQLEXPRESS;\nDatabase=DBLibrary; Trusted_Connection=True; Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(e => e.AuthorBiogr).HasColumnType("text");
            entity.Property(e => e.AuthorDate).HasColumnType("date");
            entity.Property(e => e.AuthorName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AuthorsBook>(entity =>
        {
            entity.HasOne(d => d.Author).WithMany(p => p.AuthorsBooks)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Authors_AuthorsBooks");

            entity.HasOne(d => d.Book).WithMany(p => p.AuthorsBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_AuthorsBooks");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(e => e.BookName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Genres_Books");

            entity.HasOne(d => d.PublishingHouse).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublishingHouseId)
                .HasConstraintName("FK_PublishingHouses_Books");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.ComId);

            entity.Property(e => e.ComDate).HasColumnType("datetime");
            entity.Property(e => e.ComText).HasColumnType("text");

            entity.HasOne(d => d.Book).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_Comments");

            entity.HasOne(d => d.Reader).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ReaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Readers_Comments");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.Property(e => e.GenreDescr)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.GenreName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PublishingHouse>(entity =>
        {
            entity.HasKey(e => e.PhId);

            entity.Property(e => e.PhDescr).HasColumnType("text");
            entity.Property(e => e.PhName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.ReaderId).HasName("PK_Users");

            entity.Property(e => e.ReaderName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ReadersBook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UsersBooks");

            entity.HasOne(d => d.Book).WithMany(p => p.ReadersBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_ReadersBooks");

            entity.HasOne(d => d.Reader).WithMany(p => p.ReadersBooks)
                .HasForeignKey(d => d.ReaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Readers_ReadersBooks");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
