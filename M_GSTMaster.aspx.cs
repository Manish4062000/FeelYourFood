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
    public partial class M_GSTMaster : System.Web.UI.Page
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

                    showgst();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expire, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");
                }
            }
        }

        private void showgst()
        {
            string query = " select * from GstMaster order by Id desc ";
            _c1.Retrive2(query, ref ds);
            gvgst.DataSource = ds;
            gvgst.DataBind();

        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string CGST = cgst.Text.Trim();
            string SGST = sgst.Text.Trim();
            string GST = gst.Text.Trim();
            if (string.IsNullOrEmpty(CGST))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CGST percentage is required.');", true);
                return;
            }
            if (string.IsNullOrEmpty(SGST))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('SGST percentage is required.');", true);
                return;
            }
            if (string.IsNullOrEmpty(GST))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('GST percentage is required.');", true);
                return;
            }
            double temp = Convert.ToDouble(CGST) + Convert.ToDouble(SGST);
            double gstdata = Convert.ToDouble(GST);
            if (temp != gstdata)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sum of CGST % and SGST % much be equal to GST percentage. Please enter valid percentage');", true);
                return;
            }



            int status = Convert.ToInt32(rbStatus.SelectedValue);

            if (btnSubmit.Text == "Submit")
            {
                SqlParameter[] checkParams = new SqlParameter[2];
                checkParams[0] = new SqlParameter("@sp_type", 5);
                checkParams[1] = new SqlParameter("@GST_PERCENTAGE", GST);

                object count = SqlHelper.ExecuteScalar(_c1.con, CommandType.StoredProcedure, "sp_GstMaster", checkParams);
                int exists = Convert.ToInt32(count);

                if (exists > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('This GST already exists!');", true);
                    return;
                }

                SqlParameter[] insertParams = new SqlParameter[]
                 {
                        new SqlParameter("@sp_type", 1),
                        new SqlParameter("@CGST_PERCENTAGE", CGST),
                        new SqlParameter("@SGST_PERCENTAGE", SGST),
                        new SqlParameter("@GST_PERCENTAGE", GST),
                        new SqlParameter("@ActiveStatus", status),
                        new SqlParameter("@CreatedBy", Session["adminname"] != null ? Session["adminname"].ToString() : "Addsoft")
                };

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_GstMaster", insertParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('GST saved successfully');", true);
                    showgst();
                    Clear();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error saving GST');", true);
                }

            }
            else
            {

                string id = ViewState["uniqueId"]?.ToString();


                SqlParameter[] updateParams = new SqlParameter[]
                {
                        new SqlParameter("@sp_type", 2),
                        new SqlParameter("@CGST_PERCENTAGE", CGST),
                        new SqlParameter("@SGST_PERCENTAGE", SGST),
                        new SqlParameter("@GST_PERCENTAGE", GST),
                        new SqlParameter("@ActiveStatus", status),
                        new SqlParameter("@UpdatedDate", DateTime.Now),
                        new SqlParameter("@Id", id),
                };

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_GstMaster", updateParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('GST updated successfully');", true);
                    btnSubmit.Text = "Submit";
                    showgst();
                    Clear();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error updating GST');", true);
                }

            }
        }

        void Clear()
        {

            cgst.Text = string.Empty;
            sgst.Text = string.Empty;
            gst.Text = string.Empty;
            rbStatus.SelectedIndex = 0;
            btnSubmit.Text = "Submit";

        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }



        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            SqlParameter[] deleteParams = new SqlParameter[]
            {
                new SqlParameter("@sp_type", 3),
                new SqlParameter("@Id", id)
            };

            int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_GstMaster", deleteParams);

            if (result != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('GST deleted successfully');", true);
                showgst();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Failed to delete GST');", true);
            }
        }
        protected void btnEdit_Command(object sender, CommandEventArgs e)
        {
            int ItemId = Convert.ToInt32(e.CommandArgument);
            ViewState["uniqueId"] = ItemId;
            string query = " select * from GstMaster  where Id='" + ItemId + "' ";
            _c1.Retrive2(query, ref ds);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cgst.Text = ds.Tables[0].Rows[0]["CGST_PERCENTAGE"].ToString();
                sgst.Text = ds.Tables[0].Rows[0]["SGST_PERCENTAGE"].ToString();
                gst.Text = ds.Tables[0].Rows[0]["GST_PERCENTAGE"].ToString();
                object statusObj = ds.Tables[0].Rows[0]["ActiveStatus"];
                if (statusObj != null && statusObj != DBNull.Value)
                {
                    int statusValue = Convert.ToInt32(statusObj);
                    rbStatus.SelectedValue = statusValue.ToString();
                }

                btnSubmit.Text = "Update";

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error in fetching data ');", true);
                return;
            }
        }

        protected void gvAddonItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvgst.PageIndex = e.NewPageIndex;
            showgst();
        }

       
    }
}