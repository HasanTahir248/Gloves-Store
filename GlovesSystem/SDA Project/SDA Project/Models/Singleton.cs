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
using System.Security.Cryptography.X509Certificates;

namespace SDA_Project.Models
{
    public class Singleton 
    {
        private static Singleton instance;
        private static readonly object lockObject = new object();
        private static bool adminLoggedIn = false;
        private SqlConnection conn;
        private SqlCommand cmd;

        private Singleton()
        {
            conn = new SqlConnection("Data Source=MHT;Initial Catalog=GlovesSystemDb;Integrated Security=True;Encrypt=False");
        }

        public static Singleton GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new Singleton();
                    }
                }
            }
            return instance;
        }
        
        public bool LoginAdmin(string loginEmail, string adminPassword)
        {
            if (!adminLoggedIn)
            {

                conn.Open();
                cmd = new SqlCommand("select Login_Email, Password from Admin where Login_Email = @Email AND Password = @Pass", conn);
                cmd.Parameters.AddWithValue("@Email", loginEmail);
                cmd.Parameters.AddWithValue("Pass", adminPassword);
                SqlDataReader sdr = cmd.ExecuteReader();
                bool isAuthenticated = sdr.Read();
                conn.Close();
                if (isAuthenticated)
                {
                    adminLoggedIn = true;
                }

                return isAuthenticated;
            }
            return false;
        }
        public bool Logout()
        {
            return adminLoggedIn = false;
        }
        public bool IsAdminLoggedIn()
        {
            return adminLoggedIn;
        }

    }
}
