<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MakePayment.aspx.cs" Inherits="Add_to_cart.MakePayment" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
</head>

<body onload="document.getElementById('razorpay-payment-button').click();">
    <form id="form1" runat="server" method="post">
        <script src="https://checkout.razorpay.com/v1/checkout.js"
                data-key="rzp_test_qS2SEfjC9Ww2XJ"
                  data-amount="<%= (int)(decimal.Parse(Request.QueryString["grandTotal"]) * 100) %>"

        data-currency="INR"
        data-buttontext="Pay ₹<%= Request.QueryString["grandTotal"] %>"
        data-name="Glamm Salon"
        data-description=""
        data-theme.color="#F37254"
        data-prefill.name=""
        data-prefill.email=""
        data-order_id="">
        </script>
        <asp:HiddenField ID="hiddenField" runat="server" OnValueChanged="hiddenField_ValueChanged" />

    <script runat="server">
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                decimal grandTotal = decimal.Parse(Request.QueryString["grandTotal"]);
                //lblGrandTotal.Text ="gt2:"+ grandTotal;
                hiddenField.Value = "Hidden Element";
                form1.Action = ResolveUrl("~/SessionPayment.aspx?status=success&id=" + Request.QueryString["id"]);
            }
        }
    </script>
        <asp:Label ID="lblGrandTotal" runat="server"></asp:Label>
        <br />
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </form>

    </body>
</html>



