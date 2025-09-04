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
    public partial class M_DeviceMaster : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        public readonly Connection _c1 = new Connection();
        public DataTable _dtt = new DataTable();
        public DataSet ds = new DataSet();
        private string ItemImagePath = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["restid"] != null)
                {

                    showdevice();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expire, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");
                }
            }
        }
        private void showdevice()
        {
            string query = " select * from DeviceMaster order by Id desc ";
            _c1.Retrive2(query, ref ds);
            gvdevice.DataSource = ds;
            gvdevice.DataBind();

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string Devicename = device.Text.Trim();
           
            if (string.IsNullOrEmpty(Devicename))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Device Name is required.');", true);
                return;
            }
           
            int status = Convert.ToInt32(rbStatus.SelectedValue);

            if (btnSubmit.Text == "Submit")
            {
                SqlParameter[] checkParams = new SqlParameter[2];
                checkParams[0] = new SqlParameter("@sp_type", 5);
                checkParams[1] = new SqlParameter("@DeviceName", Devicename);

                object count = SqlHelper.ExecuteScalar(_c1.con, CommandType.StoredProcedure, "sp_DeviceMaster", checkParams);
                int exists = Convert.ToInt32(count);

                if (exists > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('This Device already exists!');", true);
                    return;
                }

                SqlParameter[] insertParams = new SqlParameter[]
                 {
                new SqlParameter("@sp_type", 1),
                new SqlParameter("@DeviceName", Devicename),
                new SqlParameter("@ActiveStatus", status)
                };

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_DeviceMaster", insertParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Device saved successfully');", true);
                    showdevice();
                    Clear();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error saving Device');", true);
                }

            }
            else
            {

                string id = ViewState["uniqueId"]?.ToString();


                SqlParameter[] updateParams = new SqlParameter[]
                {
                new SqlParameter("@sp_type", 2),
                new SqlParameter("@DeviceName", Devicename),
                new SqlParameter("@ActiveStatus", status),
                new SqlParameter("@Id", id),
                };

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_DeviceMaster", updateParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Device updated successfully');", true);
                    btnSubmit.Text = "Submit";
                    showdevice();
                    Clear();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error updating Device');", true);
                }

            }
        }

        void Clear()
        {

            device.Text = string.Empty;
            rbStatus.SelectedIndex = 0;
            btnSubmit.Text = "Submit";

        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }



        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            SqlParameter[] deleteParams = new SqlParameter[]
            {
        new SqlParameter("@sp_type", 3),
        new SqlParameter("@Id", id)
            };

            int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_DeviceMaster", deleteParams);

            if (result != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Device deleted successfully');", true);
                showdevice();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Failed to delete Device');", true);
            }
        }

        protected void btnEdit_Command(object sender, CommandEventArgs e)
        {
            int Id = Convert.ToInt32(e.CommandArgument);
            ViewState["uniqueId"] = Id;
            string query = " select * from DeviceMaster  where Id='" + Id + "' ";
            _c1.Retrive2(query, ref ds);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                device.Text = ds.Tables[0].Rows[0]["Devicename"].ToString();
                object statusObj = ds.Tables[0].Rows[0]["ActiveStatus"];
                if (statusObj != null && statusObj != DBNull.Value)
                {
                    int statusValue = Convert.ToInt32(statusObj);
                    rbStatus.SelectedValue = statusValue.ToString();
                }

                btnSubmit.Text = "Update";

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error in fetching data ');", true);
                return;
            }
        }

        protected void gvdevice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvdevice.PageIndex = e.NewPageIndex;
            showdevice();
        }
    }
}