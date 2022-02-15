using System;
using System.Collections.Generic;

#nullable disable

namespace ATWebshop_SQL.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Categories { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
