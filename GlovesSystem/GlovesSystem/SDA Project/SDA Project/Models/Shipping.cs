using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDA_Project.Models
{
    public class Shipping
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ShippingAddress { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public int CartID { get; set; }
    }
}