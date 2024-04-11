using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Add_to_cart
{
    public partial class AddToCart : System.Web.UI.Page
    {
        public decimal tgp;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCartItems();
            }
        }
        private string CreateToken(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }


        protected void BindCartItems()
        {
            List<ShoppingCart.CartItem> cartItems = ShoppingCart.GetCartItems();
            if (cartItems.Count > 0)
            {
                string connectionString = "Data Source=DESKTOP-QE1G5L2\\SQLEXPRESS;Initial Catalog=dbSalon;Integrated Security=True";
                string query = "SELECT ProductId, ProductName, Price FROM Products WHERE ProductId IN ({0})";

                string productIdList = string.Join(",", cartItems.Select(item => item.ProductId));

                query = string.Format(query, productIdList);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        DataTable dtProducts = new DataTable();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dtProducts.Load(reader);
                        }
                        dtProducts.Columns.Add("Quantity", typeof(int));
                        dtProducts.Columns.Add("TotalPrice", typeof(decimal));

                        foreach (DataRow row in dtProducts.Rows)
                        {
                            int productId = Convert.ToInt32(row["ProductId"]);
                            var cartItem = cartItems.FirstOrDefault(item => item.ProductId == productId);
                            if (cartItem != null)
                            {
                                row["Quantity"] = cartItem.Quantity;
                                decimal price = Convert.ToDecimal(row["Price"]);
                                int quantity = Convert.ToInt32(row["Quantity"]);
                                decimal totalPrice = price * quantity;
                                row["TotalPrice"] = totalPrice;
                                cartItem.TotalPrice = totalPrice;
                              //   tgp = cartItem.TotalPrice;
                            }
                            else
                            {
                                row["Quantity"] = 0;
                                row["TotalPrice"] = 0;
                                     
                            }
                        }
                        GridViewCart.DataSource = dtProducts;
                        GridViewCart.DataBind();
                    }
                }
                decimal grandTotal = CalculateGrandTotal();
                tgp = grandTotal;
                lblTotalPay.Text = grandTotal.ToString("C");
            }
            else
            {
                
                 lblEmptyCartMessage.Text = "Your cart is empty.";
            }
            
        }
        protected void GridViewCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
            Response.Write("GridRowCmd");
            if (e.CommandName == "DecreaseQuantity")
            {
                int productId = Convert.ToInt32(e.CommandArgument);
                
                ShoppingCart.DecreaseQuantity(productId);
                
                BindCartItems(); 
               // Response.Write("Binded Dec");
            }
            else if (e.CommandName == "IncreaseQuantity")
            {
                int productId = Convert.ToInt32(e.CommandArgument);
                ShoppingCart.IncreaseQuantity(productId);
                BindCartItems(); 
            }
            else if (e.CommandName == "deleteProduct")
            {
                HttpContext.Current.Response.Write("deleteP init!\n");
                int productId = Convert.ToInt32(e.CommandArgument);
                int productIdToDelete;
                if (int.TryParse(e.CommandArgument.ToString(), out productIdToDelete))
                {
                    ShoppingCart.Delete(productIdToDelete);
                }
                
                BindCartItems();
            }
        }
        protected decimal CalculateGrandTotal()
{
    decimal grandTotal = 0;
    List<ShoppingCart.CartItem> cartItems = ShoppingCart.GetCartItems();

    foreach (var cartItem in cartItems)
    {
        grandTotal += cartItem.TotalPrice;
    }

    return grandTotal;
}

        protected void btnBuy_Click(object sender, EventArgs e)
        {
            decimal grandTotal = CalculateGrandTotal();

            
            if (grandTotal > 0)
            {
                Response.Redirect($"MakePayment.aspx?grandTotal={grandTotal}");
            }
            else
            {
                 ScriptManager.RegisterStartupScript(this, GetType(), "EmptyCartAlert", "alert('Your cart is empty.');", true);
            }


        }
    }

    public static class ShoppingCart
    {
        private const string CartSessionKey = "CartItems";
        
        public static List<CartItem> GetCartItems()
        {
            var cartItems = HttpContext.Current.Session[CartSessionKey] as List<CartItem>;
            if (cartItems == null)
            {
                cartItems = new List<CartItem>();
                HttpContext.Current.Session[CartSessionKey] = cartItems;
            }
            return cartItems;
        }

        public static void AddToCart(int productId)
        {
            var cartItems = GetCartItems();
            var existingItem = cartItems.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                var newItem = new CartItem(productId, 1);
                cartItems.Add(newItem);
            }

        }

        public static void DecreaseQuantity(int productId)
        {
            
            var cartItems = GetCartItems();
            var existingItem = cartItems.FirstOrDefault(item => item.ProductId == productId);
            HttpContext.Current.Response.Write("\nexistin Item: ---!"+existingItem+"$$$");
            if (existingItem != null)
            {
                HttpContext.Current.Response.Write("  insideDec!");
                if (existingItem.Quantity > 1)
                {
                    existingItem.Quantity--; 
                }
                else
                {
                    
                    cartItems.RemoveAll(item => item.ProductId == productId);
                }
            }
            HttpContext.Current.Session[CartSessionKey] = cartItems;
            
        }
        public static void IncreaseQuantity(int productId)
        {
            var cartItems = GetCartItems();
            var existingItem = cartItems.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            HttpContext.Current.Session[CartSessionKey] = cartItems;

        }
        public static void Delete(int productId)
        {
            //HttpContext.Current.Response.Write("inside delete() init!\n");
            var cartItems = GetCartItems();
            var itemToRemove = cartItems.FirstOrDefault(item => item.ProductId == productId);
            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
            }
            HttpContext.Current.Session[CartSessionKey] = cartItems;

        }



        public class CartItem
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }

            public decimal TotalPrice { get; set; }
            public CartItem(int productId, int quantity)
            {
                ProductId = productId;
                Quantity = quantity;
                TotalPrice = 0;
            }
        }


    }
}
