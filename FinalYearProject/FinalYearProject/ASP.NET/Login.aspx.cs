using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace FinalYearProject.ASP.NET
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Auto-fill username if cookie exists
                HttpCookie userCookie = Request.Cookies["UserLogin"];
                if (userCookie != null)
                {
                    TextBox1.Text = userCookie["Username"];
                }
            }
        }

        protected void ValidatePassword(object source, ServerValidateEventArgs args)
        {
            string password = args.Value;
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{12,}$";
            args.IsValid = Regex.IsMatch(password, pattern);
        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string username = TextBox1.Text;
                string password = TextBox2.Text;

                // Hash the input password before checking with the database
                string hashedPassword = HashPassword(password);
                string role = GetUserRole(username, hashedPassword);

                if (role == "customer")
                {
                    // Create a cookie for the username
                    HttpCookie userCookie = new HttpCookie("UsernameCookie", username);
                    userCookie.HttpOnly = true; // Make the cookie HttpOnly for security
                    userCookie.Secure = Request.IsSecureConnection; // Only send over HTTPS
                    userCookie.Expires = DateTime.Now.AddDays(30); // Set expiration

                    Response.Cookies.Add(userCookie);

                    // Use FormsAuthentication for both admin and customer
                    FormsAuthentication.RedirectFromLoginPage(username, false);
                    Response.Redirect("HomePage.aspx");
                }
                else if (role == "admin")
                {
                    // Create a cookie for the username
                    HttpCookie userCookie = new HttpCookie("UsernameCookie", username);
                    userCookie.HttpOnly = true; // Make the cookie HttpOnly for security
                    userCookie.Secure = Request.IsSecureConnection; // Only send over HTTPS
                    userCookie.Expires = DateTime.Now.AddDays(30); // Set expiration

                    Response.Cookies.Add(userCookie);
                    Response.Redirect("AdminMain.aspx");
                }
                else
                {
                    string message = "You haven't signed up yet! Please sign up before login!";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
                }
            }
        }



        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void SetUserCookie(string username, string role)
        {
            HttpCookie userCookie = new HttpCookie("UserLogin");
            userCookie["Username"] = username;
            userCookie["Role"] = role;

            // Set the cookie to expire in 7 days
            userCookie.Expires = DateTime.Now.AddDays(7);

            // Secure and HttpOnly for better security
            userCookie.HttpOnly = true;
            userCookie.Secure = Request.IsSecureConnection; // Ensures cookie is sent only over HTTPS

            // Add the cookie to the response
            Response.Cookies.Add(userCookie);
        }

        private string GetUserRole(string username, string hashedPassword)
        {
            string connectionString = "Data Source=DESKTOP-7U1R5G0\\SQLEXPRESS;Initial Catalog=CapstoneProject;Integrated Security=True;Encrypt=False";
            string role = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Use the correct column name 'hashedPassword' from the database
                string query = "SELECT Role FROM Users WHERE Username = @Username AND hashedPassword = @PasswordHash";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    role = reader["Role"].ToString();
                }
                reader.Close();
            }

            return role;
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            // Clear input fields
            TextBox1.Text = string.Empty;
            TextBox2.Text = string.Empty;
        }

        protected void ButtonBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("HomePage.aspx");
        }
    }
}
