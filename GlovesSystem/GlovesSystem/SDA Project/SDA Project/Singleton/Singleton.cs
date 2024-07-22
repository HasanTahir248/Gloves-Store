using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SDA_Project.Models;
using System.Web.Mvc;
using System.Web;

namespace SDA_Project.Singleton
{
    public class Singleton 
    {
        private static Singleton instance;
        private Admin admin;
        private bool isLoggedIn;

        private Singleton() 
        {
            admin = new Admin();
            isLoggedIn = false;
        }
        public static Singleton GetInstance()
        {
            if (instance == null)
            {
                instance = new Singleton();
            }
            return instance;
        }
        public Admin AdminInstance
        {
            get { return admin; }
            set { admin = value; }
        }
        public bool IsLoggedIn
        {
            get { return isLoggedIn; }
            set { isLoggedIn = value; }
        }

    }
}
