using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDA_Project.Models
{
    public class Payment
    {
        public string CardNumber { get; set; }
        public string OwnerName { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }

    }
}