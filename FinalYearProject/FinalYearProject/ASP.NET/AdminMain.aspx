<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminMain.aspx.cs" Inherits="FinalYearProject.ASP.NET.AdminMain" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="CSS/style.css" />
    <style>
        body {
            background: url(picture/Background.jpg);
        }

        .card-container {
            display: flex;
            flex-wrap: wrap; /* Allows wrapping into multiple rows */
            justify-content: center;
            gap: 20px;
            margin-top: 20px;
        }

        .card {
            border: 1px solid #ccc;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
            text-align: center;
            width: 250px;
            background-color: white;
        }

        .card img {
            border-radius: 10px;
        }

        .card h2 {
            margin-bottom: 15px;
        }
    </style>

    <center>
        <div class="card-container">
            <div class="card">
                <a href="AdminPage.aspx">
                    <h2>Product Details</h2><br />
                    <img src="Picture/cart.jpg" alt="Image 1" style="width: 200px; height: 200px;" />
                </a>
            </div>

            <div class="card">
                <a href="AdminUserInfo.aspx">
                    <h2>User Details</h2><br />
                    <img src="Picture/User.png" alt="Image 2" style="width: 200px; height: 200px;" />
                </a>
            </div>

            <div class="card">
                <a href="AdminOrderInfo.aspx">
                    <h2>Order Details</h2><br />
                    <img src="Picture/Logo.jpg" alt="Image 3" style="width: 200px; height: 200px;" />
                </a>
            </div>

            <div class="card">
                <a href="AdminPaymentInfo.aspx">
                    <h2>Payment Info</h2><br />
                    <img src="Picture/Paimon.png" alt="Image 5" style="width: 200px; height: 200px;" />
                </a>
            </div>

            <div class="card">
                <a href="AdminSignUp.aspx">
                    <h2>Newly Admin? Sign Up Here!</h2><br />
                    <img src="Picture/Paimon.png" alt="Image 4" style="width: 200px; height: 200px;" />
                </a>
            </div>
        </div>
    </center>
</asp:Content>