using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebClothes.Models
{
    public class CartModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float Price { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
    }
}
