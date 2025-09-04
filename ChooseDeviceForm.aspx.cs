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
    public partial class ChooseDeviceForm : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        public readonly Connection _c1 = new Connection();
        public DataTable _dtt = new DataTable();
        public DataSet ds = new DataSet();
        public string str = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Session["restid"] != null )
                {
                    showDetails();
                    if (Convert.ToInt32(Session["approved"]) == 1)
                    {
                        ddlKiosk.Enabled = false;
                        ddlServer.Enabled = false;
                        ddlKitchenDisplay.Enabled = false;
                        ddlQMS.Enabled = false;
                        ddlTableTablet.Enabled = false;
                        btnsubmit.Enabled = false;
                    }

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
            string query = "SELECT OrderingKiosk, server, kitchendisplay, qms, tabletab FROM ChooseDevice WHERE RestId = '" + Session["restid"].ToString() + "'";
            _c1.Retrive2(query, ref ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                object ddlkioskObj = ds.Tables[0].Rows[0]["OrderingKiosk"];
                if (ddlkioskObj != null && ddlkioskObj != DBNull.Value)
                {
                    int statusValue = Convert.ToInt32(ddlkioskObj);
                    ddlKiosk.SelectedValue = statusValue.ToString();
                }
                object ddlserverObj = ds.Tables[0].Rows[0]["server"];
                if (ddlserverObj != null && ddlserverObj != DBNull.Value)
                {
                    string statusValue = ddlserverObj.ToString();
                    ddlServer.SelectedValue = statusValue.ToString();
                }
                object ddlKitchenObj = ds.Tables[0].Rows[0]["kitchendisplay"];
                if (ddlKitchenObj != null && ddlKitchenObj != DBNull.Value)
                {
                    int statusValue = Convert.ToInt32(ddlKitchenObj);
                    ddlKitchenDisplay.SelectedValue = statusValue.ToString();
                }
                object ddlqmsObj = ds.Tables[0].Rows[0]["qms"];
                if (ddlqmsObj != null && ddlqmsObj != DBNull.Value)
                {
                    int statusValue = Convert.ToInt32(ddlqmsObj);
                    ddlQMS.SelectedValue = statusValue.ToString();
                }
                object ddltabletabObj = ds.Tables[0].Rows[0]["tabletab"];
                if (ddltabletabObj != null && ddltabletabObj != DBNull.Value)
                {
                    int statusValue = Convert.ToInt32(ddltabletabObj);
                    ddlTableTablet.SelectedValue = statusValue.ToString();
                }
            }

        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            int kiosk = Convert.ToInt32(ddlKiosk.SelectedValue);
            string server = ddlServer.SelectedValue;
            int kitchen = Convert.ToInt32(ddlKitchenDisplay.SelectedValue);
            int qms = Convert.ToInt32(ddlQMS.SelectedValue);
            int tabletab = Convert.ToInt32(ddlTableTablet.SelectedValue);
            int restId = Convert.ToInt32(Session["restid"]);
            string query = $"SELECT DeviceId FROM ChooseDevice WHERE RestId = '{restId}'";

            _c1.Retrive2(query, ref ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int deviceid = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@sp_type", "2");
                param[1] = new SqlParameter("@RestId", restId);
                param[2] = new SqlParameter("@OrderingKiosk", kiosk);
                param[3] = new SqlParameter("@Server", server);
                param[4] = new SqlParameter("@KitchenDisplay", kitchen);
                param[5] = new SqlParameter("@Qms", qms);
                param[6] = new SqlParameter("@TableTab", tabletab);
                param[7] = new SqlParameter("@UpdatedBy", restId);
                param[8] = new SqlParameter("@UpdatedDate", DateTime.Now);
                param[9] = new SqlParameter("@DeviceId", deviceid);
                int iresult = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_ChooseDevice", param);
                if (iresult != 0)
                {
                    Response.Redirect("ConfigureKioskForm.aspx");
                }
            }
            else
            {
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@sp_type", "1");
                param[1] = new SqlParameter("@RestId", restId);
                param[2] = new SqlParameter("@OrderingKiosk", kiosk);
                param[3] = new SqlParameter("@Server", server);
                param[4] = new SqlParameter("@KitchenDisplay", kitchen);
                param[5] = new SqlParameter("@Qms", qms);
                param[6] = new SqlParameter("@TableTab", tabletab);
                param[7] = new SqlParameter("@CreatedBy", restId);

                int iresult = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_ChooseDevice", param);
                if (iresult != 0)
                {
                    Response.Redirect("ConfigureKioskForm.aspx");
                }
            }


        }
    }
}