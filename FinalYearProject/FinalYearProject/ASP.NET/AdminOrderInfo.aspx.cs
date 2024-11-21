using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace FinalYearProject.ASP.NET
{
    public partial class AdminOrderInfo : Page
    {
        private readonly string connectionString = @"Data Source=DESKTOP-7U1R5G0\SQLEXPRESS;Initial Catalog=CapstoneProject;Integrated Security=True;Encrypt=False";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetOrderList();
                if (Request.QueryString["OrderID"] != null)
                {
                    PopulateOrderDetails();
                }
            }
        }

        private void GetOrderList()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Orders", connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        private void PopulateOrderDetails()
        {
            TextBox1.Text = Server.UrlDecode(Request.QueryString["OrderID"]);
            TextBox2.Text = Server.UrlDecode(Request.QueryString["Username"]);
            TextBox4.Text = Server.UrlDecode(Request.QueryString["OrderDate"]);
            CheckBoxIsCompleted.Checked = bool.Parse(Server.UrlDecode(Request.QueryString["IsCompleted"]));
            TextBox5.Text = Server.UrlDecode(Request.QueryString["Quantity"]);
            TextBox3.Text = Server.UrlDecode(Request.QueryString["ProductID"]);
            TextBox6.Text = Server.UrlDecode(Request.QueryString["Price"]);
        }


        protected void Button1_Click(object sender, EventArgs e) // Insert
        {
            string query = "INSERT INTO Orders (OrderID, Username, OrderDate, IsCompleted, Quantity, ProductID, Price) " +
                           "VALUES (@OrderID, @Username, @OrderDate, @IsCompleted, @Quantity, @ProductID, @Price)";
            ExecuteOrderQuery(query, "Order successfully added.");

        }

        protected void Button2_Click(object sender, EventArgs e) // Update
        {
            string query = @"
        UPDATE Orders 
        SET Username = @Username, 
            OrderDate = @OrderDate, 
            IsCompleted = @IsCompleted, 
            Quantity = @Quantity, 
            ProductID = @ProductID, 
            Price = @Price 
        WHERE OrderID = @OrderID";

            ExecuteOrderQuery(query, "Order successfully updated.");
        }

        protected void Button3_Click(object sender, EventArgs e) // Delete
        {
            string query = "DELETE FROM Orders WHERE OrderID = @OrderID";
            ExecuteOrderQuery(query, "Order successfully deleted.");
        }

        private void ExecuteOrderQuery(string query, string successMessage)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@OrderID", TextBox1.Text.Trim());
                    command.Parameters.AddWithValue("@Username", TextBox2.Text.Trim());
                    command.Parameters.AddWithValue("@OrderDate", DateTime.Parse(TextBox4.Text.Trim()));
                    command.Parameters.AddWithValue("@IsCompleted", CheckBoxIsCompleted.Checked);
                    command.Parameters.AddWithValue("@Quantity", int.Parse(TextBox5.Text.Trim()));
                    command.Parameters.AddWithValue("@ProductID", TextBox3.Text.Trim());
                    command.Parameters.AddWithValue("@Price", decimal.Parse(TextBox6.Text.Trim()));

                    connection.Open();
                    command.ExecuteNonQuery();
                    ShowAlert(successMessage);
                    GetOrderList();
                }
            }
            catch (Exception ex)
            {
                ShowAlert("Error: " + ex.Message);
            }
        }


        protected void Button4_Click(object sender, EventArgs e)
        {
            string orderId = TextBox1.Text.Trim();
            if (!string.IsNullOrEmpty(orderId))
            {
                FetchOrderDetails(orderId);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "script", "alert('Please enter an Order ID.');", true);
            }
        }

        private void FetchOrderDetails(string orderId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(
                    "SELECT Username, OrderDate, IsCompleted, Quantity, ProductID, Price FROM Orders WHERE OrderID = @OrderID", connection))
                {
                    command.Parameters.AddWithValue("@OrderID", orderId);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            TextBox2.Text = reader["Username"].ToString();
                            TextBox4.Text = Convert.ToDateTime(reader["OrderDate"]).ToString("yyyy-MM-dd");
                            CheckBoxIsCompleted.Checked = Convert.ToBoolean(reader["IsCompleted"]);
                            TextBox5.Text = reader["Quantity"].ToString();
                            TextBox3.Text = reader["ProductID"].ToString();
                            TextBox6.Text = reader["Price"].ToString();
                        }
                        else
                        {
                            ClearTextBoxes();
                            ScriptManager.RegisterStartupScript(this, GetType(), "script",
                                "alert('Order not found. Please check the Order ID.');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "script",
                    $"alert('Error fetching order details: {ex.Message}');", true);
            }
        }

        private void ClearTextBoxes()
        {
            TextBox2.Text = string.Empty;
            TextBox4.Text = string.Empty;
            CheckBoxIsCompleted.Checked = false;
            TextBox5.Text = string.Empty;
            TextBox3.Text = string.Empty;
            TextBox6.Text = string.Empty;
        }


        private void ShowAlert(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('{message}');", true);
        }
    }
}
