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
    public partial class M_BankDetailsMaster : System.Web.UI.Page
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
                    loaddetails();
                }
            }
        }

        private void loaddetails()
        {
            string query = " select Id,bankname,branch,accountholder,ifsc,account from bankmaster ";
            _c1.Retrive2(query, ref ds);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Session["IDS"]= ds.Tables[0].Rows[0]["Id"].ToString();
                btnSubmit.Text = "Update";
                DataRow row = ds.Tables[0].Rows[0];
                
                bankname.Text = !string.IsNullOrWhiteSpace(row["bankname"]?.ToString()) ? row["bankname"].ToString() : string.Empty;
                branchname.Text = !string.IsNullOrWhiteSpace(row["branch"]?.ToString()) ? row["branch"].ToString() : string.Empty;
                ifsc.Text = !string.IsNullOrWhiteSpace(row["ifsc"]?.ToString()) ? row["ifsc"].ToString() : string.Empty;
                accountnumber.Text = !string.IsNullOrWhiteSpace(row["account"]?.ToString()) ? row["account"].ToString() : string.Empty;
                holdername.Text = !string.IsNullOrWhiteSpace(row["accountholder"]?.ToString()) ? row["accountholder"].ToString() : string.Empty;
            }
            else
            {

            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string bankn = bankname.Text.Trim();
            string Branch=branchname.Text.Trim();
            string ifscco = ifsc.Text.Trim();
            string acc=accountnumber.Text.Trim();
            string holder=holdername.Text.Trim(); 
            if (string.IsNullOrEmpty(bankn))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Bank name is required.');", true);
                return;
            }
            if (string.IsNullOrEmpty(Branch))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Branch name is required.');", true);
                return;
            }
            if (string.IsNullOrEmpty(ifscco))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('IFSC Code is required.');", true);
                return;
            }
            if (string.IsNullOrEmpty(acc))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Account Number is required.');", true);
                return;
            }
            if (string.IsNullOrEmpty(holder))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Account Holder name is required.');", true);
                return;
            }
            if (btnSubmit.Text == "Submit")
            {
                SqlParameter[] insertParams = new SqlParameter[]
                     {
                        new SqlParameter("@sp_type", 1),
                        new SqlParameter("@bankname",bankn),
                        new SqlParameter("@branch", Branch),
                        new SqlParameter("@accountholder",holder ),
                        new SqlParameter("@ifsc", ifscco),
                         new SqlParameter("@account", acc)

                    };

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_bankmaster", insertParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Item saved successfully');", true);
                    loaddetails();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error saving Item');", true);
                }
            }
            else
            {
                SqlParameter[] insertParams = new SqlParameter[]
                     {
                        new SqlParameter("@sp_type", 2),
                        new SqlParameter("@bankname",bankn),
                        new SqlParameter("@branch", Branch),
                        new SqlParameter("@accountholder",holder ),
                        new SqlParameter("@ifsc", ifscco),
                        new SqlParameter("@account", acc),
                        new SqlParameter("@Id", Session["IDS"].ToString())

                    };

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_bankmaster", insertParams);

                if (result != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Updated successfully');", true);
                    loaddetails();
                }
            }
        }
    }
}