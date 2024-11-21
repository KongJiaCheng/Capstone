using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Web.Configuration;

namespace FinalYearProject.ASP.NET
{
    public partial class AdminSignUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                for (int i = 1; i <= 31; i++)
                {
                    day.Items.Add(new ListItem(i.ToString()));
                }
                // Populate year dropdown or text field if needed
            }
        }

        protected void ValidatePassword(object source, ServerValidateEventArgs args)
        {
            string password = args.Value;
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{12,}$";
            Regex regex = new Regex(pattern);
            args.IsValid = regex.IsMatch(password);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (txtPassword.Text != cPass.Text)
                {
                    lblPasswordMatchError.Text = "Passwords do not match.";
                    lblPasswordMatchError.CssClass = "text-danger";
                    return;
                }

                string username = name.Text;
                string password = txtPassword.Text;
                string role = "admin";
                string userAddress = address.Text;
                string userEmail = email.Text;
                string userGender = gender.SelectedValue;
                string userPhone = phone.Text;

                DateTime userDOB;
                if (!DateTime.TryParse($"{day.SelectedValue}/{month.SelectedValue}/{year.Text}", out userDOB))
                {
                    lblDateOfBirthError.Text = "Invalid date of birth.";
                    lblDateOfBirthError.CssClass = "text-danger";
                    return;
                }

                string hashedPassword = HashPassword(password);

                string connectionString = ConfigurationManager.ConnectionStrings["FinalYearProjectConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    try
                    {
                        con.Open();

                        // Check if user already exists
                        string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE UserEmail = @Email";
                        using (SqlCommand checkCmd = new SqlCommand(checkUserQuery, con))
                        {
                            checkCmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = userEmail;
                            int userExists = (int)checkCmd.ExecuteScalar();
                            if (userExists > 0)
                            {
                                lblEmailError.Text = "An account with this email already exists.";
                                lblEmailError.CssClass = "text-danger";
                                return;
                            }
                        }

                        string query = "INSERT INTO Users (UserEmail, username, hashedPassword, role, UserAddress, UserGender, UserPhone, UserDOB) VALUES (@Email, @Username, @PasswordHash, @Role, @Address, @Gender, @Phone, @DateOfBirth)";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Email", email.Text);
                            cmd.Parameters.AddWithValue("@Username", name.Text);
                            cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                            cmd.Parameters.AddWithValue("@Role", role);
                            cmd.Parameters.AddWithValue("@Address", address.Text);
                            cmd.Parameters.AddWithValue("@Gender", gender.SelectedValue);
                            cmd.Parameters.AddWithValue("@Phone", phone.Text);
                            cmd.Parameters.AddWithValue("@DateOfBirth", userDOB);

                            cmd.ExecuteNonQuery();
                        }

                        lblMessage.Text = "Your account has been created successfully!";
                        lblMessage.CssClass = "text-success";
                    }
                    catch (Exception ex)
                    {
                        // Log the exception details
                        // For example: Logger.LogError(ex);
                        lblMessage.Text = "An error occurred: " + ex.Message;
                        lblMessage.CssClass = "text-danger";
                    }
                }
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            // Clear all input fields
            name.Text = string.Empty;
            email.Text = string.Empty;
            day.SelectedIndex = 0;
            month.SelectedIndex = 0;
            year.Text = string.Empty;
            gender.SelectedIndex = 0;
            phone.Text = string.Empty;
            address.Text = string.Empty;
            txtPassword.Text = string.Empty;
            cPass.Text = string.Empty;
            lblPasswordMatchError.Text = string.Empty;
            lblDateOfBirthError.Text = string.Empty;
            lblEmailError.Text = string.Empty;
        }
    }
}
