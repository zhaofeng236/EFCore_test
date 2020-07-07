using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EFCore_test.DB.DI
{
    public class BooksService
    {
        private readonly BooksContextDI _booksContextDI;

        public BooksService(BooksContextDI contextDI) => _booksContextDI = contextDI;

        //插入记录
        public async Task AddBooksAsync()
        {
            var b1 = new Book
            {
                Title = "九月就是雨天",
                Publisher = "电子工业出版本"
            };

            var b2 = new Book
            {
                Title = "你的一生失败",
                Publisher = "电飞机出版本"
            };

            var b3 = new Book
            {
                Title = "不要在等待不值得",
                Publisher = "哈尔滨出版本"
            };
            _booksContextDI.AddRange(b1, b2, b3);
            int records = await _booksContextDI.SaveChangesAsync();
            Console.WriteLine($"{records} 条记录被插入!");
        }



        //读取记录
        public async Task ReadBooksAsync()
        {
            List<Book> books = await _booksContextDI.Books.ToListAsync();

            foreach(var b in books)
            {
                Console.WriteLine($"{b.Title}\t\t{ b.Publisher}");
            }
            Console.WriteLine();
        }

    }
}
