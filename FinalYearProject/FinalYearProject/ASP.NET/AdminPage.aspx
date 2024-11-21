<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="FinalYearProject.ASP.NET.AdminPage" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="CSS/style.css" />
    <table style="width: 100%">
        <tr>
            <td colspan="2">
                <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Size="XX-Large" Text="Product Management"></asp:Label>
                &nbsp
                <button>
                <b>
                <a href="AdminMain.aspx">Back?</a>
                </b>
                </button>
            </td>
        </tr>
        <tr>
            <td style="width: 497px">
                <asp:Label ID="Label1" runat="server" Font-Size="Medium" Text="Product ID" Font-Bold="True"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox1" runat="server" Font-Size="Medium" Width="200px"></asp:TextBox>
                <asp:Button ID="Button5" runat="server" Text="Fetch Product" OnClick="Button5_Click" Font-Bold="True" />
            </td>
        </tr>
        <tr>
            <td style="font-family: Arial, Helvetica, sans-serif; width: 497px;">
                <asp:Label ID="Label2" runat="server" Font-Size="Medium" Text="Product Name" Font-Bold="True"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox2" runat="server" Font-Size="Medium" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 497px">
                <asp:Label ID="Label3" runat="server" Text="ProductDescription" Font-Bold="True"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox3" runat="server" Height="36px" Width="198px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 497px">
                <asp:Label ID="Label4" runat="server" Text="Price" Font-Bold="True"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox4" runat="server" Font-Size="Medium" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 497px">
                <asp:Label ID="Label5" runat="server" Text="Stock" Font-Bold="True"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox5" runat="server" Height="28px" Width="196px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 497px">
                <asp:Label ID="Label6" runat="server" Font-Bold="True" Text="ImageURL"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 497px">&nbsp;</td>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Insert" Font-Bold="true" OnClick="Button1_Click"/>
            &nbsp
                <asp:Button ID="Button2" runat="server" Text="Update" Font-Bold="true" OnClick="Button2_Click"/>
            &nbsp
                <asp:Button ID="Button3" runat="server" Text="Delete" Font-Bold="true" OnClick="Button3_Click" OnClientClick="return confirm ('Are you sure to delete?')" Width="134px"/>
            &nbsp
                <asp:Button ID="Button4" runat="server" Font-Bold="True" Text="Get" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="GridView1" runat="server" style="margin-right: 1116px">
                </asp:GridView>
            </td>
        </tr>
    </table>


</asp:Content>
