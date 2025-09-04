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
    public partial class M_VerifySubscription : System.Web.UI.Page
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
                    BindVerifyGrid();
                }
            }

        }

        private void BindVerifyGrid()
        {
            string query = "SELECT u.Id, u.RestId, u.TransactionId, u.UploadedFile, u.ActiveStatus, u.payableamount, r.ResturantName, r.RestAddress, r.Name, r.MobileNo, u.Date " +
                           "FROM UploadDocument u " +
                           "INNER JOIN Resturant_And_AdminMaster r ON u.RestId = r.RestId " +
                           "ORDER BY u.Id DESC";
            DataSet ds = new DataSet();
            _c1.Retrive2(query, ref ds);
            if (ds != null && ds.Tables.Count > 0)
            {
                gvverify.DataSource = ds.Tables[0];
                gvverify.DataBind();
            }
        }


        protected void gvverify_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void btnverify_Command(object sender, CommandEventArgs e)
        {
            int restId = Convert.ToInt32(e.CommandArgument);
            string action = e.CommandName;
            int result2 = 0;
            int spType = (action == "deactivate") ? 9 : 8;
            if (spType == 8)
            {
               storeBilling(restId);
                SqlParameter[] prms = new SqlParameter[]
                   {
                            new SqlParameter("@sp_type", spType),
                            new SqlParameter("@RestId",restId )
                   };
                result2 = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_ResturantAndAdminMaster", prms);
            }
            else if (spType == 9)
            {
                SqlParameter[] prms = new SqlParameter[]
                   {
                            new SqlParameter("@sp_type", spType),
                            new SqlParameter("@RestId",restId )
                   };
                result2 = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_ResturantAndAdminMaster", prms);
            }
            int result3 = 0;
            spType = (action == "deactivate") ? 8 : 7;
            if (spType == 7)
            {
                SqlParameter[] prms = new SqlParameter[]
                   {
                            new SqlParameter("@sp_type", spType),
                            new SqlParameter("@RestId",restId )
                   };
                result3 = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_ChooseDevice", prms);
            }
            else if (spType == 8)
            {
                SqlParameter[] prms = new SqlParameter[]
                   {
                            new SqlParameter("@sp_type", spType),
                            new SqlParameter("@RestId",restId )
                   };
                result3 = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_ChooseDevice", prms);
            }

            string query1 = $"SELECT TOP 1 * FROM RestPaymentRecords WHERE RestId = {restId} ORDER BY Id DESC";
            SqlDataAdapter da1 = new SqlDataAdapter(query1, _c1.con);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            int result1 = 0;
            if (dt1.Rows.Count > 0)
            {
                ViewState["ids"] = dt1.Rows[0]["Id"];
                ViewState["paymenttype"] = dt1.Rows[0]["PaymentType"];
                ViewState["payableamount"] = Convert.ToDecimal(dt1.Rows[0]["payableamount"]);

                spType = (action == "deactivate") ? 4 : 2;
                if (spType == 2)
                {
                    DateTime expDate = DateTime.Now.AddYears(1).Date;
                    DateTime extendedDate = expDate.AddDays(1);
                    SqlParameter[] prms = new SqlParameter[]
                       {
                            new SqlParameter("@sp_type", spType),
                            new SqlParameter("@PaymentType",ViewState["paymenttype"].ToString() ),
                            new SqlParameter("@paymentdone",ViewState["payableamount"].ToString() ),
                            new SqlParameter("@lastpayment",DateTime.Now),
                            new SqlParameter("@ActivationDate",DateTime.Now),
                            new SqlParameter("@ExpiryDate",extendedDate),
                            new SqlParameter("@Id",ViewState["ids"].ToString() )
                       };
                    result1 = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_RestPayment", prms);
                }
                else if (spType == 4)
                {
                    SqlParameter[] prms = new SqlParameter[]
                       {
                            new SqlParameter("@sp_type", spType),
                            new SqlParameter("@PaymentType",ViewState["paymenttype"].ToString() ),
                            new SqlParameter("@paymentdone",null ),
                            new SqlParameter("@lastpayment",null),
                            new SqlParameter("@ActivationDate",null),
                            new SqlParameter("@ExpiryDate",null),
                            new SqlParameter("@Id",ViewState["ids"].ToString() )
                       };
                    result1 = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_RestPayment", prms);
                }
            }

            string query = $"SELECT TOP 1 Id FROM UploadDocument WHERE RestId = {restId} ORDER BY Id DESC";
            SqlDataAdapter da = new SqlDataAdapter(query, _c1.con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                int documentId = Convert.ToInt32(dt.Rows[0]["Id"]);
                spType = (action == "deactivate") ? 4 : 3;

                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@sp_type", spType),
                    new SqlParameter("@Id", documentId)
                };

                int result = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_UploadDocument", prms);

                if (result != 0 && result1 != 0 && result2 != 0)
                {
                    string msg = action == "deactivate" ? "Deactivated successfully." : "Activated successfully.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", $"alert('{msg}'); window.location.href = window.location.pathname;", true);
                }
            }

            BindVerifyGrid();
            
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
                string discountQuery = "SELECT TOP 1 Devicediscount, Subscriptiondiscount FROM SubscriptonDiscount WHERE ActiveStatus = 1 ORDER BY Id ASC";
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

    }
}