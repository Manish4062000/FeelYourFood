using FeelYourFood_Admin.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeelYourFood
{
    public partial class RegistrationForm : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        public readonly Connection _c1 = new Connection();
        public DataTable _dtt = new DataTable();
        public DataSet ds = new DataSet();
        public string str = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string name = Convert.ToString(Request.Form["name"]).Trim();
            string email = Convert.ToString(Request.Form["email"]).Trim();
            string password = Convert.ToString(Request.Form["password"]).Trim();
            string confirmPassword = Convert.ToString(Request.Form["confirmPassword"]).Trim();
            string phone = Convert.ToString(Request.Form["phone"]).Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Email is required');", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Email is required');", true);
                return;
            }
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid email format');", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Password is required');", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Confirm password is required');", true);
                return;
            }

            if (password != confirmPassword)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Passwords do not match');", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Phone number is required');", true);
                return;
            }
            string phonePattern = @"^\d{10}$";

            if (!Regex.IsMatch(phone, phonePattern))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Phone number must be exactly 10 digits');", true);
                return;
            }

            SqlParameter[] param1 = new SqlParameter[2];
            param1[0] = new SqlParameter("@sp_type", "6");
            param1[1] = new SqlParameter("@EmailId", email);

            object count = SqlHelper.ExecuteScalar(_c1.con, CommandType.StoredProcedure, "sp_ResturantAndAdminMaster", param1);
            int result = Convert.ToInt32(count);

            if (result > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Email Id already registered  Please Login !');", true);
                return;
            }
            else
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@sp_type", "1");
                param[1] = new SqlParameter("@Name", name);
                param[2] = new SqlParameter("@EmailId", email);
                param[3] = new SqlParameter("@Password", password);
                param[4] = new SqlParameter("@MobileNo", phone);
                param[5] = new SqlParameter("@AdminType", "A");

                int iresult = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_ResturantAndAdminMaster", param);
                if (iresult > 0 || iresult == -1)
                {
                    Session["SuccessMessage"] = "Account has been successfully created. Please login!";
                    Response.Redirect("LoginForm.aspx");


                }

            }

        }
    }
}