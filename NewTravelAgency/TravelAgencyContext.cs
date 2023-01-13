using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NewTravelAgency.Models;

namespace NewTravelAgency;

public partial class TravelAgencyContext : DbContext
{
    public TravelAgencyContext()
    {
    }

    public TravelAgencyContext(DbContextOptions<TravelAgencyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Care> Cares { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<CountryResort> CountryResorts { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<NewClient> NewClients { get; set; }

    public virtual DbSet<Ordering> Orderings { get; set; }

    public virtual DbSet<Resort> Resorts { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MSI;Database=TRAVEL_AGENCY;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Care>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CARE_Id");

            entity.ToTable("CARE");

            entity.Property(e => e.Name).HasMaxLength(30);

            entity.HasOne(d => d.Ticket).WithMany(p => p.Cares)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_CARE_To_TICKET");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CLIENT_Id");

            entity.ToTable("CLIENT", tb => tb.HasTrigger("GiftClient"));

            entity.HasIndex(e => new { e.CardId, e.PhoneNumber }, "UQ__CLIENT__DDA1796C7F277D7D").IsUnique();

            entity.Property(e => e.CardId).HasColumnName("CardID");
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Surname).HasMaxLength(30);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_COMPANY_Id");

            entity.ToTable("COMPANY");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(30);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_COUNTRY_Id");

            entity.ToTable("COUNTRY");

            entity.HasIndex(e => e.Name, "UQ_COUNTRY_Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(30);
        });

        modelBuilder.Entity<CountryResort>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("CountryResort");

            entity.Property(e => e.CountryName).HasMaxLength(30);
            entity.Property(e => e.ResortName).HasMaxLength(30);
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HOTEL_Id");

            entity.ToTable("HOTEL", tb => tb.HasTrigger("HotelRoom"));
            
            entity.Property(e => e.Bed).HasMaxLength(30);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Room).HasMaxLength(30);

            entity.HasOne(d => d.Resort).WithMany(p => p.Hotels)
                .HasForeignKey(d => d.ResortId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_HOTEL_To_RESORT");
        });

        modelBuilder.Entity<NewClient>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("NewClient");

            entity.Property(e => e.ClientName).HasMaxLength(30);
            entity.Property(e => e.FirstTime).HasColumnType("date");
            entity.Property(e => e.HotelName).HasMaxLength(50);
            entity.Property(e => e.ResortName).HasMaxLength(30);
            entity.Property(e => e.SecondTime).HasColumnType("date");
        });

        modelBuilder.Entity<Ordering>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ORDERING_Id");

            entity.ToTable("ORDERING");

            entity.HasOne(d => d.Client).WithMany(p => p.Orderings)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_ORDERING_To_CLIENT");

            entity.HasOne(d => d.Worker).WithMany(p => p.Orderings)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_ORDERING_To_WORKER");
        });

        modelBuilder.Entity<Resort>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_RESORT_Id");

            entity.ToTable("RESORT", tb =>
                {
                    tb.HasTrigger("ResortDeleted");
                    tb.HasTrigger("ResortInsert");
                });
           

            entity.HasIndex(e => e.Name, "UQ_RESORT_Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(30);

            entity.HasOne(d => d.Country).WithMany(p => p.Resorts)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_RESORT_To_COUNTRY");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TICKET_Id");

            entity.ToTable("TICKET");

            entity.Property(e => e.TimeArrival).HasColumnType("date");
            entity.Property(e => e.TimeDeparture).HasColumnType("date");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.HotelId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TICKET_To_HOTEL");

            entity.HasOne(d => d.Ordering).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.OrderingId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TICKET_To_ORDERING");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_WORKER_Id");

            entity.ToTable("WORKER");

            entity.HasIndex(e => new { e.CardId, e.CodeId, e.PhoneNumber }, "UQ__WORKER__2116D40233C00CD6").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(30);
            entity.Property(e => e.CardId).HasColumnName("CardID");
            entity.Property(e => e.CodeId).HasColumnName("CodeID");
            entity.Property(e => e.MiddleName).HasMaxLength(30);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Surname).HasMaxLength(30);

            entity.HasOne(d => d.Company).WithMany(p => p.Workers)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_WORKER_To_COMPANY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
