using DTOLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DTOLibrary.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }


        [Display(Name = "Category ID")]
        public int? CategoryId { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public string? Image { get; set; }
        public bool? Status { get; set; }
        public string? Description { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
};