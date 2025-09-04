using FeelYourFood_Admin.AppCode;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace FeelYourFood
{
    public partial class M_DeviceAndSubscriptionGSTMaster : System.Web.UI.Page
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
                    showGst();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expire, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");
                }
            }
            
        }

        private void showGst()
        {
            string str1 = "select * FROM DeviceGST order by Id DESC";
            _c1.Retrive2(str1, ref ds);
            gvgst.DataSource = ds;
            gvgst.DataBind();
        }

        protected void gvgst_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvgst.PageIndex = e.NewPageIndex;
            showGst();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string deviceg = devicegst.Text.Trim();
            string softwareg = softwaregst.Text.Trim();

            if (string.IsNullOrEmpty(deviceg))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Device GST is required.');", true);
                return;
            }
            if (string.IsNullOrEmpty(softwareg))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Software GST is required.');", true);
                return;
            }

            int status = Convert.ToInt32(rbStatus.SelectedValue); // 1 or 0

            if (status == 1)
            {
                SqlParameter[] deactivateAll = new SqlParameter[]
                {
            new SqlParameter("@sp_type", 8)  // Add this logic to deactivate all in SP
                };
                SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_DeviceGST", deactivateAll);
            }

            if (btnSubmit.Text == "Submit")
            {
                // Insert new record
                SqlParameter[] insertParams = new SqlParameter[]
                {
            new SqlParameter("@sp_type", 1),
            new SqlParameter("@devicegst", deviceg),
            new SqlParameter("@softwaregst", softwareg),
            new SqlParameter("@ActiveStatus", status)
                };

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_DeviceGST", insertParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Device and software GST saved successfully');", true);
                    showGst();
                    Clear();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error in saving device and software GST');", true);
                }
            }
            else
            {
                // Update existing record
                string id = ViewState["uniqueId"]?.ToString();
                SqlParameter[] updateParams = new SqlParameter[]
                {
            new SqlParameter("@sp_type", 2),
            new SqlParameter("@devicegst", deviceg),
            new SqlParameter("@softwaregst", softwareg),
            new SqlParameter("@Id", id),
            new SqlParameter("@ActiveStatus", status)
                };

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_DeviceGST", updateParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Device and software GST updated successfully');", true);
                    btnSubmit.Text = "Submit";
                    showGst();
                    Clear();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error in updating device and software GST');", true);
                }
            }
        }

        void Clear()
        {

            devicegst.Text = string.Empty;
            softwaregst.Text = string.Empty;
            btnSubmit.Text = "Submit";
            showGst();

        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void btnEdit_Command(object sender, CommandEventArgs e)
        {
            int Id = Convert.ToInt32(e.CommandArgument);
            ViewState["uniqueId"] = Id;
            string query = " select * from DeviceGST  where Id='" + Id + "' ";
            _c1.Retrive2(query, ref ds);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                devicegst.Text = ds.Tables[0].Rows[0]["devicegst"].ToString();
                softwaregst.Text = ds.Tables[0].Rows[0]["softwaregst"].ToString();
                rbStatus.SelectedValue = (Convert.ToInt16(ds.Tables[0].Rows[0]["ActiveStatus"]).ToString());
                btnSubmit.Text = "Update";

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error in fetching data ');", true);
                return;
            }
        }


    }
}