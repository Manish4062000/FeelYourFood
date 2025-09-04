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
    public partial class M_MenuMaster : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        public readonly Connection _c1 = new Connection();
        public DataTable _dtt = new DataTable();
        public DataSet ds = new DataSet();
        private string MenuImagePath = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["restid"] != null)
                {
                    showMenu();
                    loadConsumeType();
                    loadCuisine();
                    loadCategory();
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
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(str1, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cuisine.DataSource = dt;
                cuisine.DataTextField = "CuisineName";
                cuisine.DataValueField = "CuisineId";
                cuisine.DataBind();
            }
        }


        private void loadConsumeType()
        {
            string query = "SELECT ConsumeTypeId, Type FROM ConsumeTypeMaster WHERE ActiveStatus = 1 ORDER BY Type ASC";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                consumetype.DataSource = dt;
                consumetype.DataTextField = "Type";
                consumetype.DataValueField = "ConsumeTypeId";
                consumetype.DataBind();
            }
        }

        public void showMenu()
        {
            string query = @"
        SELECT 
            m.FoodMenuId,

            m.FoodMenuName,
            cat.CategoryName,
            m.MenuPhoto,
            m.ActiveStatus,
m.UpdatedDate,

            STUFF((
                SELECT ', ' + cu.CuisineName
                FROM MenuCuisine mc
                INNER JOIN CuisineMaster cu ON cu.CuisineId = mc.CuisineId
                WHERE mc.FoodMenuId = m.FoodMenuId
                FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS CuisineNames,

            STUFF((
                SELECT ', ' + ct.[Type]
                FROM MenuConsumeType mct
                INNER JOIN ConsumeTypeMaster ct ON ct.ConsumeTypeId = mct.ConsumeTypeId
                WHERE mct.FoodMenuId = m.FoodMenuId
                FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS ConsumeTypes

        FROM MenuMaster m
        LEFT JOIN FoodCategoryMaster cat ON cat.TypeId = m.CategoryId
        ORDER BY m.FoodMenuId DESC";

            DataSet ds = new DataSet();
            _c1.Retrive2(query, ref ds);
            gvMenu.DataSource = ds;
            gvMenu.DataBind();
        }



        protected void gvMenu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMenu.PageIndex = e.NewPageIndex;
            showMenu();
        }
        protected void ImageBtnEdit_Command(object sender, CommandEventArgs e)
        {
            int menuId = Convert.ToInt32(e.CommandArgument);
            ViewState["uniqueId"] = menuId;

            string query = @"
    SELECT * FROM MenuMaster WHERE FoodMenuId = @FoodMenuId;
    
    SELECT CuisineId FROM MenuCuisine WHERE FoodMenuId = @FoodMenuId;
    SELECT ConsumeTypeId FROM MenuConsumeType WHERE FoodMenuId = @FoodMenuId;";

            SqlParameter[] parameters = {
        new SqlParameter("@FoodMenuId", menuId)
    };

            DataSet ds = SqlHelper.ExecuteDataset(_c1.con, CommandType.Text, query, parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow menuRow = ds.Tables[0].Rows[0];

                menuname.Text = menuRow["FoodMenuName"].ToString();
                category.SelectedValue = menuRow["CategoryId"].ToString();
                rbStatus.SelectedValue = Convert.ToBoolean(menuRow["ActiveStatus"]) ? "1" : "0";

                // Store image path to show preview
                if (!string.IsNullOrEmpty(menuRow["MenuPhoto"].ToString()))
                {
                    imagePreview.Src = ResolveUrl(menuRow["MenuPhoto"].ToString());
                    imagePreview.Visible = true;
                }

                // Set selected cuisines
                foreach (ListItem item in cuisine.Items)
                {
                    item.Selected = ds.Tables[1].AsEnumerable()
                        .Any(r => r["CuisineId"].ToString() == item.Value);
                }

                // Set selected consume types
                foreach (ListItem item in consumetype.Items)
                {
                    item.Selected = ds.Tables[2].AsEnumerable()
                        .Any(r => r["ConsumeTypeId"].ToString() == item.Value);
                }

                btnSubmit.Text = "Update";
            }
        }

        protected void ImageBtnDelete_Command(object sender, CommandEventArgs e)
        {
            int foodMenuId = Convert.ToInt32(e.CommandArgument);

            SqlParameter[] deleteParams = new SqlParameter[]
            {
                new SqlParameter("@sp_type", 3),  // Assuming 3 is the delete operation in your sp
                new SqlParameter("@FoodMenuId", foodMenuId)
            };

            int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_MenuMaster", deleteParams);

            if (result != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Menu deleted successfully');", true);
                showMenu(); // Refresh the data
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Failed to delete menu');", true);
            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string menuName = menuname.Text.Trim();
            int status = Convert.ToInt32(rbStatus.SelectedValue);

            string categoryId = category.SelectedValue;

            List<string> selectedCuisines = cuisine.Items.Cast<ListItem>()
                .Where(i => i.Selected)
                .Select(i => i.Value)
                .ToList();

            List<string> selectedConsumeTypes = consumetype.Items.Cast<ListItem>()
                .Where(i => i.Selected)
                .Select(i => i.Value)
                .ToList();

            if (selectedConsumeTypes.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select at least one consume type.');", true);
                return;
            }
            if (selectedCuisines.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select at least one cuisine.');", true);
                return;
            }
            if (string.IsNullOrEmpty(categoryId))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select a category.');", true);
                return;
            }
            if (string.IsNullOrEmpty(menuName))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Menu name is required.');", true);
                return;
            }


            if (btnSubmit.Text == "Submit")
            {
                SqlParameter[] checkParams = new SqlParameter[2];
                checkParams[0] = new SqlParameter("@sp_type", 5);
                checkParams[1] = new SqlParameter("@FoodMenuName", menuName);

                object count = SqlHelper.ExecuteScalar(_c1.con, CommandType.StoredProcedure, "sp_MenuMaster", checkParams);
                int exists = Convert.ToInt32(count);

                if (exists > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{menuName} menu already exists!');", true);
                    return;
                }
                if (myFile.HasFile)
                {
                    string virtualFolder = "IMAGE/MenuImages/";
                    string physicalFolder = Server.MapPath(virtualFolder);
                    string fileName = Session["adminname"] + "_" + myFile.FileName;
                    myFile.SaveAs(Path.Combine(physicalFolder, fileName));
                    MenuImagePath = "~/IMAGE/MenuImages/" + fileName;

                    SqlParameter[] insertParams = new SqlParameter[]
                     {
                        new SqlParameter("@sp_type", 1),
                        new SqlParameter("@FoodMenuName", menuName),
                        new SqlParameter("@MenuPhoto", MenuImagePath),
                        new SqlParameter("@ActiveStatus", status),
                        new SqlParameter("@CategoryId", categoryId),
                        new SqlParameter("@CreatedBy","manish" ),  //Session["adminname"]
                        new SqlParameter("@CuisineIds", string.Join(",", selectedCuisines)),
                        new SqlParameter("@ConsumeTypeIds", string.Join(",", selectedConsumeTypes))
                    };

                    int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_MenuMaster", insertParams);

                    if (result != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Menu saved successfully');", true);
                        showMenu();
                        Clear();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error saving menu');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Upload image first !');", true);
                    return;
                }
            }
            else
            {

                string MenuId = ViewState["uniqueId"]?.ToString();
                if (myFile.HasFile)
                {
                    string virtualFolder = "IMAGE/MenuImages/";
                    string physicalFolder = Server.MapPath(virtualFolder);
                    string fileName = Session["adminname"] + "_" + myFile.FileName;
                    myFile.SaveAs(Path.Combine(physicalFolder, fileName));
                    MenuImagePath = "~/IMAGE/MenuImages/" + fileName;

                    SqlParameter[] updateParams = new SqlParameter[]
                    {
                        new SqlParameter("@sp_type", 2),
                        new SqlParameter("@FoodMenuName", menuName),
                        new SqlParameter("@MenuPhoto", MenuImagePath),
                        new SqlParameter("@ActiveStatus", status),
                        new SqlParameter("@CategoryId", categoryId),
                        new SqlParameter("@UpdatedDate", DateTime.Now),
                        new SqlParameter("@CuisineIds", string.Join(",", selectedCuisines)),
                        new SqlParameter("@ConsumeTypeIds", string.Join(",", selectedConsumeTypes)),
                        new SqlParameter("@FoodMenuId", MenuId),
                    };

                    int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_MenuMaster", updateParams);

                    if (result != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Menu updated successfully');", true);
                        btnSubmit.Text = "Submit";
                        showMenu();
                        Clear();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error updating menu');", true);
                    }
                }
                else
                {
                    SqlParameter[] updateParams = new SqlParameter[]
                    {
                        new SqlParameter("@sp_type", 4),
                        new SqlParameter("@FoodMenuName", menuName),
                        new SqlParameter("@ActiveStatus", status),
                        new SqlParameter("@CategoryId", categoryId),
                        new SqlParameter("@UpdatedDate", DateTime.Now),
                        new SqlParameter("@CuisineIds", string.Join(",", selectedCuisines)),
                        new SqlParameter("@ConsumeTypeIds", string.Join(",", selectedConsumeTypes)),
                        new SqlParameter("@FoodMenuId", MenuId),
                    };

                    int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_MenuMaster", updateParams);

                    if (result != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Menu updated successfully');", true);
                        btnSubmit.Text = "Submit";
                        showMenu();
                        Clear();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error updating menu');", true);
                    }
                }
            }
        }

        void Clear()
        {
            imagePreview.Src = "#";
            imagePreview.Attributes["style"] = "display:none;width:190px;height:150px;";
            menuname.Text = string.Empty;
            rbStatus.SelectedIndex = 0;
            category.ClearSelection();
            foreach (ListItem item in cuisine.Items)
                item.Selected = false;

            foreach (ListItem item in consumetype.Items)
                item.Selected = false;
            btnSubmit.Text = "Submit";

        }


        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}