using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TicketSystemDesign.Models
{
    public partial class TickerSystemContext : DbContext
    {
        public TickerSystemContext()
        {
        }

        public TickerSystemContext(DbContextOptions<TickerSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TicketProp> TicketProp { get; set; }
        public virtual DbSet<TicketTable> TicketTable { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TicketProp>(entity =>
            {
                entity.HasKey(e => e.TicketTypeId);

                entity.Property(e => e.TicketTypeId).ValueGeneratedNever();

                entity.Property(e => e.RolePermission)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TicketType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TicketTable>(entity =>
            {
                entity.HasKey(e => e.TicketId);

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Summary)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.TicketType)
                    .WithMany(p => p.TicketTable)
                    .HasForeignKey(d => d.TicketTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TicketTable_TicketProp");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TicketTable)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TicketTable_UserInfo1");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.HasIndex(e => e.UserName)
                    .HasName("IX_UserName")
                    .IsUnique();

                entity.Property(e => e.Pwd)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.UserInfo)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserInfo_Status");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.Status);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
