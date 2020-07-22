using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EFCore_test
{
    class Program
    {
        static void  Main(string[] args)
        {
            Console.WriteLine("这个是EFCore 学习程序");

            //CreateDatabase();

            //AddTwoRecordsWithOneTx();
            Console.WriteLine("\n\n");
            AddTwoRecordsWithTwoTx();

            //DeleteDatabase();
            Console.ReadKey();
        }


        private static void AddTwoRecordsWithOneTx()
        {
            Console.WriteLine(nameof(AddTwoRecordsWithOneTx));
            try
            {
                using(var context=new MenusContext())
                {
                    var card = context.MenuCards.First();

                    var m1 = new Menu
                    {
                        MenuCardId = card.MenuCardId,
                        Text = "added",
                        Price = 99.99m
                    };

                    int hightestCardId = context.MenuCards.Max(c => c.MenuCardId);
                    Console.WriteLine($"最大的Id为：{ hightestCardId}");

                    var mInvalid = new Menu
                    {
                        MenuCardId = ++hightestCardId,
                        Text = "invalid", Price = 999.99m
                    };

                    Console.WriteLine($"m1的MenuCardId={m1.MenuCardId}");
                    Console.WriteLine($"mInvalid的MenuCardId={mInvalid.MenuCardId}");

                    context.Menus.AddRange(m1, mInvalid);

                    Console.WriteLine("trying to add one invalid record to the database, this should fail...");
                    int records = context.SaveChanges();
                    Console.WriteLine($"{records} 条记录被增加！");
                }
            }
            catch(DbUpdateException ex)
            {
                Console.WriteLine($"{ex.Message}");
                Console.WriteLine($"{ex?.InnerException.Message}");
            }
            Console.WriteLine();
        }



        //一个SaveChanges插入一次
        private static void AddTwoRecordsWithTwoTx()
        {
            Console.WriteLine(nameof(AddTwoRecordsWithTwoTx));
            try
            {
                using (var context = new MenusContext())
                {
                    Console.WriteLine("adding two records with two transactions to the database. One record should be written, the other not....");
                    var card = context.MenuCards.First();
                    var m1 = new Menu { MenuCardId = card.MenuCardId, Text = "added", Price = 99.99m };

                    context.Menus.Add(m1);
                    int records = context.SaveChanges();
                    Console.WriteLine($"{records} records added");

                    int hightestCardId = context.MenuCards.Max(c => c.MenuCardId);
                    var mInvalid = new Menu { MenuCardId = ++hightestCardId, Text = "invalid", Price = 999.99m };
                    context.Menus.Add(mInvalid);

                    records = context.SaveChanges();
                    Console.WriteLine($"{records} records added");
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"{ex.Message}");
                Console.WriteLine($"{ex?.InnerException.Message}");
            }
            Console.WriteLine();
        }




        //创建数据库及数据表
        private static void CreateDatabase()
        {
            using(var context=new MenusContext())
            {
                bool created = context.Database.EnsureCreated();
            }
        }

        //删除数据库
        private static void DeleteDatabase()
        {
            Console.WriteLine("是否删除数据库 ？");
            string input = Console.ReadLine();
            if (input.ToLower() == "y")
            {
                using(var context=new MenusContext())
                {
                    bool deleted = context.Database.EnsureDeleted();
                }
            }
        }

    }
}
