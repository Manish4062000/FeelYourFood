using FeelYourFood_Admin.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Services;

namespace FeelYourFood
{
    public partial class ConfigureKioskForm : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        public readonly Connection _c1 = new Connection();
        public DataTable _dtt = new DataTable();
        public DataSet ds = new DataSet();
        public string str = string.Empty;
        public int configId = 0;
        int restId = 0;
        string CustomImagePath = String.Empty;
        int flag = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["restid"] != null)
                {
                    loadConsumeType();
                    loadCuisine();
                    loadpaymentMode();
                    loadfoodCategory();

                    string lookupSql = $"SELECT ConfigId FROM KioskConfiguration WHERE RestId ='{Session["restid"].ToString()}'";
                    ds.Clear();
                    _c1.Retrive2(lookupSql, ref ds);
                    
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        configId = Convert.ToInt32(ds.Tables[0].Rows[0]["ConfigId"]);
                        ViewState["configId"] = configId;
                        fetchAllConfigure();
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expired, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");
                }

            }

        }

        private void loadfoodCategory()
        {
            string query = "SELECT TypeId, CategoryName FROM FoodCategoryMaster WHERE ActiveStatus = 1 ORDER BY TypeId ASC";

            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            rptfoodcategory.DataSource = dt;
            rptfoodcategory.DataBind();
        }

        private void fetchAllConfigure()
        {
            KioskloadConsume();
            KioskloadCuisine();
            KioskloadPayment();
            KioskLoadMenu();
            KioskLoadCategory();
            ShowMenu();

        }

        protected string IsChecked(object consumeId)
        {
            if (Request.Form.GetValues("chkConsume")?.Contains(consumeId.ToString()) == true)
                return "checked";
            return "";
        }

        private void KioskLoadCategory()
        {
            List<int> selectedIds = new List<int>();

            string query = "SELECT CategoryId FROM Kiosk_Category WHERE ConfigId = @ConfigId";

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ConfigId", configId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        selectedIds.Add(reader.GetInt32(0));
                    }
                }
            }

            foreach (RepeaterItem item in rptfoodcategory.Items)
            {
                var chk = item.FindControl("chkCategory") as HtmlInputCheckBox; // or CheckBox if you're using asp:CheckBox
                if (chk != null)
                {
                    int val;
                    if (int.TryParse(chk.Value, out val))
                    {
                        chk.Checked = selectedIds.Contains(val);
                    }
                }
            }
        }


        private void KioskloadConsume()
        {
            List<int> selectedIds = new List<int>();

            string query = "SELECT ConsumeTypeId FROM Kiosk_ConsumeType WHERE ConfigId = @ConfigId";

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ConfigId", configId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        selectedIds.Add(reader.GetInt32(0));
                    }
                }
            }

            foreach (RepeaterItem item in rptConsumeType.Items)  // Your repeater ID for consume types
            {
                var chk = item.FindControl("chkConsume") as HtmlInputCheckBox; // or CheckBox if you're using asp:CheckBox
                if (chk != null)
                {
                    int val;
                    if (int.TryParse(chk.Value, out val))
                    {
                        chk.Checked = selectedIds.Contains(val);
                    }
                }
            }
        }

        private void KioskloadCuisine()
        {
            List<int> selectedIds = new List<int>();

            string query = "SELECT CuisineId FROM Kiosk_Cuisine WHERE ConfigId = @ConfigId";

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ConfigId", configId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        selectedIds.Add(reader.GetInt32(0));
                    }
                }
            }

            foreach (RepeaterItem item in rptCuisine.Items)
            {
                var chk = item.FindControl("chkCuisine") as HtmlInputCheckBox;
                if (chk != null)
                {
                    int val;
                    if (int.TryParse(chk.Value, out val))
                    {
                        chk.Checked = selectedIds.Contains(val);
                    }
                }
            }
        }

        private void KioskloadPayment()
        {
            List<int> selectedIds = new List<int>();

            string query = "SELECT PaymentModeId FROM Kiosk_PaymentMode WHERE ConfigId = @ConfigId";

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ConfigId", configId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        selectedIds.Add(reader.GetInt32(0));
                    }
                }
            }

            foreach (RepeaterItem item in rptpayment.Items)  // rptpayment = your repeater ID for payment
            {
                var chk = item.FindControl("chkPayment") as HtmlInputCheckBox; // or CheckBox if using asp:CheckBox
                if (chk != null)
                {
                    int val;
                    if (int.TryParse(chk.Value, out val))
                    {
                        chk.Checked = selectedIds.Contains(val);
                    }
                }
            }
        }

        private void ShowMenu()
        {
            string lookupSql = $"SELECT ConfigId FROM KioskConfiguration WHERE RestId ='{Session["restid"].ToString()}'";
            ds.Clear();
            _c1.Retrive2(lookupSql, ref ds);
            string temp = Session["restid"].ToString();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                configId = Convert.ToInt32(ds.Tables[0].Rows[0]["ConfigId"]);

            }
            var consumeIds = rptConsumeType.Items.Cast<RepeaterItem>()
                .Select(item => item.FindControl("chkConsume") as HtmlInputCheckBox)
                .Where(chk => chk != null && chk.Checked)
                .Select(chk => chk.Value)
                .ToList();

            var cuisineIds = rptCuisine.Items.Cast<RepeaterItem>()
                .Select(item => item.FindControl("chkCuisine") as HtmlInputCheckBox)
                .Where(chk => chk != null && chk.Checked)
                .Select(chk => chk.Value)
                .ToList();

            var categoryIds = rptfoodcategory.Items.Cast<RepeaterItem>()
                .Select(item => item.FindControl("chkCategory") as HtmlInputCheckBox)
                .Where(chk => chk != null && chk.Checked)
                .Select(chk => chk.Value)
                .ToList();

            var sb = new StringBuilder(@"
    SELECT 
        m.FoodMenuId,
        m.FoodMenuName,
        STUFF((
            SELECT ', ' + cu.CuisineName
            FROM MenuCuisine mc
            INNER JOIN CuisineMaster cu ON cu.CuisineId = mc.CuisineId
            WHERE mc.FoodMenuId = m.FoodMenuId
            FOR XML PATH(''), TYPE
        ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS CuisineNames,
        STUFF((
            SELECT ', ' + ct.[Type]
            FROM MenuConsumeType mct
            INNER JOIN ConsumeTypeMaster ct ON ct.ConsumeTypeId = mct.ConsumeTypeId
            WHERE mct.FoodMenuId = m.FoodMenuId
            FOR XML PATH(''), TYPE
        ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS ConsumeTypes
    FROM MenuMaster m
    WHERE 1 = 1
");

            var parameters = new List<SqlParameter>();

            if (consumeIds.Any())
            {
                string consumeCsv = string.Join(",", consumeIds);
                sb.Append(@"
        AND EXISTS (
            SELECT 1 FROM MenuConsumeType mct
            WHERE mct.FoodMenuId = m.FoodMenuId
            AND mct.ConsumeTypeId IN (
                SELECT CAST(Value AS INT) FROM dbo.fnSplitString(@ConsumeCsv, ',')
            )
        )");
                parameters.Add(new SqlParameter("@ConsumeCsv", consumeCsv));
            }

            if (cuisineIds.Any())
            {
                string cuisineCsv = string.Join(",", cuisineIds);
                sb.Append(@"
        AND EXISTS (
            SELECT 1 FROM MenuCuisine mc
            WHERE mc.FoodMenuId = m.FoodMenuId
            AND mc.CuisineId IN (
                SELECT CAST(Value AS INT) FROM dbo.fnSplitString(@CuisineCsv, ',')
            )
        )");
                parameters.Add(new SqlParameter("@CuisineCsv", cuisineCsv));
            }

            // Filter by Category
            if (categoryIds.Any())
            {
                string categoryCsv = string.Join(",", categoryIds);
                sb.Append(@"
        AND m.CategoryId IN (
            SELECT CAST(Value AS INT) FROM dbo.fnSplitString(@CategoryCsv, ',')
        )");
                parameters.Add(new SqlParameter("@CategoryCsv", categoryCsv));
            }

            // Exclude menus already in current config
            if (flag == 0 && configId > 0 && Session["restid"] != null)
            {
                sb.Append(@"
        AND m.FoodMenuId NOT IN (
            SELECT FoodMenuId
            FROM Kiosk_MenuDetails
            WHERE ConfigId = @ConfigId AND RestId = @RestId
        )");
                parameters.Add(new SqlParameter("@ConfigId", configId));
                parameters.Add(new SqlParameter("@RestId", Session["restid"].ToString()));
            }

            sb.Append(" ORDER BY m.FoodMenuName ASC");

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            using (var cmd = new SqlCommand(sb.ToString(), conn))
            {
                cmd.Parameters.AddRange(parameters.ToArray());
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);

                ddlmenulist.Items.Clear();
                ddlmenulist.DataSource = dt;
                ddlmenulist.DataTextField = "FoodMenuName";
                ddlmenulist.DataValueField = "FoodMenuId";
                ddlmenulist.DataBind();
            }

            ddlmenulist.Items.Insert(0, new ListItem("-- Select a Menu --", "0"));
        }

        private void KioskLoadMenu()
        {
            string configId = ViewState["configId"] != null 
                              ? ViewState["configId"].ToString()
                              : "0";

            string str1 = "SELECT * FROM Kiosk_MenuDetails  WHERE ConfigId = '" + configId + "' ORDER BY KioskMenuDetailId DESC";

            DataSet ds = new DataSet();
            _c1.Retrive2(str1, ref ds);
            gvMenu.DataSource = ds;
            gvMenu.DataBind();
        }

        private void loadConsumeType()
        {
            string query = "SELECT ConsumeTypeId, Type FROM ConsumeTypeMaster WHERE ActiveStatus = 1 ORDER BY Type ASC";

            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            rptConsumeType.DataSource = dt;
            rptConsumeType.DataBind();
        }
        private void loadCuisine()
        {
            string str1 = "SELECT  CuisineId,CuisineName FROM CuisineMaster WHERE ActiveStatus = 1 ORDER BY CuisineId ASC";

            SqlCommand cmd = new SqlCommand(str1, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            rptCuisine.DataSource = dt;

            rptCuisine.DataBind();

        }
        private void loadpaymentMode()
        {
            string query = "SELECT  PaymentMode,PaymentModeId FROM PaymentModeMaster WHERE ActiveStatus = 1 ORDER BY PaymentModeId ASC";


            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            rptpayment.DataSource = dt;

            rptpayment.DataBind();

        }

        protected string selectedMenuImagePath = ""; // To store for DB insert

        protected void ddlmenulist_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedMenuId;
            if (int.TryParse(ddlmenulist.SelectedValue, out selectedMenuId) && selectedMenuId > 0)
            {
                string query = "SELECT MenuPhoto FROM MenuMaster WHERE FoodMenuId = @FoodMenuId";
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FoodMenuId", selectedMenuId);
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        string selectedMenuImagePath = result.ToString();
                        imagePreview.Src = ResolveUrl(selectedMenuImagePath);
                        imagePreview.Attributes["style"] = "display:block;width:190px;height:120px;";
                        ViewState["MenuImagePath"] = selectedMenuImagePath;
                    }
                    else
                    {
                        imagePreview.Attributes["style"] = "display:none;";
                        ViewState["MenuImagePath"] = null;
                    }
                }
            }
            else
            {
                imagePreview.Attributes["style"] = "display:none;";
                ViewState["MenuImagePath"] = null;
            }
        }

        protected void gvMenu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMenu.PageIndex = e.NewPageIndex;
            KioskLoadMenu();
        }
        protected void ImageBtnEdit_Command(object sender, CommandEventArgs e)
        {
            flag = 1;
            ShowMenu();

            int menuId = Convert.ToInt32(e.CommandArgument);
            ViewState["uniqueId"] = menuId;

            string query = @"
                            
                            SELECT * FROM Kiosk_MenuDetails WHERE KioskMenuDetailId = @KioskMenuDetailId;";
            SqlParameter[] parameters = { new SqlParameter("@KioskMenuDetailId", menuId) };
            DataSet ds = SqlHelper.ExecuteDataset(_c1.con, CommandType.Text, query, parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {

                DataRow menuRow = ds.Tables[0].Rows[0];
                string foodMenuId = menuRow["FoodMenuId"].ToString();

                if (ddlmenulist.Items.FindByValue(foodMenuId) != null)
                {
                    ddlmenulist.SelectedValue = foodMenuId;
                }

                rbStatus.SelectedValue = Convert.ToBoolean(menuRow["ActiveStatus"]) ? "1" : "0";

                // Store image path to show preview
                if (!string.IsNullOrEmpty(menuRow["MenuPhotoPath"].ToString()))
                {
                    imagePreview.Src = ResolveUrl(menuRow["MenuPhotoPath"].ToString());
                    imagePreview.Visible = true;
                    imagePreview.Attributes["style"] = "display:block;width:190px;height:150px;";
                }
                btnSubmit.Text = "Update";
            }
        }

        protected void ImageBtnDelete_Command(object sender, CommandEventArgs e)
        {
            int foodMenuId = Convert.ToInt32(e.CommandArgument);

            string query = "update Kiosk_MenuDetails set ActiveStatus =0 where KioskMenuDetailId='" + foodMenuId + "'";
            _c1.Execute2(query, ref ds);

            KioskLoadMenu();

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSubmit.Text == "Submit")
                {
                    if (ddlmenulist.SelectedIndex == 0)
                    {
                        ShowAlert("Please select a valid menu.");
                        return;
                    }

                    if (ViewState["configId"] == null || string.IsNullOrEmpty(ViewState["configId"].ToString()))
                    {
                        ShowAlert("Please submit configuration first.");
                        return;
                    }

                    int configId = Convert.ToInt32(ViewState["configId"]);
                    int menuId = Convert.ToInt32(ddlmenulist.SelectedValue);
                    int activeStatus = Convert.ToInt32(rbStatus.SelectedValue);
                    string restId = Session["restid"]?.ToString();

                    SqlParameter[] menuParams = new SqlParameter[]
                    {
                        new SqlParameter("@sp_type", 6),
                        new SqlParameter("@ConfigId", configId),
                        new SqlParameter("@MenuId", menuId),
                        new SqlParameter("@RestId", restId),
                        new SqlParameter("@ActiveStatus", activeStatus),
                        new SqlParameter("@ReturnConfigId", SqlDbType.Int) { Direction = ParameterDirection.Output }
                    };
                    SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_KioskConfiguration", menuParams);

                    // Handle Image Upload
                    if (myFile.HasFile)
                    {
                        string virtualFolder = "IMAGE/customMenuImage/";
                        string physicalFolder = Server.MapPath(virtualFolder);
                        string fileName = Session["adminname"] + "_" + Path.GetFileName(myFile.FileName);
                        string savedPath = Path.Combine(physicalFolder, fileName);
                        myFile.SaveAs(savedPath);

                        string imagePath = "~/" + virtualFolder + fileName;

                        using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
                        {
                            con.Open();
                            using (var cmd = new SqlCommand(@"
                        UPDATE Kiosk_MenuDetails
                        SET MenuPhotoPath = @MenuPhotoPath
                        WHERE ConfigId = @ConfigId AND FoodMenuId = @FoodMenuId", con))
                            {
                                cmd.Parameters.AddWithValue("@MenuPhotoPath", imagePath);
                                cmd.Parameters.AddWithValue("@ConfigId", configId);
                                cmd.Parameters.AddWithValue("@FoodMenuId", menuId);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    ShowAlert("Menu saved successfully.");
                }
                else
                {
                    // Edit mode
                    int activeStatus = Convert.ToInt32(rbStatus.SelectedValue);
                    int id = Convert.ToInt32(ViewState["uniqueId"]);
                    string menuName = ddlmenulist.SelectedItem.Text;

                    if (myFile.HasFile)
                    {
                        string virtualFolder = "IMAGE/customMenuImage/";
                        string physicalFolder = Server.MapPath(virtualFolder);
                        string fileName = Session["adminname"] + "_" + Path.GetFileName(myFile.FileName);
                        string savedPath = Path.Combine(physicalFolder, fileName);
                        myFile.SaveAs(savedPath);

                        string imagePath = "~/" + virtualFolder + fileName;

                        SqlParameter[] updateParams = new SqlParameter[]
                        {
                            new SqlParameter("@sp_type", 4),
                            new SqlParameter("@FoodMenuId", ddlmenulist.SelectedValue),
                            new SqlParameter("@FoodMenuName", menuName),
                            new SqlParameter("@MenuPhotoPath", imagePath),
                            new SqlParameter("@ActiveStatus", activeStatus),
                            new SqlParameter("@CreatedDate", DateTime.Now),
                            new SqlParameter("@KioskMenuDetailId", id),
                            new SqlParameter("@ReturnConfigId", SqlDbType.Int) { Direction = ParameterDirection.Output }
                        };

                        SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_KioskConfiguration", updateParams);
                    }
                    else
                    {
                        SqlParameter[] updateParams = new SqlParameter[]
                        {
                            new SqlParameter("@sp_type", 5),
                            new SqlParameter("@FoodMenuId", ddlmenulist.SelectedValue),
                            new SqlParameter("@FoodMenuName", menuName),
                            new SqlParameter("@ActiveStatus", activeStatus),
                            new SqlParameter("@CreatedDate", DateTime.Now),
                            new SqlParameter("@KioskMenuDetailId", id),
                            new SqlParameter("@ReturnConfigId", SqlDbType.Int) { Direction = ParameterDirection.Output }
                        };

                        SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_KioskConfiguration", updateParams);
                    }

                    btnSubmit.Text = "Submit";
                    ShowAlert($"{menuName} menu updated successfully.");
                }

                Clear();
                KioskLoadMenu();
                fetchAllConfigure();
                ShowMenu();
            }
            catch (Exception ex)
            {
                ShowAlert("Error: " + ex.Message);
            }
        }

        private void Clear()
        {
            ddlmenulist.ClearSelection();
            rbStatus.SelectedIndex = 0;
            imagePreview.Src = "#";
            imagePreview.Attributes["style"] = "display:none;width:190px;height:150px;";
            KioskLoadMenu();
            ViewState["uniqueId"] = null;
            btnSubmit.Text = "Submit";
            ShowMenu();
        }

        private void ShowAlert(string message)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{message}');", true);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void Filter_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime now = DateTime.Now;

                string consumeCsv = string.Join(",", rptConsumeType.Items.Cast<RepeaterItem>()
                                    .Select(item => item.FindControl("chkConsume") as HtmlInputCheckBox)
                                    .Where(chk => chk != null && chk.Checked)
                                    .Select(chk => chk.Value));

                string cuisineCsv = string.Join(",", rptCuisine.Items.Cast<RepeaterItem>()
                                    .Select(item => item.FindControl("chkCuisine") as HtmlInputCheckBox)
                                    .Where(chk => chk != null && chk.Checked)
                                    .Select(chk => chk.Value));

                string categoryCsv = string.Join(",", rptfoodcategory.Items.Cast<RepeaterItem>()
                                    .Select(item => item.FindControl("chkCategory") as HtmlInputCheckBox)
                                    .Where(chk => chk != null && chk.Checked)
                                    .Select(chk => chk.Value));

                string paymentCsv = string.Join(",", rptpayment.Items.Cast<RepeaterItem>()
                                    .Select(item => item.FindControl("chkPayment") as HtmlInputCheckBox)
                                    .Where(chk => chk != null && chk.Checked)
                                    .Select(chk => chk.Value));

                if (string.IsNullOrEmpty(consumeCsv) || string.IsNullOrEmpty(cuisineCsv) || string.IsNullOrEmpty(categoryCsv) || string.IsNullOrEmpty(paymentCsv))
                {
                    ShowAlert("Please select at least one Consume Type, Cuisine, Category, and Payment Mode.");
                    return;
                }

                string restId = Session["restid"]?.ToString();
                string createdBy = Session["restid"]?.ToString();

                if (string.IsNullOrEmpty(restId))
                {
                    ShowAlert("Session expired. Please log in again.");
                    return;
                }

                int configId = 0;
                ds.Clear();
                _c1.Retrive2($"SELECT ConfigId FROM KioskConfiguration WHERE RestId = '{restId}'", ref ds);
                bool isUpdate = ds.Tables[0].Rows.Count > 0;
                if (isUpdate)
                {
                    configId = Convert.ToInt32(ds.Tables[0].Rows[0]["ConfigId"]);
                }

                SqlParameter[] configParams = new SqlParameter[]
                {
            new SqlParameter("@sp_type", isUpdate ? 2 : 1),
            new SqlParameter("@ConfigId", isUpdate ? (object)configId : DBNull.Value),
            new SqlParameter("@RestId", restId),
            new SqlParameter("@CreatedBy", createdBy),
            new SqlParameter("@UpdatedDate", now),
            new SqlParameter("@ConsumeTypeIds", consumeCsv),
            new SqlParameter("@CuisineIds", cuisineCsv),
            new SqlParameter("@CategoryIds", categoryCsv),
            new SqlParameter("@PaymentModeIds", paymentCsv),
            new SqlParameter("@MenuId", DBNull.Value),
            new SqlParameter("@ActiveStatus", DBNull.Value),
            new SqlParameter("@FoodMenuId", DBNull.Value),
            new SqlParameter("@FoodMenuName", DBNull.Value),
            new SqlParameter("@MenuPhotoPath", DBNull.Value),
            new SqlParameter("@CreatedDate", DBNull.Value),
            new SqlParameter("@KioskMenuDetailId", DBNull.Value),
            new SqlParameter("@ReturnConfigId", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_KioskConfiguration", configParams);

                int newConfigId = Convert.ToInt32(configParams[configParams.Length - 1].Value);
                ViewState["configId"] = newConfigId;

                ShowMenu();
            }
            catch (Exception ex)
            {
                ShowAlert("Error: " + ex.Message);
            }
        }

    }
}