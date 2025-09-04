using FeelYourFood_Admin.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeelYourFood
{
    public partial class ResetpasswordForm : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        public readonly Connection _c1 = new Connection();
        public DataTable _dtt = new DataTable();
        public DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string password = Convert.ToString(Request.Form["password"]).Trim();
            string confirmPassword = Convert.ToString(Request.Form["confirmPassword"]).Trim();

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

            int restid = 0;

            if (!string.IsNullOrEmpty(Request.QueryString["restId"]))
            {
                int.TryParse(Request.QueryString["restId"], out restid);
            }

            string str = "Update Resturant_And_AdminMaster set Password='" + password + "' where RestId='" + restid + "'";

            int i = _c1.ExecNonquery(str);
            if (i != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertRedirect", "alert('Passwords Updated successfully. Please login with new password'); window.location='LoginForm.aspx';", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Somethings wents wrong recheck your mail');", true);
                return;
            }

        }
    }
}