using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class DataContext : DbContext
    {
        #region Declaration

        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        #endregion

        #region Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Constants.ConnectionString);
        }

        #endregion
    }
}
