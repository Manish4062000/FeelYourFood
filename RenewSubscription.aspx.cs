using FeelYourFood_Admin.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Media.Media3D;

namespace FeelYourFood
{
    public partial class RenewSubscription : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["restid"] != null)
                {
                    int restId = Convert.ToInt32(Session["restid"]);
                    hfRestId.Value = restId.ToString();
                    loadPaymentHistory(restId);
                    getdetails();
                }
            }
        }

        protected void getdetails()
        {
            int restId = Convert.ToInt32(Session["restid"]);

            string query = "SELECT TOP 1 subscriptiontype FROM Billing WHERE RestId = @RestId ORDER BY Id DESC";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@RestId", restId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string subscriptionTypeText = reader["subscriptiontype"] != DBNull.Value ? reader["subscriptiontype"].ToString() : "N/A";
                        subscriptionType.InnerText = subscriptionTypeText;
                        
                    }
                    else
                    {
                        subscriptionType.InnerText = "N/A";
                        
                    }
                }
            }
            string query1 = "SELECT TOP 1 ExpiryDate FROM RestPaymentRecords WHERE RestId = @RestId ORDER BY Id DESC";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query1, conn))
            {
                cmd.Parameters.AddWithValue("@RestId", restId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string expirytxt = reader["ExpiryDate"] != DBNull.Value ? reader["ExpiryDate"].ToString() : "N/A";
                        expiryDate.InnerText = expirytxt;

                    }
                    else
                    {
                        expiryDate.InnerText = "N/A";

                    }
                }
            }

        }
        [WebMethod]
        public static void StoreDueAmount(decimal amount, string emiType)
        {
            HttpContext.Current.Session["DueAmount"] = amount;
            HttpContext.Current.Session["SubsType"] = emiType;
        }

        [WebMethod]
        public static object showdevicedata(string restid)
        {
            List<object> devices = new List<object>();
            string constr = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;

            decimal subscriptionDiscount = 0;
            decimal softwareGST = 0;

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                // 1. Fetch discount from SubscriptonDiscount table
                string discountQuery = "SELECT TOP 1 Subscriptiondiscount FROM SubscriptonDiscount WHERE ActiveStatus = 1 and DiscountName!='Joining Discount' ORDER BY Id DESC";
                using (SqlCommand discountCmd = new SqlCommand(discountQuery, con))
                {
                    object discountObj = discountCmd.ExecuteScalar();
                    if (discountObj != DBNull.Value && discountObj != null)
                    {
                        subscriptionDiscount = Convert.ToDecimal(discountObj);
                    }
                }

                // 2. Fetch GST from DeviceGST table
                string gstQuery = "SELECT TOP 1 softwaregst FROM DeviceGST WHERE ActiveStatus = 1 ORDER BY Id DESC";
                using (SqlCommand gstCmd = new SqlCommand(gstQuery, con))
                {
                    object gstObj = gstCmd.ExecuteScalar();
                    if (gstObj != DBNull.Value && gstObj != null)
                    {
                        softwareGST = Convert.ToDecimal(gstObj);
                    }
                }

                // 3. Fetch devices and quantities
                string query = @"
                       SELECT *
            FROM (
                SELECT 
                    ds.DeviceId,
                    dm.DeviceName,
                    ds.SubscriptionPrice,
                    CASE dm.DeviceName
                        WHEN 'Ordering Kiosk' THEN CAST(cd.OrderingKiosk AS VARCHAR)
                        WHEN 'Kitchen Display' THEN CAST(cd.KitchenDisplay AS VARCHAR)
                        WHEN 'QMS (Order Display)' THEN CAST(cd.qms AS VARCHAR)
                        WHEN 'Table Tablet' THEN CAST(cd.tabletab AS VARCHAR)
                        WHEN 'Desktop Server' THEN CASE WHEN cd.Server = 'desktopserver' THEN '1' ELSE NULL END
                        WHEN 'Android Server' THEN CASE WHEN cd.Server = 'androidserver' THEN '1' ELSE NULL END
                        ELSE NULL
                    END AS Quantity
                FROM [RMS].[dbo].[DeviceAndSubscriptionMaster] ds
                INNER JOIN [RMS].[dbo].[DeviceMaster] dm ON ds.DeviceId = dm.Id
                LEFT JOIN [RMS].[dbo].[ChooseDevice] cd ON cd.RestId = @RestId
            ) AS result
            WHERE Quantity IS NOT NULL AND Quantity != '0'

        ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RestId", restid);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int quantity = Convert.ToInt32(rdr["Quantity"]);
                            decimal baseSubPrice = rdr["SubscriptionPrice"] != DBNull.Value ? Convert.ToDecimal(rdr["SubscriptionPrice"]) : 0;
                            decimal subscriptionPrice = quantity * baseSubPrice;

                            devices.Add(new
                            {
                                DeviceName = rdr["DeviceName"].ToString(),
                                Quantity = quantity,
                                SubscriptionPrice = subscriptionPrice,
                                BaseSubscriptionRate = baseSubPrice,
                                SubscriptionDiscount = subscriptionDiscount,
                                SoftwareGST = softwareGST
                            });
                        }
                    }
                }
            }

            return devices;
        }

        private void loadPaymentHistory(int restid)
        {
            string constr = System.Configuration.ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = @"SELECT Id, RestId, PaymentType, payableamount, paymentdone, lastpayment, ActivationDate, ExpiryDate
                         FROM RestPaymentRecords
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
                            row.Cells.Add(new TableCell { Text = reader["PaymentType"]?.ToString() ?? "-" });
                            row.Cells.Add(new TableCell { Text = reader["payableamount"] != DBNull.Value ? Convert.ToDecimal(reader["payableamount"]).ToString("N0") : "-" });
                            row.Cells.Add(new TableCell { Text = reader["paymentdone"] != DBNull.Value ? Convert.ToDecimal(reader["paymentdone"]).ToString("N0") : "-" });
                            row.Cells.Add(new TableCell { Text = reader["lastpayment"] != DBNull.Value ? Convert.ToDateTime(reader["lastpayment"]).ToString("dd MMM yyyy") : "-" });
                            row.Cells.Add(new TableCell { Text = reader["ActivationDate"] != DBNull.Value ? Convert.ToDateTime(reader["ActivationDate"]).ToString("dd MMM yyyy") : "-" });
                            row.Cells.Add(new TableCell { Text = reader["ExpiryDate"] != DBNull.Value ? Convert.ToDateTime(reader["ExpiryDate"]).ToString("dd MMM yyyy") : "-" });

                            paymentTableBody.Controls.Add(row);
                            slno++;
                        }

                        if (!hasData)
                        {
                            TableRow emptyRow = new TableRow();
                            TableCell emptyCell = new TableCell
                            {
                                ColumnSpan = 7, // Adjust based on number of columns
                                HorizontalAlign = HorizontalAlign.Center,
                                Text = "No payment records found."
                            };
                            emptyRow.Cells.Add(emptyCell);
                            paymentTableBody.Controls.Add(emptyRow);
                        }
                    }
                }
            }
        }

    }
}