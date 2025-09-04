using FeelYourFood_Admin.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeelYourFood
{
    public partial class M_AddonItemMaster : System.Web.UI.Page
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

        private void LoadMenu()
        {
            string str1 = "SELECT AddonMenuName, AddonMenuId FROM AddonMenuMaster WHERE ActiveStatus = 1 ORDER BY AddonMenuId ASC";
            _c1.FillDropDown(str1, ddlmenulist);
            ddlmenulist.Items.Insert(0, "----- Select Addon Menu -----");
        }

        protected void gvItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAddonItem.PageIndex = e.NewPageIndex;
            showAddonItem();
        }

        private void showAddonItem()
        {
            string query = " select im.*,mm.AddonMenuName from AddOnItemMaster im inner join AddonMenuMaster mm on im.AddonMenuId=mm.AddonMenuId order by im.AddOnItemId desc";
            _c1.Retrive2(query, ref ds);
            gvAddonItem.DataSource = ds;
            gvAddonItem.DataBind();

        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int addonmenuid = Convert.ToInt32(ddlmenulist.SelectedValue);
            string Itemname = itemName.Text.Trim();
            int status = Convert.ToInt32(rbStatus.SelectedValue);

            if (ddlmenulist.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select a valid addon Menu.');", true);
                return;
            }
            if (string.IsNullOrEmpty(Itemname))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Addon Item name is required.');", true);
                return;
            }
            string descript = description.Text.Trim();
            double pricetxt = Convert.ToDouble(price.Text.Trim());
            if (btnSubmit.Text == "Submit")
            {
                SqlParameter[] checkParams = new SqlParameter[2];
                checkParams[0] = new SqlParameter("@sp_type", 5);
                checkParams[1] = new SqlParameter("@AddonItemName", Itemname);

                object count = SqlHelper.ExecuteScalar(_c1.con, CommandType.StoredProcedure, "sp_AddOnItemMaster", checkParams);
                int exists = Convert.ToInt32(count);

                if (exists > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{Itemname} addon item already exists!');", true);
                    return;
                }
                if (myFile.HasFile)
                {
                    string virtualFolder = "IMAGE/AddonItemImages/";
                    string physicalFolder = Server.MapPath(virtualFolder);
                    string fileName = Session["adminname"] + "_" + myFile.FileName;
                    myFile.SaveAs(Path.Combine(physicalFolder, fileName));
                    ItemImagePath = "~/IMAGE/AddonItemImages/" + fileName;

                    SqlParameter[] insertParams = new SqlParameter[]
                     {
                        new SqlParameter("@sp_type", 1),
                        new SqlParameter("@AddonMenuId", addonmenuid),
                        new SqlParameter("@AddonItemName", Itemname),
                        new SqlParameter("@ItemDescription",descript ),
                        new SqlParameter("@AddOnItemPhoto", ItemImagePath),
                        new SqlParameter("@price", pricetxt),
                        new SqlParameter("@ActiveStatus", status),
                        new SqlParameter("@CreatedBy", Session["adminname"] != null ? Session["adminname"].ToString() : "Addsoft")
                    };

                    int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_AddOnItemMaster", insertParams);

                    if (result != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Addon Item saved successfully');", true);
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('upload an image first !');", true);
                    return;
                }
            }
            else
            {
                string ItemId = ViewState["uniqueId"]?.ToString();
                if (myFile.HasFile)
                {
                    string virtualFolder = "IMAGE/AddonItemImages/";
                    string physicalFolder = Server.MapPath(virtualFolder);
                    string fileName = Session["adminname"] + "_" + myFile.FileName;
                    myFile.SaveAs(Path.Combine(physicalFolder, fileName));
                    ItemImagePath = "~/IMAGE/AddonItemImages/" + fileName;

                    SqlParameter[] updateParams = new SqlParameter[]
                    {
                        new SqlParameter("@sp_type", 2),
                        new SqlParameter("@AddonMenuId", addonmenuid),
                        new SqlParameter("@AddonItemName", Itemname),
                        new SqlParameter("@ItemDescription",descript ),
                        new SqlParameter("@AddOnItemPhoto", ItemImagePath),
                        new SqlParameter("@price", pricetxt),
                        new SqlParameter("@ActiveStatus", status),
                        new SqlParameter("@UpdatedDate", DateTime.Now),
                        new SqlParameter("@AddOnItemId", ItemId),
                    };

                    int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_AddOnItemMaster", updateParams);

                    if (result != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Addon Item updated successfully');", true);
                        btnSubmit.Text = "Submit";
                        showAddonItem();
                        Clear();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error updating Item');", true);
                    }
                }
                else
                {
                    SqlParameter[] updateParams = new SqlParameter[]
                    {
                        new SqlParameter("@sp_type", 4),
                        new SqlParameter("@AddonMenuId", addonmenuid),
                        new SqlParameter("@AddonItemName", Itemname),
                        new SqlParameter("@ItemDescription",descript ),
                        new SqlParameter("@price", pricetxt),
                        new SqlParameter("@ActiveStatus", status),
                        new SqlParameter("@UpdatedDate", DateTime.Now),
                        new SqlParameter("@AddOnItemId", ItemId),
                    };

                    int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_AddOnItemMaster", updateParams);

                    if (result != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Item updated successfully');", true);
                        btnSubmit.Text = "Submit";
                        showAddonItem();
                        Clear();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error updating Item');", true);
                    }
                }
            }
        }

        void Clear()
        {
            imagePreview.Src = "#";
            imagePreview.Attributes["style"] = "display:none;width:190px;height:150px;";
            itemName.Text = string.Empty;
            rbStatus.SelectedIndex = 0;
            description.Text = string.Empty;
            price.Text = "0.00";
            ddlmenulist.SelectedIndex = 0;
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
                new SqlParameter("@AddOnItemId", ItemId)
            };

            int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_AddOnItemMaster", deleteParams);

            if (result != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Addon item deleted successfully');", true);
                showAddonItem();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Failed to delete addon item');", true);
            }
        }



        protected void gvAddonItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAddonItem.PageIndex = e.NewPageIndex;
            showAddonItem();
        }

        protected void btnEdit_Command(object sender, CommandEventArgs e)
        {
            int ItemId = Convert.ToInt32(e.CommandArgument);
            ViewState["uniqueId"] = ItemId;
            string query = " select * from AddOnItemMaster  where AddOnItemId='" + ItemId + "' ";
            _c1.Retrive2(query, ref ds);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                LoadMenu();
                object addonmenuIdObj = ds.Tables[0].Rows[0]["AddonMenuId"];
                if (addonmenuIdObj != null && addonmenuIdObj != DBNull.Value)
                {
                    string menuId = addonmenuIdObj.ToString();
                    ddlmenulist.SelectedValue = menuId;
                }

                itemName.Text = ds.Tables[0].Rows[0]["AddonItemName"].ToString();
                description.Text = ds.Tables[0].Rows[0]["ItemDescription"].ToString();
                price.Text = ds.Tables[0].Rows[0]["Price"].ToString();

                object statusObj = ds.Tables[0].Rows[0]["ActiveStatus"];
                if (statusObj != null && statusObj != DBNull.Value)
                {
                    int statusValue = Convert.ToInt32(statusObj);
                    rbStatus.SelectedValue = statusValue.ToString();
                }
                string photoPath = ds.Tables[0].Rows[0]["AddOnItemPhoto"].ToString();
                ViewState["AddOnItemPhoto"] = photoPath;
                if (!string.IsNullOrEmpty(photoPath))
                {
                    imagePreview.Src = ResolveUrl(photoPath);
                    imagePreview.Attributes["style"] = "display:block;width:190px;height:150px;";
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