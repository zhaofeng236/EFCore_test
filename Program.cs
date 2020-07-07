using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using EFCore_test.DB.DI;

namespace EFCore_test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("这个是EFCore 学习程序");
            var p = new Program();

            p.InitializeServices();
            var service = p.Container.GetService<BooksService>();
            await service.AddBooksAsync();
            await service.ReadBooksAsync();

            p.Container.Dispose();

            Console.ReadKey();
        }


        public ServiceProvider Container { get; private set; }

        private void InitializeServices()
        {
            const string ConnectionString = @"server=localhost;" +
                "database=BooksStore;" +
                "trusted_connection=true;";

            var services = new ServiceCollection();
            services.AddTransient<BooksService>()
                .AddEntityFrameworkSqlServer()
                .AddDbContext<BooksContextDI>(options =>
                options.UseSqlServer(ConnectionString));

            Container = services.BuildServiceProvider();
        }

    }
}
