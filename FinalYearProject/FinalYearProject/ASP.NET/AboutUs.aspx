<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AboutUs.aspx.cs" Inherits="FinalYearProject.ASP.NET.AboutUs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<link rel="stylesheet" href="../CSS/styles.css">
<style>
    body {
        background: url('picture/Background.jpg') no-repeat center center fixed;
    }

    .greetings {
        position: relative;
        text-align: center;
    }

    .scroll {
        width: 100%; /* Adjust width as needed */
        height: auto; /* Maintain aspect ratio */
    }

    .text-overlay {
        position: absolute;
        top: 50%; /* Adjust as needed */
        left: 50%; /* Adjust as needed */
        transform: translate(-50%, -50%);
        background-color: rgba(255, 255, 255, 0.8); /* Adjust opacity and color as needed */
        padding: 20px;
        z-index: 1; /* Ensure text-overlay appears above other elements */
        max-width: 80%; /* Adjust width as needed */
    }

        .text-overlay span {
            font-size: 100px;
        }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="h3"><h3>About ByteBazaar</h3></div>
    <div class="text">
        <p>Greetings from ByteBazaar, your one-stop shop for everything tech related!</p>
        <p>Our mission at ByteBazaar is to power your digital lifestyle with innovative products and first-rate service.</p>
        <p>We provide dependable hardware for both novice and experienced gamers, programmers, and general users.</p>
        <p>Our team of professionals has carefully chosen the newest PCs, peripherals, and accessories to guarantee quality and functionality.</p>
        <p>As part of our dedication to client pleasure, we work hard to make sure your shopping experience is easy and fun.</p>
        <p>Take a look around our virtual lanes and allow ByteBazaar be your reliable guide through the ever changing technological landscape.</p>
    </div>
    <div class="greetings">
        <img class="scroll" src="picture/scroll.png" alt="Scroll">
        <div class="text-overlay">
            <p>We sincerely hope that you can find the reasonable price and the best quality product over ByteBazaar, your support is much appreciated!!</p>
            <span>&#128519;</span>
        </div>
    </div>
</asp:Content>
