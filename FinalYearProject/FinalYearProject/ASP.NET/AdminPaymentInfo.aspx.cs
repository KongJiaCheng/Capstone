using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace FinalYearProject.ASP.NET
{
    public partial class AdminPaymentInfo : Page
    {
        private readonly string connectionString = @"Data Source=DESKTOP-7U1R5G0\SQLEXPRESS;Initial Catalog=CapstoneProject;Integrated Security=True;Encrypt=False";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetPaymentList();
                if (Request.QueryString["PaymentID"] != null)
                {
                    PopulatePaymentDetails();
                }
            }
        }

        private void GetPaymentList()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Payment", connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        private void PopulatePaymentDetails()
        {
            TextBox1.Text = Server.UrlDecode(Request.QueryString["PaymentID"]);
            TextBox2.Text = Server.UrlDecode(Request.QueryString["OrderID"]);
            TextBox3.Text = Server.UrlDecode(Request.QueryString["Username"]);
            TextBox4.Text = Server.UrlDecode(Request.QueryString["OrderDate"]);
            TextBox5.Text = Server.UrlDecode(Request.QueryString["ProductID"]);
            TextBox6.Text = Server.UrlDecode(Request.QueryString["ProductName"]);
            TextBox7.Text = Server.UrlDecode(Request.QueryString["Price"]);
            TextBox8.Text = Server.UrlDecode(Request.QueryString["Quantity"]);
            TextBox9.Text = Server.UrlDecode(Request.QueryString["TotalPrice"]);
            TextBox10.Text = Server.UrlDecode(Request.QueryString["PaymentStatus"]);
        }

        protected void Button1_Click(object sender, EventArgs e) // Insert
        {
            string query = "INSERT INTO Payment (PaymentID, OrderID, Username, OrderDate, ProductID, ProductName, Price, Quantity, TotalPrice, PaymentStatus) " +
                           "VALUES (@PaymentID, @OrderID, @Username, @OrderDate, @ProductID, @ProductName, @Price, @Quantity, @TotalPrice, @PaymentStatus)";
            ExecutePaymentQuery(query, "Payment record successfully added.");
        }

        protected void Button2_Click(object sender, EventArgs e) // Update
        {
            string query = "UPDATE Payment SET OrderID=@OrderID, Username=@Username, OrderDate=@OrderDate, ProductID=@ProductID, ProductName=@ProductName, " +
                           "Price=@Price, Quantity=@Quantity, TotalPrice=@TotalPrice, PaymentStatus=@PaymentStatus WHERE PaymentID=@PaymentID";
            ExecutePaymentQuery(query, "Payment record successfully updated.");
        }

        protected void Button3_Click(object sender, EventArgs e) // Delete
        {
            string query = "DELETE FROM Payment WHERE PaymentID=@PaymentID";
            ExecutePaymentQuery(query, "Payment record successfully deleted.");
        }

        private void ExecutePaymentQuery(string query, string successMessage)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PaymentID", TextBox1.Text.Trim());
                    command.Parameters.AddWithValue("@OrderID", TextBox2.Text.Trim());
                    command.Parameters.AddWithValue("@Username", TextBox3.Text.Trim());
                    command.Parameters.AddWithValue("@OrderDate", TextBox4.Text.Trim());
                    command.Parameters.AddWithValue("@ProductID", TextBox5.Text.Trim());
                    command.Parameters.AddWithValue("@ProductName", TextBox6.Text.Trim());
                    command.Parameters.AddWithValue("@Price", TextBox7.Text.Trim());
                    command.Parameters.AddWithValue("@Quantity", TextBox8.Text.Trim());
                    command.Parameters.AddWithValue("@TotalPrice", TextBox9.Text.Trim());
                    command.Parameters.AddWithValue("@PaymentStatus", TextBox10.Text.Trim());

                    connection.Open();
                    command.ExecuteNonQuery();
                    ScriptManager.RegisterStartupScript(this, GetType(), "script", $"alert('{successMessage}');", true);
                    GetPaymentList();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "script", $"alert('Error: {ex.Message}');", true);
            }
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            string paymentId = TextBox1.Text.Trim();
            if (!string.IsNullOrEmpty(paymentId))
            {
                FetchPaymentDetails(paymentId);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "script", "alert('Please enter a Payment ID.');", true);
            }
        }

        private void FetchPaymentDetails(string paymentId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand("SELECT * FROM Payment WHERE PaymentID = @PaymentID", connection))
                {
                    command.Parameters.AddWithValue("@PaymentID", paymentId);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            TextBox2.Text = reader["OrderID"].ToString();
                            TextBox3.Text = reader["Username"].ToString();
                            TextBox4.Text = reader["OrderDate"].ToString();
                            TextBox5.Text = reader["ProductID"].ToString();
                            TextBox6.Text = reader["ProductName"].ToString();
                            TextBox7.Text = reader["Price"].ToString();
                            TextBox8.Text = reader["Quantity"].ToString();
                            TextBox9.Text = reader["TotalPrice"].ToString();
                            TextBox10.Text = reader["PaymentStatus"].ToString();
                        }
                        else
                        {
                            ClearTextBoxes();
                            ScriptManager.RegisterStartupScript(this, GetType(), "script",
                                "alert('Payment not found. Please check the Payment ID.');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "script",
                    $"alert('Error fetching payment details: {ex.Message}');", true);
            }
        }

        private void ClearTextBoxes()
        {
            TextBox2.Text = string.Empty;
            TextBox3.Text = string.Empty;
            TextBox4.Text = string.Empty;
            TextBox5.Text = string.Empty;
            TextBox6.Text = string.Empty;
            TextBox7.Text = string.Empty;
            TextBox8.Text = string.Empty;
            TextBox9.Text = string.Empty;
            TextBox10.Text = string.Empty;
        }
    }
}
