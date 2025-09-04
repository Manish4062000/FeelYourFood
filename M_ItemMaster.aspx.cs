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
    public partial class M_ItemMaster : System.Web.UI.Page
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
                    loadCuisine();
                    loadCategory();
                    showItem();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expire, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");
                }
            }
        }

        private void loadCategory()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            {
                string query = "SELECT TypeId, CategoryName FROM FoodCategoryMaster WHERE ActiveStatus = 1 ORDER BY TypeId ASC";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                category.DataSource = dt;
                category.DataTextField = "CategoryName";
                category.DataValueField = "TypeId";
                category.DataBind();
            }

        }

        private void loadCuisine()
        {
            string str1 = "SELECT CuisineName, CuisineId FROM CuisineMaster WHERE ActiveStatus = 1 ORDER BY CuisineId ASC";
            _c1.FillDropDown(str1, ddlcuisine);
            ddlcuisine.Items.Insert(0, "----- Select Consume Type -----");

        }

        protected void ddlcuisine_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadMenu();
        }

        private void loadMenu()
        {
            string str1 = "SELECT  m.FoodMenuName,m.FoodMenuId FROM MenuMaster m INNER JOIN MenuCuisine c ON c.FoodMenuId = m.FoodMenuId WHERE m.ActiveStatus = '1'  AND c.CuisineId ='" + ddlcuisine.SelectedValue + "' ORDER BY m.FoodMenuId ASC;";
            _c1.FillDropDown(str1, ddlmenulist);
            ddlmenulist.Items.Insert(0, "----- Select Menu Name -----");
        }

        protected void gvItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvItem.PageIndex = e.NewPageIndex;
            showItem();
        }

        private void showItem()
        {
            string query = " select im.*,cm.CuisineName,mm.FoodMenuName,fcm.CategoryName from ItemMaster im inner join CuisineMaster cm on im.CuisineId=cm.CuisineId inner join MenuMaster mm on im.MenuId=mm.FoodMenuId  inner join FoodCategoryMaster fcm on im.categoryId=fcm.TypeId order by im.ItemId desc";
            _c1.Retrive2(query, ref ds);
            gvItem.DataSource = ds;
            gvItem.DataBind();

        }

        protected void ImageBtnEdit_Command(object sender, CommandEventArgs e)
        {
            int ItemId = Convert.ToInt32(e.CommandArgument);
            ViewState["uniqueId"] = ItemId;
            string query = " select im.* from ItemMaster im where im.ItemId='" + ItemId + "' ";
            _c1.Retrive2(query, ref ds);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                object cuisineIdObj = ds.Tables[0].Rows[0]["CuisineId"];
                if (cuisineIdObj != null && cuisineIdObj != DBNull.Value)
                {
                    string cuisineId = cuisineIdObj.ToString();
                    ddlcuisine.SelectedValue = cuisineId;
                }
                loadMenu();
                object MenuIdObj = ds.Tables[0].Rows[0]["MenuId"];
                if (MenuIdObj != null && MenuIdObj != DBNull.Value)
                {
                    string menuId = MenuIdObj.ToString();
                    ddlmenulist.SelectedValue = menuId;
                }
                object categoryIdObj = ds.Tables[0].Rows[0]["categoryId"];
                if (categoryIdObj != null && categoryIdObj != DBNull.Value)
                {
                    string categoryId = categoryIdObj.ToString();
                    category.SelectedValue = categoryId;
                }
                ItemName.Text = ds.Tables[0].Rows[0]["ItemName"].ToString();
                itemdescription.Text = ds.Tables[0].Rows[0]["ItemDescription"].ToString();
                price.Text = ds.Tables[0].Rows[0]["Price"].ToString();
                object addonObj = ds.Tables[0].Rows[0]["AddOnAvailable"];
                if (addonObj != null && addonObj != DBNull.Value)
                {
                    int statusValue = Convert.ToInt32(addonObj);
                    addon.SelectedValue = statusValue.ToString();
                }
                object statusObj = ds.Tables[0].Rows[0]["ActiveStatus"];
                if (statusObj != null && statusObj != DBNull.Value)
                {
                    int statusValue = Convert.ToInt32(statusObj);
                    rbStatus.SelectedValue = statusValue.ToString();
                }
                string photoPath = ds.Tables[0].Rows[0]["ItemPhoto"].ToString();
                ViewState["ItemPhoto"] = photoPath;
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

        protected void ImageBtnDelete_Command(object sender, CommandEventArgs e)
        {
            int ItemId = Convert.ToInt32(e.CommandArgument);

            SqlParameter[] deleteParams = new SqlParameter[]
            {
                new SqlParameter("@sp_type", 3),
                new SqlParameter("@ItemId", ItemId)
            };

            int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_ItemMaster", deleteParams);

            if (result != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('item deleted successfully');", true);
                showItem();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Failed to delete item');", true);
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string Itemname = ItemName.Text.Trim();
            int status = Convert.ToInt32(rbStatus.SelectedValue);
            if (ddlcuisine.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select a valid cuisine name.');", true);
                return;
            }
            if (ddlmenulist.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select a valid Menu name.');", true);
                return;
            }
            if (string.IsNullOrEmpty(Itemname))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Item name is required.');", true);
                return;
            }
            string description = itemdescription.Text.Trim();
            int addonavailable = Convert.ToInt32(addon.SelectedValue);
            double pricetxt = Convert.ToDouble(price.Text.Trim());
            if (btnSubmit.Text == "Submit")
            {
                SqlParameter[] checkParams = new SqlParameter[2];
                checkParams[0] = new SqlParameter("@sp_type", 5);
                checkParams[1] = new SqlParameter("@ItemName", Itemname);

                object count = SqlHelper.ExecuteScalar(_c1.con, CommandType.StoredProcedure, "sp_ItemMaster", checkParams);
                int exists = Convert.ToInt32(count);

                if (exists > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{Itemname} item already exists!');", true);
                    return;
                }
                if (myFile.HasFile)
                {
                    string virtualFolder = "IMAGE/ItemImages/";
                    string physicalFolder = Server.MapPath(virtualFolder);
                    string fileName = Session["adminname"] + "_" + myFile.FileName;
                    myFile.SaveAs(Path.Combine(physicalFolder, fileName));
                    ItemImagePath = "~/IMAGE/ItemImages/" + fileName;

                    SqlParameter[] insertParams = new SqlParameter[]
                     {
                        new SqlParameter("@sp_type", 1),
                        new SqlParameter("@CuisineId", ddlcuisine.SelectedValue),
                        new SqlParameter("@MenuId", ddlmenulist.SelectedValue),
                        new SqlParameter("@categoryId", category.SelectedValue),
                        new SqlParameter("@ItemName", Itemname),
                        new SqlParameter("@ItemDescription",description ),
                        new SqlParameter("@Price", pricetxt),
                        new SqlParameter("@ItemPhoto", ItemImagePath),
                        new SqlParameter("@AddOnAvailable", addonavailable),
                        new SqlParameter("@ActiveStatus", status),
                        new SqlParameter("@CreatedBy", Session["adminname"] != null ? Session["adminname"].ToString() : "Addsoft")
 //
                    };

                    int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_ItemMaster", insertParams);

                    if (result != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Item saved successfully');", true);
                        showItem();
                        Clear();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error saving Item');", true);
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
                    string virtualFolder = "IMAGE/ItemImages/";
                    string physicalFolder = Server.MapPath(virtualFolder);
                    string fileName = Session["adminname"] + "_" + myFile.FileName;
                    myFile.SaveAs(Path.Combine(physicalFolder, fileName));
                    ItemImagePath = "~/IMAGE/ItemImages/" + fileName;

                    SqlParameter[] updateParams = new SqlParameter[]
                    {
                        new SqlParameter("@sp_type", 2),
                        new SqlParameter("@CuisineId", ddlcuisine.SelectedValue),
                        new SqlParameter("@MenuId", ddlmenulist.SelectedValue),
                        new SqlParameter("@categoryId", category.SelectedValue),
                        new SqlParameter("@ItemName", Itemname),
                        new SqlParameter("@ItemDescription",description ),
                        new SqlParameter("@Price", pricetxt),
                        new SqlParameter("@ItemPhoto", ItemImagePath),
                        new SqlParameter("@AddOnAvailable", addonavailable),
                        new SqlParameter("@ActiveStatus", status),
                        new SqlParameter("@UpdatedDate", DateTime.Now),
                        new SqlParameter("@ItemId", ItemId),
                    };

                    int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_ItemMaster", updateParams);

                    if (result != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Item updated successfully');", true);
                        btnSubmit.Text = "Submit";
                        showItem();
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
                        new SqlParameter("@CuisineId", ddlcuisine.SelectedValue),
                        new SqlParameter("@MenuId", ddlmenulist.SelectedValue),
                        new SqlParameter("@categoryId", category.SelectedValue),
                        new SqlParameter("@ItemName", Itemname),
                        new SqlParameter("@ItemDescription",description ),
                        new SqlParameter("@Price", pricetxt),
                        new SqlParameter("@AddOnAvailable", addonavailable),
                        new SqlParameter("@ActiveStatus", status),
                        new SqlParameter("@UpdatedDate", DateTime.Now),
                        new SqlParameter("@ItemId", ItemId),
                    };

                    int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_ItemMaster", updateParams);

                    if (result != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Item updated successfully');", true);
                        btnSubmit.Text = "Submit";
                        showItem();
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
            ItemName.Text = string.Empty;
            rbStatus.SelectedIndex = 0;
            addon.SelectedIndex = 0;
            price.Text = "0.00";
            ddlcuisine.SelectedIndex = 0;
            ddlmenulist.SelectedIndex = 0;
            btnSubmit.Text = "Submit";
            category.SelectedIndex = 0;
            itemdescription.Text = string.Empty;
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}