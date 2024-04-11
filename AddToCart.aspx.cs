using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Add_to_cart
{
    public partial class AddToCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCartItems();
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
                                row["TotalPrice"] = price * quantity;
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
            }
            else
            {
                
                 lblEmptyCartMessage.Text = "Your cart is empty.";
            }
            
        }
        protected void GridViewCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
           // Response.Write("GridRowCmd");
            if (e.CommandName == "DecreaseQuantity")
            {
                int productId = Convert.ToInt32(e.CommandArgument);
                // Response.Write("Callin Dec");
                ShoppingCart.DecreaseQuantity(productId);
                //Response.Write("Callin Dec");
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
                    // Implement deletion logic here
                    ShoppingCart.Delete(productIdToDelete);
                  //  BindCartGridView(); // Call a method to rebind the GridView
                }
                
                BindCartItems();
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
                HttpContext.Current.Response.Write("\nYeah CartItem called---'!");
                cartItems = new List<CartItem>();
                HttpContext.Current.Session[CartSessionKey] = cartItems;
            }
            return cartItems;
        }

        public static void AddToCart(int productId)
        {
            HttpContext.Current.Response.Write("inside!addToCart");
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
            HttpContext.Current.Response.Write("inside!DecQTY  ");
            var cartItems = GetCartItems();
            var existingItem = cartItems.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                HttpContext.Current.Response.Write("  insideDec!");
                if (existingItem.Quantity > 1)
                {
                    HttpContext.Current.Response.Write("  Decreasin'!");
                    existingItem.Quantity--; // Decrease the quantity by 1
                }
                else
                {
                    HttpContext.Current.Response.Write("Decreasin at Zerooo'!");
                    cartItems.Remove(existingItem);
                   
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

            public CartItem(int productId, int quantity)
            {
                ProductId = productId;
                Quantity = quantity;
            }
        }


    }
}
