using FeelYourFood_Admin.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Controls.Primitives;

namespace FeelYourFood
{
    public partial class Subscription : System.Web.UI.Page
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
                    if (Session["TrnId"] != null)
                    {
                        string trnId = Session["TrnId"].ToString();
                        string query = "SELECT TOP 1 Id, Status FROM UPITransaction WHERE TransactionId = @TrnId ORDER BY Id DESC";

                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@TrnId", trnId);
                                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                                {
                                    DataSet ds = new DataSet();
                                    da.Fill(ds);
                                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        string status = ds.Tables[0].Rows[0]["Status"].ToString();
                                        if (status == "PAID")
                                        {
                                            paymentdone();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    getSubscriptionDetails();
                }
                else
                {
                    // Optionally redirect to login or show an error
                    Response.Redirect("LoginForm.aspx");
                }
            }
        }


        private void getSubscriptionDetails()
        {
            string restId = Session["restid"]?.ToString();
            string query = "SELECT TOP 1 * FROM RestPaymentRecords WHERE RestId = @RestId ORDER BY Id DESC";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@RestId", restId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Response.Redirect("NewSubscription.aspx");
                        return;
                    }

                    reader.Read();
                    double payableAmount = reader["payableamount"] != DBNull.Value ? Convert.ToDouble(reader["payableamount"]) : 0.0;
                    double paymentDone = reader["paymentdone"] != DBNull.Value ? Convert.ToDouble(reader["paymentdone"]) : 0.0;
                    string paymentType = reader["PaymentType"] != DBNull.Value ? reader["PaymentType"].ToString() : string.Empty;


                    if (payableAmount == paymentDone)
                    {
                        if (paymentType == "OneTime")
                        {
                            RedirectBasedOnUploadStatus(restId);
                        }
                        else
                        {
                            Response.Redirect("RenewSubscription.aspx");
                        }
                    }
                    else if (payableAmount > paymentDone)
                    {
                        if (paymentType == "OneTime")
                        {
                            Response.Redirect("Waiting.aspx");
                        }
                        else
                        {
                            Response.Redirect("EMISubscription.aspx");
                        }
                    }
                }
            }
        }

        private void RedirectBasedOnUploadStatus(string restId)
        {
            string query = "SELECT TOP 1 ActiveStatus FROM UploadDocument WHERE RestId = @RestId ORDER BY Id DESC";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@RestId", restId);
                conn.Open();

                object result = cmd.ExecuteScalar();
                bool isActive = result != null && Convert.ToBoolean(result);

                if (!isActive)
                {
                    Response.Redirect("Waiting.aspx");
                }
                else
                {
                    Response.Redirect("RenewSubscription.aspx");
                }
            }
        }
        private void paymentdone()
        {

            int result1 = 0;
            string query = " select * from RestPaymentRecords where RestId='" + Session["restid"].ToString() + "' order by Id Desc ";
            _c1.Retrive2(query, ref ds);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                double totalamount = Convert.ToDouble(ds.Tables[0].Rows[0]["payableamount"]);
                double paymentdone = Convert.ToDouble(ds.Tables[0].Rows[0]["paymentdone"]);
                DateTime act= Convert.ToDateTime(ds.Tables[0].Rows[0]["ActivationDate"]);
                DateTime exp = Convert.ToDateTime(ds.Tables[0].Rows[0]["ExpiryDate"]);
                if (totalamount == paymentdone)
                {
                    DateTime previousexpiry = Convert.ToDateTime(ds.Tables[0].Rows[0]["ExpiryDate"]);
                    string plan = Session["SubsType"]?.ToString()?.Trim();

                    DateTime newexpdate = previousexpiry;

                    switch (plan)
                    {
                        case "Quarterly":
                            newexpdate = previousexpiry.AddMonths(3);
                            break;
                        case "HalfYearly":
                            newexpdate = previousexpiry.AddMonths(6);
                            break;
                        case "Yearly":
                            newexpdate = previousexpiry.AddMonths(12);
                            break;
                    }

                    SqlParameter[] insertParams1 = new SqlParameter[]
                    {
                            new SqlParameter("@sp_type", 1),
                            new SqlParameter("@RestId", Session["restid"]?.ToString()?.Trim()),
                            new SqlParameter("@PaymentType", "Subscription"),
                            new SqlParameter("@payableamount", Session["DueAmount"]?.ToString()?.Trim()),
                            new SqlParameter("@paymentdone", Session["DueAmount"]?.ToString()?.Trim()),
                            new SqlParameter("@lastpayment", DateTime.Now),
                            new SqlParameter("@ActivationDate", previousexpiry),
                            new SqlParameter("@ExpiryDate", newexpdate)
                    };
                    result1 = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_RestPayment", insertParams1);
                    storeBillingnew(Convert.ToInt32(Session["restid"].ToString()));

                }
                else
                {
                    string query1 = "SELECT * FROM EmiDetails WHERE RestId = '" + Session["restid"].ToString() + "' ORDER BY Id DESC";
                    _c1.Retrive2(query1, ref ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int restId = Convert.ToInt32(Session["restid"]);
                        double totalpayable = Convert.ToDouble(ds.Tables[0].Rows[0]["totalamount"]);
                        string subst = ds.Tables[0].Rows[0]["subscriptiontype"].ToString();
                        decimal emi= Convert.ToDecimal(ds.Tables[0].Rows[0]["emiamount"]);
                        double totalpay = Convert.ToDouble(ds.Tables[0].Rows[0]["paidamount"]) + Convert.ToDouble(Session["DueAmount"]);
                        double remaining = totalpayable - totalpay;

                        DateTime previousexpiry = Convert.ToDateTime(ds.Tables[0].Rows[0]["nextpaymentdate"]);
                        DateTime newexpdate = previousexpiry;

                        switch (subst.Trim())
                        {
                            case "Quarterly":
                                newexpdate = previousexpiry.AddMonths(3);
                                break;
                            case "HalfYearly":
                                newexpdate = previousexpiry.AddMonths(6);
                                break;
                            case "Yearly":
                                newexpdate = previousexpiry.AddMonths(12);
                                break;
                        }
                        SqlParameter[] insertParams1 = new SqlParameter[]
                        {
                                 new SqlParameter("@sp_type", 1),
                                 new SqlParameter("@RestId", Session["restid"].ToString()),
                                 new SqlParameter("@PaymentType", "EMI"),
                                 new SqlParameter("@payableamount",totalpayable),
                                 new SqlParameter("@paymentdone",totalpay),
                                 new SqlParameter("@lastpayment ",DateTime.Now),
                                 new SqlParameter("@ActivationDate ",act),
                                 new SqlParameter("@ExpiryDate ",exp)
                        };
                        result1 = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_RestPayment", insertParams1);
                        InsertEmiDetails(restId, Convert.ToDecimal(totalpayable), subst, emi, Convert.ToDecimal(totalpay), Convert.ToDecimal(remaining), DateTime.Now, newexpdate);
                    }
                }
            }
            else
            {
                DateTime expDate = DateTime.Now.AddYears(1).Date;
                DateTime extendedDate = expDate.AddDays(1);
                SqlParameter[] insertParams1 = new SqlParameter[]
                {
                     new SqlParameter("@sp_type", 1),
                     new SqlParameter("@RestId", Session["restid"].ToString()),
                     new SqlParameter("@PaymentType", "EMI"),
                     new SqlParameter("@payableamount",Session["Payable"].ToString()),
                     new SqlParameter("@paymentdone",Session["DueAmount"].ToString()),
                     new SqlParameter("@lastpayment ",DateTime.Now),
                     new SqlParameter("@ActivationDate ",DateTime.Now),
                     new SqlParameter("@ExpiryDate ",extendedDate)
                };
                result1 = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_RestPayment", insertParams1);

                int result2 = 0;
                SqlParameter[] prms = new SqlParameter[]
                {
                            new SqlParameter("@sp_type", 8),
                            new SqlParameter("@RestId",Session["restid"].ToString())
                };
                result2 = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_ResturantAndAdminMaster", prms);

                int result3 = 0;
                SqlParameter[] prms1 = new SqlParameter[]
                {
                            new SqlParameter("@sp_type", 7),
                            new SqlParameter("@RestId",Session["restid"].ToString() )
                };
                result3 = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_ChooseDevice", prms1);
                storeBilling(Convert.ToInt32(Session["restid"].ToString()));
                int restId = Convert.ToInt32(Session["restid"]);
                decimal totalAmount = Convert.ToDecimal(Session["Payable"]);
                decimal paidAmount = Convert.ToDecimal(Session["DueAmount"]);
                decimal remaining = totalAmount - paidAmount;
                decimal emiamount= Convert.ToDecimal(Session["EmiAmount"]);
                string subscriptionType = Session["SelectedPlan"]?.ToString()?.Trim();

                DateTime previousexpiry = DateTime.Now;
                DateTime newexpdate = previousexpiry;

                switch (subscriptionType)
                {
                    case "Quarterly":
                        newexpdate = previousexpiry.AddMonths(3);
                        break;
                    case "HalfYearly":
                        newexpdate = previousexpiry.AddMonths(6);
                        break;
                    case "Yearly":
                        newexpdate = previousexpiry.AddMonths(12);
                        break;
                }

                InsertEmiDetails(restId, totalAmount, subscriptionType,emiamount, paidAmount, remaining, DateTime.Now, newexpdate);

            }

        }
        private void storeBilling(int restId)
        {
            List<dynamic> devices = new List<dynamic>();

            string constr = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
            using (SqlConnection con1 = new SqlConnection(constr))
            {
                con1.Open();

                decimal deviceDiscountPercent = 0;
                decimal subscriptionDiscountPercent = 0;
                decimal deviceGSTPercent = 0;
                decimal softwareGSTPercent = 0;

                // Fetch discount percentages
                string discountQuery = "SELECT TOP 1 Devicediscount, Subscriptiondiscount FROM SubscriptonDiscount WHERE ActiveStatus = 1 and DiscountName!='Joining Discount' ORDER BY Id ASC";
                using (SqlCommand discountCmd = new SqlCommand(discountQuery, con1))
                using (SqlDataReader reader = discountCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader["Devicediscount"] != DBNull.Value)
                            deviceDiscountPercent = Convert.ToDecimal(reader["Devicediscount"]);
                        if (reader["Subscriptiondiscount"] != DBNull.Value)
                            subscriptionDiscountPercent = Convert.ToDecimal(reader["Subscriptiondiscount"]);
                    }
                }

                // Fetch GST percentages
                string gstQuery = "SELECT TOP 1 devicegst, softwaregst FROM DeviceGST WHERE ActiveStatus = 1 ORDER BY Id DESC";
                using (SqlCommand gstCmd = new SqlCommand(gstQuery, con1))
                using (SqlDataReader reader = gstCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader["devicegst"] != DBNull.Value)
                            deviceGSTPercent = Convert.ToDecimal(reader["devicegst"]);
                        if (reader["softwaregst"] != DBNull.Value)
                            softwareGSTPercent = Convert.ToDecimal(reader["softwaregst"]);
                    }
                }

                // Fetch device & subscription data
                string query = @"
        SELECT * FROM (
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
                    cmd.Parameters.AddWithValue("@RestId", restId);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int qty = Convert.ToInt32(rdr["Quantity"]);
                            decimal devPrice = rdr["DevicePrice"] != DBNull.Value ? Convert.ToDecimal(rdr["DevicePrice"]) : 0;
                            decimal subPrice = rdr["SubscriptionPrice"] != DBNull.Value ? Convert.ToDecimal(rdr["SubscriptionPrice"]) : 0;

                            devices.Add(new
                            {
                                DeviceName = rdr["DeviceName"].ToString(),
                                Quantity = qty,
                                DevPrice = devPrice,
                                SubPrice = subPrice,
                                DeviceTotal = qty * devPrice,
                                SubTotal = qty * subPrice
                            });
                        }
                    }
                }

                if (devices.Count == 0) return;

                // Totals for the restaurant
                int totalQty = devices.Sum(d => (int)d.Quantity);
                decimal totalDevPrice = devices.Sum(d => (decimal)d.DeviceTotal);
                decimal totalSubPrice = devices.Sum(d => (decimal)d.SubTotal);

                decimal devDiscountAmt = (totalDevPrice * deviceDiscountPercent) / 100;
                decimal devTaxable = totalDevPrice - devDiscountAmt;
                decimal devGSTAmt = (devTaxable * deviceGSTPercent) / 100;
                decimal devPayable = devTaxable + devGSTAmt;

                decimal subDiscountAmt = (totalSubPrice * subscriptionDiscountPercent) / 100;
                decimal subTaxable = totalSubPrice - subDiscountAmt;
                decimal subGSTAmt = (subTaxable * softwareGSTPercent) / 100;
                decimal subPayable = subTaxable + subGSTAmt;

                decimal totalPayable = devPayable + subPayable;
                // Insert row per device
                foreach (var device in devices)
                {
                    using (SqlCommand insertCmd = new SqlCommand("sp_InsertBilling", con1))
                    {
                        insertCmd.CommandType = CommandType.StoredProcedure;

                        // Include sp_type = 1
                        insertCmd.Parameters.AddWithValue("@sp_type", 1);

                        insertCmd.Parameters.AddWithValue("@RestId", restId);
                        insertCmd.Parameters.AddWithValue("@Devices", device.DeviceName);
                        insertCmd.Parameters.AddWithValue("@Price", device.DevPrice);
                        insertCmd.Parameters.AddWithValue("@Quantity", device.Quantity);
                        insertCmd.Parameters.AddWithValue("@TotalDevicePrice", device.DeviceTotal);
                        insertCmd.Parameters.AddWithValue("@DiscountAmount", devDiscountAmt);
                        insertCmd.Parameters.AddWithValue("@TaxableAmount", devTaxable);
                        insertCmd.Parameters.AddWithValue("@GSTAmount", devGSTAmt);
                        insertCmd.Parameters.AddWithValue("@DevicePayable", devPayable);
                        insertCmd.Parameters.AddWithValue("@SubscriptionType", "Yearly");
                        insertCmd.Parameters.AddWithValue("@SubscriptionAmount", device.SubTotal);
                        insertCmd.Parameters.AddWithValue("@totalsubscriptionamount", totalSubPrice);
                        insertCmd.Parameters.AddWithValue("@SubDiscount", subDiscountAmt);
                        insertCmd.Parameters.AddWithValue("@SubTaxable", subTaxable);
                        insertCmd.Parameters.AddWithValue("@SubGST", subGSTAmt);
                        insertCmd.Parameters.AddWithValue("@SubscriptionPayable", subPayable);
                        insertCmd.Parameters.AddWithValue("@TotalPayable", totalPayable);

                        insertCmd.ExecuteNonQuery();
                    }
                }

            }
        }

        private void storeBillingnew(int restId)
        {
            string constr = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;

            using (SqlConnection con1 = new SqlConnection(constr))
            {
                con1.Open();

                decimal deviceDiscountPercent = 0;
                decimal subscriptionDiscountPercent = 0;
                decimal deviceGSTPercent = 0;
                decimal softwareGSTPercent = 0;

                // Fetch discount percentages
                string discountQuery = "SELECT TOP 1 Devicediscount, Subscriptiondiscount FROM SubscriptonDiscount WHERE ActiveStatus = 1 and DiscountName!='Joining Discount' ORDER BY Id ASC";
                using (SqlCommand discountCmd = new SqlCommand(discountQuery, con1))
                using (SqlDataReader reader = discountCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader["Devicediscount"] != DBNull.Value)
                            deviceDiscountPercent = Convert.ToDecimal(reader["Devicediscount"]);
                        if (reader["Subscriptiondiscount"] != DBNull.Value)
                            subscriptionDiscountPercent = Convert.ToDecimal(reader["Subscriptiondiscount"]);
                    }
                }

                // Fetch GST percentages
                string gstQuery = "SELECT TOP 1 devicegst, softwaregst FROM DeviceGST WHERE ActiveStatus = 1 ORDER BY Id DESC";
                using (SqlCommand gstCmd = new SqlCommand(gstQuery, con1))
                using (SqlDataReader reader = gstCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader["devicegst"] != DBNull.Value)
                            deviceGSTPercent = Convert.ToDecimal(reader["devicegst"]);
                        if (reader["softwaregst"] != DBNull.Value)
                            softwareGSTPercent = Convert.ToDecimal(reader["softwaregst"]);
                    }
                }

                // Fetch device & subscription data
                List<dynamic> devices = new List<dynamic>();

                string query = @"
        SELECT * FROM (
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
                    cmd.Parameters.AddWithValue("@RestId", restId);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int qty = Convert.ToInt32(rdr["Quantity"]);
                            decimal devPrice = rdr["DevicePrice"] != DBNull.Value ? Convert.ToDecimal(rdr["DevicePrice"]) : 0;
                            decimal subPrice = rdr["SubscriptionPrice"] != DBNull.Value ? Convert.ToDecimal(rdr["SubscriptionPrice"]) : 0;

                            devices.Add(new
                            {
                                DeviceName = rdr["DeviceName"].ToString(),
                                Quantity = qty,
                                DevPrice = devPrice,
                                SubPrice = subPrice,
                                DeviceTotal = qty * devPrice,
                                SubTotal = qty * subPrice
                            });
                        }
                    }
                }

                if (devices.Count == 0) return;

                decimal totalSubPrice = devices.Sum(d => (decimal)d.SubTotal);

                decimal subDiscountAmt = (totalSubPrice * subscriptionDiscountPercent) / 100;
                decimal subTaxable = totalSubPrice - subDiscountAmt;
                decimal subGSTAmt = (subTaxable * softwareGSTPercent) / 100;
                decimal subPayable = subTaxable + subGSTAmt;

                string subtypes = Session["SubsType"]?.ToString();

                decimal totalPayable =subPayable;

                // Insert summary billing record using sp_type = 2
                using (SqlCommand insertCmd = new SqlCommand("sp_InsertBilling", con1))
                {
                    insertCmd.CommandType = CommandType.StoredProcedure;
                    insertCmd.Parameters.AddWithValue("@sp_type", 2);
                    insertCmd.Parameters.AddWithValue("@RestId", restId);
                    insertCmd.Parameters.AddWithValue("@SubscriptionType", subtypes);
                    insertCmd.Parameters.AddWithValue("@SubscriptionAmount", totalSubPrice);            // total before discount
                    insertCmd.Parameters.AddWithValue("@totalsubscriptionamount", totalSubPrice);
                    insertCmd.Parameters.AddWithValue("@SubDiscount", subDiscountAmt);
                    insertCmd.Parameters.AddWithValue("@SubTaxable", subTaxable);
                    insertCmd.Parameters.AddWithValue("@SubGST", subGSTAmt);
                    insertCmd.Parameters.AddWithValue("@SubscriptionPayable", subPayable);
                    insertCmd.Parameters.AddWithValue("@TotalPayable", totalPayable);
                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        private void InsertEmiDetails(int restId, decimal totalAmount, string subscriptionType, decimal emiamount, decimal paidAmount, decimal remainingAmount, DateTime lastPaymentDate, DateTime nextPaymentDate)
        {
            string constr = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("sp_EmiDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@sp_type", 1);
                    cmd.Parameters.AddWithValue("@RestId", restId);
                    cmd.Parameters.AddWithValue("@totalamount", totalAmount);
                    cmd.Parameters.AddWithValue("@subscriptiontype", subscriptionType);
                    cmd.Parameters.AddWithValue("@emiamount", emiamount);
                    cmd.Parameters.AddWithValue("@paidamount", paidAmount);
                    cmd.Parameters.AddWithValue("@remainingamount", remainingAmount);
                    cmd.Parameters.AddWithValue("@lastpaymentdate", lastPaymentDate);
                    cmd.Parameters.AddWithValue("@nextpaymentdate", nextPaymentDate);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}