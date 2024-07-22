using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDA_Project.Models
{
    public class Products
    {
        public int ProductID { get; set; }
        public string ArticleNo { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public string ImageURL { get; set; }
        public int Quantity { get; set; }
        public HttpPostedFileBase ProductImage { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int OrderID { get; set; }

    }
}