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

namespace FeelYourFood
{
    public partial class M_ResturantCrediential : System.Web.UI.Page
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
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Session Expire, Kindly Login Again to continue....!');", true);
                    Response.Redirect("LogOut.aspx");
                }
            }
        }
        [WebMethod]
        public static List<RestaurantItem> GetFilteredData(string selectedOption)
        {
            List<RestaurantItem> list = new List<RestaurantItem>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "";

                    switch (selectedOption)
                    {
                        case "1": 
                            query = "SELECT  ResturantName, RestId FROM Resturant_And_AdminMaster WHERE RestId != 1";
                            break;
                        case "2": 
                            query = "WITH PaymentCTE AS (SELECT Id, RestId, PaymentType, ActivationDate, ExpiryDate, ROW_NUMBER() OVER (PARTITION BY RestId ORDER BY ActivationDate ASC) AS RowNum FROM RestPaymentRecords WHERE CAST(GETDATE() AS DATE) BETWEEN ActivationDate AND ExpiryDate ) SELECT RA.ResturantName, RA.RestId FROM Resturant_And_AdminMaster RA LEFT JOIN PaymentCTE P ON RA.RestId = P.RestId AND P.RowNum = 1 WHERE RA.Approved = 1 AND RA.RestId <> 1 AND CAST(GETDATE() AS DATE) < P.ExpiryDate ORDER BY RA.RestId ASC";
                            break;
                        case "3": 
                            query = "WITH PaymentCTE AS (SELECT RestId FROM RestPaymentRecords WHERE CAST(GETDATE() AS DATE) BETWEEN ActivationDate AND ExpiryDate GROUP BY RestId) SELECT ResturantName, RestId FROM Resturant_And_AdminMaster WHERE RestId != 1 AND RestId NOT IN (SELECT RestId FROM PaymentCTE) ORDER BY RestId ASC";
                            break;
                        default:
                            return list;
                    }
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        list.Add(new RestaurantItem
                        {
                            Id = Convert.ToInt32(rdr["RestId"]),
                            Name = rdr["ResturantName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DB Error: " + ex.Message);
            }

            return list;
        }

        [WebMethod]
        public static object GetRestaurantDetails(string restId)
        {
            var c1 = new Connection();
            var ds = new DataSet();

            string category = "", cuisine = "", consume = "", payment = "";
            string server = "N/A", orderingKiosk = "N/A", kitchenDisplay = "N/A", qms = "N/A", tableTab = "N/A";

            // Category
            string query = @"SELECT fcm.CategoryName 
        FROM FoodCategoryMaster fcm 
        INNER JOIN Kiosk_Category kc ON fcm.TypeId = kc.CategoryId  
        WHERE kc.RestId = '" + restId + "'";
            c1.Retrive2(query, ref ds);
            if (ds.Tables[0].Rows.Count > 0)
                category = string.Join(", ", ds.Tables[0].AsEnumerable().Select(r => r["CategoryName"].ToString()));
            ds.Clear();

            // Cuisine
            string query1 = @"SELECT cm.CuisineName 
        FROM CuisineMaster cm 
        INNER JOIN Kiosk_Cuisine kc ON cm.CuisineId = kc.CuisineId  
        WHERE kc.RestId = '" + restId + "'";
            c1.Retrive2(query1, ref ds);
            if (ds.Tables[0].Rows.Count > 0)
                cuisine = string.Join(", ", ds.Tables[0].AsEnumerable().Select(r => r["CuisineName"].ToString()));
            ds.Clear();

            // Consume
            string query2 = @"SELECT ctm.Type 
        FROM ConsumeTypeMaster ctm 
        INNER JOIN Kiosk_ConsumeType kc ON ctm.ConsumeTypeId = kc.ConsumeTypeId  
        WHERE kc.RestId = '" + restId + "'";
            c1.Retrive2(query2, ref ds);
            if (ds.Tables[0].Rows.Count > 0)
                consume = string.Join(", ", ds.Tables[0].AsEnumerable().Select(r => r["Type"].ToString()));
            ds.Clear();

            // Payment
            string query3 = @"SELECT pmm.PaymentMode 
        FROM PaymentModeMaster pmm 
        INNER JOIN Kiosk_PaymentMode kpm ON pmm.PaymentModeId = kpm.PaymentModeId  
        WHERE kpm.RestId = '" + restId + "'";
            c1.Retrive2(query3, ref ds);
            if (ds.Tables[0].Rows.Count > 0)
                payment = string.Join(", ", ds.Tables[0].AsEnumerable().Select(r => r["PaymentMode"].ToString()));
            ds.Clear();

            // Devices
            string query4 = @"SELECT OrderingKiosk, server, qms, tabletab, KitchenDisplay 
                      FROM ChooseDevice WHERE RestId = '" + restId + "'";
            c1.Retrive2(query4, ref ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];
                server = row["server"].ToString();
                orderingKiosk = row["OrderingKiosk"].ToString();
                kitchenDisplay = row["KitchenDisplay"].ToString();
                qms = row["qms"].ToString();
                tableTab = row["tabletab"].ToString();
            }

            return new
            {
                Category = category,
                Cuisine = cuisine,
                Consume = consume,
                Payment = payment,
                Server = server,
                OrderingKiosk = orderingKiosk,
                KitchenDisplay = kitchenDisplay,
                QMS = qms,
                TableTab = tableTab
            };
        }

        
        [WebMethod]
        public static List<MenuItem> GetVegMenuDetails(string restId)
        {
            List<MenuItem> menuList = new List<MenuItem>();

            string str = @"SELECT km.FoodMenuId,km.FoodMenuName, km.MenuPhotoPath 
                   FROM MenuMaster mm
                   INNER JOIN Kiosk_MenuDetails km ON mm.FoodMenuId = km.FoodMenuId
                   INNER JOIN FoodCategoryMaster fc ON mm.CategoryId = fc.TypeId
                   WHERE km.RestId = @RestId AND fc.CategoryName = 'Veg'";

            string connectionString = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(str, con))
                {
                    // 🟢 Add the parameter here
                    cmd.Parameters.AddWithValue("@RestId", restId);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            menuList.Add(new MenuItem
                            {
                                FoodMenuId = Convert.ToInt32(reader["FoodMenuId"]),
                                FoodMenuName = reader["FoodMenuName"].ToString(),
                                MenuPhotoPath = reader["MenuPhotoPath"].ToString().Replace("~", "")
                            });
                        }
                    }
                }
            }

            return menuList;
        }

        [WebMethod]
        public static List<MenuItem> GetNonVegMenuDetails(string restId)
        {
            List<MenuItem> menuList = new List<MenuItem>();

            string str = @"SELECT km.FoodMenuId,km.FoodMenuName, km.MenuPhotoPath 
                   FROM MenuMaster mm
                   INNER JOIN Kiosk_MenuDetails km ON mm.FoodMenuId = km.FoodMenuId
                   INNER JOIN FoodCategoryMaster fc ON mm.CategoryId = fc.TypeId
                   WHERE km.RestId = @RestId AND fc.CategoryName = 'Non Veg'";

            string connectionString = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(str, con))
                {
                    // 🟢 Add the parameter here
                    cmd.Parameters.AddWithValue("@RestId", restId);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            menuList.Add(new MenuItem
                            {
                                FoodMenuId = Convert.ToInt32(reader["FoodMenuId"]),
                                FoodMenuName = reader["FoodMenuName"].ToString(),
                                MenuPhotoPath = reader["MenuPhotoPath"].ToString().Replace("~", "")
                            });
                        }
                    }
                }
            }

            return menuList;
        }

        [WebMethod]
        public static List<ItemDetail> GetItemDetails(int restId, int foodMenuId, string categoryType)
        {
            List<ItemDetail> itemList = new List<ItemDetail>();
            string query = @"SELECT kif.ItemName, kif.itemPhoto FROM Kiosk_ItemFood kif INNER JOIN ItemMaster im ON kif.ItemId = im.ItemId 
INNER JOIN FoodCategoryMaster fcm ON im.categoryId = fcm.TypeId WHERE kif.RestId = @RestId AND kif.menuid = @FoodMenuId AND fcm.CategoryName = @categoryType";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@RestId", restId);
                cmd.Parameters.AddWithValue("@FoodMenuId", foodMenuId);
                cmd.Parameters.AddWithValue("@CategoryType", categoryType); 

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        itemList.Add(new ItemDetail
                        {
                            ItemName = reader["ItemName"].ToString(),
                            ItemPhotoPath = reader["itemPhoto"].ToString().Replace("~", "")
                        });
                    }
                }
            }

            return itemList;
        }



    }
    public class ItemDetail
    {
        public string ItemName { get; set; }
        public string ItemPhotoPath { get; set; }
    }
    public class MenuItem
    {
        public int FoodMenuId { get; set; }
        public string FoodMenuName { get; set; }
        public string MenuPhotoPath { get; set; }
    }

    public class RestaurantItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}