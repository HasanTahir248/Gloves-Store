using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDA_Project.Models
{
    public class Order
    {
        public int OrdeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Payment { get; set; }
        public string OrderDate{ get; set; }

    }
}