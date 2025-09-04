using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeelYourFood
{
    public partial class M_ProfileForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack  )
            {
                if (Session["restid"] != null)
                {
                    int restId = Convert.ToInt32(Session["restid"]);
                    string query = "SELECT * FROM Resturant_And_AdminMaster WHERE RestId = @RestId";

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@RestId", restId);
                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            lblName.Text = reader["Name"].ToString();
                            lblMobile.Text = reader["MobileNo"].ToString();
                            lblEmail.Text = reader["EmailId"].ToString();
                        }
                    }
                }
                else
                {
                    Response.Redirect("LogOut.aspx");
                }
            }
        }
    }
}