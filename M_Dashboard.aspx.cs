using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeelYourFood
{
    public partial class M_Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["restid"] != null)
            {
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expire, Kindly Login Again to continue....!');", true);
                Response.Redirect("LogOut.aspx");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetRestaurants()
        {
            int total = 0, active = 0, totalapprove = 0;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Resturant_And_AdminMaster where RestId!=1", con))
                    total = (int)cmd.ExecuteScalar();

                using (var cmd = new SqlCommand("WITH PaymentCTE AS (SELECT Id, RestId, PaymentType, ActivationDate, ExpiryDate, ROW_NUMBER() OVER (PARTITION BY RestId ORDER BY ActivationDate ASC) AS RowNum FROM RestPaymentRecords   WHERE CAST(GETDATE() AS DATE) BETWEEN ActivationDate AND ExpiryDate)    SELECT COUNT(*)FROM Resturant_And_AdminMaster RA LEFT JOIN PaymentCTE P ON RA.RestId = P.RestId AND P.RowNum = 1 WHERE RA.RestId <> 1 AND CAST(GETDATE() AS DATE) < P.ExpiryDate", con))
                    active = (int)cmd.ExecuteScalar();

                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Resturant_And_AdminMaster WHERE RestId!=1 and Approved=1", con))
                    totalapprove = (int)cmd.ExecuteScalar();
            }
            int notActive = totalapprove - active;
            int activesub = totalapprove - active;

            return new
            {
                Total = total,
                Approved = totalapprove,
                NotApproved = notActive,
                ActiveSubs = active
            };
        }

        [WebMethod]
        public static object DeviceData()
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            {
                string query = @"
            SELECT 
                SUM(D.OrderingKiosk) AS OrderingKiosk, 
                SUM(D.kitchendisplay) AS kitchendisplay, 
                SUM(D.qms) AS qms, 
                SUM(D.tabletab) AS tabletab, 
                SUM(CASE WHEN D.[server] <> '0' THEN 1 ELSE 0 END) AS ServerCount
            FROM ChooseDevice D
            INNER JOIN Resturant_And_AdminMaster R ON R.RestId = D.RestId
            WHERE R.Approved = 1
        ";

                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        result.Add(row);
                    }
                }
            }

            return result;
        }
        [WebMethod]
        public static object SubscriptionData()
        {
            var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            Dictionary<string, int> registrations = months.ToDictionary(m => m, m => 0);
            Dictionary<string, int> subscriptions = months.ToDictionary(m => m, m => 0);

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            {
                string query = @"
        SELECT 
            DATENAME(MONTH, CreateDate) AS [Month],
            MONTH(CreateDate) AS MonthNumber,
            COUNT(*) AS TotalRegistrations, 
            SUM(CASE WHEN Approved = 1 THEN 1 ELSE 0 END) AS TotalSubscriptions 
        FROM Resturant_And_AdminMaster
        WHERE RestId <> 1 
        GROUP BY MONTH(CreateDate), DATENAME(MONTH, CreateDate)
        ORDER BY MonthNumber";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string month = reader["Month"].ToString().Substring(0, 3); 
                        registrations[month] = Convert.ToInt32(reader["TotalRegistrations"]);
                        subscriptions[month] = Convert.ToInt32(reader["TotalSubscriptions"]);
                    }
                }
            }

            return new
            {
                Labels = months,
                Registrations = months.Select(m => registrations[m]).ToList(),
                Subscriptions = months.Select(m => subscriptions[m]).ToList()
            };
        }


        [WebMethod]
        public static Restaurant[] GetRestaurantList()
        {
            List<Restaurant> restaurantList = new List<Restaurant>();

            string query = @"
        SELECT TOP 5 
            ResturantName AS Name,
            RestAddress AS Address,
            RestPhone AS Phone,
            EmailId AS Email,
            RestLogo AS Logo
        FROM Resturant_And_AdminMaster
        WHERE RestId <> 1
        ORDER BY RestId DESC";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        restaurantList.Add(new Restaurant
                        {
                            Name = reader["Name"].ToString(),
                            Address = reader["Address"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Email = reader["Email"].ToString(),
                            Logo = reader["Logo"].ToString().Replace("~", "")
                        });
                    }
                }
            }

            return restaurantList.ToArray();
        }



    }
    public class Restaurant
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
    }

}