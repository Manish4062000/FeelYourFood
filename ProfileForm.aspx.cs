using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace FeelYourFood
{
    public partial class ProfileForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack )
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
                            txtName.Text = reader["Name"].ToString();
                            txtMobile.Text = reader["MobileNo"].ToString();
                            txtEmail.Text = reader["EmailId"].ToString();
                            txtRestaurant.Text = reader["ResturantName"].ToString();
                            txtAddress.Text = reader["RestAddress"].ToString();
                            txtRestPhone.Text = reader["RestPhone"].ToString();
                            txtGstNo.Text = reader["GstNo"].ToString();

                            string logoPath = reader["RestLogo"].ToString().Replace("~", "");
                            if (!string.IsNullOrEmpty(logoPath))
                            {
                                imgLogo.ImageUrl = ResolveUrl(logoPath);
                                imgLogo.Attributes["style"] = "display:block;width:300px;height:200px;";
                            }
                            else
                            {
                                imgLogo.ImageUrl = "~/images/no-image.png";
                            }

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
