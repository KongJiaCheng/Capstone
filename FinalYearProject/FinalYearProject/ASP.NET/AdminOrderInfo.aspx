<%@ Page Title="Order Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminOrderInfo.aspx.cs" Inherits="FinalYearProject.ASP.NET.AdminOrderInfo" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%">
        <tr>
            <td colspan="2">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="XX-Large" Text="Order Management"></asp:Label>
                &nbsp;
                <button>
                    <b>
                        <a href="AdminMain.aspx">Back?</a>
                    </b>
                </button>
            </td>
        </tr>
        <!-- Order ID -->
        <tr>
            <td style="width: 250px">
                <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Order ID"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                <asp:Button ID="Button4" runat="server" Text="Fetch Order" OnClick="Button4_Click" Font-Bold="True" />
            </td>
        </tr>
        <!-- Username -->
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Username"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            </td>
        </tr>
        <!-- Order Date -->
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Font-Bold="True" Text="Order Date"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox4" runat="server" TextMode="Date"></asp:TextBox>
            </td>
        </tr>
        <!-- Is Completed -->
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="Is Completed"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="CheckBoxIsCompleted" runat="server" />
            </td>
        </tr>
        <!-- Quantity -->
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Font-Bold="True" Text="Quantity"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
            </td>
        </tr>
        <!-- Product ID -->
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" Font-Bold="True" Text="Product ID"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
            </td>
        </tr>
        <!-- Price -->
        <tr>
            <td>
                <asp:Label ID="Label8" runat="server" Font-Bold="True" Text="Price"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
            </td>
        </tr>
        <!-- Action Buttons -->
        <tr>
            <td colspan="2">
                <asp:Button ID="Button1" runat="server" Text="Insert" OnClick="Button1_Click" />
                <asp:Button ID="Button2" runat="server" Text="Update" OnClick="Button2_Click" />
                <asp:Button ID="Button3" runat="server" Text="Delete" OnClick="Button3_Click" />
            </td>
        </tr>
        <!-- Orders List -->
        <tr>
            <td colspan="2">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true"></asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
