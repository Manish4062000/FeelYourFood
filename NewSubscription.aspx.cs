using FeelYourFood_Admin.AppCode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeelYourFood
{
    public partial class NewSubscription : System.Web.UI.Page
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
                    hfRestId.Value = Session["restid"].ToString();
                }
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void StorePayableAmount(decimal amount)
        {
            HttpContext.Current.Session["Payable"] = amount;
        } 
        [WebMethod]
        public static object showdevicedata(string restid)
        {
            List<object> devices = new List<object>();

            string constr = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;

            using (SqlConnection con1 = new SqlConnection(constr))
            {
                con1.Open();

                decimal deviceDiscount = 0;
                decimal subscriptionDiscount = 0;
                decimal deviceGST = 0;
                decimal softwareGST = 0;

                // Fetch discounts
                string discountQuery = "SELECT TOP 1 Devicediscount, Subscriptiondiscount FROM SubscriptonDiscount WHERE ActiveStatus = 1 ORDER BY Id ASC";
                using (SqlCommand discountCmd = new SqlCommand(discountQuery, con1))
                {
                    using (SqlDataReader reader = discountCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["Devicediscount"] != DBNull.Value)
                                deviceDiscount = Convert.ToDecimal(reader["Devicediscount"]);
                            if (reader["Subscriptiondiscount"] != DBNull.Value)
                                subscriptionDiscount = Convert.ToDecimal(reader["Subscriptiondiscount"]);
                        }
                    }
                }

                // Fetch GST values
                string gstQuery = "SELECT TOP 1 devicegst, softwaregst FROM DeviceGST WHERE ActiveStatus = 1 ORDER BY Id DESC";
                using (SqlCommand gstCmd = new SqlCommand(gstQuery, con1))
                {
                    using (SqlDataReader reader = gstCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["devicegst"] != DBNull.Value)
                                deviceGST = Convert.ToDecimal(reader["devicegst"]);
                            if (reader["softwaregst"] != DBNull.Value)
                                softwareGST = Convert.ToDecimal(reader["softwaregst"]);
                        }
                    }
                }

                // Main data fetch
                string query = @"SELECT * FROM (
                            SELECT 
                                ds.DeviceId,
                                dm.DeviceName,
                                ds.DevicePrice,
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
                            FROM DeviceAndSubscriptionMaster ds
                            INNER JOIN DeviceMaster dm ON ds.DeviceId = dm.Id
                            LEFT JOIN ChooseDevice cd ON cd.RestId = @RestId
                        ) AS result
                        WHERE Quantity IS NOT NULL AND Quantity != '0'";

                using (SqlCommand cmd = new SqlCommand(query, con1))
                {
                    cmd.Parameters.AddWithValue("@RestId", restid);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int quantity = Convert.ToInt32(rdr["Quantity"]);
                            decimal baseSubPrice = rdr["SubscriptionPrice"] != DBNull.Value ? Convert.ToDecimal(rdr["SubscriptionPrice"]) : 0;
                            decimal baseDevPrice = rdr["DevicePrice"] != DBNull.Value ? Convert.ToDecimal(rdr["DevicePrice"]) : 0;

                            devices.Add(new
                            {
                                DeviceName = rdr["DeviceName"].ToString(),
                                DevPrice = baseDevPrice,
                                Quantity = quantity,
                                DevicePrice = quantity * baseDevPrice,
                                SubscriptionPrice = quantity * baseSubPrice,
                                DeviceDiscount = deviceDiscount,
                                SubscriptionDiscount = subscriptionDiscount,
                                DeviceGST = deviceGST,
                                SoftwareGST = softwareGST
                            });
                        }
                    }
                }
            }

            return devices;
        }
        protected void btnProceed1_Click(object sender, EventArgs e)
        {
            Response.Redirect("UploadDocuments.aspx");
        }

        static float emiDue = 0;
        static float emiInstallment = 0;
        static float downPayment = 0;
        static string emiType = string.Empty;

        [WebMethod]
        public static void getdata(string emiType, float emiDue, float emiInstallment, float downPayment)
        {
            // Store values in static or session as needed
            NewSubscription.emiType = emiType;
            NewSubscription.emiDue = emiDue;
            NewSubscription.emiInstallment = emiInstallment;
            NewSubscription.downPayment = downPayment;
        }


        protected void Button_Click(object sender, EventArgs e)
        {
            Session["emitype"] = emiType;
            Session["emidue"] = emiDue;
            Session["emiInstallment"] = emiInstallment;
            Session["downpayment"] = downPayment;

            Response.Redirect("EmiInstallment.aspx");
        }
        [WebMethod]
        public static void StoreEMIData(decimal downPayment, string selectedPlan, decimal emiAmount)
        {
            // Store down payment in session
            HttpContext.Current.Session["downpayment"] = downPayment;
            HttpContext.Current.Session["DueAmount"] = downPayment;
            HttpContext.Current.Session["SelectedPlan"] = selectedPlan;
            HttpContext.Current.Session["EmiAmount"] = emiAmount;

        }


    }
}