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
    public partial class M_AddonMenuMaster : System.Web.UI.Page
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

                    ShowAddonMenu();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expire, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");
                }
            }
        }
        private void ShowAddonMenu()
        {
            string query = " select * from AddonMenuMaster  order by AddonMenuId desc";
            _c1.Retrive2(query, ref ds);
            gvaddonmenu.DataSource = ds;
            gvaddonmenu.DataBind();
        }
        protected void ImageBtnDelete_Command(object sender, CommandEventArgs e)
        {
            int addonmenuid = Convert.ToInt32(e.CommandArgument);

            SqlParameter[] deleteParams = new SqlParameter[]
            {
                new SqlParameter("@sp_type", 3),
                new SqlParameter("@AddonMenuId", addonmenuid)
            };

            int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_AddonMenuMaster", deleteParams);

            if (result != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Addon Menu deleted successfully');", true);
                ShowAddonMenu();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Failed to delete Addon Menu');", true);
            }
        }
        protected void ImageBtnEdit_Command(object sender, CommandEventArgs e)
        {
            int addonmenuid = Convert.ToInt32(e.CommandArgument);
            ViewState["addonmenuid"] = addonmenuid;
            string query = " select im.* from AddonMenuMaster im where im.AddonMenuId='" + addonmenuid + "' ";
            _c1.Retrive2(query, ref ds);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                menuName.Text = ds.Tables[0].Rows[0]["AddonMenuName"].ToString();

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
        protected void gvaddonmenu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvaddonmenu.PageIndex = e.NewPageIndex;
            ShowAddonMenu();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string addonmenu = menuName.Text.Trim();
            int status = Convert.ToInt32(rbStatus.SelectedValue);

            if (string.IsNullOrEmpty(addonmenu))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Addon Menu name is required.');", true);
                return;
            }
            if (btnSubmit.Text == "Submit")
            {
                SqlParameter[] checkParams = new SqlParameter[2];
                checkParams[0] = new SqlParameter("@sp_type", 5);
                checkParams[1] = new SqlParameter("@AddonMenuName", addonmenu);

                object count = SqlHelper.ExecuteScalar(_c1.con, CommandType.StoredProcedure, "sp_AddonMenuMaster", checkParams);
                int exists = Convert.ToInt32(count);

                if (exists > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{addonmenu} Addon Menu already exists!');", true);
                    return;
                }
                SqlParameter[] insertParams = new SqlParameter[]
                 {
            new SqlParameter("@sp_type", 1),
            new SqlParameter("@AddonMenuName", addonmenu),
            new SqlParameter("@ActiveStatus", status),
            new SqlParameter("@CreatedBy", Session["adminname"] != null ? Session["adminname"].ToString() : "Addsoft")
                };

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_AddonMenuMaster", insertParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Addon Menu saved successfully');", true);
                    ShowAddonMenu();
                    Clear();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error saving Addon Menu');", true);
                }

            }
            else
            {
                string addonmenuId = ViewState["addonmenuid"]?.ToString();
                SqlParameter[] updateParams = new SqlParameter[]
                {
            new SqlParameter("@sp_type", 2),
            new SqlParameter("@AddonMenuName", addonmenu),
            new SqlParameter("@ActiveStatus", status),
            new SqlParameter("@UpdatedDate", DateTime.Now),
            new SqlParameter("@AddonMenuId", addonmenuId),
                };

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_AddonMenuMaster", updateParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Addon Menu updated successfully');", true);
                    btnSubmit.Text = "Submit";
                    ShowAddonMenu();
                    Clear();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error updating Addon Menu');", true);
                }


            }

        }
        void Clear()
        {

            menuName.Text = string.Empty;
            rbStatus.SelectedIndex = 0;

            btnSubmit.Text = "Submit";

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void btnEdit_Command(object sender, CommandEventArgs e)
        {
            int addonmenuid = Convert.ToInt32(e.CommandArgument);
            ViewState["addonmenuid"] = addonmenuid;
            string query = " select im.* from AddonMenuMaster im where im.AddonMenuId='" + addonmenuid + "' ";
            _c1.Retrive2(query, ref ds);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                menuName.Text = ds.Tables[0].Rows[0]["AddonMenuName"].ToString();

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

        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            int addonmenuid = Convert.ToInt32(e.CommandArgument);

            SqlParameter[] deleteParams = new SqlParameter[]
            {
                new SqlParameter("@sp_type", 3),
                new SqlParameter("@AddonMenuId", addonmenuid)
            };

            int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_AddonMenuMaster", deleteParams);

            if (result != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Addon Menu deleted successfully');", true);
                ShowAddonMenu();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Failed to delete Addon Menu');", true);
            }
        }
    }
}