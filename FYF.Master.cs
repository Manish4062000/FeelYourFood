using FeelYourFood_Admin.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeelYourFood
{
    public partial class FYF : System.Web.UI.MasterPage
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        public readonly Connection _c1 = new Connection();
        public DataTable _dtt = new DataTable();
        public DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["restid"] != null)
                {
                    if (Session["adminname"] != null || Session["adminname"].ToString() != "")
                    {
                        string adminName = Session["adminname"].ToString();
                        lbladminname.Text = adminName.Length > 6 ? adminName.Substring(0, 6) + ".." : adminName;
                    }
                    else
                    {
                        lbladminname.Text = "Admin";
                    }

                    if (Session["restlogo"].ToString() != "")
                    {
                        imgRestLogo.ImageUrl = Session["restlogo"].ToString();
                        adminIcon.ImageUrl = Session["restlogo"].ToString();
                        adminIcon1.ImageUrl = Session["restlogo"].ToString();
                    }
                    else
                    {
                        imgRestLogo.ImageUrl = ResolveUrl("~/images/icon.png");
                        adminIcon.ImageUrl = ResolveUrl("~/images/icon.png");
                        adminIcon1.ImageUrl = ResolveUrl("~/images/icon.png");
                    }
                    if (Session["restname"] != null)
                    {
                        dynamicImage.InnerHtml = Session["restname"].ToString();
                    }
                    if (Session["restname"].ToString() != "")
                    {
                        dynamicImage.InnerHtml = Session["restname"].ToString();

                    }
                    else
                    {
                        dynamicImage.InnerHtml = "Feel Your Food";
                    }

                    string restId = Session["restid"]?.ToString();

                    if (!string.IsNullOrEmpty(restId))
                    {
                        string str1 = "SELECT ConsumeTypeId FROM Kiosk_ConsumeType WHERE RestId = '" + restId + "'";
                        DataSet ds = new DataSet();
                        _c1.Retrive2(str1, ref ds);

                        if (ds.Tables.Count > 0)
                        {
                            var rows = ds.Tables[0].AsEnumerable();
                            var allAreThree = rows.All(row => row["ConsumeTypeId"].ToString() == "3");

                            liTableMaster.Visible = !allAreThree;
                        }
                    }

                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expire, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");

                }



            }
        }
    }
}