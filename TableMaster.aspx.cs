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
    public partial class TableMaster : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        public readonly Connection _c1 = new Connection();
        public DataTable _dtt = new DataTable();
        public DataSet ds = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["restid"] != null && !string.IsNullOrEmpty(Session["restid"].ToString()))
                {
                    Showtable();
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
            if (String.IsNullOrEmpty(tablename.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter a valid table name.');", true);
                return;
            }
            if (String.IsNullOrEmpty(chaircount.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please enter a valid chair for the table.');", true);
                return;
            }
            string table = tablename.Text.Trim();
            int count = Convert.ToInt32(chaircount.Text);
            int activeStatus = Convert.ToInt32(rbStatus.SelectedValue);
            if (btnSubmit.Text == "Submit")
            {

                SqlParameter[] insertParams = new SqlParameter[6];
                insertParams[0] = new SqlParameter("@sp_type", 1);
                insertParams[1] = new SqlParameter("@RestaurantId", Session["restid"].ToString());
                insertParams[2] = new SqlParameter("@TableName", table);
                insertParams[3] = new SqlParameter("@ChareCount", count);
                insertParams[4] = new SqlParameter("@ActiveStatus", activeStatus);
                insertParams[5] = new SqlParameter("@CreatedBy", Session["restid"].ToString());

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_TableMaster", insertParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{table} table added successfully');", true);
                    Showtable();
                    Clear();
                }
            }
            else
            {
                SqlParameter[] insertParams = new SqlParameter[8];
                insertParams[0] = new SqlParameter("@sp_type", 2);
                insertParams[1] = new SqlParameter("@RestaurantId", Session["restid"].ToString());
                insertParams[2] = new SqlParameter("@TableName", table);
                insertParams[3] = new SqlParameter("@ChareCount", count);
                insertParams[4] = new SqlParameter("@ActiveStatus", activeStatus);
                insertParams[5] = new SqlParameter("@UpdatedBy", Session["restid"].ToString());
                insertParams[6] = new SqlParameter("@UpdatedDate", DateTime.Now);
                insertParams[7] = new SqlParameter("@TableId", ViewState["id"].ToString());

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_TableMaster", insertParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{table} table updated successfully');", true);
                    Showtable();
                    Clear();
                }
            }
            Clear();
        }

        private void Clear()
        {
            tablename.Text = string.Empty;
            chaircount.Text = string.Empty;
            rbStatus.SelectedIndex = 0;
            btnSubmit.Text = "Submit";
            Showtable();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void gvtable_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvtable.PageIndex = e.NewPageIndex;
            Showtable();
        }

        private void Showtable()
        {
            string str1 = "select * FROM TableMaster where RestaurantId='" + Session["restid"].ToString() + "' order by TableId desc";
            _c1.Retrive2(str1, ref ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvtable.DataSource = ds;
                gvtable.DataBind();
            }
        }

        protected void btnEdit_Command(object sender, CommandEventArgs e)
        {
            int UniqIId = Convert.ToInt32(e.CommandArgument);
            ViewState["id"] = UniqIId;
            string str = "select * FROM TableMaster  Where TableId='" + UniqIId + "'";
            _c1.Retrive2(str, ref ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                tablename.Text = ds.Tables[0].Rows[0]["TableName"].ToString();
                chaircount.Text = ds.Tables[0].Rows[0]["ChareCount"].ToString();

                rbStatus.SelectedValue = (Convert.ToInt16(ds.Tables[0].Rows[0]["ActiveStatus"]).ToString());

                btnSubmit.Text = "Update";
            }
        }

        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            int TableId = Convert.ToInt32(e.CommandArgument);

            string query = "update TableMaster set ActiveStatus =0 where TableId='" + TableId + "'";
            _c1.Execute2(query, ref ds);
            Showtable();
        }
    }
}