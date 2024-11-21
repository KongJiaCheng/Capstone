using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections.Generic;
using FinalYearProject.Models;
using System.Web;

namespace FinalYearProject.ASP.NET
{
    public partial class ProductPage : System.Web.UI.Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["ECommerceDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindProductRepeater();
            }
        }

        private void BindProductRepeater()
        {
            string query = "SELECT ProductID, ProductName, ProductDescription, Price, Stock, ImageURL FROM product";
            ExecuteAndBind(query, null);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Please enter a search term.');", true);
                return;
            }

            string query = "SELECT ProductID, ProductName, ProductDescription, Price, Stock, ImageURL FROM product WHERE ProductName LIKE @ProductName";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductName", "%" + txtSearch.Text + "%");

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    ProductRepeaterControl.DataSource = rdr;
                    ProductRepeaterControl.DataBind();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Error: {ex.Message}');", true);
            }
        }



        protected void FilterProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FilterProduct.SelectedValue == "All")
            {
                string query = "SELECT ProductID, ProductName, ProductDescription, Price, Stock, ImageURL FROM product";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        ProductRepeaterControl.DataSource = rdr;
                        ProductRepeaterControl.DataBind();
                    }
                }
            }
            if (FilterProduct.SelectedValue == "Laptop")
            {
                string query = "SELECT ProductID, ProductName, ProductDescription, Price, Stock, ImageURL FROM product WHERE ProductID LIKE 'L%'";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        ProductRepeaterControl.DataSource = rdr;
                        ProductRepeaterControl.DataBind();
                    }
                }
            }
            if (FilterProduct.SelectedValue == "Desktop")
            {
                string query = "SELECT ProductID, ProductName, ProductDescription, Price, Stock, ImageURL FROM product WHERE ProductID LIKE 'D%'";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        ProductRepeaterControl.DataSource = rdr;
                        ProductRepeaterControl.DataBind();
                    }
                }
            }
            if (FilterProduct.SelectedValue == "Accessories")
            {
                string query = "SELECT ProductID, ProductName, ProductDescription, Price, Stock, ImageURL FROM product WHERE ProductID LIKE 'A%'";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        ProductRepeaterControl.DataSource = rdr;
                        ProductRepeaterControl.DataBind();
                    }
                }
            }
        }

        private void ExecuteAndBind(string query, Dictionary<string, object> parameters)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (parameters != null)
                    foreach (var param in parameters)
                        cmd.Parameters.AddWithValue(param.Key, param.Value);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                ProductRepeaterControl.DataSource = rdr;
                ProductRepeaterControl.DataBind();
            }
        }

        protected void AddToCart_Click(object sender, CommandEventArgs e)
        {
            string username = Session["Username"]?.ToString();
            if (string.IsNullOrEmpty(username))
            {
                HttpCookie userCookie = Request.Cookies["UsernameCookie"];
                if (userCookie != null)
                {
                    username = userCookie.Value;
                }
            }
            string productId = e.CommandArgument.ToString();

            if (username == null)
            {
                ShowAlert("Please register first.");
                return;
            }

            string orderId = GetOrCreateOrderId(username, productId);
            decimal price = GetProductPrice(productId);
            AddToCartProcedure(productId, username, orderId, price);
        }

        private void AddToCartProcedure(string productId, string username, string orderId, decimal price)
        {
            if (!IsProductInStock(productId))
            {
                ShowAlert("Product is out of stock.");
                return;
            }

            string productName = GetProductName(productId);
            int PaymentId = GetNextPaymentId(); // Store result to ensure it's not null or invalid

            string query = @"
            BEGIN TRANSACTION;

            DECLARE @CurrentStatus NVARCHAR(50);
            SELECT @CurrentStatus = PaymentStatus 
            FROM Payment 
            WHERE ProductID = @ProductID AND OrderID = @OrderID;

            IF @CurrentStatus = 'Completed'
            BEGIN
                INSERT INTO Payment 
                    (PaymentID, OrderID, Username, OrderDate, ProductID, ProductName, Price, Quantity, TotalPrice, PaymentStatus)
                VALUES 
                    (@NewPaymentID, @OrderID, @Username, GETDATE(), @ProductID, @ProductName, @Price, 1, @Price, 'Pending');
            END
            ELSE IF @CurrentStatus = 'Pending' 
            BEGIN
                UPDATE Payment 
                SET Quantity = Quantity + 1, TotalPrice = TotalPrice + @Price 
                WHERE ProductID = @ProductID AND OrderID = @OrderID;
            END;
            ELSE IF @CurrentStatus IS NULL
            BEGIN
                INSERT INTO Payment 
                    (PaymentID, OrderID, Username, OrderDate, ProductID, ProductName, Price, Quantity, TotalPrice, PaymentStatus)
                VALUES 
                    (@NewPaymentID, @OrderID, @Username, GETDATE(), @ProductID, @ProductName, @Price, 1, @Price, 'Pending');
            END

            UPDATE product SET Stock = Stock - 1 WHERE ProductID = @ProductID;

            COMMIT;
            ";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlTransaction transaction = con.BeginTransaction())
                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@OrderID", orderId);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@ProductName", productName);
                        cmd.Parameters.AddWithValue("@NewPaymentID", PaymentId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                            ShowAlert("Transaction rolled back: No data was inserted/updated.");
                        }
                    }
                }

                BindProductRepeater();
            }
            catch (Exception ex)
            {
                // Log and handle the rollback
                ShowAlert($"Error occurred: {ex.Message}");
            }

        }



        protected void RemoveFromCart_Click(object sender, CommandEventArgs e)
        {
            string username = Session["Username"]?.ToString();
            if (string.IsNullOrEmpty(username))
            {
                HttpCookie userCookie = Request.Cookies["UsernameCookie"];
                if (userCookie != null)
                {
                    username = userCookie.Value;
                }
            }
            if (username == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Please register first.');", true);
                return;
            }

            string productId = e.CommandArgument.ToString();
            string orderId = GetOrCreateOrderId(username, productId);
            int quantity = GetQuantity(productId);
            RemoveFromCartProcedure(productId, orderId);
        }

        private void RemoveFromCartProcedure(string productId, string orderId)
        {
            // Get the quantity of the product in the Payment table
            int paymentQuantity = GetPaymentQuantity(productId, orderId);

            // If quantity in Payment is greater than 0, proceed with removal
            if (paymentQuantity > 0)
            {
                string query = @"
            BEGIN TRANSACTION;

            -- Update stock by incrementing
            UPDATE product SET Stock = Stock + 1 WHERE ProductID = @ProductID;

            -- Decrement quantity in Payment table
            UPDATE Payment SET Quantity = Quantity - 1 
            WHERE ProductID = @ProductID AND OrderID = @OrderID;

            -- Check if quantity has dropped to zero after the update
            IF EXISTS (SELECT 1 FROM Payment WHERE ProductID = @ProductID AND OrderID = @OrderID AND Quantity <= 0)
            BEGIN
                -- Delete from Payment if quantity is now zero
                DELETE FROM Payment WHERE ProductID = @ProductID AND OrderID = @OrderID;
            END

            COMMIT;
            ";

                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlTransaction transaction = con.BeginTransaction())
                        using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ProductID", productId);
                            cmd.Parameters.AddWithValue("@OrderID", orderId);

                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log error (consider using a logging framework)
                    ShowAlert($"Error occurred: {ex.Message}");
                    // Optionally handle transaction rollback if needed
                }
            }

            if (paymentQuantity == 0)
            {
                DisableRemoveButton(productId);
            }

            // Refresh the Repeater to reflect the changes in UI
            BindProductRepeater();
        }


        // Method to get the quantity of a product in the Payment table
        private int GetPaymentQuantity(string productId, string orderId)
        {
            string query = "SELECT Quantity FROM Payment WHERE ProductID = @ProductID AND OrderID = @OrderID";
            return int.Parse(ExecuteScalar(query, new Dictionary<string, object> { { "@ProductID", productId }, { "@OrderID", orderId } }));
        }



        private void DisableRemoveButton(string productId)
        {
            // Loop through each item in the Repeater and find the Remove button for the specified productId
            foreach (RepeaterItem item in ProductRepeaterControl.Items)
            {
                var hiddenProductId = item.FindControl("HiddenProductId") as HiddenField;
                var btnRemove = item.FindControl("btnRemove") as Button;

                if (hiddenProductId != null && btnRemove != null && hiddenProductId.Value == productId)
                {
                    btnRemove.Enabled = false; // Disable the Remove button
                }
            }
        }

        private string GetOrCreateOrderId(string username, string productId)
        {
            string existingOrderId = GetExistingOrderId(username);
            return !string.IsNullOrEmpty(existingOrderId) ? existingOrderId : CreateNewOrder(productId, username);
        }

        private string GetExistingOrderId(string username)
        {
            string query = "SELECT TOP 1 OrderID FROM Orders WHERE Username = @Username AND IsCompleted = 0 ORDER BY OrderDate DESC";
            return ExecuteScalar(query, new Dictionary<string, object> { { "@Username", username } });
        }

        private string CreateNewOrder(string productId, string username)
        {
            string query = @"
                INSERT INTO Orders (Username, OrderDate, IsCompleted, ProductID) 
                OUTPUT INSERTED.OrderID 
                VALUES (@Username, GETDATE(), 0, @ProductID)";
            return ExecuteScalar(query, new Dictionary<string, object> { { "@Username", username }, { "@ProductID", productId } });
        }

        private int GetNextPaymentId()
        {
            string query = "SELECT ISNULL(MAX(PaymentID), 0) + 1 FROM Payment";
            return int.Parse(ExecuteScalar(query, null));
        }

        private int GetQuantity(string productId)
        {
            string query = @"
                SELECT Quantity FROM Orders WHERE ProductID = @ProductID
                UNION ALL
                SELECT Quantity FROM Payment WHERE ProductID = @ProductID";

            return int.Parse(ExecuteScalar(query, new Dictionary<string, object> { { "@ProductID", productId } }));
        }
        private void ExecuteNonQuery(string query, string productId, string username, string orderId, decimal price)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@ProductName", GetProductName(productId));
                    cmd.Parameters.AddWithValue("@NewPaymentID", GetNextPaymentId());

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log the error or display it
                ShowAlert($"Error occurred: {ex.Message}");
            }
        }


        private decimal GetProductPrice(string productId)
        {
            string query = "SELECT Price FROM product WHERE ProductID = @ProductID";
            return decimal.Parse(ExecuteScalar(query, new Dictionary<string, object> { { "@ProductID", productId } }));
        }

        private bool IsProductInStock(string productId)
        {
            string query = "SELECT Stock FROM product WHERE ProductID = @ProductID";
            int stock = int.Parse(ExecuteScalar(query, new Dictionary<string, object> { { "@ProductID", productId } }));
            if (stock <= 0)
            {
                ShowAlert("The product is out of stock.");
                return false;
            }
            return true;
        }

        private string GetProductName(string productId)
        {
            string query = "SELECT ProductName FROM product WHERE ProductID = @ProductID";
            return ExecuteScalar(query, new Dictionary<string, object> { { "@ProductID", productId } });
        }

        private string ExecuteScalar(string query, Dictionary<string, object> parameters)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (parameters != null)
                    foreach (var param in parameters)
                        cmd.Parameters.AddWithValue(param.Key, param.Value);

                con.Open();
                return cmd.ExecuteScalar()?.ToString();
            }
        }

        private void ShowAlert(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('{message}');", true);
        }
    }
}
