using FeelYourFood_Admin.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace FeelYourFood
{
    public partial class DashBoardForm : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        public readonly Connection _c1 = new Connection();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["restid"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expired, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");
                }
                else
                {
                    getdetails();
                }
            }
        }

        [WebMethod(EnableSession = true)]
        public static object GetRestaurantDetails(string filterType)
        {
            try
            {
                int restid = Convert.ToInt32(HttpContext.Current.Session["restId"]);
                Connection c1 = new Connection();
                DataSet ds = new DataSet();

                int totalOrders = 0, totalSales = 0, totalMenu = 0, totalCustomers = 0;
                string dateCondition = "";

                if (filterType == "today")
                {
                    dateCondition = "CAST(OrderDate AS DATE) = CAST(GETDATE() AS DATE)";
                }
                else if (filterType == "monthly")
                {
                    dateCondition = "MONTH(OrderDate) = MONTH(GETDATE()) AND YEAR(OrderDate) = YEAR(GETDATE())";
                }
                else if (filterType == "yearly")
                {
                    dateCondition = "YEAR(OrderDate) = YEAR(GETDATE())";
                }

                // Total Orders
                string query1 = $"SELECT COUNT(*) FROM [Order] WHERE {dateCondition} AND RestId = {restid}";
                c1.Retrive2(query1, ref ds);
                totalOrders = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                // Total Sales
                string query2 = $"SELECT ISNULL(SUM(TotalAmount), 0) FROM [Order] WHERE {dateCondition} AND RestId = {restid}";
                c1.Retrive2(query2, ref ds);
                totalSales = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                // Total Menu
                string query3 = $"SELECT COUNT(*) FROM Kiosk_MenuDetails WHERE ActiveStatus = 1 AND RestId = {restid}";
                c1.Retrive2(query3, ref ds);
                totalMenu = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                // Total Customers
                string query4 = $"SELECT ISNULL(SUM(CASE WHEN ConsumeType = 'Take Away' THEN 1 ELSE ISNULL(TotalProple, 0) END), 0) FROM [Order] WHERE {dateCondition}  AND RestId ={restid}";

                c1.Retrive2(query4, ref ds);
                totalCustomers = Convert.ToInt32(ds.Tables[0].Rows[0][0]);


                // Consume Types (just names)
                string query5 = $"SELECT CTM.ConsumeTypeId, CTM.Type AS ConsumeTypeName FROM ConsumeTypeMaster CTM INNER JOIN Kiosk_ConsumeType KCT ON CTM.ConsumeTypeId = KCT.ConsumeTypeId WHERE CTM.ActiveStatus = 1 AND KCT.RestId = {restid}";
                c1.Retrive2(query5, ref ds);

                List<object> consumeTypeNames = new List<object>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    consumeTypeNames.Add(new
                    {
                        id = row["ConsumeTypeId"].ToString(),
                        name = row["ConsumeTypeName"].ToString()
                    });
                }

                // Consume Types with total order count (for pie chart)
                string query6 = $"SELECT CTM.ConsumeTypeId, CTM.Type AS ConsumeTypeName, COUNT(O.OrderId) AS TotalOrders FROM [Order] O INNER JOIN ConsumeTypeMaster CTM ON O.ConsumeType = CTM.Type WHERE  O.RestId = {restid} AND {dateCondition} GROUP BY CTM.ConsumeTypeId, CTM.Type";
                c1.Retrive2(query6, ref ds);

                List<object> consumeTypeOrderCounts = new List<object>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    consumeTypeOrderCounts.Add(new
                    {
                        id = row["ConsumeTypeId"].ToString(),
                        name = row["ConsumeTypeName"].ToString(),
                        value = Convert.ToInt32(row["TotalOrders"])
                    });
                }

                List<object> revenueChartData = new List<object>();
                if (filterType == "today")
                {
                    string query = $@"
                             SELECT DATEPART(HOUR, OrderTime) AS TimeSegment, ISNULL(SUM(TotalAmount), 0) AS Revenue
                             FROM [Order]
                             WHERE {dateCondition} AND RestId = {restid}
                             GROUP BY DATEPART(HOUR, OrderTime)
                             ORDER BY TimeSegment";
                    c1.Retrive2(query, ref ds);

                    for (int i = 0; i <= DateTime.Now.Hour; i++)
                    {
                        var row = ds.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToInt32(r["TimeSegment"]) == i);
                        revenueChartData.Add(new
                        {
                            label = $"{i}:00",
                            value = row != null ? Convert.ToInt32(row["Revenue"]) : 0
                        });
                    }
                }
                else if (filterType == "monthly")
                {
                    int daysInMonth = DateTime.Now.Day;

                    string query = $@"
                                    SELECT DAY(OrderDate) AS TimeSegment, ISNULL(SUM(TotalAmount), 0) AS Revenue
                                    FROM [Order]
                                    WHERE {dateCondition} AND RestId = {restid}
                                    GROUP BY DAY(OrderDate)
                                    ORDER BY TimeSegment";

                    c1.Retrive2(query, ref ds);

                    for (int i = 1; i <= daysInMonth; i++)
                    {
                        var row = ds.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToInt32(r["TimeSegment"]) == i);
                        revenueChartData.Add(new
                        {
                            label = $"{i}",
                            value = row != null ? Convert.ToInt32(row["Revenue"]) : 0
                        });
                    }
                }
                else if (filterType == "yearly")
                {
                    int currentMonth = DateTime.Now.Month;
                    string[] monthNames = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                                  "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

                    string query = $@"
                                    SELECT MONTH(OrderDate) AS TimeSegment, ISNULL(SUM(TotalAmount), 0) AS Revenue
                                    FROM [Order]
                                    WHERE {dateCondition} AND RestId = {restid}
                                    GROUP BY MONTH(OrderDate)
                                    ORDER BY TimeSegment";
                    DataSet dsYearly = new DataSet();
                    c1.Retrive2(query, ref dsYearly);

                    for (int i = 1; i <= currentMonth; i++)
                    {
                        var row = dsYearly.Tables[0].AsEnumerable()
                                   .FirstOrDefault(r => Convert.ToInt32(r["TimeSegment"]) == i);

                        revenueChartData.Add(new
                        {
                            label = monthNames[i - 1],
                            value = row != null ? Convert.ToInt32(row["Revenue"]) : 0
                        });
                    }
                }

                return new
                {
                    success = true,
                    data = new
                    {
                        Orders = totalOrders,
                        Sales = totalSales,
                        Menu = totalMenu,
                        Customers = totalCustomers,
                        ConsumeTypeNames = consumeTypeNames,
                        ConsumeTypeOrderCounts = consumeTypeOrderCounts,
                        RevenueChart = revenueChartData

                    }
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = ex.Message
                };
            }
        }
        [WebMethod(EnableSession = true)]
        public static object GetKioskDetails(string filterType)
        {
            try
            {
                int restid = Convert.ToInt32(HttpContext.Current.Session["restId"]);
                Connection c1 = new Connection();
                DataSet ds = new DataSet();

                string dateCondition = "";

                if (filterType == "today")
                {
                    dateCondition = "CAST(OrderDate AS DATE) = CAST(GETDATE() AS DATE)";
                }
                else if (filterType == "monthly")
                {
                    dateCondition = "MONTH(OrderDate) = MONTH(GETDATE()) AND YEAR(OrderDate) = YEAR(GETDATE())";
                }
                else if (filterType == "yearly")
                {
                    dateCondition = "YEAR(OrderDate) = YEAR(GETDATE())";
                }

                string query = $@"
            SELECT K.KioskName, K.KioskId,
                   COUNT(O.OrderId) AS TotalOrders,
                   ISNULL(SUM(O.TotalAmount), 0) AS Revenue
            FROM [Order] O
            INNER JOIN Ordering_Kiosk K ON O.KioskId = K.KioskId
            WHERE {dateCondition} AND O.RestId = {restid}
            GROUP BY K.KioskName, K.KioskId
            ORDER BY K.KioskName";

                c1.Retrive2(query, ref ds);

                List<object> kiosks = new List<object>();
                int slNo = 1;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    kiosks.Add(new
                    {
                        SlNo = slNo++,
                        Kiosk = row["KioskName"].ToString(),
                        Orders = Convert.ToInt32(row["TotalOrders"]),
                        Revenue = Convert.ToDecimal(row["Revenue"]).ToString("0.00")
                    });
                }

                return new
                {
                    success = true,
                    data = kiosks
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = ex.Message
                };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object GetOrderDetails(string filterType)
        {
            try
            {
                int restid = Convert.ToInt32(HttpContext.Current.Session["restId"]);
                string dateCondition = "";

                if (filterType == "today")
                {
                    dateCondition = "CAST(TransactionDate AS DATE) = CAST(GETDATE() AS DATE)";
                }
                else if (filterType == "monthly")
                {
                    dateCondition = "MONTH(TransactionDate) = MONTH(GETDATE()) AND YEAR(TransactionDate) = YEAR(GETDATE())";
                }
                else if (filterType == "yearly")
                {
                    dateCondition = "YEAR(TransactionDate) = YEAR(GETDATE())";
                }

                string query = $@"
                                SELECT TOP 5
                                    KF.itemPhoto,
                                    KF.ItemName,
                                    KF.Price AS ItemPrice,
                                    SUM(OI.Quantity) AS TotalOrderedQty
                                FROM OrderItem OI
                                INNER JOIN [Order] O ON OI.OrderId = O.OrderId AND OI.RestId = O.RestId
                                INNER JOIN Kiosk_ItemFood KF ON OI.ItemId = KF.ItemId AND OI.RestId = KF.RestId
                                WHERE OI.RestId = {restid} AND {dateCondition}
                                GROUP BY KF.itemPhoto, KF.ItemName, KF.Price
                                ORDER BY TotalOrderedQty DESC";
                Connection c1 = new Connection();
                DataSet ds = new DataSet();
                c1.Retrive2(query, ref ds);

                List<object> itemList = new List<object>();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    itemList.Add(new
                    {
                        Photo= row["itemPhoto"].ToString().Replace("~", ""),
                        ItemName = row["ItemName"].ToString(),
                        ItemPrice = Convert.ToDecimal(row["ItemPrice"]).ToString("0.00"),
                        TotalQuantity = Convert.ToInt32(row["TotalOrderedQty"])
                    });
                }

                return new
                {
                    success = true,
                    data = itemList
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = ex.Message
                };
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
                        string expirytxt = reader["ExpiryDate"] != DBNull.Value? Convert.ToDateTime(reader["ExpiryDate"]).ToString("dd/MM/yyyy"): "N/A";

                        expiryDate.InnerText = expirytxt;

                    }
                    else
                    {
                        expiryDate.InnerText = "N/A";

                    }
                }
            }

        }


    }
}
