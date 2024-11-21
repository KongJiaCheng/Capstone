<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="FinalYearProject.ASP.NET.HomePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
    body {
        background: url('picture/Background.jpg') no-repeat center center fixed;
    }
    </style>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="CSS/styles.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <div id="Paimon">
        <div class="text-overlay">
            Welcome to ByteBazaar, anything I can help you with?Click the button below to browse our products!
        </div>
            <img src="picture/Paimon.png" alt="Paimon">
        <asp:Button ID="button" runat="server" Text="Click here for browsing products!" Onclick="Button_Click"/>
    </div>
</asp:Content>
