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
            //AddRecords();

            //UpdateRecords();
            //ChangeUntracked();
            AddHundredRecords();

            //DeleteDatabase();
            Console.ReadKey();
        }

        //批处理
        private static void AddHundredRecords()
        {
            Console.WriteLine(nameof(AddHundredRecords));
            using(var context=new MenusContext())
            {
                var card = context.MenuCards.FirstOrDefault();
                if (card != null)
                {
                    var menus = Enumerable.Range(1, 10000).Select(x => new Menu
                    {
                        MenuCard = card,
                        Text = $"menu {x}",
                        Price = 9.9m
                    });

                   context.Menus.AddRange(menus);
                   
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    int records =  context.SaveChanges();
                    stopwatch.Stop();
                    Console.WriteLine($"{records} 条记录被插入，用时" +
                        $" {stopwatch.ElapsedMilliseconds} 毫秒");
                }
                Console.WriteLine();
            }
        }

        private static void AddRecords()
        {
            Console.WriteLine(nameof(AddRecords));
            try
            {
                using (var context = new MenusContext())
                {
                    var soupCard = new MenuCard();
                    Menu[] soups =
                    {
                        new Menu
                        {
                            Text="红烧钳子",
                            Price=28m,
                            MenuCard=soupCard
                        },

                        new Menu
                        {
                            Text="白族生肉",
                            Price=40m,
                            MenuCard=soupCard
                        },

                        new Menu
                        {
                            Text="宾川海稍鱼",
                            Price=88m,
                            MenuCard=soupCard
                        },

                        new Menu
                        {
                            Text="丽江腊排骨",
                            Price=68m,
                            MenuCard=soupCard
                        },
                    };

                    soupCard.Title = "Soups";
                    soupCard.Menus.AddRange(soups);
                    context.MenuCards.Add(soupCard);

                    ShowState(context);
                    int records = context.SaveChanges();
                    Console.WriteLine($"{records} 条记录被插入!");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine();
        }

        //==显示状态==
        //==ChangeTracker的Entries方法返回变更跟踪器的对象==
        public static void ShowState(MenusContext context)
        {
            foreach (EntityEntry entry in context.ChangeTracker.Entries())
            {
                Console.WriteLine($"类型：{entry.Entity.GetType().Name}," +
                    $"状态:{entry.State},{entry.Entity}");
            }
        }

        private static void ObjectTracking()
        {
            Console.WriteLine(nameof(ObjectTracking));
            using (var context = new MenusContext())
            {
                var m1 = (from m in context.Menus
                          where m.Text.StartsWith("Con")
                          select m).FirstOrDefault();

                var m2 = (from m in context.Menus
                          where m.Text.Contains("(")
                          select m).FirstOrDefault();

                if (object.ReferenceEquals(m1, m2))
                {
                    Console.WriteLine("对象相同！");
                }
                else
                {
                    Console.WriteLine("对象不相同！");
                }
                ShowState(context);
            }
            Console.WriteLine();
        }

        //更新对象
        private static void UpdateRecords()
        {
            Console.WriteLine(nameof(UpdateRecords));
            using(var context=new MenusContext())
            {
                Menu menu = context.Menus
                    .Skip(1)
                    .FirstOrDefault();

                ShowState(context);
                menu.Price += 20m;
                ShowState(context);

                int records = context.SaveChanges();
                Console.WriteLine($"{records} 条记录被更新！");
                ShowState(context);
            }
            Console.WriteLine();
        }


        private static void ChangeUntracked()
        {
            Console.WriteLine(nameof(ChangeUntracked));
            Menu GetMenu()
            {
                using(var context=new MenusContext())
                {
                    Menu menu = context.Menus
                        .Skip(0)
                        .FirstOrDefault();
                    return menu;
                }
            }

            Menu m = GetMenu();
            m.Price += 50m;
            UpdateUntracked(m);
        }

        private static void UpdateUntracked(Menu m)
        {
            using(var context=new MenusContext())
            {
                ShowState(context);
                context.Menus.Update(m);

                ShowState(context);
                context.SaveChanges();
            }
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
