using System;
using System.Collections.Generic;

#nullable disable

namespace ATWebshop_SQL.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public double? UnitPrice { get; set; }
        public int? Quantity { get; set; }

        public virtual Customer Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
