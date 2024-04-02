using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MatrixWebAPI.Models;

public partial class AccountsManagementContext : DbContext
{
    public AccountsManagementContext()
    {
    }

    public AccountsManagementContext(DbContextOptions<AccountsManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountsUser> AccountsUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-PB4PM5VU\\SQLEXPRESS;Database=AccountsManagement;Trusted_Connection=True;encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.CompanyName)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.Website).HasMaxLength(500);
        });

        modelBuilder.Entity<AccountsUser>(entity =>
        {
            entity.ToTable("Accounts_Users");

            entity.HasOne(d => d.Account).WithMany(p => p.AccountsUsers)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Accounts_Users__Accounts");

            entity.HasOne(d => d.User).WithMany(p => p.AccountsUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Accounts_Users__Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_Users").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(128)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
