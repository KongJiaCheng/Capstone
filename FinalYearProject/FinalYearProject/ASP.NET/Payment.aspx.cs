using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Linq;

namespace FinalYearProject.ASP.NET
{
    public partial class Payment : System.Web.UI.Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["ECommerceDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPaymentList();
            }
        }

        private void BindPaymentList()
        {
            string username = GetUsername();
            if (string.IsNullOrEmpty(username))
            {
                ShowAlert("User not logged in.");
                return;
            }

            string query = @"
                SELECT ProductName, Price, Quantity, 
                       (Price * Quantity) AS TotalPrice, PaymentStatus 
                FROM Payment 
                WHERE Username = @Username";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptPaymentDetails.DataSource = dt;
                rptPaymentDetails.DataBind();
            }

            decimal finalTotalAmount = GetFinalPendingTotal(username);
            lblFinalTotalAmount.Text = $"Total Amount (Pending Only): RM {finalTotalAmount:0.00}";
        }

        private decimal GetFinalPendingTotal(string username)
        {
            string query = @"
                SELECT SUM(Price * Quantity) 
                FROM Payment 
                WHERE Username = @Username AND PaymentStatus = 'Pending'";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                con.Open();

                object result = cmd.ExecuteScalar();
                return result == DBNull.Value || result == null ? 0 : Convert.ToDecimal(result);
            }
        }

        private string GetUsername()
        {
            return Session["Username"]?.ToString() ?? Request.Cookies["UsernameCookie"]?.Value;
        }

        protected void btnGenerateReceipt_Click(object sender, EventArgs e)
        {
            string username = GetUsername();
            string paymentMethod = rblPaymentMethod.SelectedValue;

            if (string.IsNullOrEmpty(username))
            {
                ShowAlert("User not logged in.");
                return;
            }
            if (string.IsNullOrEmpty(paymentMethod))
            {
                ShowAlert("Please select a payment method.");
                return;
            }

            if (GetFinalPendingTotal(username) == 0)
            {
                ShowAlert("No pending payments available to generate a receipt.");
                return;
            }
            if (paymentMethod == "QR")
            {
                qrCodeSection.Visible = true;
            }
            else
            {
                qrCodeSection.Visible = false;
            }

            PopulateReceipt(username, paymentMethod);
            pnlReceipt.Visible = true;
        }

        private void PopulateReceipt(string username, string paymentMethod)
        {
            string query = @"
                SELECT PaymentID, ProductName, Price, Quantity, 
                       (Price * Quantity) AS TotalPrice 
                FROM Payment 
                WHERE Username = @Username AND PaymentStatus = 'Pending'";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    StringWriter receiptHtml = new StringWriter();
                    receiptHtml.WriteLine("<table class='receipt-table'>");
                    receiptHtml.WriteLine("<thead><tr><th>Product</th><th>Price</th><th>Qty</th><th>Total</th></tr></thead>");
                    receiptHtml.WriteLine("<tbody>");

                    decimal totalAmount = 0;
                    string paymentID = string.Empty;

                    while (reader.Read())
                    {
                        receiptHtml.WriteLine("<tr>");
                        receiptHtml.WriteLine($"<td>{reader["ProductName"]}</td>");
                        receiptHtml.WriteLine($"<td>RM {Convert.ToDecimal(reader["Price"]):0.00}</td>");
                        receiptHtml.WriteLine($"<td>{reader["Quantity"]}</td>");
                        receiptHtml.WriteLine($"<td>RM {Convert.ToDecimal(reader["TotalPrice"]):0.00}</td>");
                        receiptHtml.WriteLine("</tr>");

                        totalAmount += Convert.ToDecimal(reader["TotalPrice"]);
                        paymentID = reader["PaymentID"].ToString();
                    }

                    receiptHtml.WriteLine("</tbody></table>");
                    ltReceiptDetails.Text = receiptHtml.ToString();

                    lblReceiptPaymentID.Text = paymentID;
                    lblReceiptUsername.Text = username;
                    lblReceiptPaymentMethod.Text = paymentMethod;
                    lblReceiptTotalAmount.Text = $"RM {totalAmount:0.00}";
                    lblReceiptDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                }
            }
        }
        /*
         
         */

        protected void btnConfirmPayment_Click(object sender, EventArgs e)
        {
            string username = GetUsername();
            string paymentMethod = rblPaymentMethod.SelectedValue;

            if (string.IsNullOrEmpty(username))
            {
                ShowAlert("User not logged in.");
                return;
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = @"
                        UPDATE Payment
                        SET PaymentStatus = 'Completed', PaymentMethod = @PaymentMethod
                        WHERE Username = @Username AND PaymentStatus = 'Pending'";

                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                    cmd.ExecuteNonQuery();
                }
            }

            ShowAlert("Payment confirmed successfully.");
            pnlReceipt.Visible = false;
            BindPaymentList();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Receipt.pdf");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            using (Document pdfDoc = new Document(PageSize.A4, 50, 50, 25, 25))
            {
                PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();

                pdfDoc.Add(new Paragraph("Receipt", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD)));
                pdfDoc.Add(new Paragraph("\n"));
                pdfDoc.Add(new Paragraph($"Payment ID: {lblReceiptPaymentID.Text}"));
                pdfDoc.Add(new Paragraph($"Username: {lblReceiptUsername.Text}"));
                pdfDoc.Add(new Paragraph($"Payment Method: {lblReceiptPaymentMethod.Text}"));
                pdfDoc.Add(new Paragraph($"Total Amount: {lblReceiptTotalAmount.Text}"));
                pdfDoc.Add(new Paragraph($"Date: {lblReceiptDate.Text}"));
                pdfDoc.Add(new Paragraph("\n"));

                PdfPTable table = new PdfPTable(4) { WidthPercentage = 100 };
                table.AddCell("Product");
                table.AddCell("Price");
                table.AddCell("Qty");
                table.AddCell("Total");

                foreach (var row in ltReceiptDetails.Text.Split(new[] { "<tr>" }, StringSplitOptions.None).Skip(1))
                {
                    foreach (var cell in row.Split(new[] { "<td>" }, StringSplitOptions.None).Skip(1))
                    {
                        table.AddCell(cell.Split(new[] { "</td>" }, StringSplitOptions.None)[0]);
                    }
                }

                pdfDoc.Add(table);
                pdfDoc.Close();
            }

            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        private void ShowAlert(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('{message}');", true);
        }
    }
}