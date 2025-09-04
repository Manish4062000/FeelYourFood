using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;

namespace FeelYourFood
{
    public partial class EMISubscription : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["restid"] != null)
                {
                    int restid = Convert.ToInt32(Session["restid"].ToString());
                    getAllData(restid);
                    loadEmiHistory(restid);

                }
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void StoreNextInstallment(string nextInstallment)
        {
            HttpContext.Current.Session["DueAmount"] = nextInstallment;
        }


        private void getAllData(int restid)
        {
            string constr = System.Configuration.ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = @"SELECT TOP 1 totalamount, subscriptiontype,emiamount, paidamount, remainingamount, lastpaymentdate, nextpaymentdate 
                         FROM EmiDetails 
                         WHERE RestId = @restid 
                         ORDER BY Id DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@restid", restid);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblTotalAmount.Text = Convert.ToDecimal(reader["totalamount"]).ToString("N0");
                            lblSubscriptionType.Text = reader["subscriptiontype"].ToString();
                            lblPaidAmount.Text = Convert.ToDecimal(reader["paidamount"]).ToString("N0");
                            lblDueAmount.Text = Convert.ToDecimal(reader["remainingamount"]).ToString("N0");
                            lblLastPayment.Text = Convert.ToDateTime(reader["lastpaymentdate"]).ToString("dd MMM yyyy");
                            lblNextInstallment.Text = Convert.ToDecimal(reader["emiamount"]).ToString("N0");
                            lblNextPayment.Text = Convert.ToDateTime(reader["nextpaymentdate"]).ToString("dd MMM yyyy");
                            hfNextInstallment.Value = Convert.ToDecimal(reader["emiamount"]).ToString("N0");
                        }
                    }
                }
            }
        }
        private void loadEmiHistory(int restid)
        {
            string constr = System.Configuration.ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = @"SELECT totalamount, subscriptiontype, emiamount, paidamount, remainingamount, lastpaymentdate, nextpaymentdate
                         FROM EmiDetails 
                         WHERE RestId = @restid 
                         ORDER BY Id DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@restid", restid);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int slno = 1;
                        bool hasData = false;

                        while (reader.Read())
                        {
                            hasData = true;
                            TableRow row = new TableRow();

                            row.Cells.Add(new TableCell { Text = slno.ToString() });
                            row.Cells.Add(new TableCell { Text = reader["totalamount"] != DBNull.Value ? Convert.ToDecimal(reader["totalamount"]).ToString("N0") : "-" });
                            row.Cells.Add(new TableCell { Text = reader["subscriptiontype"]?.ToString() ?? "-" });
                            row.Cells.Add(new TableCell { Text = reader["emiamount"] != DBNull.Value ? Convert.ToDecimal(reader["emiamount"]).ToString("N0") : "-" });
                            row.Cells.Add(new TableCell { Text = reader["paidamount"] != DBNull.Value ? Convert.ToDecimal(reader["paidamount"]).ToString("N0") : "-" });
                            row.Cells.Add(new TableCell { Text = reader["remainingamount"] != DBNull.Value ? Convert.ToDecimal(reader["remainingamount"]).ToString("N0") : "-" });
                            row.Cells.Add(new TableCell { Text = reader["lastpaymentdate"] != DBNull.Value ? Convert.ToDateTime(reader["lastpaymentdate"]).ToString("dd MMM yyyy") : "-" });
                            row.Cells.Add(new TableCell { Text = reader["nextpaymentdate"] != DBNull.Value ? Convert.ToDateTime(reader["nextpaymentdate"]).ToString("dd MMM yyyy") : "-" });

                            emiTableBody.Controls.Add(row);
                            slno++;
                        }

                        if (!hasData)
                        {
                            TableRow emptyRow = new TableRow();
                            TableCell emptyCell = new TableCell
                            {
                                ColumnSpan = 8,
                                HorizontalAlign = HorizontalAlign.Center,
                                Text = "No EMI records found."
                            };
                            emptyRow.Cells.Add(emptyCell);
                            emiTableBody.Controls.Add(emptyRow);
                        }
                    }
                }
            }
        }


    }
}