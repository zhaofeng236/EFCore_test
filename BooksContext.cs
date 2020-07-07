using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace EFCore_test
{
    public class BooksContext:DbContext
    {
        private const string ConnectionString = @"server=localhost;" +
            @"database=BooksStore;" +
            @"trusted_connection=true;";

        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}
