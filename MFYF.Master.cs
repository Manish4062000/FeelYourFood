using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeelYourFood
{
    public partial class MFYF : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["restid"] != null)
                {
                    if (Session["adminname"] != null)
                    {
                        string adminName = Session["adminname"].ToString();
                        lbladminname.Text = adminName.Length > 6 ? adminName.Substring(0, 6) + ".." : adminName;
                    }
                    else
                    {
                        lbladminname.Text = "Sadmin";
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