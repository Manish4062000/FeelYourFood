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
    public partial class M_AddonAssignMaster : System.Web.UI.Page
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
                    LoadItem();
                    LoadMenu();
                    showAddonItem();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expire, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");
                }
            }
        }
        private void LoadItem()
        {
            string str1 = "SELECT ItemName, ItemId FROM ItemMaster WHERE ActiveStatus = 1 and AddOnAvailable=1 ORDER BY ItemId ASC";
            _c1.FillDropDown(str1, ddlitemlist);
            ddlitemlist.Items.Insert(0, "----- Select Food Item -----");
        }
        private void LoadMenu()
        {
            string str1 = "SELECT AddonMenuName, AddonMenuId FROM AddonMenuMaster WHERE ActiveStatus = 1 ORDER BY AddonMenuId ASC";
            _c1.FillDropDown(str1, ddlmenulist);
            ddlmenulist.Items.Insert(0, "----- Select Addon Menu -----");
        }



        private void showAddonItem()
        {
            string query = " select im.ItemName,adm.AddonMenuName,asm.* from AddonAssignMaster asm inner join ItemMaster im on im.ItemId=asm.ItemId inner join AddonMenuMaster adm on adm.AddonMenuId=asm.AddOnMenuId order by asm.Id desc ";
            _c1.Retrive2(query, ref ds);
            gvaddonassign.DataSource = ds;
            gvaddonassign.DataBind();

        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int itemid = Convert.ToInt32(ddlitemlist.SelectedValue);
            int addonmenuid = Convert.ToInt32(ddlmenulist.SelectedValue);
            if (ddlitemlist.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select a valid food item.');", true);
                return;
            }

            if (ddlmenulist.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select a valid addon Menu.');", true);
                return;
            }
            int status = Convert.ToInt32(rbStatus.SelectedValue);

            if (btnSubmit.Text == "Submit")
            {
                SqlParameter[] checkParams = new SqlParameter[3];
                checkParams[0] = new SqlParameter("@sp_type", 5);
                checkParams[1] = new SqlParameter("@ItemId", itemid);
                checkParams[2] = new SqlParameter("@AddOnMenuId", addonmenuid);

                object count = SqlHelper.ExecuteScalar(_c1.con, CommandType.StoredProcedure, "sp_AddOnAssignMaster", checkParams);
                int exists = Convert.ToInt32(count);

                if (exists > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('addon item already exists!');", true);
                    return;
                }

                SqlParameter[] insertParams = new SqlParameter[]
                 {
                        new SqlParameter("@sp_type", 1),
                        new SqlParameter("@ItemId", itemid),
                        new SqlParameter("@AddOnMenuId", addonmenuid),
                        new SqlParameter("@ActiveStatus", status),
                        new SqlParameter("@CreatedBy", Session["adminname"] != null ? Session["adminname"].ToString() : "Addsoft")
                };

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_AddOnAssignMaster", insertParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Assign Addon menu to item saved successfully');", true);
                    showAddonItem();
                    Clear();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error saving addon Item');", true);
                }

            }
            else
            {

                string ItemId = ViewState["uniqueId"]?.ToString();


                SqlParameter[] updateParams = new SqlParameter[]
                {
                        new SqlParameter("@sp_type", 2),
                        new SqlParameter("@ItemId", itemid),
                        new SqlParameter("@AddOnMenuId", addonmenuid),
                        new SqlParameter("@ActiveStatus", status),
                        new SqlParameter("@UpdatedDate", DateTime.Now),
                        new SqlParameter("@Id", ItemId),
                };

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_AddOnAssignMaster", updateParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Assign Addon updated successfully');", true);
                    btnSubmit.Text = "Submit";
                    showAddonItem();
                    Clear();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error updating Assign Addon');", true);
                }

            }
        }

        void Clear()
        {

            ddlmenulist.SelectedIndex = 0;
            ddlitemlist.SelectedIndex = 0;
            rbStatus.SelectedIndex = 0;
            btnSubmit.Text = "Submit";

        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }



        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            int ItemId = Convert.ToInt32(e.CommandArgument);

            SqlParameter[] deleteParams = new SqlParameter[]
            {
                new SqlParameter("@sp_type", 3),
                new SqlParameter("@Id", ItemId)
            };

            int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_AddOnAssignMaster", deleteParams);

            if (result != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Assign Addon item deleted successfully');", true);
                showAddonItem();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Failed to delete assign addon item');", true);
            }
        }

        protected void gvAddonItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvaddonassign.PageIndex = e.NewPageIndex;
            showAddonItem();
        }

        protected void btnEdit_Command(object sender, CommandEventArgs e)
        {
            int ItemId = Convert.ToInt32(e.CommandArgument);
            ViewState["uniqueId"] = ItemId;
            string query = " select * from AddonAssignMaster  where Id='" + ItemId + "' ";
            _c1.Retrive2(query, ref ds);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                LoadItem();
                object IdObj = ds.Tables[0].Rows[0]["ItemId"];
                if (IdObj != null && IdObj != DBNull.Value)
                {
                    string itemid = IdObj.ToString();
                    ddlitemlist.SelectedValue = itemid;
                }
                LoadMenu();
                object addonmenuIdObj = ds.Tables[0].Rows[0]["AddOnMenuId"];
                if (addonmenuIdObj != null && addonmenuIdObj != DBNull.Value)
                {
                    string menuId = addonmenuIdObj.ToString();
                    ddlmenulist.SelectedValue = menuId;
                }
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
    }
}