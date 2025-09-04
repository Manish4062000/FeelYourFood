using FeelYourFood_Admin.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeelYourFood
{
    public partial class M_SubscriptionMaster : System.Web.UI.Page
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

                    showSubscription();
                    loadDevice();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expire, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");
                }
            }
        }

        private void loadDevice()
        {
            try
            {


                string query = $"SELECT DeviceName, Id FROM DeviceMaster";

                ds.Clear();
                _c1.Retrive2(query, ref ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddldevice.Items.Clear();
                    ddldevice.DataSource = ds.Tables[0];
                    ddldevice.DataTextField = "DeviceName";
                    ddldevice.DataValueField = "Id";
                    ddldevice.DataBind();
                    ddldevice.Items.Insert(0, new ListItem("----Select Menu----", ""));
                    ddldevice.SelectedIndex = 0;
                }
                else
                {
                    ddldevice.Items.Clear();
                    ddldevice.Items.Add(new ListItem("-- No Menus Available --", ""));
                }
            }
            catch (Exception ex)
            {
                ddldevice.Items.Clear();
                ddldevice.Items.Add(new ListItem("-- Error Loading Menu --", ""));
            }
        }

        private void showSubscription()
        {
            string query = " select ds.*,d.DeviceName from DeviceAndSubscriptionMaster ds inner join DeviceMaster d on ds.Id=d.Id order by ds.Id desc ";
            _c1.Retrive2(query, ref ds);
            gvsubscription.DataSource = ds;
            gvsubscription.DataBind();

        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string device = ddldevice.SelectedValue;
            string devicep = deviceprice.Text.Trim();
            string subscriptionp = subscriptionprice.Text.Trim();
            if (device=="0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select a valid Device.');", true);
                return;
            }
            if (string.IsNullOrEmpty(devicep))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Device price is required.');", true);
                return;
            }
            if (string.IsNullOrEmpty(subscriptionp))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Subscription Price is required.');", true);
                return;
            }
            
            if (btnSubmit.Text == "Submit")
            {
                SqlParameter[] checkParams = new SqlParameter[2];
                checkParams[0] = new SqlParameter("@sp_type", 5);
                checkParams[1] = new SqlParameter("@DeviceId", device);

                object count = SqlHelper.ExecuteScalar(_c1.con, CommandType.StoredProcedure, "sp_DeviceAndSubscriptionMaster", checkParams);
                int exists = Convert.ToInt32(count);

                if (exists > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('This device already exists!');", true);
                    return;
                }

                SqlParameter[] insertParams = new SqlParameter[]
                 {
                        new SqlParameter("@sp_type", 1),
                        new SqlParameter("@DeviceId", device),
                        new SqlParameter("@DevicePrice", devicep),
                        new SqlParameter("@SubscriptionPrice", subscriptionp)
                };

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_DeviceAndSubscriptionMaster", insertParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Device Subscription saved successfully');", true);
                    showSubscription();
                    Clear();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error saving Device Subscription');", true);
                }

            }
            else
            {

                string id = ViewState["uniqueId"]?.ToString();


                SqlParameter[] updateParams = new SqlParameter[]
                {
                        new SqlParameter("@sp_type", 2),
                        new SqlParameter("@DeviceId", device),
                        new SqlParameter("@DevicePrice", devicep),
                        new SqlParameter("@SubscriptionPrice", subscriptionp),
                        new SqlParameter("@Id", id),
                };

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_DeviceAndSubscriptionMaster", updateParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Device Subscription  updated successfully');", true);
                    btnSubmit.Text = "Submit";
                    showSubscription();
                    Clear();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error updating Device Subscription ');", true);
                }

            }
        }
        void Clear()
        {

            deviceprice.Text = string.Empty;
            subscriptionprice.Text = string.Empty;
            ddldevice.SelectedIndex = 0;
            btnSubmit.Text = "Submit";
            showSubscription();

        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void btnEdit_Command(object sender, CommandEventArgs e)
        {
            int ItemId = Convert.ToInt32(e.CommandArgument);
            ViewState["uniqueId"] = ItemId;
            string query = " select * from DeviceAndSubscriptionMaster  where Id='" + ItemId + "' ";
            _c1.Retrive2(query, ref ds);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                deviceprice.Text = ds.Tables[0].Rows[0]["DevicePrice"].ToString();
                subscriptionprice.Text = ds.Tables[0].Rows[0]["SubscriptionPrice"].ToString();
                object statusObj = ds.Tables[0].Rows[0]["DeviceId"];
                if (statusObj != null && statusObj != DBNull.Value)
                {
                    int statusValue = Convert.ToInt32(statusObj);
                    ddldevice.SelectedValue = statusValue.ToString();
                }

                btnSubmit.Text = "Update";

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error in fetching data ');", true);
                return;
            }
        }


        protected void gvsubscription_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvsubscription.PageIndex = e.NewPageIndex;
            showSubscription();
        }
    }
}