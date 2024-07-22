using SDA_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Collections;

namespace SDA_Project.Controllers
{
    public class AdminController : Controller
    {
        SqlConnection conn = new SqlConnection("Data Source=MHT;Initial Catalog=GlovesSystemDb;Integrated Security=True;Encrypt=False");
        SqlCommand cmd;
        List<Categories> cList;
        List<Products> pList;
        List<Products> opList;
        List<Order> oList;
        List<Admin> aList;
        List<Review> rList;
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["adminLoggedIn"] == null || !(bool)Session["adminLoggedIn"])
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            FetchOrders();
            return View(oList);
        }
        public ActionResult Logout()
        {
            Singleton.GetInstance().Logout();
            Session["adminLoggedIn"] = null;
            return RedirectToAction("LoginOption", "Home");
        }
        public ActionResult Reveiws() 
        {
            FetchReviews();
            return View(rList);
        }
        public ActionResult OrderProducts(int id)
        {
            FetchOrderProducts(id);
            return View(opList);
        }
        public ActionResult AllAdmins()
        {
            FetchAdmins();
            return View(aList);
        }
        public ActionResult Products()
        {
            FetchCategories();
            FetchProducts();
            CategoriesAndProductsComposite cp = new CategoriesAndProductsComposite();
            cp.pList = pList;
            cp.cList = cList;
            return View(cp);
        }
        public ActionResult DeleteProduct(int id)
        {

                conn.Open();
                cmd = new SqlCommand("DeleteProduct", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                TempData["Message"] = "<script> alert('Product Deleted Successfully!')  </script>";
                conn.Close();
            return RedirectToAction("Products");

        }
        public ActionResult DeleteCategories(int id)
        {
            conn.Open();
            cmd = new SqlCommand("DeleteCategory", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            TempData["Message"] = "<script> alert('Category Deleted Successfully!')  </script>";
            conn.Close();
            return RedirectToAction("Products");
        }
        public void FetchCategories()
        {
            cList = new List<Categories>();
            conn.Open();
            cmd = new SqlCommand("select CategoryID, CategoryName, CategoryImage from Categories", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    Categories c = new Categories();
                    c.CategoryID = Convert.ToInt32(rdr["CategoryID"]);
                    c.CategoryName = rdr["CategoryName"].ToString();
                    c.CategoryImage = rdr["CategoryImage"].ToString();
                    cList.Add(c);
                }
            }
            conn.Close();
        }
        public void FetchProducts()
        {
            pList = new List<Products>();
            conn.Open();
            cmd = new SqlCommand("select * from ProductsWithCategories", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    Products p = new Products();
                    p.ProductID = Convert.ToInt32(rdr["ProductID"]);
                    p.ArticleNo = rdr["ArticleNo"].ToString();
                    p.ProductName = rdr["ProductName"].ToString();
                    p.CategoryName = rdr["CategoryName"].ToString();
                    p.ProductPrice = Convert.ToInt32(rdr["ProductPrice"]);
                    p.ImageURL = rdr["ImageURL"].ToString();
                    pList.Add(p);
                }
            }
            conn.Close();
        }
        public ActionResult AddProducts() 
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProducts(Products p)
        { 
            if (p.ProductName == string.Empty || p.ProductDescription == string.Empty || p.ArticleNo == string.Empty  || p.ProductPrice == 0 || p.ProductImage.FileName == string.Empty || p.CategoryID.Equals(null))
            {
                TempData["Message"] = "<script> alert('Please fill out the required fields!')  </script>";
            }
            else
            {
                //try
                //{
                   

                    if (p.ProductImage != null && p.ProductImage.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(p.ProductImage.FileName);
                        string imgpath = Path.Combine(Server.MapPath("~/Images/"), filename);
                        p.ProductImage.SaveAs(imgpath);
                    }
                    conn.Open();
                    cmd = new SqlCommand("INSERT INTO Products VALUES (@Article, @PName, @Pdescription, @pprice, @category , @Image)", conn);
                    cmd.Parameters.AddWithValue("@Article", p.ArticleNo);
                    cmd.Parameters.AddWithValue("@PName", p.ProductName);
                    cmd.Parameters.AddWithValue("@Pdescription", p.ProductDescription);
                    cmd.Parameters.AddWithValue("@pprice", p.ProductPrice);
                    cmd.Parameters.AddWithValue("@category", p.CategoryID);
                    
                    cmd.Parameters.AddWithValue("@Image", p.ProductImage.FileName);
                    cmd.ExecuteNonQuery();
                    TempData["Message"] = "<script> alert('Product Added Successfully!')  </script>";
                    conn.Close();
                //}
                //catch (Exception ex)
                //{
                //    TempData["Message"] = "<script>An Un-expected error occurred </script>";
                //}

            }
            return View();
        }
        public ActionResult AddCategories() 
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCategories(Categories c) 
        {
            if (c.CategoryName == string.Empty || c.CategoryDescription == string.Empty )
            {
                TempData["Message"] = "<script> alert('Please fill out the required fields!')  </script>";
            }
            else
            {
                try
                {


                    if (c.CategoryFile != null && c.CategoryFile.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(c.CategoryFile.FileName);
                        string imgpath = Path.Combine(Server.MapPath("~/Images/"), filename);
                        c.CategoryFile.SaveAs(imgpath);
                    }
                    conn.Open();
                    cmd = new SqlCommand("INSERT INTO Categories VALUES (@CName, @CDescription, @CImage)", conn);
                    cmd.Parameters.AddWithValue("@CName", c.CategoryName);
                    cmd.Parameters.AddWithValue("@CDescription", c.CategoryDescription);
                    
                    cmd.Parameters.AddWithValue("@CImage", c.CategoryFile.FileName);
                    cmd.ExecuteNonQuery();
                    TempData["Message"] = "<script> alert('Category Added Successfully!')  </script>";
                    conn.Close();
                }
                catch (Exception ex)
                {
                TempData["Message"] = "<script>An Un-expected error occurred </script>";
                }

            }
            
            return View();
        }
        public ActionResult AddAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddAdmin(Admin a)
        {
            if (a.loginEmail == string.Empty || a.adminPassword == string.Empty || a.adminPassword == string.Empty || a.adminPassword2 == string.Empty || a.adminPhone == string.Empty)
            {
                TempData["Message"] = "<script> alert('Please fill out the required fields!')  </script>";
            }
            else
            {
                if (a.adminPassword != a.adminPassword2)
                {
                    TempData["Message"] = "<script> alert('Both Password are different!.\nPlease confirm your password')  </script>";
                }
                else
                {
                    try
                    {
                        conn.Open();
                        cmd = new SqlCommand("INSERT INTO Admin VALUES (@name, @email, @password, @phone)", conn);
                        cmd.Parameters.AddWithValue("@name", a.adminName);
                        cmd.Parameters.AddWithValue("@email", a.loginEmail);
                        cmd.Parameters.AddWithValue("@password", a.adminPassword);
                        cmd.Parameters.AddWithValue("@phone", a.adminPhone);
                        cmd.ExecuteNonQuery();
                        TempData["Message"] = "<script> alert('New Admin Created Successfully!')  </script>";
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        TempData["Message"] = "<script>An Un-expected error occurred </script>";
                    }
                }
            }
            return View();
        }
        public void FetchOrders()
        {
            oList = new List<Order>();
            conn.Open();
            cmd = new SqlCommand("select * from OrdersData", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    Order o = new Order();
                    o.OrdeID = Convert.ToInt32(rdr["OrderID"]);
                    o.FirstName = rdr["FirstName"].ToString();
                    o.LastName = rdr["LastName"].ToString();
                    o.Country = rdr["Country"].ToString();
                    o.City = rdr["City"].ToString();
                    o.OrderDate = rdr["OrderDate"].ToString();
                    oList.Add(o);
                }
            }
            conn.Close();
        }
        public void FetchAdmins()
        {
            aList = new List<Admin>();
            conn.Open();
            cmd = new SqlCommand("select * from AdminData", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    Admin a = new Admin();
                    a.adminId = Convert.ToInt32(rdr["AdminID"]);
                    a.adminName = rdr["Admin_Name"].ToString();
                    a.loginEmail = rdr["Login_Email"].ToString();
                    a.adminPhone = rdr["Admin_Phone"].ToString();
                    aList.Add(a);
                }
            }
            conn.Close();
        }
        public void FetchOrderProducts(int id)
        {
            opList = new List<Products>();
            conn.Open();
            cmd = new SqlCommand("SelectOrderItems", conn);
            cmd.Parameters.AddWithValue("@orderID", id);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    Products p = new Products();
                    p.ProductID = Convert.ToInt32(rdr["ProductID"]);
                    p.ProductName = rdr["ProductName"].ToString();
                    p.ProductPrice = Convert.ToInt32(rdr["ProductPrice"]);
                    p.Quantity = Convert.ToInt32(rdr["Quantity"]);
                    opList.Add(p);
                }
            }
            conn.Close();
        }
        public void FetchReviews()
        {
            rList = new List<Review>();
            conn.Open();
            cmd = new SqlCommand("select * from ReviewsView", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    Review r = new Review();  
                    r.ReviewID = Convert.ToInt32(rdr["ReviewID"]);
                    r.FirstName = rdr["FirstName"].ToString();
                    r.LastName = rdr["LastName"].ToString();
                    r.CategoryId = Convert.ToInt32(rdr["CategoryId"]);
                    r.ProductName = rdr["ProductName"].ToString() ;
                    r.ReviewText = rdr["ReviewText"].ToString();
                    rList.Add(r);
                }
            }
            conn.Close();
        }
        public ActionResult DeleteAdmin(int id)
        {
            conn.Open();
            cmd = new SqlCommand("DeleteAdmin", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            TempData["Message"] = "<script> alert('Admin Deleted Successfully!')  </script>";
            conn.Close();
            return RedirectToAction("AllAdmins");
        }
        public ActionResult DeleteReview(int id)
        {

            conn.Open();
            cmd = new SqlCommand("DeleteReview", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            TempData["Message"] = "<script> alert('Review Deleted Successfully')  </script>";
            conn.Close();
            return RedirectToAction("Reveiws");

        }
    }
    
}