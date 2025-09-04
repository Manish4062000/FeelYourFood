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
    public partial class AddonSelectionForm : System.Web.UI.Page
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

                    Loadmenu();
                    ShowItems();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expire, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");
                }
            }
        }

        private void Loadmenu()
        {
            try
            {
                

                string query = $"SELECT FoodMenuName, FoodMenuId FROM Kiosk_MenuDetails WHERE  RestId={Session["restid"].ToString()} and ActiveStatus=1 ";

                ds.Clear();
                _c1.Retrive2(query, ref ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlmenulist.Items.Clear();
                    ddlmenulist.DataSource = ds.Tables[0];
                    ddlmenulist.DataTextField = "FoodMenuName";
                    ddlmenulist.DataValueField = "FoodMenuId";
                    ddlmenulist.DataBind();
                    ddlmenulist.Items.Insert(0, new ListItem("----Select Menu----", ""));
                    ddlmenulist.SelectedIndex = 0;
                }
                else
                {
                    ddlmenulist.Items.Clear();
                    ddlmenulist.Items.Add(new ListItem("-- No Menus Available --", ""));
                }
            }
            catch (Exception ex)
            {
                ddlmenulist.Items.Clear();
                ddlmenulist.Items.Add(new ListItem("-- Error Loading Menu --", ""));
            }
        }
        protected void ddlmenulist_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadItem();
        }
        private void ShowItems()
        {
            string lookupSql = $"SELECT * FROM AddonAssignToRestaurant WHERE RestId ='" + Session["restid"].ToString() + "' order by id desc ";
            ds.Clear();
            _c1.Retrive2(lookupSql, ref ds);
            gvAddon.DataSource = ds;
            gvAddon.DataBind();
        }

        private void LoadItem()
        {
            try
            {
                string query = $"SELECT ItemName, ItemId FROM Kiosk_ItemFood WHERE  RestId={Session["restid"].ToString()} and ActiveStatus=1 and menuid='" + ddlmenulist.SelectedValue + "'";

                ds.Clear();
                _c1.Retrive2(query, ref ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlitemlist.Items.Clear();
                    ddlitemlist.DataSource = ds.Tables[0];
                    ddlitemlist.DataTextField = "ItemName";
                    ddlitemlist.DataValueField = "ItemId";
                    ddlitemlist.DataBind();
                    ddlitemlist.Items.Insert(0, new ListItem("----Select Food Item----", ""));
                    ddlitemlist.SelectedIndex = 0;
                }
                else
                {
                    ddlitemlist.Items.Clear();
                    ddlitemlist.Items.Add(new ListItem("-- No Food Item Available --", ""));
                }
            }
            catch (Exception ex)
            {
                ddlitemlist.Items.Clear();
                ddlitemlist.Items.Add(new ListItem("-- Error Loading Food Item --", ""));
            }
        }

        protected void ddlitemlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAddonMenu();
        }

        private void LoadAddonMenu()
        {
            try
            {
                string query = $"SELECT am.AddonMenuId, am.AddonMenuName FROM AddonMenuMaster am INNER JOIN AddonAssignMaster aam ON am.AddonMenuId = aam.AddOnMenuId INNER JOIN Kiosk_ItemFood kif ON aam.Id = kif.ItemId WHERE kif.RestId = {Session["restid"].ToString()} AND kif.ItemId = '" + ddlitemlist.SelectedValue + "' AND am.ActiveStatus = 1;";

                ds.Clear();
                _c1.Retrive2(query, ref ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddladdonmenu.Items.Clear();
                    ddladdonmenu.DataSource = ds.Tables[0];
                    ddladdonmenu.DataTextField = "AddonMenuName";
                    ddladdonmenu.DataValueField = "AddonMenuId";
                    ddladdonmenu.DataBind();
                    ddladdonmenu.Items.Insert(0, new ListItem("----Select Addon Menu----", ""));
                    ddladdonmenu.SelectedIndex = 0;
                }
                else
                {
                    ddladdonmenu.Items.Clear();
                    ddladdonmenu.Items.Add(new ListItem("-- No Addon Menu Available --", ""));
                }
            }
            catch (Exception ex)
            {
                ddladdonmenu.Items.Clear();
                ddladdonmenu.Items.Add(new ListItem("-- Error Loading Addon Menu --", ""));
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int restid = Convert.ToInt32(Session["restid"]);

            string addItem = addonname.Text.Trim();
            double mrp = Convert.ToDouble(Price.Text.Trim());
            if (String.IsNullOrEmpty(addItem))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter the addon item name.');", true);
                return;
            }
            if (String.IsNullOrEmpty(Price.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter the addon item Price.');", true);
                return;
            }
            
            string description = addondiscription.Text.Trim();
            int activeStatus = Convert.ToInt32(rbStatus.SelectedValue);
            int itemid = Convert.ToInt32(ddlitemlist.SelectedValue);
            int AMenuid = Convert.ToInt32(ddlitemlist.SelectedValue);
            string ddladdonmenutxt = ddladdonmenu.SelectedItem.ToString();
            if (btnSubmit.Text == "Submit")
            {
                
                if (ddlmenulist.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select a valid menu first.');", true);
                    return;
                }
                if (ddlitemlist.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select a valid item first.');", true);
                    return;
                }
                if (ddladdonmenu.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select a valid addon menu first.');", true);
                    return;
                }
                if (myFile.HasFile)
                {
                    string virtualFolder = "IMAGE/CustomAddonItemImage/";
                    string physicalFolder = Server.MapPath(virtualFolder);
                    string fileName = Session["adminname"] + "_" + Path.GetFileName(myFile.FileName);
                    string savedPath = Path.Combine(physicalFolder, fileName);
                    myFile.SaveAs(savedPath);

                    string itemimagepath = "~/" + virtualFolder + fileName;
                    ViewState["imgpath"] = itemimagepath;

                    SqlParameter[] insertParams = new SqlParameter[10];
                    insertParams[0] = new SqlParameter("@sp_type", 1);
                    insertParams[1] = new SqlParameter("@ItemId", itemid);
                    insertParams[2] = new SqlParameter("@RestId", Session["restid"].ToString());
                    insertParams[3] = new SqlParameter("@AddonMenuId", AMenuid);
                    insertParams[4] = new SqlParameter("@AddonMenuName", ddladdonmenutxt);
                    insertParams[5] = new SqlParameter("@AddonItemName", addItem);
                    insertParams[6] = new SqlParameter("@Description", description);
                    insertParams[7] = new SqlParameter("@price", mrp);
                    insertParams[8] = new SqlParameter("@Photo", ViewState["imgpath"].ToString());
                    insertParams[9] = new SqlParameter("@ActiveStatus", activeStatus);

                    int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_KioskAddonItem", insertParams);

                    if (result != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{addItem} item added successfully');", true);
                        ShowItems();
                        Clear();
                    }
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Upload an image for addon item first.');", true);
                    return;

                }

            }
            else
            {
                int id = Convert.ToInt32(ViewState["uniqueId"].ToString());
                if (myFile.HasFile)
                {
                    string virtualFolder = "IMAGE/CustomAddonItemImage/";
                    string physicalFolder = Server.MapPath(virtualFolder);
                    string fileName = Session["adminname"] + "_" + Path.GetFileName(myFile.FileName);
                    string savedPath = Path.Combine(physicalFolder, fileName);
                    myFile.SaveAs(savedPath);
                    string itemimagepath = "~/" + virtualFolder + fileName;
                    ViewState["imgpath"] = itemimagepath;
                }

                SqlParameter[] insertParams = new SqlParameter[11];
                insertParams[0] = new SqlParameter("@sp_type", 2);
                insertParams[1] = new SqlParameter("@ItemId", itemid);
                insertParams[2] = new SqlParameter("@RestId", Session["restid"].ToString());
                insertParams[3] = new SqlParameter("@AddonMenuId", AMenuid);
                insertParams[4] = new SqlParameter("@AddonMenuName", ddladdonmenutxt);
                insertParams[5] = new SqlParameter("@AddonItemName", addItem);
                insertParams[6] = new SqlParameter("@Description", description);
                insertParams[7] = new SqlParameter("@price", mrp);
                insertParams[8] = new SqlParameter("@Photo", ViewState["imgpath"].ToString());
                insertParams[9] = new SqlParameter("@ActiveStatus", activeStatus);
                insertParams[10] = new SqlParameter("@id", id);
                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_KioskAddonItem", insertParams);
                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{addItem} item updated successfully');", true);
                    ShowItems();
                    Clear();
                }
            }

        }
        protected void gvMenu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAddon.PageIndex = e.NewPageIndex;
            ShowItems();

        }
        protected void ImageBtnEdit_Command(object sender, CommandEventArgs e)
        {
            int Id = Convert.ToInt32(e.CommandArgument);
            ViewState["uniqueId"] = Id;

            string query = "SELECT * FROM AddonAssignToRestaurant WHERE id = @id";
            SqlParameter[] parameters = { new SqlParameter("@id", Id) };
            DataSet ds = SqlHelper.ExecuteDataset(_c1.con, CommandType.Text, query, parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow AddonRow = ds.Tables[0].Rows[0];

                addonname.Text = AddonRow["AddonItemName"].ToString();
                addondiscription.Text = AddonRow["Description"].ToString();
                Price.Text = AddonRow["price"].ToString();

                int itemId = Convert.ToInt32(AddonRow["ItemId"]);
                int addonMenuId = Convert.ToInt32(AddonRow["AddonMenuId"]);

                // Load item dropdown based on menu first
                string menuQuery = "SELECT DISTINCT menuid FROM Kiosk_ItemFood WHERE ItemId = @itemId";
                SqlParameter[] menuParams = { new SqlParameter("@itemId", itemId) };
                object menuIdObj = SqlHelper.ExecuteScalar(_c1.con, CommandType.Text, menuQuery, menuParams);

                if (menuIdObj != null && menuIdObj != DBNull.Value)
                {
                    int menuId = Convert.ToInt32(menuIdObj);
                    ddlmenulist.SelectedValue = menuId.ToString();

                    LoadItem(); // now menu is selected, load item list
                    ddlitemlist.SelectedValue = itemId.ToString();

                    LoadAddonMenu(); // item selected, load addon menu
                    ddladdonmenu.SelectedValue = addonMenuId.ToString();
                }

                rbStatus.SelectedValue = Convert.ToBoolean(AddonRow["ActiveStatus"]) ? "1" : "0";

                if (!string.IsNullOrEmpty(AddonRow["Photo"].ToString()))
                {
                    ViewState["imgpath"] = AddonRow["Photo"].ToString();
                    imagePreview.Src = ResolveUrl(AddonRow["Photo"].ToString());
                    imagePreview.Visible = true;
                    imagePreview.Attributes["style"] = "display:block;width:190px;height:150px;";
                }

                ddlmenulist.Enabled = false;
                ddlitemlist.Enabled = false;
                ddladdonmenu.Enabled = false;
                btnSubmit.Text = "Update";
            }
        }

        protected void ImageBtnDelete_Command(object sender, CommandEventArgs e)
        {
            int Id = Convert.ToInt32(e.CommandArgument);

            string query = "update AddonAssignToRestaurant set ActiveStatus =0 where id='" + Id + "'";
            _c1.Execute2(query, ref ds);
            ShowItems();
            Clear();
        }
        private void Clear()
        {
            ddlmenulist.Enabled = true;
            ddlitemlist.Enabled = true;
            ddladdonmenu.Enabled = true;
            ddlmenulist.SelectedIndex = 0;
            ddlitemlist.SelectedIndex = -1;
            ddladdonmenu.SelectedIndex = -1;
            addonname.Text = String.Empty;
            addondiscription.Text = string.Empty;
            Price.Text = string.Empty;
            rbStatus.SelectedIndex = 0;
            btnSubmit.Text = "Submit";
            imagePreview.Src = "#";
            imagePreview.Attributes["style"] = "display:none;width:190px;height:150px;";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}