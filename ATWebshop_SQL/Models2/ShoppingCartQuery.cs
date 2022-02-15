using ATWebshop_SQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATWebshop_SQL
{
    class ShoppingCartQuery
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }

    }
}