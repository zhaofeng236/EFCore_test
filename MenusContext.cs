using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            //使用配置文件来配置
            //modelBuilder.ApplyConfiguration(new MenuCardConfiguration());
            // modelBuilder.ApplyConfiguration(new MenuConfiguration());

            modelBuilder.Entity<MenuCard>()
                .ToTable("MenuCards")
                .HasKey(c => c.MenuCardId);

            modelBuilder.Entity<MenuCard>()
                .Property<int>(c => c.MenuCardId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<MenuCard>()
                .Property<string>(c => c.Title)
                .HasMaxLength(50);

            //menu的配置
            modelBuilder.Entity<Menu>()
                .ToTable("Menus")
                .HasKey(m => m.MenuId);

            modelBuilder.Entity<Menu>()
                .Property<int>(m => m.MenuId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Menu>()
                .Property<string>(m => m.Text)
                .HasMaxLength(120);

            modelBuilder.Entity<Menu>()
                .Property<decimal>(m => m.Price)
                .HasColumnType("money");

            //关联关系，一个菜单包含很多菜品
            modelBuilder.Entity<MenuCard>()
                .HasMany(c => c.Menus)
                .WithOne(m => m.MenuCard);

            modelBuilder.Entity<Menu>()
                .HasOne(m => m.MenuCard)
                .WithMany(c => c.Menus)
                .HasForeignKey(m => m.MenuCardId);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
