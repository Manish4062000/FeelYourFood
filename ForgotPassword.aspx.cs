using FeelYourFood_Admin.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FeelYourFood
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        public readonly Connection _c1 = new Connection();
        public DataTable _dtt = new DataTable();
        public DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected async void btnLogin_Click(object sender, EventArgs e)
        {
            string email = Convert.ToString(Request.Form["email"]).Trim();
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            if (!Regex.IsMatch(email, emailPattern))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid email format');", true);
                return;
            }

            string strChk = "SELECT RestId, EmailId FROM Resturant_And_AdminMaster WHERE EmailId='" + email + "'";
            DataSet dsChk = new DataSet();
            _c1.Retrive2(strChk, ref dsChk);

            if (dsChk.Tables[0].Rows.Count > 0)
            {
                string restid = dsChk.Tables[0].Rows[0][0].ToString();

                await SendEmailAsync(restid, email);

                Response.Redirect("ForgetEmailMatch.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('The entered email ID is not registered. Please enter a valid registered email address.');", true);
                return;
            }
        }

        private async Task SendEmailAsync(string restId, string email)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("manishkumar4062000@gmail.com", "wyfb nwbk hurh lkub"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("manishkumar4062000@gmail.com"),
                    Subject = "Reset Your FeelYourFood Password",
                    Body = $@"
                <p>Dear User,</p>
                <p>We received a request to reset the password for your account.</p>
                <p>To reset your password, please click the link below:</p>
                <p><a href='http://192.168.3.94:421/ResetpasswordForm.aspx?restId={restId}'>Reset Password</a></p>
                <br/><br/>
                <p>If you did not request a password reset or need assistance, feel free to contact our 
                    <a href='https://www.addsofttech.com/support.html'>support team</a>.
                </p>
                <p>Thank you,<br/>FeelYourFood Team</p>
            ",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }
}