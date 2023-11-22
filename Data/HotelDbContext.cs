using HotelServer.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace HotelServer.Data
{
    public class HotelDbContext : IdentityDbContext<User>
    {
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<TypeHotel> TypeHotel { get; set; }
        public virtual DbSet<TypeRoom> TypeRoom { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DbConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {

        }

        public HotelDbContext()
        {
        }
    }
}
