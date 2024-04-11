<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="Add_to_cart.Dashboard" %>

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
    <form id="formDashboard" runat="server">
         <nav>
             <a href="DummyDashboard.aspx">Dashboard</a>
            <a href="Products.aspx">Products</a>
            <a href="AddToCart.aspx">Add to Cart</a>
        </nav>
        <div>

            <asp:GridView ID="GridViewProducts" runat="server" AutoGenerateColumns="False" OnRowCommand="GridViewProducts_RowCommand">
                <Columns>
                    <asp:BoundField DataField="ProductId" HeaderText="Product ID" />
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                    <asp:BoundField DataField="Price" HeaderText="Price" />
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart" CommandName="AddToCart" CommandArgument='<%# Eval("ProductId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>

</body>
</html>
