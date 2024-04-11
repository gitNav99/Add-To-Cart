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
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindProducts();
            }
        }

        protected void BindProducts()
        {
            
            string connectionString = "Data Source=DESKTOP-QE1G5L2\\SQLEXPRESS;Initial Catalog=dbSalon;Integrated Security=True";

          
            string query = "SELECT ProductId, ProductName, Price FROM Products";

    
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

                    
                    GridViewProducts.DataSource = dtProducts;
                    GridViewProducts.DataBind();
                }
            }

        }

        protected void GridViewProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                
                int productId = Convert.ToInt32(e.CommandArgument);
                ShoppingCart.AddToCart(productId);
               
            }
        }
    }
}