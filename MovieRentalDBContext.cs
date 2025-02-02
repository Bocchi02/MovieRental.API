using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MovieRental.API.Models;

namespace MovieRental.API
{
    public class MovieRentalDBContext : DbContext
    {
        public MovieRentalDBContext(DbContextOptions<MovieRentalDBContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<RentalHeader> RentalHeaders { get; set; }
        public DbSet<RentalHeaderDetail> RentalHeaderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.MovieId);
                entity.Property(m => m.Title)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(m => m.Genre)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(m => m.Director)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(m => m.RentalPrice)
                    .HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.CustomerId);
                entity.Property(c => c.Lastname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .UseCollation("Latin1_General_CI_AS");
                entity.Property(c => c.Firstname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .UseCollation("Latin1_General_CI_AS");
                entity.HasIndex(c => c.Email)
                    .IsUnique();
                entity.Property(c => c.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(c => c.Address)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(c => c.MembershipDate)
                    .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<RentalHeader>(entity =>
            {
                entity.HasKey(rh => rh.RentalHeaderId);
                entity.HasOne(rh => rh.Customer)
                    .WithMany(c => c.RentalHeaders)
                    .HasForeignKey(rh => rh.CustomerId);
                entity.Property(rh => rh.RentalDate)
                   .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<RentalHeaderDetail>(entity =>
            {
                entity.HasKey(rd => rd.RentalHeaderDetailId);
                entity.HasOne(rd => rd.RentalHeader)
                    .WithMany(rh => rh.RentalHeaderDetails)
                    .HasForeignKey(rd => rd.RentalHeaderDetailId);
                entity.HasOne(rd => rd.Movie)
                    .WithMany(m  => m.RentalHeaderDetails)
                    .HasForeignKey(rd => rd.MovieId);
            });
        }
    }
}
