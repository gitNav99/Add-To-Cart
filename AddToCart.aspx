<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddToCart.aspx.cs" Inherits="Add_to_cart.AddToCart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        nav {
            background-color: #333;
            overflow: hidden;
        }

        nav a {
            float: left;
            display: block;
            color: white;
            text-align: center;
            padding: 14px 16px;
            text-decoration: none;
        }

        nav a:hover {
            background-color: #111;
        }
    </style>

</head>
<body>
    <form id="addToCart" runat="server">
        <nav>
            <a href="DummyDashboard.aspx">DashBoard</a>
            <a href="Products.aspx">Products</a>
            <a href="AddToCart.aspx">Add to Cart</a>
        </nav>
        <div>
             <h2>Shopping Cart</h2>
            <asp:GridView ID="GridViewCart" runat="server" AutoGenerateColumns="False" CssClass="gridview" OnRowCommand="GridViewCart_RowCommand">
                <Columns>
                    
        
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                    <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Button ID="btnMinus" runat="server" Text="-" CommandName="DecreaseQuantity" CommandArgument='<%# Eval("ProductId") %>' />
                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                            <asp:Button ID="btnAdd" runat="server" Text="+" CommandName="IncreaseQuantity" CommandArgument='<%# Eval("ProductId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="totalPrice" HeaderText="Total Price" DataFormatString="{0:C}" />
                     <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="deleteProduct" CommandArgument='<%# Eval("ProductId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
             <asp:Label ID="lblTotalP" runat="server" Text="Total Payable Amount : "></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
             <asp:Label ID="lblTotalPay" runat="server"></asp:Label>
             <br />
             <asp:Label ID="lblEmptyCartMessage" runat="server" ForeColor="Red"></asp:Label>
        </div>
        <asp:Button ID="btnBuy" runat="server" BackColor="Purple" ForeColor="White" Text="Proceed to checkout" OnClick="btnBuy_Click" />
    </form>
</body>
</html>
