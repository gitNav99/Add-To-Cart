using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Add_to_cart
{
    public partial class MakePayment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["grandTotal"]))
                {
                    decimal grandTotal = decimal.Parse(Request.QueryString["grandTotal"]);
                    string script = $@"
                var amountElement = document.querySelector('[data-amount]');
                if (amountElement) {{
                    amountElement.setAttribute('data-amount', '{(grandTotal * 100):0}');
                }}";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SetAmountScript", script, true);
                }
                else
                {
                    lblError.Text = "Something went wrong!";
                }

                form1.Action = ResolveUrl("~/SessionPayment.aspx?status=success&id=" + Request.QueryString["id"]);
            }
        }

        protected void hiddenField_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}