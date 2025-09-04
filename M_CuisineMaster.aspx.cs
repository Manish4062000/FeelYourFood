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
    public partial class M_CuisineMaster : System.Web.UI.Page
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
                    showCuisine();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expire, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");
                }
            }
        }

        public void showCuisine()
        {

            string str1 = "select * FROM CuisineMaster order by CuisineId desc";
            _c1.Retrive2(str1, ref ds);
            gvcred.DataSource = ds;
            gvcred.DataBind();
        }

        protected void gvrestdetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvcred.PageIndex = e.NewPageIndex;
            showCuisine();
        }
        protected void ImageBtnEdit_Command(object sender, CommandEventArgs e)
        {
            int UniqIId = Convert.ToInt32(e.CommandArgument);
            ViewState["uniqueId"] = UniqIId;
            string str = "select * FROM CuisineMaster  Where CuisineId='" + UniqIId + "'";
            _c1.Retrive2(str, ref ds);

            cuisinename.Text = ds.Tables[0].Rows[0]["CuisineName"].ToString();
            ViewState["cuisinename"] = cuisinename.Text;
            rbStatus.SelectedValue = (Convert.ToInt16(ds.Tables[0].Rows[0]["ActiveStatus"]).ToString());
            ViewState["status"] = rbStatus.SelectedValue;

            btnSubmit.Text = "Update";
        }
        protected void ImageBtnDelete_Command(object sender, CommandEventArgs e)
        {
            int UniqIId = Convert.ToInt32(e.CommandArgument);
            string str = "Update CuisineMaster Set ActiveStatus='0' Where CuisineId='" + UniqIId + "'";
            _c1.Execute2(str, ref ds);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Cuisine updated successfully');", true);
            showCuisine();
            return;

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string cuisine = (cuisinename.Text).Trim();
            if (string.IsNullOrWhiteSpace(cuisine))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Cuisine name is required');", true);
                return;
            }
            int status = Convert.ToInt32(rbStatus.SelectedValue);
            if (btnSubmit.Text == "Submit")
            {
                // Check for duplicate
                SqlParameter[] checkParams = new SqlParameter[2];
                checkParams[0] = new SqlParameter("@sp_type", 5);
                checkParams[1] = new SqlParameter("@CuisineName", cuisine);

                object count = SqlHelper.ExecuteScalar(_c1.con, CommandType.StoredProcedure, "sp_CuisineMaster", checkParams);
                int exists = Convert.ToInt32(count);

                if (exists > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{cuisine} cuisine already exists!');", true);
                    return;
                }

                // Proceed with insert
                SqlParameter[] insertParams = new SqlParameter[4];
                insertParams[0] = new SqlParameter("@sp_type", 1);
                insertParams[1] = new SqlParameter("@CuisineName", cuisine);
                insertParams[2] = new SqlParameter("@ActiveStatus", status);
                insertParams[3] = new SqlParameter("@CreatedBy", Session["restid"].ToString());

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_CuisineMaster", insertParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{cuisine} cuisine added successfully');", true);
                    showCuisine();
                    Clear();
                }
            }
            else
            {
                string originalName = ViewState["cuisinename"]?.ToString();
                string originalStatus = ViewState["status"]?.ToString();
                string cuisineId = ViewState["uniqueId"]?.ToString();

                bool nameChanged = originalName != cuisine;
                bool statusChanged = originalStatus != rbStatus.SelectedValue;

                if (!nameChanged && !statusChanged)
                {
                    Clear();
                    btnSubmit.Text = "Submit";
                    return;
                }

                if (nameChanged)
                {
                    SqlParameter[] checkParams = new SqlParameter[2];
                    checkParams[0] = new SqlParameter("@sp_type", 5);
                    checkParams[1] = new SqlParameter("@CuisineName", cuisine);

                    object count = SqlHelper.ExecuteScalar(_c1.con, CommandType.StoredProcedure, "sp_CuisineMaster", checkParams);
                    int exists = Convert.ToInt32(count);

                    if (exists > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{cuisine} cuisine already exists!');", true);
                        return;
                    }
                }

                SqlParameter[] updateParams = new SqlParameter[5];
                updateParams[0] = new SqlParameter("@sp_type", 2);
                updateParams[1] = new SqlParameter("@CuisineName", cuisine);
                updateParams[2] = new SqlParameter("@ActiveStatus", status);
                updateParams[3] = new SqlParameter("@UpdatedDate", DateTime.Now);
                updateParams[4] = new SqlParameter("@CuisineId", cuisineId);

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_CuisineMaster", updateParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{cuisine} cuisine updated successfully');", true);
                    showCuisine();
                    Clear();
                    btnSubmit.Text = "Submit";
                }
            }

        }
        void Clear()
        {
            cuisinename.Text = string.Empty;
            rbStatus.SelectedIndex = 0;
            btnSubmit.Text = "Submit";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}