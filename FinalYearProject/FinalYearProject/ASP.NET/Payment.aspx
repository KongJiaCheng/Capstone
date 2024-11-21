<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="FinalYearProject.ASP.NET.Payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="CSS/styles.css" />
        <style>
        body {
            background: url('picture/Background.jpg') no-repeat center center fixed;
            color: white;
            font-size: large;
        }

        .receipt-panel {
            border: 1px solid #ccc;
            padding: 15px;
            width: 50%;
            margin: auto;
            margin-top: 20px;
            text-align: left;
            background-color: rgba(0, 0, 0, 0.9);
        }

        .receipt-panel h2 {
            text-align: center;
        }

        .payment-table {
            width: 80%;
            margin: auto;
            margin-top: 20px;
            border-collapse: collapse;
        }

        .payment-table th, .payment-table td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: center;
        }

        .payment-table th {
            background-color: black;
        }
        .payment-table td{
            background-color: grey;
        }

        .payment-method {
            margin: 20px auto;
            text-align: center;
        }

        .btn-confirm {
            display: block;
            width: 30%;
            margin: 20px auto;
            padding: 10px;
            background-color: #4CAF50;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
        }

        .btn-confirm:hover {
            background-color: #3e8e41;
        }

        .qr-code {
            text-align: center;
            margin-top: 20px;
        }
    </style>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-container">
        <!-- General Payment Table (All Statuses) -->
        <asp:Repeater ID="rptPaymentDetails" runat="server">
            <HeaderTemplate>
                <table class="payment-table">
                    <thead>
                        <tr>
                            <th>Product Name</th>
                            <th>Price</th>
                            <th>Quantity</th>
                            <th>Total Price</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("ProductName") %></td>
                    <td><%# Eval("Price", "RM {0:0.00}") %></td>
                    <td><%# Eval("Quantity") %></td>
                    <td><%# Eval("TotalPrice", "RM {0:0.00}") %></td>
                    <td><%# Eval("PaymentStatus") %></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Label ID="lblFinalTotalAmount" runat="server" CssClass="total-amount-label"></asp:Label>
        <asp:RadioButtonList ID="rblPaymentMethod" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Value="COD" Text="Cash on Delivery"></asp:ListItem>
            <asp:ListItem Value="QR" Text="QR Code Payment"></asp:ListItem>
        </asp:RadioButtonList>
        <asp:Button ID="btnGenerateReceipt" runat="server" Text="Generate Receipt" CssClass="btn-confirm" OnClick="btnGenerateReceipt_Click" />

        <!-- Receipt Panel (Pending Payments Only) -->
        <asp:Panel ID="pnlReceipt" runat="server" Visible="false" CssClass="receipt-panel">
            <h2>Receipt of ByteBazaar</h2>
            <p><strong>Payment ID:</strong> <asp:Label ID="lblReceiptPaymentID" runat="server"></asp:Label></p>
            <p><strong>Username:</strong> <asp:Label ID="lblReceiptUsername" runat="server"></asp:Label></p>
            <p><strong>Payment Method:</strong> <asp:Label ID="lblReceiptPaymentMethod" runat="server"></asp:Label></p>
            <p><strong>Total Amount:</strong> <asp:Label ID="lblReceiptTotalAmount" runat="server"></asp:Label></p>
            <p><strong>Date:</strong> <asp:Label ID="lblReceiptDate" runat="server"></asp:Label></p>
            <h3>Payment Details</h3>
            <asp:Literal ID="ltReceiptDetails" runat="server"></asp:Literal>
            <asp:Button ID="btnPrint" runat="server" Text="Download Receipt" CssClass="btn-confirm" OnClick="btnPrint_Click" />
            <asp:Button ID="btnConfirmPayment" runat="server" Text="Confirm Payment" CssClass="btn-confirm" OnClick="btnConfirmPayment_Click" />
        </asp:Panel>
         <asp:Panel ID="qrCodeSection" runat="server" Visible="false" CssClass="qr-code">
         <h3>Scan the QR Code</h3>
         <asp:Image ID="imgQRCode" runat="server" ImageUrl="picture/TNGQRCode.jpg" AlternateText="QR Code" />
         <br />
         <asp:Label ID="lblQRCodeInstruction" runat="server" Text="Scan the QR code to confirm payment." />
         <br />
         <br />
         <asp:HyperLink id="hyperlink1" 
         NavigateUrl="https://wa.link/os4pgz"
         Text="Admin Whatsapp Link ; Send Receipt and Transaction Image"
         Target="_new"
         runat="server"
         Style="color: black; background-color: white; padding: 10px; text-decoration: none; border-radius: 3px;"/>
         </asp:Panel>
    </div>
</asp:Content>