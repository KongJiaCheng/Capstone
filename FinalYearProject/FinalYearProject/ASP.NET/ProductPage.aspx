<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductPage.aspx.cs" Inherits="FinalYearProject.ASP.NET.ProductPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="CSS/styles.css" />
    <style>
        body {
    background: url('picture/Background.jpg') no-repeat center center fixed;
    color: white;
    font-size: large;
    }

    .product-list {
    display: grid;
    grid-template-columns: repeat(5, 1fr);
    gap: 20px; /* Space between items */
    }

    .product-item {
    background-color: rgba(255, 255, 255, 0.9); /* Slightly transparent white background */
    border: 1px solid #ddd;
    border-radius: 5px;
    padding: 20px;
    flex: 1 1 calc(33.333% - 20px);
    box-sizing: border-box;
    text-align: center;
    color: #333; /* Darker text for readability */
    }

    .product-item img {
        max-width: 100%;
        height: 100px;
        margin-bottom: 15px;
    }

    .product-item h3 {
        margin: 0 0 10px;
    }

    .product-item .price {
        font-size: 18px;
        color: #333;
    }
    
    .product-item .stock {
        font-size: 14px;
        color: #777;
    }

        .auto-style1 {
            margin-right: 0px;
        }

    .search-container {
    display: flex;
    align-items: center;
    }

    .search-container input[type="text"] {
    width: 50%;
    padding: 10px;
    font-size: 16px;
    border: 1px solid #ccc;
    }

.search-container button[type="button"] {
    width: 20%;
    padding: 10px;
    font-size: 16px;
    background-color: #4CAF50;
    color: #fff;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    }

.search-container select {
    padding: 10px;
    font-size: 16px;
    border: 1px solid #ccc;
    }

.search-container button[type="button"]:hover {
    background-color: #3e8e41;
    }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="search-container">
        <asp:TextBox ID="txtSearch" runat="server" placeholder="Search products..." />
        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
        <asp:DropDownList ID="FilterProduct" runat="server" AutoPostBack="true" OnSelectedIndexChanged="FilterProduct_SelectedIndexChanged">
            <asp:ListItem Value="All">All</asp:ListItem>
            <asp:ListItem Value="Laptop">Laptop</asp:ListItem>
            <asp:ListItem Value="Desktop">Desktop</asp:ListItem>
            <asp:ListItem Value="Accessories">Accessories</asp:ListItem>
        </asp:DropDownList>
    </div>

    <asp:Repeater ID="ProductRepeaterControl" runat="server">
    <ItemTemplate>
        <div class="product-item">

            <img src='<%# Eval("ImageURL") %>' alt='<%# Eval("ProductName") %>' />
            <h3><%# Eval("ProductName") %></h3>
            <p><%# Eval("ProductDescription") %></p>
            <span class="price">RM<%# Eval("Price") %></span><br/><span class="stock">Stock: <%# Eval("Stock") %></span><br/>
            <asp:Button 
            ID="btnAddToCart" 
            runat="server" 
            Text="Add to Cart" 
            OnCommand="AddToCart_Click" 
            CommandName="AddToCart" 
            CommandArgument='<%# Eval("ProductID") %>' 
            Enabled='<%# Convert.ToInt32(Eval("Stock")) > 0 %>' />

            <asp:Button 
            ID="btnRemoveFromCart" 
            runat="server"
            Text="Remove from Cart" 
            OnCommand="RemoveFromCart_Click" 
            CommandName="RemoveFromCart" 
            CommandArgument='<%# Eval("ProductID") %>' />
        </div>
        </div>
    </ItemTemplate>
</asp:Repeater>

</asp:Content>
