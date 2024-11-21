<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ContactUs.aspx.cs" Inherits="FinalYearProject.ASP.NET.ContactUs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Online Computer Shop</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <link rel="stylesheet" href="CSS/styles.css">
    <style>
    body {
        background: url('picture/Background.jpg') no-repeat center center fixed;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="h3"><h3>Contact Us</h3></div>
<div class="text-overlay">
    <p>You can contact us by using the following below!!</p>
    <ul>
        <li><a href="https://www.facebook.com/profile.php?id=61561396232966&is_tour_dismissed"><i class="fa fa-facebook"></i></a>Facebook: ByteBazaar Official Facebook</li>
        <li><a href="https://www.instagram.com/byte_bazaar2024?utm_source=ig_web_button_share_sheet&igsh=ZDNlZDc0MzIxNw=="><i class="fa fa-instagram"></i></a>Instagram: ByteBazaar Official Instagram</li>
        <li><a href="https://wa.link/os4pgz"><i class="fa fa-whatsapp"></i></a>Admin Whatsapp link</li>
    </ul>
    <p>PS: do register first before contact us!!</p>
</div>
</asp:Content>
