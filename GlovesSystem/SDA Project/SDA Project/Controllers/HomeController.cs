using SDA_Project.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using SDA_Project.Singleton;

namespace SDA_Project.Controllers
{
    public class HomeController : Controller
    {

        SqlConnection conn = new SqlConnection("Data Source=MHT;Initial Catalog=GlovesSystemDb;Integrated Security=True;Encrypt=False");
        SqlCommand cmd;
        List<Categories> categoriesList;
        List<Products> productsList;
        CartProducts _cartProducts;
        List<Products> _cartItems;
        public ActionResult HomePage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult HomePage(Customer c)
        {
            if (c.UserName == null || c.Email == null || c.PhoneNumber == null || c.UserPassword == null || c.ConfirmPassword == null)
            {
                TempData["Message"] = "<script> alert('Please Fill all the Values!')  </script>";
            }
            else
            {
                if (c.UserPassword != c.ConfirmPassword)
                {
                    TempData["Message"] = "<script> alert('Invalid Confirm Password!')  </script>";
                }
                else
                {
                    //try
                    //{
                    
                    conn.Open();
                    cmd = new SqlCommand("AddCustomerAndCreateCart", conn);

                    cmd.Parameters.AddWithValue("@UserName", c.UserName);
                    cmd.Parameters.AddWithValue("@Email", c.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", c.PhoneNumber);
                    cmd.Parameters.AddWithValue("@UserPassword", c.UserPassword);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    TempData["Message"] = "<script> alert('New Customer Created Successfully!')  </script>";
                    conn.Close();
                    return RedirectToAction("SignIn");
                    //}
                    //catch (Exception ex)
                    //{
                    //    TempData["Message"] = "<script>An Un-expected error occurred </script>";
                    //}
                }
            }
            return View();
        }
        public ActionResult LoginOption()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(Customer c)
        {

            if (c.UserName == null || c.UserPassword == null)
            {
                TempData["Message"] = "<script> alert('Please fill out the required fields!')  </script>";
            }
            else
            {
                
                conn.Open();
                cmd = new SqlCommand("select CustomerID, UserName, UserPassword from Customer where UserName = @UserName AND UserPassword = @UserPassword", conn);
                cmd.Parameters.AddWithValue("@UserName", c.UserName);
                cmd.Parameters.AddWithValue("@UserPassword", c.UserPassword);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    c.CustomerID = Convert.ToInt32(sdr["CustomerID"]);
                    Session["UserInfo"] = c;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "<script> alert('Invalid Credential!')  </script>";
                    return View();
                }

            }
            conn.Close();

            return View();
        }
        public ActionResult SignUp()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Categories() 
        {
            FetchCategories();
            return View(categoriesList); 
        }
        public void FetchCategories()
        {
            categoriesList = new List<Categories>();
            
            conn.Open();
            cmd = new SqlCommand("select * from Categories", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    Categories c = new Categories();
                    c.CategoryID = Convert.ToInt32(rdr["CategoryID"]);
                    c.CategoryName = rdr["CategoryName"].ToString();
                    c.CategoryDescription = rdr["CategoryDescription"].ToString();
                    c.CategoryImage = rdr["CategoryImage"].ToString();
                    categoriesList.Add(c);
                }
            }
            conn.Close();
        }
        public ActionResult Products(int id) 
        {
            FetchProducts(id);
            return View(productsList);
        }
        public void FetchProducts(int id)
        {
            productsList = new List<Products>();
            
            conn.Open();
            cmd = new SqlCommand("select ProductID, ArticleNo, ProductName, ProductDescription, ProductPrice, ImageURL from Products where CategoryID = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    Products p = new Products();
                    p.ProductID = Convert.ToInt32(rdr["ProductID"]);
                    p.ArticleNo = rdr["ArticleNo"].ToString();
                    p.ProductName = rdr["ProductName"].ToString();
                    p.ProductDescription = rdr["ProductDescription"].ToString();
                    p.ProductPrice = Convert.ToInt32(rdr["ProductPrice"]);
                    p.ImageURL = rdr["ImageURL"].ToString();
                    productsList.Add(p);
                }
            }
            conn.Close();
        }
        //public ActionResult Productstocart(CartProducts cp)
        //{
        //    Customer user = Session["UserInfo"] as Customer;
        //    int customerId = user.CustomerID;
            
        //    conn.Open();
        //    cmd = new SqlCommand("insert into CartProducts Values (@ProductID, @ProductName, @ProductPrice, @ImageURL, @Quantity, @CartID, @CustomerID)", conn);
        //    cmd.Parameters.AddWithValue("@ProductID", cp.singleProd.ProductID);
        //    cmd.Parameters.AddWithValue("@ProductName", cp.singleProd.ProductName);
        //    cmd.Parameters.AddWithValue("@ProductPrice", cp.singleProd.ProductPrice);
        //    cmd.Parameters.AddWithValue("@ImageURL", cp.singleProd.ImageURL);
        //    cmd.Parameters.AddWithValue("@Quantity", cp.singleProd.Quantity);
        //    cmd.Parameters.AddWithValue("@CartID", customerId);
        //    cmd.Parameters.AddWithValue("@CustomerID", customerId);
        //    cmd.ExecuteNonQuery();
        //    TempData["Message"] = "<script> alert('Product Added Successfully')  </script>";
        //    conn.Close();
        //    return RedirectToAction("Cart");
        //}

        public ActionResult Productstocart(int id,int Quantity)
        {
            Customer user = Session["UserInfo"] as Customer;
            int customerId = user.CustomerID;
            conn.Open();
            cmd = new SqlCommand("InsertCartItems", conn);
            cmd.Parameters.AddWithValue("@prodId", id);
            cmd.Parameters.AddWithValue("@prodQuantity", Quantity);
            cmd.Parameters.AddWithValue("@cartId", customerId);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            TempData["Message"] = "<script> alert('Product Added Successfully')  </script>";
            conn.Close();
            return RedirectToAction("Cart");
        }

        //public ActionResult Testimonial() 
        //{
        //    return View();
        //}
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(Contact c)
        {
            if (c.ContactName == null || c.PhoneNumber == null || c.ContactEmail == null || c.ContactMessage == null)
            {
                TempData["Message"] = "<script> alert('Please fill the required fields!')  </script>";
            }
            else
            {
                
                conn.Open();
                    cmd = new SqlCommand("Insert into Contact values (@Name, @Phone, @Email, @Message)", conn);
                    cmd.Parameters.AddWithValue("@Name", c.ContactName);
                    cmd.Parameters.AddWithValue("@Phone", c.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", c.ContactEmail);
                    cmd.Parameters.AddWithValue("@Message", c.ContactMessage);
                    cmd.ExecuteNonQuery();
                    TempData["Message"] = "<script> alert('Your Message has been sent successfully!')  </script>";
                    conn.Close();           
            }
            return View();
        }
        public ActionResult AdminLogin() 
        {
            return View();
        }
        [HttpPost]

        public ActionResult AdminLogin(Admin A)
        {
            if (string.IsNullOrEmpty(A.loginEmail) || string.IsNullOrEmpty(A.adminPassword))
            {
                TempData["Message"] = "<script>alert('Please fill out the required fields!')</script>";
                return View();
            }

            Singleton admin = Singleton.GetInstance();
            bool isAuthenticated = admin.LoginAdmin(A.loginEmail, A.adminPassword);

            if (isAuthenticated)
            {
                Session["adminLoggedIn"] = true;
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                TempData["Message"] = "<script>alert('Invalid login credentials!')</script>";
                return View();
            }
        }
        

        //Cart Functions-------------------->


        public ActionResult Cart()
        {
            Customer user = Session["UserInfo"] as Customer;
            int customerId = user.CustomerID;
            _cartProducts = new CartProducts();
            _cartProducts.CartID = customerId;
            _cartProducts.CustomerID = customerId;
            GetCartItems(customerId);
            _cartProducts._Products = _cartItems;
            //ViewBag.TotalBill = CalculateTotalBill(cartItems);
            return View(_cartProducts);
        }

        private void GetCartItems(int customerId)
        {
            _cartItems = new List<Products>();

            using (conn)
            {
                conn.Open();
                string query = "CartItems";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {       
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    int totalPrice = 0;
                     
                     //CartProducts cp = new CartProducts();
                    while (reader.Read())
                    {
                        Products myCart = new Products();
                        int sinPro = Convert.ToInt32(reader["ProductPrice"]);
                        int sinQut = Convert.ToInt32(reader["Quantity"]);

                        myCart.ProductID = Convert.ToInt32(reader["ProductID"]);
                        myCart.ProductName = reader["ProductName"].ToString();
                        myCart.ImageURL = reader["ImageURL"].ToString();
                        myCart.ProductPrice = sinPro * sinQut;
                        totalPrice += myCart.ProductPrice;
                        //myCart.p = reader["ProductID"].ToString();
                        _cartItems.Add(myCart);

                    }
                   _cartProducts.totalPrice = totalPrice;



                }
            }

        }

        

        //------------------------------->






        //Shipping Functions------------------>
        public ActionResult Shipping() { 
            return View();
        }
        [HttpPost]
        public ActionResult Shipping(Shipping S)
        {
            if (S.FirstName == null || S.LastName == null || S.ShippingAddress == null || S.Country == null || S.ZipCode == null || S.City == null || S.State == null || S.Email == null || S.PhoneNumber == null)
            {
                TempData["Message"] = "<script> alert('Please fill the required fields!')  </script>";
            }
            else
            {
                Customer user = Session["UserInfo"] as Customer;
                int customerId = user.CustomerID;

                
                conn.Open();
                    cmd = new SqlCommand("Insert into Shipping values (@FName, @LName, @Address, @Country, @Zipcode, @City, @State, @Email, @Phone, @CartID)", conn);
                    cmd.Parameters.AddWithValue("@FName", S.FirstName);
                    cmd.Parameters.AddWithValue("@LName", S.LastName);
                    cmd.Parameters.AddWithValue("@Address", S.ShippingAddress);
                    cmd.Parameters.AddWithValue("@Country", S.Country);
                    cmd.Parameters.AddWithValue("@ZipCode", S.ZipCode);
                    cmd.Parameters.AddWithValue("@City", S.City);
                    cmd.Parameters.AddWithValue("@State", S.State);
                    cmd.Parameters.AddWithValue("@Email", S.Email);
                    cmd.Parameters.AddWithValue("@Phone", S.PhoneNumber);
                    cmd.Parameters.AddWithValue("@CartID", customerId);
                cmd.ExecuteNonQuery();
                    TempData["Message"] = "<script> alert('Shipping Address Recorded Successfully!')  </script>";
                    return RedirectToAction("Payment", "Home");


               
            }
            return View();
        }
        //------------------------------->
        //Payment------------------------>
        public ActionResult Payment() { 
            return View();
        }
        [HttpPost]
        public ActionResult Payment(Payment P)
        {
            if (P.CardNumber == null || P.OwnerName == null || P.ExpiryDate == null || P.CVV == null)
            {
                TempData["Message"] = "<script> alert('Please fill the required fields!')  </script>";
            }
            else
            {
                Customer user = Session["UserInfo"] as Customer;
                int customerId = user.CustomerID;
                //try
                //{   
                    conn.Open();
                    cmd = new SqlCommand("AddPaymentWithOrders", conn);
                    cmd.Parameters.AddWithValue("@CardNumber", P.CardNumber);
                    cmd.Parameters.AddWithValue("@Owner", P.OwnerName);
                    cmd.Parameters.AddWithValue("@Expiry", P.ExpiryDate);
                    cmd.Parameters.AddWithValue("@CVV", P.CVV);
                    cmd.Parameters.AddWithValue("@CartID", customerId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    TempData["Message"] = "<script> alert('Your Order Placed Successfully')  </script>";
                    conn.Close();
                    insertOrderProducts(customerId);
                   
                //}
                //catch (Exception ex)
                //{
                //    TempData["Message"] = "<script> alert('An unexpected error occured') "+ex.Message+" </script>";
                //}
            }
            return View();
        }

        //Review Functions----------------------->
        public ActionResult Review(){
            var Categories = GetCategories();
            ViewBag.Categories = new SelectList(Categories, "CategoryID", "CategoryName");
            return View();
        }

        [HttpPost]
        public ActionResult Review(Review model)
        {
            //try
            //{
            
            conn.Open();
                cmd = new SqlCommand("INSERT INTO Reviews VALUES (@FirstName, @LastName, @Email, @ContactNo, @CategoryId, @ProductId, @ReviewText)", conn);
                    cmd.Parameters.AddWithValue("@FirstName", model.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", model.LastName);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@ContactNo", model.ContactNo);
                    cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
                    cmd.Parameters.AddWithValue("@ProductId", model.ProductId);
                    cmd.Parameters.AddWithValue("@ReviewText", model.ReviewText);
                    cmd.ExecuteNonQuery();
                TempData["Message"] = "<script> alert('Your Review has been recorded successfully!')  </script>";

                conn.Close();

            //}
            //catch (Exception ex)
            //    {
            //    TempData["Message"] = "<script> alert('An unexpected error occured') " + ex.Message + " </script>";
            //    }

            return RedirectToAction("Review");
        }

        [HttpPost]
        public JsonResult GetProducts(int CategoryId)
        {
            var Products = GetProductsByCategory(CategoryId);
            //----------------------------------------
            ViewBag.Products = new SelectList(Products, "ProductID", "ProductName");
            //----------------------------------------
            return Json(Products);
        }

        private List<Categories> GetCategories()
        {
            var Categories = new List<Categories>();
            
            conn.Open();
            cmd = new SqlCommand("SELECT CategoryID, CategoryName FROM Categories", conn);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Categories.Add(new Categories { CategoryID = sdr.GetInt32(0), CategoryName = sdr.GetString(1) });
            }
            conn.Close();
            return Categories;
        }

        private List<Products> GetProductsByCategory(int CategoryId)
        {
            var Products = new List<Products>();
            
            {
                conn.Open();
                cmd = new SqlCommand("SELECT ProductID, ProductName FROM Products WHERE CategoryID = @CategoryId", conn);
                cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Products.Add(new Products { ProductID = reader.GetInt32(0), ProductName = reader.GetString(1) });
                }
            }
            return Products;
        }


        //---------------------------------->


        public void getProducts()
        {
            conn.Open();
            cmd = new SqlCommand("select ProductName,ProductPrice, ImageURL from Products", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
        }
        public void insertOrderProducts(int custId) {
            List<Products> pList = new List<Products>();
            conn.Open();
            cmd = new SqlCommand("select * from CartProducts where CartID = @CUSTID", conn);
            cmd.Parameters.AddWithValue("@CUSTID", custId);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows) {
                while (sdr.Read()) {
                    Products p = new Products();
                    p.ProductID = Convert.ToInt32(sdr["ProductID"]);
                    p.ProductName = sdr["ProductName"].ToString();
                    p.ProductPrice = Convert.ToInt32(sdr["ProductPrice"]);
                    p.ImageURL = sdr["ImageURL"].ToString();
                    p.Quantity = Convert.ToInt32(sdr["Quantity"]);
                    pList.Add(p);
                }
            }
            conn.Close();
            CartProducts cp = new CartProducts();
            cp._Products = pList;
            cp.CustomerID = custId;
            insertIntoOrderProducts(cp);
        }
        public void insertIntoOrderProducts(CartProducts cp) {
            conn.Open();
            foreach (var singleProd in cp._Products) { 
                cmd = new SqlCommand("InsertOrderItems", conn);
                cmd.Parameters.AddWithValue("@prodId", singleProd.ProductID);
                cmd.Parameters.AddWithValue("@prodName", singleProd.ProductName);
                cmd.Parameters.AddWithValue("@prodPrice", singleProd.ProductPrice);
                cmd.Parameters.AddWithValue("@imgUrl", singleProd.ImageURL);
                cmd.Parameters.AddWithValue("@qty", singleProd.Quantity);
                cmd.Parameters.AddWithValue("@cartId", cp.CustomerID);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            conn.Close();

        }
        public ActionResult DeleteCartProduct(int id)
        {
            conn.Open();
            cmd = new SqlCommand("DeleteCartProd", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            TempData["Message"] = "<script> alert('Product Removed from Cart Successfully!')  </script>";
            conn.Close();
            return RedirectToAction("Cart");
        }
    }
}