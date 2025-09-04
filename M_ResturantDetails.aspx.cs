using FeelYourFood_Admin.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeelYourFood
{
    public partial class M_ResturantDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!IsPostBack)
                {
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expire, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");
                }
            }
            // Nothing needed on initial load
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetRestaurantData(bool hasSubscription)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            string condition = hasSubscription ? " AND CAST(GETDATE() AS DATE) < P.ExpiryDate" : "";


            string query = $@"
                WITH PaymentCTE AS (
                SELECT Id, RestId, PaymentType, ActivationDate, ExpiryDate, ROW_NUMBER() OVER (PARTITION BY RestId ORDER BY ActivationDate ASC) AS RowNum FROM RestPaymentRecords WHERE CAST(GETDATE() AS DATE) BETWEEN ActivationDate AND ExpiryDate)
                SELECT RA.RestId, RA.CreateDate, RA.CreateTime, RA.Name, RA.MobileNo, RA.EmailId, RA.Password, RA.ResturantName, RA.RestAddress, RA.RestPhone, RA.RestLogo, RA.GstNo, RA.AdminType, RA.ActiveStatus, RA.UpdatedBy, RA.UpdatedDate, RA.Approved, P.Id AS PaymentRecordId, P.PaymentType, P.ActivationDate, P.ExpiryDate FROM Resturant_And_AdminMaster RA

                LEFT JOIN PaymentCTE P
                    ON RA.RestId = P.RestId AND P.RowNum = 1 WHERE RA.RestId <> 1 {condition}
                ORDER BY RA.RestId ASC";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var dict = new Dictionary<string, string>
                    {
                        ["ResturantName"] = reader["ResturantName"].ToString(),
                        ["RestAddress"] = reader["RestAddress"].ToString(),
                        ["RestPhone"] = reader["RestPhone"].ToString(),
                        ["EmailId"] = reader["EmailId"].ToString(),
                        ["RestLogo"] = reader["RestLogo"].ToString().Replace("~", ""),
                        ["GstNo"] = reader["GstNo"].ToString(),
                        ["ActivationDate"] = reader["ActivationDate"] != DBNull.Value
                            ? Convert.ToDateTime(reader["ActivationDate"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                        ["ExpiryDate"] = reader["ExpiryDate"] != DBNull.Value
                            ? Convert.ToDateTime(reader["ExpiryDate"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""
                    };
                    result.Add(dict);
                }
                reader.Close();
            }
            return result;
        }
    }
}