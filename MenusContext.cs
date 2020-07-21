using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore_test
{
    public class MenusContext : DbContext
    {
        private const string ConnectionString =
          @"server=localhost;" +
          @"database=MenuStore;" +
          @"trusted_connection=true;";

        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuCard> MenuCards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("mc");

            modelBuilder.ApplyConfiguration(new MenuCardConfiguration());
            modelBuilder.ApplyConfiguration(new MenuConfiguration());
        }
    }
}
