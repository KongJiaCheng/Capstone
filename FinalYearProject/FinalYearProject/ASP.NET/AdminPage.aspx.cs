using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalYearProject.ASP.NET
{
    public partial class AdminPage : System.Web.UI.Page
    {
        private readonly string connectionString = @"Data Source=DESKTOP-7U1R5G0\SQLEXPRESS;Initial Catalog=CapstoneProject;Integrated Security=True;Encrypt=False";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetProductList();
                if (Request.QueryString["ProductID"] != null)
                {
                    PopulateProductDetails();
                }
            }
        }

        private void PopulateProductDetails()
        {
            TextBox1.Text = Server.UrlDecode(Request.QueryString["ProductID"]);
            TextBox2.Text = Server.UrlDecode(Request.QueryString["ProductName"]);
            TextBox3.Text = Server.UrlDecode(Request.QueryString["ProductDescription"]);
            TextBox4.Text = Server.UrlDecode(Request.QueryString["Price"]);
            TextBox5.Text = Server.UrlDecode(Request.QueryString["Stock"]);
            TextBox6.Text = Server.UrlDecode(Request.QueryString["ImageURL"]);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ExecuteProductQuery(
                "INSERT INTO Product (ProductID, ProductName, ProductDescription, Price, Stock, ImageURL) VALUES (@ProductID, @ProductName, @ProductDescription, @Price, @Stock, @ImageURL)",
                "Successfully saved"
            );
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ExecuteProductQuery(
                "UPDATE Product SET ProductName=@ProductName, ProductDescription=@ProductDescription, Price=@Price, Stock=@Stock, ImageURL=@ImageURL WHERE ProductID=@ProductID",
                "Successfully updated"
            );
        }


        protected void Button3_Click(object sender, EventArgs e)
        {
            string productId = TextBox1.Text.Trim();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Payment WHERE ProductID = @ProductID", connection))
            {
                checkCommand.Parameters.AddWithValue("@ProductID", productId);
                connection.Open();

                // If there are dependencies in Payment table, show an alert and return
                if ((int)checkCommand.ExecuteScalar() > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "script",
                        "alert('Cannot delete this product as there are associated payments.');", true);
                    return;
                }

                // If no dependencies, delete the product
                using (SqlCommand deleteCommand = new SqlCommand("DELETE FROM Product WHERE ProductID = @ProductID", connection))
                {
                    deleteCommand.Parameters.AddWithValue("@ProductID", productId);
                    deleteCommand.ExecuteNonQuery();
                    ScriptManager.RegisterStartupScript(this, GetType(), "script", "alert('Successfully deleted');", true);
                }
            }

            GetProductList(); // Refresh the product list
        }



        private void ExecuteProductQuery(string query, string successMessage)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", TextBox1.Text.Trim());
                    command.Parameters.AddWithValue("@ProductName", TextBox2.Text.Trim());
                    command.Parameters.AddWithValue("@ProductDescription", TextBox3.Text.Trim());
                    command.Parameters.AddWithValue("@Price", TextBox4.Text.Trim());

                    if (int.TryParse(TextBox5.Text.Trim(), out int stock))
                    {
                        command.Parameters.AddWithValue("@Stock", stock);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "script", "alert('Invalid stock value.');", true);
                        return;
                    }

                    command.Parameters.AddWithValue("@ImageURL", TextBox6.Text.Trim());

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery(); // Get number of affected rows

                    if (rowsAffected > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "script", $"alert('{successMessage}');", true);
                        GetProductList(); // Refresh the product list
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "script", "alert('No matching Product ID found to delete.');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "script", $"alert('Error: {ex.Message}');", true);
            }
        }


        protected void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand("SELECT * FROM Product WHERE ProductID=@ProductID", connection))
                {
                    command.Parameters.AddWithValue("@ProductID", TextBox1.Text.Trim());

                    SqlDataAdapter sd = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    sd.Fill(dt);
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "script", $"alert('Error: {ex.Message}');", true);
            }
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            string productId = TextBox1.Text.Trim();
            if (!string.IsNullOrEmpty(productId))
            {
                FetchProductDetails(productId);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "script", "alert('Please enter a Product ID.');", true);
            }
        }

        private void FetchProductDetails(string productId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("SELECT ProductName, ProductDescription, Price, Stock, ImageURL FROM Product WHERE ProductID = @ProductID", con))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId.Trim());

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            // Populate the TextBox controls with the fetched data
                            TextBox2.Text = reader["ProductName"].ToString();
                            TextBox3.Text = reader["ProductDescription"].ToString();
                            TextBox4.Text = reader["Price"].ToString();
                            TextBox5.Text = reader["Stock"].ToString();
                            TextBox6.Text = reader["ImageURL"].ToString();
                        }
                        else
                        {
                            ClearTextBoxes();
                            ScriptManager.RegisterStartupScript(this, GetType(), "script",
                                "alert('Product not found. Please check the Product ID.');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "script",
                    $"alert('Error fetching product details: {ex.Message}');", true);
            }
        }


        private void ClearTextBoxes()
        {
            TextBox2.Text = string.Empty;
            TextBox3.Text = string.Empty;
            TextBox4.Text = string.Empty;
            TextBox5.Text = string.Empty;
            TextBox6.Text = string.Empty;
        }

        private void GetProductList()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand("SELECT * FROM Product", connection))
                {
                    SqlDataAdapter sd = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    sd.Fill(dt);
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "script", $"alert('Error: {ex.Message}');", true);
            }
        }

        protected void btnViewProduct_Click(object sender, EventArgs e)
        {
            try
            {
                string url = $"ProductInfo.aspx?ProductID={Server.UrlEncode(TextBox1.Text)}&ProductName={Server.UrlEncode(TextBox2.Text)}&ProductDescription={Server.UrlEncode(TextBox3.Text)}&Price={Server.UrlEncode(TextBox4.Text)}&Stock={Server.UrlEncode(TextBox5.Text)}&ImageURL={Server.UrlEncode(TextBox6.Text)}";
                Response.Redirect(url);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "script", $"alert('Error: {ex.Message}');", true);
            }
        }
    }
}
