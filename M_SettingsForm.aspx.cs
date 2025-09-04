using System;
using System.Configuration;
using System.Data.SqlClient;

namespace FeelYourFood
{
    public partial class M_SettingsForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["restid"] != null)
                {
                    LoadProfile();
                }
                else
                {
                    Response.Redirect("LogOut.aspx");
                }

            }
        }

        private void LoadProfile()
        {
            int restId = Convert.ToInt32(Session["restid"]);
            string query = "SELECT Name, MobileNo, EmailId, Password FROM Resturant_And_AdminMaster WHERE RestId = @RestId";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@RestId", restId);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtName.Text = reader["Name"].ToString();
                    txtMobile.Text = reader["MobileNo"].ToString();
                    txtEmail.Text = reader["EmailId"].ToString();

                    ViewState["Password"] = reader["Password"].ToString(); // store password temporarily
                }
            }
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            int restId = Convert.ToInt32(Session["restid"]);
            string name = txtName.Text.Trim();
            string mobile = txtMobile.Text.Trim();
            string email = txtEmail.Text.Trim();

            string query = "UPDATE Resturant_And_AdminMaster SET Name = @Name, MobileNo = @MobileNo, EmailId = @EmailId, UpdatedDate = GETDATE() WHERE RestId = @RestId";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@MobileNo", mobile);
                cmd.Parameters.AddWithValue("@EmailId", email);
                cmd.Parameters.AddWithValue("@RestId", restId);

                con.Open();
                int result = cmd.ExecuteNonQuery();

                lblProfileMsg.Text = result > 0 ? "Profile updated successfully!" : "Update failed.";
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (ViewState["Password"] == null) return;

            int restId = Convert.ToInt32(Session["restid"]);
            string oldPassword = txtOldPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (oldPassword != ViewState["Password"].ToString())
            {
                lblPasswordMsg.Text = "Old password is incorrect.";
                return;
            }

            if (newPassword != confirmPassword)
            {
                lblPasswordMsg.Text = "New and confirm passwords do not match.";
                return;
            }

            string query = "UPDATE Resturant_And_AdminMaster SET Password = @Password, UpdatedDate = GETDATE() WHERE RestId = @RestId";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Password", newPassword);
                cmd.Parameters.AddWithValue("@RestId", restId);

                con.Open();
                int result = cmd.ExecuteNonQuery();

                lblPasswordMsg.ForeColor = System.Drawing.Color.Green;
                lblPasswordMsg.Text = result > 0 ? "Password changed successfully." : "Password update failed.";
            }
        }
    }
}
