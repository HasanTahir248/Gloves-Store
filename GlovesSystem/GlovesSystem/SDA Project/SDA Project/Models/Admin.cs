using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDA_Project.Models
{
    public class Admin
    {
        public int adminId { get; set; }
        public string adminName { get; set; }
        public string loginEmail { get; set; }
        public string adminPassword { get; set; }
        public string adminPassword2 { get; set; }
        public string adminPhone { get; set;}
    }
}