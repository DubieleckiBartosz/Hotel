using Domain.Common;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Database
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {}

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<BookingRoom> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<RoomPicture> RoomPictures { get; set; }
        public DbSet<HotelPicture> HotelPictures { get; set; }
        public DbSet<HotelAttachment> HotelAttachments { get; set; }
        public DbSet<RoomAttachment> RoomAttachments { get; set; }
        public async Task<int> SaveChangesAsync()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((AuditableEntity)entityEntry.Entity).LastModified = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((AuditableEntity)entityEntry.Entity).Created = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BookingRoom>(x =>
            {
                x.HasKey(c => c.Id);

                x.Property(c => c.UserId).HasMaxLength(450)
                .IsRequired(false);

                x.Property(p => p.Discount)
                 .HasColumnType("decimal(18,2)")
                 .IsRequired(false);

                x.Property(c => c.Email).IsRequired();

                x.Property(c => c.FullPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

                x.Property(c => c.From).IsRequired();
                x.Property(c => c.To).IsRequired();

                x.HasOne<Room>(c => c.Room)
                .WithMany(s => s.BookingRooms)
                .HasForeignKey(s=>s.RoomId).IsRequired();

                x.Property(c => c.NumberOfGuests)
                .IsRequired();

            });
            modelBuilder.Entity<Room>(x =>
            {
                x.HasKey(c => c.Id);
                x.HasOne<Hotel>(c => c.Hotel)
                .WithMany(c => c.Rooms).HasForeignKey(c => c.HotelId);
                x.Property(c => c.NumberOfBeds).IsRequired();
                x.Property(c => c.PricePerPerson)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
                x.Property(c => c.Promotion)
                .IsRequired(false);
                x.HasMany(c => c.RoomPictures)
                .WithOne(c => c.Room).HasForeignKey(c => c.RoomId);
                x.HasMany(c => c.RoomAttachments)
                .WithOne(c => c.Room).HasForeignKey(c => c.RoomId);
            });
            modelBuilder.Entity<Address>(x =>
            {
                x.HasKey(c => c.Id);
                x.Property(c => c.Street)
                .HasMaxLength(100).IsRequired();
                x.Property(c => c.ZipCode)
                .HasMaxLength(10).IsRequired();
                x.Property(c => c.City)
                 .HasMaxLength(50).IsRequired();
                x.HasOne(c => c.Hotel)
                .WithOne(s => s.Address)
                .HasForeignKey<Hotel>(q => q.AddressId);
            });

            modelBuilder.Entity<Hotel>(x =>
            {
                x.HasKey(c => c.Id);
                x.Property(c => c.HotelName)
                .HasMaxLength(100).IsRequired();
                x.Property(c => c.Stars)
                .HasDefaultValue(0);
                x.Property(c=>c.Email)
                .HasMaxLength(50)
                .IsRequired();
                x.Property(c => c.Email)
                .HasMaxLength(45).IsRequired();
                x.HasMany(c => c.HotelPictures)
                .WithOne(c => c.Hotel)
                .HasForeignKey(c => c.HotelId);
                x.HasMany(c => c.HotelAttachments)
                .WithOne(c => c.Hotel)
                .HasForeignKey(c => c.HotelId);
            });

            modelBuilder.Entity<HotelPicture>(x =>
            {
                x.HasKey(c => c.Id);
                x.Property(c => c.Image).IsRequired();
                x.Property(c => c.Name)
                .HasMaxLength(100).IsRequired(false);
     
            });
            modelBuilder.Entity<RoomPicture>(x =>
            {
                x.HasKey(c => c.Id);
                x.Property(c => c.Image).IsRequired();
                x.Property(c => c.Name)
                .HasMaxLength(100).IsRequired(false);
            });
            modelBuilder.Entity<HotelAttachment>(x =>
            {
                x.HasKey(c => c.Id);
                x.Property(c => c.Path).IsRequired();
            });
            modelBuilder.Entity<RoomAttachment>(x =>
            {
                x.HasKey(c => c.Id);
                x.Property(c => c.Path).IsRequired();
            });
        }
    }

    //public class DapperContext
    //{
    //    private readonly IConfiguration _configuration;
    //    private readonly string _connectionString;
    //    public DapperContext(IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    //    }
    //    public IDbConnection GetConnection() =>
    //        new SqlConnection(_connectionString);
    //}
}
