using DTOLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using DAOLibrary.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebClothes.Models;

namespace WebClothes.Models
{
    public class ProductModel 
    {
        //public int ProductId { get; set; }
        //public string ProductName { get; set; }
        //public int CategoryId { get; set; }

        //public float Price { get; set; }

        //public int Quantity { get; set; }
        //public string Image { get; set; }

        //public bool Status { get; set; }


        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int CategoryId { get; set; }

        public float Price { get; set; }

        public int Quantity { get; set; }

        public string Image { get; set; }

        public bool Status { get; set; }

    };
}