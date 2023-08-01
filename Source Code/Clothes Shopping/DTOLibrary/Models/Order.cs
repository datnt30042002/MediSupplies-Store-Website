using System;
using System.Collections.Generic;

namespace DTOLibrary.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public string? Address { get; set; }
        public double? TotalPrice { get; set; }
        public int? NumberOfItem { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }

        public virtual User? Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
