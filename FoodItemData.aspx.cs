using FeelYourFood_Admin.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeelYourFood
{
    public partial class FoodItemData : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        public readonly Connection _c1 = new Connection();
        public DataTable _dtt = new DataTable();
        public DataSet ds = new DataSet();
        public string str = string.Empty;
        string itemimagepath = string.Empty;
        int flag = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["restid"]!=null)
                {
                    string lookupSql = $"SELECT ConfigId FROM KioskConfiguration WHERE RestId ='" + Session["restid"].ToString() + "'";
                    ds.Clear();
                    _c1.Retrive2(lookupSql, ref ds);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int configId = Convert.ToInt32(ds.Tables[0].Rows[0]["ConfigId"]);
                        ViewState["configId"] = configId;
                        Loadmenu();

                    }
                    ShowItems();

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
            try
            {
                string menuid = ddlmenulist.SelectedValue;
                string configId = ViewState["configId"]?.ToString() ?? "0";
                string restid = Session["restid"]?.ToString();

                // Get all CategoryIds for the restid
                string lookupSql = $"SELECT CategoryId FROM Kiosk_Category WHERE RestId = '{restid}'";
                ds.Clear();
                _c1.Retrive2(lookupSql, ref ds);

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    ddlitemlist.Items.Clear();
                    ddlitemlist.Items.Add(new ListItem("-- No categories found --", ""));
                    return;
                }

                // Join all category ids into a comma-separated string
                var categoryIds = ds.Tables[0].AsEnumerable()
                                    .Select(row => row["CategoryId"].ToString())
                                    .ToList();
                string categoryIdCsv = string.Join(",", categoryIds);

                // Build the query
                StringBuilder query = new StringBuilder($@"
            SELECT ItemName, ItemId 
            FROM ItemMaster 
            WHERE MenuId = '{menuid}' 
              AND CategoryId IN ({categoryIdCsv})");

                if (flag != 1)
                {
                    query.Append($@"
              AND ItemId NOT IN (
                  SELECT ItemId 
                  FROM Kiosk_ItemFood 
                  WHERE ConfigId = {configId} AND RestId = {restid}
              )");
                }

                ds.Clear();
                _c1.Retrive2(query.ToString(), ref ds);

                ddlitemlist.Items.Clear();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlitemlist.DataSource = ds.Tables[0];
                    ddlitemlist.DataTextField = "ItemName";
                    ddlitemlist.DataValueField = "ItemId";
                    ddlitemlist.DataBind();
                    ddlitemlist.Items.Insert(0, new ListItem(" ---- Select Item ---- ", ""));
                }
                else
                {
                    ddlitemlist.Items.Add(new ListItem("-- No item Available --", ""));
                }
            }
            catch (Exception ex)
            {
                ddlitemlist.Items.Clear();
                ddlitemlist.Items.Add(new ListItem("-- Error Loading items --", ""));
            }
        }

        private void Loadmenu()
        {
            try
            {
                string configId = ViewState["configId"].ToString();

                string query = $"SELECT FoodMenuName, FoodMenuId FROM Kiosk_MenuDetails WHERE ConfigId = '{configId}' and RestId={Session["restid"].ToString()} and ActiveStatus=1";

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
        protected void ddlitemlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string itemid = ddlitemlist.SelectedValue;

            string query = $"SELECT * FROM ItemMaster WHERE ItemId = '{itemid}'";

            ds.Clear();
            _c1.Retrive2(query, ref ds);
            ItemDescription.Text = ds.Tables[0].Rows[0]["ItemDescription"].ToString();
            Price.Text = ds.Tables[0].Rows[0]["Price"].ToString();
            rbStatus.SelectedValue = ds.Tables[0].Rows[0]["ActiveStatus"].ToString();
            itemimagepath = ds.Tables[0].Rows[0]["ItemPhoto"].ToString();
            ViewState["imgpath"] = itemimagepath;
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ItemPhoto"].ToString()))
            {
                imagePreview.Src = ResolveUrl(ds.Tables[0].Rows[0]["ItemPhoto"].ToString());

                imagePreview.Visible = true;
                imagePreview.Attributes["style"] = "display:block;width:190px;height:150px;";
            }
        }

        protected void ddlmenulist_SelectedIndexChanged(object sender, EventArgs e)
        {

            LoadItem();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Price.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Price is required.');", true);
                return;
            }
            double pricetxt = Convert.ToDouble(Price.Text.Trim());
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
            if (String.IsNullOrEmpty(Price.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Price is required.');", true);
                return;
            }
            int ddlmenuid = Convert.ToInt32(ddlmenulist.SelectedValue);
            int ddlitemid = Convert.ToInt32(ddlitemlist.SelectedValue);
            int activeStatus = Convert.ToInt32(rbStatus.SelectedValue);
            string tempdes = ItemDescription.Text.Trim();
            if (btnSubmit.Text == "Submit")
            {
                if (myFile.HasFile)
                {
                    string virtualFolder = "IMAGE/customItemImage/";
                    string physicalFolder = Server.MapPath(virtualFolder);
                    string fileName = Session["adminname"] + "_" + Path.GetFileName(myFile.FileName);
                    string savedPath = Path.Combine(physicalFolder, fileName);
                    myFile.SaveAs(savedPath);

                    string itemimagepath = "~/" + virtualFolder + fileName;
                    ViewState["imgpath"] = itemimagepath;

                }
                SqlParameter[] insertParams = new SqlParameter[12];
                insertParams[0] = new SqlParameter("@sp_type", 1);
                insertParams[1] = new SqlParameter("@ConfigId", ViewState["configId"].ToString());
                insertParams[2] = new SqlParameter("@RestId", Session["restid"].ToString());
                insertParams[3] = new SqlParameter("@MenuId", ddlmenuid);
                insertParams[4] = new SqlParameter("@ItemId", ddlitemid);
                insertParams[5] = new SqlParameter("@ItemName", ddlitemlist.SelectedItem.ToString());
                insertParams[6] = new SqlParameter("@description", tempdes);
                insertParams[7] = new SqlParameter("@Price", pricetxt);
                insertParams[8] = new SqlParameter("@ItemPhoto", ViewState["imgpath"].ToString());
                insertParams[9] = new SqlParameter("@ActiveStatus", activeStatus);
                insertParams[10] = new SqlParameter("@CreatedBy", Session["adminname"]);
                insertParams[11] = new SqlParameter("@CreatedDate", DateTime.Now);

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_KioskItemMaster", insertParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{ddlitemlist.SelectedItem.ToString()} item added successfully');", true);
                    ShowItems();
                    Clear();
                }
            }
            else
            {
                if (myFile.HasFile)
                {
                    string virtualFolder = "IMAGE/customItemImage/";
                    string physicalFolder = Server.MapPath(virtualFolder);
                    string fileName = Session["adminname"] + "_" + Path.GetFileName(myFile.FileName);
                    string savedPath = Path.Combine(physicalFolder, fileName);
                    myFile.SaveAs(savedPath);
                    string itemimagepath = "~/" + virtualFolder + fileName;
                    ViewState["imgpath"] = itemimagepath;
                }

                SqlParameter[] insertParams = new SqlParameter[12];
                insertParams[0] = new SqlParameter("@sp_type", 2);
                insertParams[1] = new SqlParameter("@ConfigId", ViewState["configId"].ToString());
                insertParams[2] = new SqlParameter("@RestId", Session["restid"].ToString());
                insertParams[3] = new SqlParameter("@MenuId", ddlmenuid);
                insertParams[4] = new SqlParameter("@ItemId", ddlitemid);
                insertParams[5] = new SqlParameter("@ItemName", ddlitemlist.SelectedItem.ToString());
                insertParams[6] = new SqlParameter("@description", tempdes);
                insertParams[7] = new SqlParameter("@Price", pricetxt);
                insertParams[8] = new SqlParameter("@ItemPhoto", ViewState["imgpath"].ToString());
                insertParams[9] = new SqlParameter("@ActiveStatus", activeStatus);
                insertParams[10] = new SqlParameter("@CreatedDate", DateTime.Now);
                insertParams[11] = new SqlParameter("@Id", ViewState["ID"].ToString());
                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_KioskItemMaster", insertParams);
                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{ddlitemlist.SelectedItem.ToString()} item added successfully');", true);
                    ShowItems();
                    Clear();
                }
            }
        }

        private void ShowItems()
        {
            string configId = ViewState["configId"] != null && !string.IsNullOrWhiteSpace(ViewState["configId"].ToString())
                  ? ViewState["configId"].ToString()
                  : "0";
            string str1 = "select * FROM Kiosk_ItemFood md where ConfigId='" + configId + "' order by Id DESC";
            _c1.Retrive2(str1, ref ds);
            gvItem.DataSource = ds;
            gvItem.DataBind();
        }

        private void Clear()
        {
            ddlmenulist.SelectedIndex = 0;

            ddlitemlist.Items.Clear();
            ddlitemlist.Items.Insert(0, new ListItem("----Select Item----", ""));
            Loadmenu();
            LoadItem();
            ItemDescription.Text = string.Empty;
            Price.Text = string.Empty;
            rbStatus.SelectedIndex = 0;

            imagePreview.Src = "#";
            imagePreview.Attributes["style"] = "display:none;width:190px;height:150px;";
            btnSubmit.Text = "Submit";
            ShowItems();
            ViewState["uniqueId"] = null;
            ViewState["ID"] = null;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void gvItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvItem.PageIndex = e.NewPageIndex;
            ShowItems();
        }
        protected void ImageBtnEdit_Command(object sender, CommandEventArgs e)
        {
            flag = 1;
            int ItemId = Convert.ToInt32(e.CommandArgument);
            ViewState["ID"] = ItemId;
            string query = @"
                            SELECT * FROM Kiosk_ItemFood WHERE Id = @Id;";
            SqlParameter[] parameters = { new SqlParameter("@Id", ItemId) };
            DataSet ds = SqlHelper.ExecuteDataset(_c1.con, CommandType.Text, query, parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {

                DataRow ItemRow = ds.Tables[0].Rows[0];

                string foodMenuId = ItemRow["MenuId"].ToString();
                if (ddlmenulist.Items.FindByValue(foodMenuId) != null)
                {
                    ddlmenulist.SelectedValue = foodMenuId;
                }
                LoadItem();
                string itemId = ItemRow["ItemId"].ToString();
                if (ddlitemlist.Items.FindByValue(itemId) != null)
                {
                    ddlitemlist.SelectedValue = itemId;

                }
                ItemDescription.Text = ItemRow["description"].ToString();
                Price.Text = ItemRow["Price"].ToString();

                rbStatus.SelectedValue = Convert.ToBoolean(ItemRow["ActiveStatus"]) ? "1" : "0";

                // Store image path to show preview
                if (!string.IsNullOrEmpty(ItemRow["itemPhoto"].ToString()))
                {
                    imagePreview.Src = ResolveUrl(ItemRow["itemPhoto"].ToString());
                    ViewState["imgpath"] = ItemRow["itemPhoto"].ToString();
                    imagePreview.Visible = true;
                    imagePreview.Attributes["style"] = "display:block;width:190px;height:150px;";
                }
                btnSubmit.Text = "Update";
            }
        }

        protected void ImageBtnDelete_Command(object sender, CommandEventArgs e)
        {
            int foodItemId = Convert.ToInt32(e.CommandArgument);

            string query = "update Kiosk_ItemFood set ActiveStatus =0 where Id='" + foodItemId + "'";
            _c1.Execute2(query, ref ds);
            ShowItems();

        }
    }
}