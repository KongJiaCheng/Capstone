using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FinalYearProject.ASP.NET
{
    public partial class AdminUserInfo : Page
    {
        private readonly string connectionString = @"Data Source=DESKTOP-7U1R5G0\SQLEXPRESS;Initial Catalog=CapstoneProject;Integrated Security=True;Encrypt=False";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetUserList();
                if (Request.QueryString["username"] != null)
                {
                    PopulateUserDetails();
                }
            }
        }

        private void GetUserList()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Users", connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        private void PopulateUserDetails()
        {
            TextBox1.Text = Server.UrlDecode(Request.QueryString["username"]);
            TextBox2.Text = Server.UrlDecode(Request.QueryString["UserEmail"]);
            TextBox3.Text = Server.UrlDecode(Request.QueryString["role"]);
        }

        protected void Button1_Click(object sender, EventArgs e) // Insert
        {
            string query = "INSERT INTO Users (username, UserEmail, role) VALUES (@username, @UserEmail, @role)";
            ExecuteUserQuery(query, "User successfully added.");
        }

        protected void Button2_Click(object sender, EventArgs e) // Update
        {
            string query = "UPDATE Users SET UserEmail=@UserEmail, role=@role WHERE username=@username";
            ExecuteUserQuery(query, "User details successfully updated.");
        }

        protected void Button3_Click(object sender, EventArgs e) // Delete
        {
            string query = "DELETE FROM Users WHERE username=@username";
            ExecuteUserQuery(query, "User successfully deleted.");
        }

        protected void Button4_Click(object sender, EventArgs e) // Fetch User Details
        {
            string username = TextBox1.Text.Trim();
            if (!string.IsNullOrEmpty(username))
            {
                FetchUserDetails(username);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "script", "alert('Please enter a Username.');", true);
            }
        }

        private void FetchUserDetails(string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand("SELECT UserEmail, role FROM Users WHERE username = @username", connection))
                {
                    command.Parameters.AddWithValue("@username", username.Trim());
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            TextBox2.Text = reader["UserEmail"].ToString();
                            TextBox3.Text = reader["role"].ToString();
                        }
                        else
                        {
                            ClearTextBoxes();
                            ScriptManager.RegisterStartupScript(this, GetType(), "script",
                                "alert('User not found. Please check the Username.');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "script",
                    $"alert('Error fetching user details: {ex.Message}');", true);
            }
        }

        private void ExecuteUserQuery(string query, string successMessage)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", TextBox1.Text.Trim());
                    command.Parameters.AddWithValue("@UserEmail", TextBox2.Text.Trim());
                    command.Parameters.AddWithValue("@role", TextBox3.Text.Trim());

                    connection.Open();
                    command.ExecuteNonQuery();
                    ScriptManager.RegisterStartupScript(this, GetType(), "script", $"alert('{successMessage}');", true);
                    GetUserList();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "script", $"alert('Error: {ex.Message}');", true);
            }
        }

        private void ClearTextBoxes()
        {
            TextBox1.Text = string.Empty;
            TextBox2.Text = string.Empty;
            TextBox3.Text = string.Empty;
        }
    }
}
