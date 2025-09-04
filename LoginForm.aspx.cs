using FeelYourFood_Admin.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;

namespace FeelYourFood
{
    public partial class LoginForm : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        public readonly Connection _c1 = new Connection();
        public DataTable _dtt = new DataTable();
        public DataSet ds = new DataSet();
        public string str = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //RegisterAsyncTask(new PageAsyncTask(Syncdata));
            }
            if (Session["SuccessMessage"] != null)
            {
                string message = Session["SuccessMessage"].ToString();
                Session.Remove("SuccessMessage");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{message}');", true);
                
            }
            

        }



        protected void btnLogin_Click(object sender, EventArgs e)
        {
            
            //string captchaResponse = Request.Form["g-recaptcha-response"];
            //string secretKey = "6LfKWWIrAAAAAHsdBGSWl00B5fE9VxurkI3A6e32";

            //using (var client = new System.Net.WebClient())
            //{
            //    var result = client.DownloadString($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}");
            //    var obj = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<ReCaptchaResult>(result);
            //    if (!obj.success)
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Captcha verification failed. Please try again.');", true);
            //        return;
            //    }
            //}
            string email = Convert.ToString(Request.Form["email"]).Trim();
            string password = Convert.ToString(Request.Form["password"]).Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Email is required');", true);
                return;
            }
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            if (!Regex.IsMatch(email, emailPattern))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid email format');", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Password is required');", true);
                return;
            }

            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@sp_type", "5");
            param[1] = new SqlParameter("@EmailId", email);
            param[2] = new SqlParameter("@Password", password);

            int iresult = SqlHelper.ExecuteNonQuery(_c1.con, CommandType.StoredProcedure, "sp_ResturantAndAdminMaster", param);
            if (iresult != 0)
            {
                string strChk = "SELECT  RestId,AdminType,ResturantName,Name,RestLogo,Approved FROM Resturant_And_AdminMaster where EmailId='" + email + "' and Password='" + password + "'";
                DataSet dsChk = new DataSet();
                _c1.Retrive2(strChk, ref dsChk);
                if (dsChk.Tables[0].Rows.Count > 0)
                {


                    string admintype = (dsChk.Tables[0].Rows[0][1]).ToString();
                    Session["adminname"] = (dsChk.Tables[0].Rows[0][3]).ToString();
                    if (admintype == "A")
                    {
                        Session["restid"] = Convert.ToInt32(dsChk.Tables[0].Rows[0][0]);
                        Session["restname"] = (dsChk.Tables[0].Rows[0][2]).ToString();

                        Session["restlogo"] = (dsChk.Tables[0].Rows[0][4]).ToString();
                        Session["approved"] = Convert.ToInt32(dsChk.Tables[0].Rows[0]["Approved"]);
                        Response.Redirect("DashBoardForm.aspx");
                    }
                    else if (admintype == "M")
                    {
                        Session["restid"] = Convert.ToInt32(dsChk.Tables[0].Rows[0][0]);
                        Response.Redirect("M_Dashboard.aspx");
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('User ID and Password are not registered.');", true);
                    return;
                }

            }

        }

        private async Task Syncdata()
        {
            try
            {
                string baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
                string apiUrl = baseUrl.TrimEnd('/') + "/api/StoreOrders/FetchAndStoreAllTransactions";

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(apiUrl);
                    if (!response.IsSuccessStatusCode)
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        // Log or store the error somewhere if needed
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception appropriately — log it instead of MessageBox
            }
        }



    }
    public class ReCaptchaResult
    {
        public bool success { get; set; }
        public List<string> error_codes { get; set; }
    }
}