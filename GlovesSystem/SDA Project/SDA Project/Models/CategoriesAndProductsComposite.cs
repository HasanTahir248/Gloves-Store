using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDA_Project.Models
{
    public class CategoriesAndProductsComposite
    {
        public List<Categories> cList { get; set; }
        public List<Products> pList { get; set; }
    }
}