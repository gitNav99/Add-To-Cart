<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DummyDashboard.aspx.cs" Inherits="Add_to_cart.DummyDashboard" %>

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
    <form id="DDashBoard" runat="server">
        
            <nav>
            <a href="#">Home</a>

            <a href="Products.aspx">Products</a>
            <a href="AddToCart.aspx">Add to Cart</a>
        </nav>
     
    </form>
</body>
</html>
