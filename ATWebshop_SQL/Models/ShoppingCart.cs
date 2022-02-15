using System;
using System.Collections.Generic;

#nullable disable

namespace ATWebshop_SQL.Models
{
    public partial class ShoppingCart
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }

        public virtual Product Product { get; set; }
    }
}
