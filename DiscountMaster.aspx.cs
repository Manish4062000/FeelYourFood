using FeelYourFood_Admin.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeelYourFood
{
    public partial class DiscountMaster : System.Web.UI.Page
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
                    ShowDiscountData();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expire, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(discountname.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter a valid Discount name.');", true);
                return;
            }
            if (String.IsNullOrEmpty(discountper.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter a valid discount Percentage.');", true);
                return;
            }
            string disname = discountname.Text.Trim();
            int disper = Convert.ToInt32(discountper.Text);
            int activeStatus = Convert.ToInt32(rbStatus.SelectedValue);
            if (btnSubmit.Text == "Submit")
            {

                SqlParameter[] insertParams = new SqlParameter[6];
                insertParams[0] = new SqlParameter("@sp_type", 1);
                insertParams[1] = new SqlParameter("@RestId", Session["restid"].ToString());
                insertParams[2] = new SqlParameter("@DiscountName", disname);
                insertParams[3] = new SqlParameter("@DiscountPercentage", disper);
                insertParams[4] = new SqlParameter("@ActiveStatus", activeStatus);
                insertParams[5] = new SqlParameter("@CreatedBy", Session["restid"].ToString());

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_DiscountMaster", insertParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{disname} discount added successfully');", true);
                    ShowDiscountData();
                    Clear();
                }
            }
            else
            {
                SqlParameter[] insertParams = new SqlParameter[7];
                insertParams[0] = new SqlParameter("@sp_type", 2);
                insertParams[1] = new SqlParameter("@RestId", Session["restid"].ToString());
                insertParams[2] = new SqlParameter("@DiscountName", disname);
                insertParams[3] = new SqlParameter("@DiscountPercentage", disper);
                insertParams[4] = new SqlParameter("@ActiveStatus", activeStatus);
                insertParams[5] = new SqlParameter("@UpdatedDate", DateTime.Now);
                insertParams[6] = new SqlParameter("@Id", ViewState["id"].ToString());

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_DiscountMaster", insertParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{disname} discount updated successfully');", true);
                    ShowDiscountData();
                    
                }
            }
            Clear();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void Clear()
        {
            discountname.Text = string.Empty;
            discountper.Text = string.Empty;
            rbStatus.SelectedIndex = 0;
            btnSubmit.Text = "Submit";
            ShowDiscountData();
        }
        private void ShowDiscountData()
        {
            string str1 = "select * FROM DiscountMaster where RestId='" + Session["restid"].ToString() + "' order by Id desc";
            _c1.Retrive2(str1, ref ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvdiscount.DataSource = ds;
                gvdiscount.DataBind();
            }
        }

        protected void btnEdit_Command(object sender, CommandEventArgs e)
        {
            int UniqIId = Convert.ToInt32(e.CommandArgument);
            ViewState["id"] = UniqIId;
            string str = "select * FROM DiscountMaster  Where Id='" + UniqIId + "'";
            _c1.Retrive2(str, ref ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                discountname.Text = ds.Tables[0].Rows[0]["DiscountName"].ToString();
                discountper.Text = ds.Tables[0].Rows[0]["DiscountPercentage"].ToString();

                rbStatus.SelectedValue = (Convert.ToInt16(ds.Tables[0].Rows[0]["ActiveStatus"]).ToString());

                btnSubmit.Text = "Update";
            }
        }

        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            int Id = Convert.ToInt32(e.CommandArgument);

            string query = "update DiscountMaster set ActiveStatus =0 where Id='" + Id + "'";
            _c1.Execute2(query, ref ds);
            ShowDiscountData();
        }
        protected void gvdiscount_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvdiscount.PageIndex = e.NewPageIndex;
            ShowDiscountData();
        }
    }
}