using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SDA_Project.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }
        public int CustomerID { get; set; }
        //public int ProductID { get; set; }
        //public string ProductName { get; set; }
        //public string ImageURL { get; set; }
        //public decimal Price { get; set; }
        //public int Quantity { get; set; }
        //public int ProductPrice { get; set; }
        //public decimal TotalPrice => Price * Quantity;
        //public int totalPrice { get; set; }
    }
}