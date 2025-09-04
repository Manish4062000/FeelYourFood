using FeelYourFood_Admin.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeelYourFood
{
    public partial class UploadDocuments : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        public readonly Connection _c1 = new Connection();
        public DataTable _dtt = new DataTable();
        public DataSet ds = new DataSet();
        string filePath = string.Empty;
        protected string dueamounttopay = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["restid"] != null)
                {
                    if (Session["Payable"] != null)
                    {
                        dueamounttopay = Session["Payable"].ToString();
                    }

                    loaddetails();
                }
            }
        }
        private void loaddetails()
        {
            string query = "SELECT Id, bankname, branch, accountholder, ifsc, account FROM bankmaster";
            _c1.Retrive2(query, ref ds);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                DataRow row = ds.Tables[0].Rows[0];

                bankname.Text = !string.IsNullOrWhiteSpace(row["bankname"]?.ToString()) ? row["bankname"].ToString() : string.Empty;
                branch.Text = !string.IsNullOrWhiteSpace(row["branch"]?.ToString()) ? row["branch"].ToString() : string.Empty;
                accountno.Text = !string.IsNullOrWhiteSpace(row["account"]?.ToString()) ? row["account"].ToString() : string.Empty;
                ifsc.Text = !string.IsNullOrWhiteSpace(row["ifsc"]?.ToString()) ? row["ifsc"].ToString() : string.Empty;
                holdername.Text = !string.IsNullOrWhiteSpace(row["accountholder"]?.ToString()) ? row["accountholder"].ToString() : string.Empty;
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string transit = transaction.Text.Trim();

            if (string.IsNullOrEmpty(transit))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Transaction number is required.');", true);
                return;
            }
            if (myFile.HasFile)
            {
                string virtualFolder = "IMAGE/Files/";
                string physicalFolder = Server.MapPath(virtualFolder);
                string fileName = Session["adminname"] + "_" + myFile.FileName;
                myFile.SaveAs(Path.Combine(physicalFolder, fileName));
                filePath = "~/IMAGE/Files/" + fileName;

                SqlParameter[] insertParams = new SqlParameter[]
                 {
                     new SqlParameter("@sp_type", 1),
                     new SqlParameter("@RestId", Session["restid"].ToString()),
                     new SqlParameter("@TransactionId", transit),
                     new SqlParameter("@payableamount",Session["Payable"]),
                     new SqlParameter("@UploadedFile",filePath ),
                     new SqlParameter("@ActiveStatus", "0")
                };

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_UploadDocument", insertParams);
                double paydone = 0;
                SqlParameter[] insertParams1 = new SqlParameter[]
                 {
                     new SqlParameter("@sp_type", 1),
                     new SqlParameter("@RestId", Session["restid"].ToString()),
                     new SqlParameter("@PaymentType", "OneTime"),
                     new SqlParameter("@payableamount",Session["Payable"].ToString()),
                     new SqlParameter("@paymentdone",paydone),
                     new SqlParameter("@lastpayment ",null)
                };

                int result1 = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_RestPayment", insertParams1);

                if (result != 0 && result1!= 0)
                {
                    string script = "alert('Saved successfully'); window.location='Waiting.aspx';";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertRedirect", script, true);
                }

            }
        }

    }
}