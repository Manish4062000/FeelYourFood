using FeelYourFood_Admin.AppCode;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Web.Services;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;

namespace FeelYourFood
{
    public partial class EmiInstallment : System.Web.UI.Page
    {
        protected string TrnId = "";
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        public readonly Connection _c1 = new Connection();
        public DataTable _dtt = new DataTable();
        public DataSet ds = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InsertUpiTransaction();

                string amount = "0.00";
                if (Session["DueAmount"] != null)
                {
                    amount = Session["DueAmount"].ToString();
                    DueAmounttopay.InnerText = "₹" + amount;
                }
                else
                {
                    DueAmounttopay.InnerText = "₹0";
                }
            }
        }

        private void InsertUpiTransaction()
        {
            try
            {
                string trnId = "EZ" + DateTime.Now.ToString("yyMMddHHmmssfff");
                Session["TrnId"] = trnId;

                decimal amount = Session["DueAmount"] != null ? Convert.ToDecimal(Session["DueAmount"]) : 0;
                int restId = Session["restId"] != null ? Convert.ToInt32(Session["restId"]) : 0;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_upitransaction", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@sp_type", 1); // Insert
                        cmd.Parameters.AddWithValue("@RestId", restId);
                        cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Today);
                        cmd.Parameters.AddWithValue("@PaymentTime", DateTime.Now);
                        cmd.Parameters.AddWithValue("@TransactionId", trnId);
                        cmd.Parameters.AddWithValue("@TotalAmount", amount);
                        cmd.Parameters.AddWithValue("@PaidAmount", 0);
                        cmd.Parameters.AddWithValue("@ReceiptNumber", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", "PENDING");

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                // You can optionally handle errors or silently fail
            }
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static UpiSettings GetUpiSettings()
        {
            UpiSettings settings = new UpiSettings();
            string constr = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SELECT TOP 1 QRDuration, RP_AppKey, RP_UserName, BaseUrlStatus, BaseUrlGenerate FROM UpiSetting";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            settings.QRDuration = reader["QRDuration"] != DBNull.Value ? Convert.ToInt32(reader["QRDuration"]) : 50;
                            settings.RP_AppKey = reader["RP_AppKey"].ToString();
                            settings.RP_UserName = reader["RP_UserName"].ToString();
                            settings.BaseUrlStatus = reader["BaseUrlStatus"].ToString();
                            settings.BaseUrlGenerate = reader["BaseUrlGenerate"].ToString();
                        }
                    }
                }
            }

            return settings;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GenerateUpiQr()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("GenerateUpiQr called");

                string sessionTrnId = HttpContext.Current.Session["TrnId"]?.ToString() ?? "EZ" + DateTime.Now.ToString("yyMMddHHmmssfff");
                HttpContext.Current.Session["TrnId"] = sessionTrnId;

                string Amount = HttpContext.Current.Session["DueAmount"]?.ToString() ?? "0";

                Amount = Amount.Replace(",", "").Trim();
                
                if (string.IsNullOrEmpty(Amount) || Amount == "0")
                {
                    throw new Exception("Invalid amount. Cannot generate QR for ₹0.");
                }

                UpiSettings upiSettings = GetUpiSettings();
                if (upiSettings == null) throw new Exception("UPI settings not found");

                var payload = new
                {
                    username = upiSettings.RP_UserName,
                    appKey = upiSettings.RP_AppKey,
                    TrnId = sessionTrnId,
                    amount = Amount
                };

                string jsonPayload = JsonConvert.SerializeObject(payload, Formatting.Indented);
                System.Diagnostics.Debug.WriteLine("Payload: " + jsonPayload);

                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(upiSettings.BaseUrlGenerate, content).Result;

                    string result = response.Content.ReadAsStringAsync().Result;
                    System.Diagnostics.Debug.WriteLine("API Response: " + result);

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic json = JsonConvert.DeserializeObject(result);
                        string ezetapTxnId = json?.txnId;
                        string qrCodeUri = json?.qrCodeUri;

                        System.Diagnostics.Debug.WriteLine($"txnId: {ezetapTxnId}");
                        System.Diagnostics.Debug.WriteLine($"qrCodeUri: {qrCodeUri}");

                        if (string.IsNullOrEmpty(ezetapTxnId))
                        {
                            throw new Exception("API did not return txnId");
                        }
                        if (string.IsNullOrEmpty(qrCodeUri))
                        {
                            throw new Exception("API did not return qrCodeUri");
                        }

                        HttpContext.Current.Session["EzetapTxnId"] = ezetapTxnId;
                        return qrCodeUri;
                    }
                    else
                    {
                        throw new Exception($"API call failed: {response.StatusCode} | {result}");
                    }
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in GenerateUpiQr: " + ex.Message);
                return null;
            }
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string CheckPaymentStatus()
        {
            try
            {
                string txnId = HttpContext.Current.Session["EzetapTxnId"]?.ToString();
                if (string.IsNullOrEmpty(txnId)) return "ERROR: txnId not found";

                string rp_username = "", rp_appkey = "", baseUrlStatus = "";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT RP_UserName, RP_AppKey, BaseUrlStatus FROM UpiSetting", con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        rp_username = reader["RP_UserName"].ToString();
                        rp_appkey = reader["RP_AppKey"].ToString();
                        baseUrlStatus = reader["BaseUrlStatus"].ToString();
                    }
                }

                var requestData = new { txnId = txnId, username = rp_username, appKey = rp_appkey };

                using (HttpClient client = new HttpClient())
                {
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(baseUrlStatus, jsonContent).Result;
                    response.EnsureSuccessStatusCode();

                    string responseText = response.Content.ReadAsStringAsync().Result;
                    var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseText);
                    string status = jsonResponse.ContainsKey("status") ? jsonResponse["status"].ToString() : "PENDING";

                    if (status.Equals("AUTHORIZED", StringComparison.OrdinalIgnoreCase) || status.Equals("PAID", StringComparison.OrdinalIgnoreCase))
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
                        {
                            string query = "UPDATE UPITransaction SET Status = 'PAID', PaidAmount = @PaidAmount WHERE TransactionId = @TrnId";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@TrnId", HttpContext.Current.Session["TrnId"]?.ToString());
                            cmd.Parameters.AddWithValue("@PaidAmount", Convert.ToDouble(HttpContext.Current.Session["DueAmount"]?.ToString() ?? "0"));
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }

                        return "PAID";
                    }


                    return status;
                }

            }
            catch (Exception ex)
            {
                return "ERROR: " + ex.Message;
            }
        }

    }

    public class UpiSettings
    {
        public int QRDuration { get; set; }
        public string RP_AppKey { get; set; }
        public string RP_UserName { get; set; }
        public string BaseUrlStatus { get; set; }
        public string BaseUrlGenerate { get; set; }
    }

    public class UpiQrRequest
    {
        public string TrnId { get; set; }
        public string amount { get; set; }
        public string username { get; set; }
        public string appKey { get; set; }
        public string baseUrlGenerate { get; set; }
    }
}
