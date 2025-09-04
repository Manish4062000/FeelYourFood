using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace FeelYourFood_Admin.AppCode
{
    public class Connection
    {
        public static int HospitalId;
        static string connStr = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
        public SqlConnection con = new SqlConnection(connStr);

        DataTable Dt = new DataTable();
        public DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();

        public void read(string pronm, string val)
        {
            ds.Reset();
            da = new SqlDataAdapter(pronm + val, con);
            da.Fill(ds);
        }

        public DataSet redt(string pronm, string val)
        {
            ds.Reset();
            da = new SqlDataAdapter(pronm + val, con);
            da.Fill(ds);
            return ds;
        }

        public DataTable Ldata(String ssql)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(ssql, con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);

                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
            }
        }

        public void Retrive(string cmd, ref DataTable Dt)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            // OleDbDataAdapter Da = new OleDbDataAdapter(cmd,con);
            SqlDataAdapter Da = new SqlDataAdapter(cmd, con);
            Dt.Clear();
            Dt.Reset();
            Da.Fill(Dt);
            con.Close();
        }

        public void Retrive2(string cmd, ref DataSet ds)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            //OleDbDataAdapter Da = new OleDbDataAdapter(cmd, con);
            SqlDataAdapter Da = new SqlDataAdapter(cmd, con);
            ds.Clear();
            ds.Reset();
            Da.Fill(ds);
            con.Close();
        }

        public void Execute(string str)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            // OleDbDataAdapter da = new OleDbDataAdapter(str, con);
            SqlDataAdapter da = new SqlDataAdapter(str, con);
            ds.Clear();
            ds.Reset();
            da.Fill(ds);
            //OleDbCommand cmd1 = new OleDbCommand(str, con);
            //cmd1.ExecuteNonQuery();
            //cmd1.Dispose();
            //Dr.Read();
            con.Close();
        }
        public void Execute2(string str, ref DataSet ds)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            // OleDbDataAdapter da = new OleDbDataAdapter(str, con);
            SqlDataAdapter da = new SqlDataAdapter(str, con);
            ds.Clear();
            ds.Reset();
            da.Fill(ds);
            //OleDbCommand cmd1 = new OleDbCommand(str, con);
            //cmd1.ExecuteNonQuery();
            //cmd1.Dispose();
            //Dr.Read();
            con.Close();
        }
        public DataSet FillGridvieww(string str, ref GridView grv)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            // OleDbDataAdapter da = new OleDbDataAdapter(str, con);
            SqlDataAdapter da = new SqlDataAdapter(str, con);
            ds.Clear();
            da.Fill(ds);
            grv.DataSource = ds;
            grv.DataBind();
            con.Close();
            return ds;
        }
        public void FillGridview(string str, ref GridView grv)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            // OleDbDataAdapter da = new OleDbDataAdapter(str, con);
            SqlDataAdapter da = new SqlDataAdapter(str, con);
            ds.Clear();
            da.Fill(ds);
            grv.DataSource = ds;
            grv.DataBind();

            con.Close();
        }

        public void ExecQuery(string Str)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd = new SqlCommand(Str, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public string ReturnValue(string str, ref DataTable Dt)
        {
            string value = string.Empty;
            SqlDataAdapter da = new SqlDataAdapter(str, con);
            da.Fill(Dt);
            if (Dt.Rows.Count > 0)
            {
                value = Dt.Rows[0][0].ToString();
            }
            return value;
        }

        public void FillDropDown(string str, DropDownList dl)
        {
            ds.Reset();
            ds.Clear();
            da = new SqlDataAdapter(str, con);
            da.Fill(ds);
            dl.DataSource = ds;
            dl.DataTextField = ds.Tables[0].Columns[0].ColumnName;
            dl.DataValueField = ds.Tables[0].Columns[1].ColumnName;
            dl.DataBind();
            con.Close();
        }

        //-------------------Added on 31st may 2014-------------
        public DataSet rd(string uname)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            da = new SqlDataAdapter("select * from tbl_login where cflag='true' and cusername='" + uname + "' ", con);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public int ExecNonquery(string Str)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd = new SqlCommand(Str, con);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }
        private void PrepareCommand(SqlCommand cmd, SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParameters)
        {

            //if (conn.State != ConnectionState.Open)
            //    conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;
            if (cmdParameters != null)
            {
                foreach (SqlParameter param in cmdParameters)
                {
                    cmd.Parameters.Add(param);
                }
            }
        }
        public int DDLopration(CommandType cmdType, string cmdText, params SqlParameter[] cmdParameters)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, con, cmdType, cmdText, cmdParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.CommandTimeout = 2000;
                return val;
            }
            catch (Exception ex)
            {
                throw ex;

                //return ds;
                //globalvariable.WriteLog(ex.Message.ToString());
            }
            finally
            {
                con.Close();
            }
        }
    }
}