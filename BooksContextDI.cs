using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore_test.DB.DI
{
    public class BooksContextDI:DbContext
    {
        public BooksContextDI(DbContextOptions<BooksContextDI> options)
            : base(options) { }

        public DbSet<Book> Books { get; set; }
    }
}
