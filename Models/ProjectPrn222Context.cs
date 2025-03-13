using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AssignmentPRN222.Models;

public partial class ProjectPrn222Context : DbContext
{
    public ProjectPrn222Context()
    {
    }

    public ProjectPrn222Context(DbContextOptions<ProjectPrn222Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Cinema> Cinemas { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<ProVince> ProVinces { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<SeatsBooking> SeatsBookings { get; set; }

    public virtual DbSet<ShowTime> ShowTimes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfigurationRoot configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Cinema>(entity =>
        {
            entity.HasIndex(e => e.ProvinceId, "IX_Cinemas_ProvinceId");

            entity.Property(e => e.IsDelete).HasColumnName("isDelete");
            entity.Property(e => e.Version)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Province).WithMany(p => p.Cinemas).HasForeignKey(d => d.ProvinceId);
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.Property(e => e.Images).HasColumnName("images");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasIndex(e => e.CinemaId, "IX_Rooms_CinemaId");

            entity.HasOne(d => d.Cinema).WithMany(p => p.Rooms).HasForeignKey(d => d.CinemaId);
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasIndex(e => e.RoomId, "IX_Seats_RoomId");

            entity.HasOne(d => d.Room).WithMany(p => p.Seats).HasForeignKey(d => d.RoomId);
        });

        modelBuilder.Entity<SeatsBooking>(entity =>
        {
            entity.ToTable("SeatsBooking");

            entity.HasIndex(e => e.OrderId, "IX_SeatsBooking_OrderId");

            entity.HasIndex(e => e.SeatId, "IX_SeatsBooking_SeatId");

            entity.HasIndex(e => e.ShowTimeId, "IX_SeatsBooking_ShowTimeId");

            entity.HasOne(d => d.Order).WithMany(p => p.SeatsBookings).HasForeignKey(d => d.OrderId);

            entity.HasOne(d => d.Seat).WithMany(p => p.SeatsBookings).HasForeignKey(d => d.SeatId);

            entity.HasOne(d => d.ShowTime).WithMany(p => p.SeatsBookings).HasForeignKey(d => d.ShowTimeId);
        });

        modelBuilder.Entity<ShowTime>(entity =>
        {
            entity.ToTable("ShowTime");

            entity.HasIndex(e => e.MovieId, "IX_ShowTime_MovieId");

            entity.Property(e => e.IsBooked).HasColumnName("isBooked");

            entity.HasOne(d => d.Movie).WithMany(p => p.ShowTimes).HasForeignKey(d => d.MovieId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
