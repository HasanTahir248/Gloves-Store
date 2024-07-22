using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDA_Project.Models
{
    public class CartView
    {
        public List <Products> _Products { get; set; }
        public  Cart _cart { get; set; }
        public  CartProducts _Cartproducts { get; set; }
    }
}