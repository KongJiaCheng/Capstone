
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace FinalYearProject
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["ECommerceDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UpdateCartCount();
                HandleUserWelcome();
                HandleReturnUrlRedirection();
            }
        }

        private void UpdateCartCount()
        {
            string username = Session["Username"]?.ToString();

            // Use cookie if session is null
            if (string.IsNullOrEmpty(username))
            {
                HttpCookie userCookie = Request.Cookies["UsernameCookie"];
                if (userCookie != null)
                {
                    username = userCookie.Value;
                }
            }

            if (!string.IsNullOrEmpty(username))
            {
                string query = @"SELECT SUM(Quantity) 
                         FROM Payment 
                         WHERE Username = @Username AND PaymentStatus = 'Pending'";

                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        con.Open();

                        object result = cmd.ExecuteScalar();
                        int cartCount = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        lblCartCount.Text = cartCount > 0 ? cartCount.ToString() : string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error updating cart count: {ex}");
                    lblCartCount.Text = "0";
                }
            }
        }


        private void HandleUserWelcome()
        {
            HttpCookie userCookie = Request.Cookies["UsernameCookie"];
            lblMasterWelcome.Text = userCookie != null
                ? "Welcome, " + userCookie.Value
                : "Welcome, Guest";
        }


        private void HandleReturnUrlRedirection()
        {
            string returnUrl = Request.QueryString["ReturnUrl"];
            if (!string.IsNullOrWhiteSpace(returnUrl) && !Request.Url.AbsolutePath.EndsWith("Login.aspx"))
            {
                // Validate return URL to prevent open redirection attacks
                Uri returnUri;
                if (Uri.TryCreate(returnUrl, UriKind.Relative, out returnUri))
                {
                    Response.Redirect("~/ASP.NET/login.aspx?ReturnUrl=" + HttpUtility.UrlEncode(returnUrl));
                }
            }
        }

        protected void LoginStatus1_LoggingIn(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void LoginStatus1_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            // Sign out the user, clear session, and expire cookies
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            ExpireAllCookies(); // Clear cookies only during logout
        }

        private void ExpireAllCookies()
        {
            foreach (string cookieName in Request.Cookies.AllKeys)
            {
                ExpireCookie(cookieName);
            }
        }

        private void ExpireCookie(string cookieName)
        {
            if (Request.Cookies[cookieName] != null)
            {
                HttpCookie cookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1) };
                Response.Cookies.Add(cookie);
            }
        }

        public Label WelcomeLabel
        {
            get { return lblMasterWelcome; }
        }
    }
}
