<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="signup.aspx.cs" Inherits="FinalYearProject.ASP.NET.signup" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.4.1/dist/css/bootstrap.min.css" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous"/>
    <title>Sign Up Form</title>
    <style>
        body {
            background-image: url(picture/Background.jpg);
            background-repeat: no-repeat;
            background-size: cover;
            color: white;
            font-size: large;
        }
        .form-group {
            margin-bottom: 15px;
        }
        .error-message {
            color: red;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1><strong>Welcome to ByteBazaar Register Website!! Please register to proceed!</strong></h1>
        &nbsp
        <button>
            <b>
                <a href="HomePage.aspx">Back?</a>
            </b>
        </button>

        <div class="form-group">
            <label>Name:</label>
            <asp:TextBox ID="name" runat="server" CssClass="auto-style1" Width="848px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="name"
                ErrorMessage="Please enter your name" CssClass="error-message"></asp:RequiredFieldValidator>
        </div>

        <div class="form-group">
            <label>Email:</label>
            <asp:TextBox ID="email" runat="server" CssClass="auto-style1" Width="848px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="email"
                ErrorMessage="Please enter your email" CssClass="error-message"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="email"
                ErrorMessage="Please enter the correct email address format" CssClass="error-message"
                ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"></asp:RegularExpressionValidator>
        </div>

        <div class="form-group">
            <label>Date of Birth:</label>
            <asp:DropDownList ID="day" runat="server" CssClass="auto-style1" Width="346px">
                <asp:ListItem>Select your day</asp:ListItem>
                <%-- Populate days dynamically in code-behind --%>
            </asp:DropDownList>
            <asp:DropDownList ID="month" runat="server" CssClass="auto-style1" Width="346px">
                <asp:ListItem>Select your month</asp:ListItem>
                <asp:ListItem>January</asp:ListItem>
                <asp:ListItem>February</asp:ListItem>
                <asp:ListItem>March</asp:ListItem>
                <asp:ListItem>April</asp:ListItem>
                <asp:ListItem>May</asp:ListItem>
                <asp:ListItem>June</asp:ListItem>
                <asp:ListItem>July</asp:ListItem>
                <asp:ListItem>August</asp:ListItem>
                <asp:ListItem>September</asp:ListItem>
                <asp:ListItem>October</asp:ListItem>
                <asp:ListItem>November</asp:ListItem>
                <asp:ListItem>December</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="year" runat="server" MaxLength="4" CssClass="auto-style1" Width="346px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                ControlToValidate="year" ErrorMessage="Please enter your Date of Birth" CssClass="error-message"></asp:RequiredFieldValidator>
            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="year"
                ErrorMessage="Please enter a valid Date of Birth" CssClass="error-message"
                MaximumValue="2024" MinimumValue="1800"></asp:RangeValidator>
        </div>

        <div class="form-group">
            <label>Gender:</label>
            <asp:DropDownList ID="gender" runat="server" CssClass="auto-style1" Width="848px">
                <asp:ListItem>Choose your gender</asp:ListItem>
                <asp:ListItem>Male</asp:ListItem>
                <asp:ListItem>Female</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="genders" runat="server" ControlToValidate="gender"
                ErrorMessage="Please select your gender" CssClass="error-message" InitialValue="Choose your gender"></asp:RequiredFieldValidator>
        </div>

        <div class="form-group">
            <label>Phone No:</label>
            <asp:TextBox ID="phone" runat="server" CssClass="auto-style1" Width="848px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="phone"
                ErrorMessage="Please enter your Phone No" CssClass="error-message"></asp:RequiredFieldValidator>
        </div>

        <div class="form-group">
            <label>Address:</label>
            <asp:TextBox ID="address" runat="server" MaxLength="250" CssClass="auto-style1" Width="1405px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="address"
                ErrorMessage="Please enter your address" CssClass="error-message"></asp:RequiredFieldValidator>
        </div>

        <div class="form-group">
            <label>Password:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="auto-style1" Width="848px"></asp:TextBox>
            <asp:CustomValidator ID="cvPassword" runat="server" ControlToValidate="txtPassword"
                ErrorMessage="Password must be at least 12 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character."
                OnServerValidate="ValidatePassword" Display="Dynamic" CssClass="error-message"></asp:CustomValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtPassword"
                ErrorMessage="Please enter a password" CssClass="error-message"></asp:RequiredFieldValidator>
        </div>

        <div class="form-group">
            <label>Confirm Password:</label>
            <asp:TextBox ID="cPass" runat="server" TextMode="Password" MaxLength="80" CssClass="auto-style1" Width="848px"></asp:TextBox>
        </div>

        <div class="form-group">
            <asp:Label ID="lblPasswordMatchError" runat="server" CssClass="error-message"></asp:Label>
            <asp:Label ID="lblDateOfBirthError" runat="server" CssClass="error-message"></asp:Label>
            <asp:Label ID="lblEmailError" runat="server" CssClass="error-message"></asp:Label>
            <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="Button1_Click" CssClass="btn btn-success" BackColor="Lime" BorderColor="Black" BorderWidth="4px" ForeColor="White" />
            <asp:Button ID="Button2" runat="server" Text="Clear" OnClick="Button2_Click" CssClass="btn btn-secondary" OnClientClick="return ClearForm();" BackColor="Red" BorderColor="Black" BorderWidth="4px" ForeColor="White" />
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="error-message"></asp:Label>
    </form>

    <script type="text/javascript">
        function ClearForm() {
            var inputs = document.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "text" || inputs[i].type == "password") {
                    inputs[i].value = "";
                }
            }
            var dropdowns = document.getElementsByTagName("select");
            for (var i = 0; i < dropdowns.length; i++) {
                dropdowns[i].selectedIndex = 0;
            }
            return false; // Prevent postback
        }
    </script>
</body>
</html>