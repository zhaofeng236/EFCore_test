using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore_test.DB
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string Text { get; set; }
        public decimal Price { get; set; }
        
        //外键
        public int MenuCardId { get; set; }
        public MenuCard MenuCard { get; set; }
        public override string ToString() => Text;
    }
}
