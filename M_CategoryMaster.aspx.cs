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
    public partial class M_CategoryMaster : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        public readonly Connection _c1 = new Connection();
        public DataTable _dtt = new DataTable();
        public DataSet ds = new DataSet();
        private string CategoryImagePath = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["restid"] != null)
                {
                    showCategory();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expire, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");
                }
            }
        }

        public void showCategory()
        {

            string str1 = "select * FROM FoodCategoryMaster order by TypeId DESC";
            _c1.Retrive2(str1, ref ds);
            gvcred.DataSource = ds;
            gvcred.DataBind();
        }

        protected void gvrestdetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvcred.PageIndex = e.NewPageIndex;
            showCategory();
        }
        protected void ImageBtnEdit_Command(object sender, CommandEventArgs e)
        {
            int UniqIId = Convert.ToInt32(e.CommandArgument);
            ViewState["uniqueId"] = UniqIId;
            string str = "select * FROM FoodCategoryMaster  Where TypeId='" + UniqIId + "'";
            _c1.Retrive2(str, ref ds);

            CategoryName.Text = ds.Tables[0].Rows[0]["CategoryName"].ToString();
            ViewState["CategoryName"] = CategoryName.Text;

            rbStatus.SelectedValue = (Convert.ToInt16(ds.Tables[0].Rows[0]["ActiveStatus"]).ToString());
            ViewState["status"] = rbStatus.SelectedValue;
            string photoPath = ds.Tables[0].Rows[0]["Photo"].ToString(); // e.g., IMAGE/ConsumeType/21022025033828dinein.png
            ViewState["img"] = photoPath;
            if (!string.IsNullOrEmpty(photoPath))
            {
                imagePreview.Src = ResolveUrl(photoPath);
                imagePreview.Attributes["style"] = "display:block;width:190px;height:150px;";
            }
            btnSubmit.Text = "Update";
        }
        protected void ImageBtnDelete_Command(object sender, CommandEventArgs e)
        {
            int UniqIId = Convert.ToInt32(e.CommandArgument);
            string str = "Update FoodCategoryMaster Set ActiveStatus='0' Where TypeId='" + UniqIId + "'";
            _c1.Execute2(str, ref ds);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Food Category updated successfully');", true);
            showCategory();
            return;

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string Categoryname = CategoryName.Text.Trim();
            if (string.IsNullOrWhiteSpace(Categoryname))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Food Category name is required');", true);
                return;
            }
            int status = Convert.ToInt32(rbStatus.SelectedValue);

            if (btnSubmit.Text == "Submit")
            {
                // Check for duplicate
                SqlParameter[] checkParams = new SqlParameter[2];
                checkParams[0] = new SqlParameter("@sp_type", 6);
                checkParams[1] = new SqlParameter("@CategoryName", Categoryname);

                object count = SqlHelper.ExecuteScalar(_c1.con, CommandType.StoredProcedure, "sp_foodCategoryMaster", checkParams);
                int exists = Convert.ToInt32(count);

                if (exists > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{Categoryname} category already exists!');", true);
                    return;
                }

                if (myFile.HasFile)
                {
                    string virtualFolder = "IMAGE/foodCategory/";
                    string physicalFolder = Server.MapPath(virtualFolder);
                    string fileName = Session["adminname"] + "_" + myFile.FileName;
                    myFile.SaveAs(Path.Combine(physicalFolder, fileName));
                    CategoryImagePath = "~/IMAGE/foodCategory/" + fileName;

                    SqlParameter[] insertParams = new SqlParameter[5];
                    insertParams[0] = new SqlParameter("@sp_type", 1);
                    insertParams[1] = new SqlParameter("@CategoryName", Categoryname);
                    insertParams[2] = new SqlParameter("@Photo", CategoryImagePath);
                    insertParams[3] = new SqlParameter("@ActiveStatus", status);
                    insertParams[4] = new SqlParameter("@CreatedBy", Session["adminname"]);

                    int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_foodCategoryMaster", insertParams);

                    if (result != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{Categoryname} category added successfully');", true);
                        showCategory();
                        Clear();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Upload image first!');", true);
                    return;
                }
            }
            else
            {
                string originalName = ViewState["CategoryName"]?.ToString();
                string originalStatus = ViewState["status"]?.ToString();
                string originalimage = ViewState["img"]?.ToString();
                string categoryid = ViewState["uniqueId"]?.ToString();

                bool nameChanged = originalName != Categoryname;
                bool statusChanged = originalStatus != rbStatus.SelectedValue;
                string temp = myFile.HasFile ? "~/IMAGE/foodCategory/" + Session["adminname"] + "_" + Path.GetFileName(myFile.FileName) : null;
                bool imageChanged = originalimage != temp;

                if (!nameChanged && !statusChanged && !imageChanged)
                {
                    Clear();
                    btnSubmit.Text = "Submit";
                    return;
                }

                if (nameChanged)
                {
                    SqlParameter[] checkParams = new SqlParameter[2];
                    checkParams[0] = new SqlParameter("@sp_type", 6);
                    checkParams[1] = new SqlParameter("@CategoryName", Categoryname);

                    object count = SqlHelper.ExecuteScalar(_c1.con, CommandType.StoredProcedure, "sp_foodCategoryMaster", checkParams);
                    int exists = Convert.ToInt32(count);

                    if (exists > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{Categoryname} category already exists!');", true);
                        Clear();
                        return;
                    }
                }
                if (myFile.HasFile)
                {
                    string virtualFolder = "IMAGE/foodCategory/";
                    string physicalFolder = Server.MapPath(virtualFolder);
                    string fileName = Session["adminname"] + "_" + myFile.FileName;
                    myFile.SaveAs(Path.Combine(physicalFolder, fileName));
                    CategoryImagePath = "~/IMAGE/foodCategory/" + fileName;

                    SqlParameter[] updateParams = new SqlParameter[6];
                    updateParams[0] = new SqlParameter("@sp_type", 2);
                    updateParams[1] = new SqlParameter("@CategoryName", Categoryname);
                    updateParams[2] = new SqlParameter("@Photo", CategoryImagePath);
                    updateParams[3] = new SqlParameter("@ActiveStatus", status);
                    updateParams[4] = new SqlParameter("@UpdatedDate", DateTime.Now);
                    updateParams[5] = new SqlParameter("@TypeId", categoryid);

                    int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_foodCategoryMaster", updateParams);

                    if (result != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{Categoryname} Category updated successfully');", true);
                        showCategory();
                        Clear();
                        btnSubmit.Text = "Submit";
                    }
                }
                else
                {
                    SqlParameter[] updateParams = new SqlParameter[5];
                    updateParams[0] = new SqlParameter("@sp_type", 7);
                    updateParams[1] = new SqlParameter("@CategoryName", Categoryname);
                    updateParams[2] = new SqlParameter("@ActiveStatus", status);
                    updateParams[3] = new SqlParameter("@UpdatedDate", DateTime.Now);
                    updateParams[4] = new SqlParameter("@TypeId", categoryid);

                    int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_foodCategoryMaster", updateParams);

                    if (result != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{Categoryname} Category updated successfully');", true);
                        showCategory();
                        Clear();
                        btnSubmit.Text = "Submit";
                    }
                }
            }
        }

        void Clear()
        {
            imagePreview.Src = "#";
            imagePreview.Attributes["style"] = "display:none;width:190px;height:150px;";

            // Optionally clear ViewState
            ViewState["existingImagePath"] = null;
            ViewState["uniqueId"] = null;
            ViewState["status"] = null;
            ViewState["CategoryName"] = null;
            CategoryName.Text = string.Empty;
            rbStatus.SelectedIndex = 0;
            btnSubmit.Text = "Submit";

        }


        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}