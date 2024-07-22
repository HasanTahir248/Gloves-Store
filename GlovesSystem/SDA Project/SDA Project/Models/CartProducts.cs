using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDA_Project.Models
{
    public class CartProducts
    {
        public int ID { get; set; }
        public int CartID { get; set; }
        public int CustomerID { get; set; }
        public int totalPrice { get; set; }
        public Products singleProd { get; set; }
        public List<Products> _Products { get; set; }
    }
}