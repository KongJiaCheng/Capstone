<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FinalYearProject.ASP.NET.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.4.1/dist/css/bootstrap.min.css" 
          integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" 
          crossorigin="anonymous"/>
    <title>Login Form</title>
    <style>
        body {
            background-image: url(picture/Background.jpg);
            background-repeat: no-repeat;
            background-size: cover;
            color: white;
            font-size: large;
        }
        .container {
            margin-top: 100px;
            max-width: 400px;
            background-color: rgba(0, 0, 0, 0.7);
            padding: 20px;
            border-radius: 10px;
        }
        .form-control {
            margin-bottom: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Login Form" CssClass="h3"></asp:Label>
            </div>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Username:" CssClass="h5"></asp:Label>
                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="TextBox1" ErrorMessage="Username is required" 
                    CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group">
                <asp:Label ID="Label3" runat="server" Text="Password:" CssClass="h5"></asp:Label>
                <asp:TextBox ID="TextBox2" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="TextBox2" ErrorMessage="Password is required" 
                    CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="TextBox2" 
                    OnServerValidate="ValidatePassword" 
                    ErrorMessage="Password must be at least 12 characters long and include one uppercase letter, one lowercase letter, one digit, and one special character." 
                    CssClass="text-danger" Display="Dynamic"></asp:CustomValidator>
            </div>
            <div class="form-group text-center">
                <asp:Button ID="ButtonLogin" runat="server" BackColor="Green" BorderColor="Black" 
                    BorderWidth="4px" Font-Bold="True" ForeColor="White" Height="62px" 
                    Text="Login" Width="145px" OnClick="ButtonLogin_Click" CssClass="btn btn-success" />
                <asp:Button ID="ButtonCancel" runat="server" BackColor="Red" BorderColor="Black" 
                    BorderWidth="4px" Font-Bold="True" ForeColor="White" Height="62px" Text="Cancel" 
                    Width="174px" OnClick="ButtonCancel_Click" CssClass="btn btn-danger" />
                <asp:Button ID="ButtonBack" runat="server" BackColor="#66FF33" Font-Bold="True" 
                    Height="57px" Text="Back?" OnClick="ButtonBack_Click" />
            </div>
            <div class="form-group">
                <!-- Error Label to display login errors -->
                <asp:Label ID="lblError" runat="server" CssClass="text-danger" 
                    EnableViewState="false" Visible="false"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
