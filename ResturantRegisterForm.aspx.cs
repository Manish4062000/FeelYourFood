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
    public partial class ResturantRegisterForm : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        public readonly Connection _c1 = new Connection();
        public DataTable _dtt = new DataTable();
        public DataSet ds = new DataSet();
        public string str = string.Empty;
        private string ResturantImagePath = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["restid"] != null && !string.IsNullOrEmpty(Session["restid"].ToString()))
                {
                    showDetails();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expire, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");
                }
            }
        }
        private void showDetails()
        {
            string strChk = "SELECT ResturantName,RestAddress,RestPhone,RestLogo,GstNo FROM Resturant_And_AdminMaster where RestId='" + Session["restid"].ToString() + "'";
            DataSet dsChk = new DataSet();
            _c1.Retrive2(strChk, ref dsChk);
            if (dsChk != null && dsChk.Tables.Count > 0 && dsChk.Tables[0].Rows.Count > 0)
            {
                restname.Text = dsChk.Tables[0].Rows[0]["ResturantName"].ToString();
                location.Text = dsChk.Tables[0].Rows[0]["RestAddress"].ToString();
                restphone.Text = dsChk.Tables[0].Rows[0]["RestPhone"].ToString();
                gstno.Text = dsChk.Tables[0].Rows[0]["GstNo"].ToString();
                string photoPath = dsChk.Tables[0].Rows[0]["RestLogo"].ToString();
                if (!string.IsNullOrEmpty(photoPath))
                {
                    imagePreview.Src = ResolveUrl(photoPath);
                    imagePreview.Attributes["style"] = "display:block;width:190px;height:150px;";
                }
            }
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            string restName = Convert.ToString(restname.Text).Trim();
            string locations = Convert.ToString(location.Text).Trim();
            string restPhone = Convert.ToString(restphone.Text).Trim();
            string gstNo = Convert.ToString(gstno.Text).Trim();
            string temprest = restName.Length > 4 ? restName.Substring(0, 4) : restName;

            if (string.IsNullOrWhiteSpace(restName))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Restaurant name is required.');", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(locations))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Location is required.');", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(restPhone))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Restaurant phone number is required.');", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(gstNo))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('GST number is required.');", true);
                return;
            }
            if (Session["restid"] != null && !string.IsNullOrEmpty(Session["restid"].ToString()))
            {
                if (imageUpload.HasFile)
                {
                    var virtualFolder = "IMAGE/ResturantLogo/";
                    var physicalFolder = Server.MapPath(virtualFolder);
                    string file1 = temprest + "_" + imageUpload.FileName;
                    imageUpload.SaveAs(physicalFolder + file1);
                    ResturantImagePath = "~/IMAGE/ResturantLogo/" + file1;
                    Session["restlogo"] = ResturantImagePath;
                    Session["restname"] = restName;

                    SqlParameter[] param = new SqlParameter[11];
                    param[0] = new SqlParameter("@sp_type", "2");
                    param[1] = new SqlParameter("@ResturantName", restName);
                    param[2] = new SqlParameter("@RestAddress", locations);
                    param[3] = new SqlParameter("@RestPhone", restPhone);
                    param[4] = new SqlParameter("@RestLogo", ResturantImagePath);
                    param[5] = new SqlParameter("@GstNo", gstNo);
                    param[6] = new SqlParameter("@ActiveStatus", "1");
                    param[7] = new SqlParameter("@UpdatedBy", Session["restid"].ToString());
                    param[8] = new SqlParameter("@UpdatedDate", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss"));
                    param[9] = new SqlParameter("@Approved", "0");
                    param[10] = new SqlParameter("@RestId", Session["restid"].ToString());

                    int iresult = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_ResturantAndAdminMaster", param);
                    if (iresult != 0)
                    {
                        Response.Redirect("ChooseDeviceForm.aspx");
                    }
                }
                else
                {
                    Session["restname"] = restName;
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@sp_type", "7");
                    param[1] = new SqlParameter("@ResturantName", restName);
                    param[2] = new SqlParameter("@RestAddress", locations);
                    param[3] = new SqlParameter("@RestPhone", restPhone);
                    param[4] = new SqlParameter("@GstNo", gstNo);
                    param[5] = new SqlParameter("@ActiveStatus", "1");
                    param[6] = new SqlParameter("@UpdatedBy", Session["restid"]);
                    param[7] = new SqlParameter("@UpdatedDate", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss"));
                    param[8] = new SqlParameter("@Approved", "0");
                    param[9] = new SqlParameter("@RestId", Session["restid"].ToString());

                    int iresult = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_ResturantAndAdminMaster", param);
                    if (iresult != 0)
                    {
                        Response.Redirect("ChooseDeviceForm.aspx");

                    }
                }
            }


        }
    }
}