using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Data;

namespace 物联网智能门禁系统
{
    class DbTools
    {
        public static string sqlSelectUserMessage(string cardid)
        {
            string mes = null;
            using (SqlConnection sqlcon = new SqlConnection("Server=.;user=sa;pwd=123456;database=myIotDb"))
            {
                sqlcon.Open();
                SqlCommand sql = new SqlCommand("select UsName,dbo.Users.UsID,UsCreateTime,count(UsSignInTime),max(UsSignInTime)from dbo.Users,dbo.UserSignIn where  dbo.Users.UsID=dbo.UserSignIn.UsID and dbo.Users.UsID='"+cardid+"' group by dbo.UserSignIn.UsID,UsName,dbo.Users.UsID,UsCreateTime", sqlcon);
                SqlDataReader reader = sql.ExecuteReader();
                if (reader.Read() == true)
                {
                    mes = "用户信息如下：\n用户姓名：" + reader.GetString(0) + "\n用户卡号：" + reader.GetString(1) + "\n用户创建时间：" + reader.GetDateTime(2).ToString() + "\n用户签到次数：" + reader.GetInt32(3) + "\n用户最后一次签到时间：" + reader.GetDateTime(4).ToString();
                }
                else
                {
                    mes = "查询失败！";
                }

            }
            return mes;
        }
        public static void sqlExecuteNonQuery(string sqlText)
        {
            using (SqlConnection sqlcon = new SqlConnection("Server=.;user=sa;pwd=123456;database=myIotDb"))
            {
                sqlcon.Open();
                SqlCommand sql = new SqlCommand(sqlText,sqlcon);
                sql.ExecuteNonQuery();
            }
        }

        public static void sqlExecuteReader(string sqlText, DataTable dt)
        {
            
            using (SqlConnection sqlcon = new SqlConnection("Server=.;user=sa;pwd=123456;database=myIotDb"))
            {
                sqlcon.Open();
                SqlCommand sql = new SqlCommand(sqlText, sqlcon);
                SqlDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr["姓名"] = reader.GetString(0);
                    dr["卡号"] = reader.GetString(1);
                    dr["打卡时间"] = reader.GetDateTime(2).ToString();
                    dt.Rows.Add(dr);
                }
            }
           
        }

        public static string sqlSelectPad(string adname)
        {
            string pad = null;

            using (SqlConnection sqlcon = new SqlConnection("Server=.;user=sa;pwd=123456;database=myIotDb"))
            {
                sqlcon.Open();
                SqlCommand sql = new SqlCommand("select AdPassword from dbo.Administrators where dbo.Administrators.AdName='" + adname+"'", sqlcon);
                SqlDataReader reader = sql.ExecuteReader();
                if (reader.Read() == true)
                {
                    pad = reader.GetString(0);
                }
                else
                {
                    pad = "用户不存在";
                }
            }
            return pad;
        }

        public static string sqlUserSignIn(string user,string pad)
        {
            string level=null;

            using (SqlConnection sqlcon = new SqlConnection("Server=.;user=sa;pwd=123456;database=myIotDb"))
            {
                sqlcon.Open();
                SqlCommand sql = new SqlCommand("select AdLevel from Administrators where dbo.Administrators.AdName='" + user + "' and dbo.Administrators.AdPassword='" + pad + "'", sqlcon);
                SqlDataReader reader = sql.ExecuteReader();
                if (reader.Read() == true)
                {
                    level = reader.GetString(0);
                }
                else
                {
                    level = "用户不存在";
                }
            }
            return level;
        }
        
        public static string sqlSelectUserAndInsertMessage(string CardID,DateTime time)
        {
            string username = null;
            try
            {
                
                using (SqlConnection sqlcon = new SqlConnection("Server=.;user=sa;pwd=123456;database=myIotDb"))
                {
                    sqlcon.Open();
                    SqlCommand sql = new SqlCommand("select UsName from dbo.Users where UsID='" + CardID + "'", sqlcon);
                    SqlDataReader reader = sql.ExecuteReader();
                    if (reader.Read() == true)
                    {
                        username = reader.GetString(0);
                    }
                    else
                    {
                        username = "非法卡";
                    }
                }
                DbTools.sqlExecuteNonQuery("insert into dbo.UserSignIn(UsID, UsSignInTime) values('"+CardID+"', '"+time.ToString()+"')");
            }
            catch
            {
                username = "非法卡";
            }
            return username;
        }
        public static bool sqlSelectAdminPad(string adname,string pad)
        {
          
            try
            {

                using (SqlConnection sqlcon = new SqlConnection("Server=.;user=sa;pwd=123456;database=myIotDb"))
                {
                    sqlcon.Open();
                    SqlCommand sql = new SqlCommand("select AdPassword from dbo.Administrators where AdName='"+adname+"'", sqlcon);
                    SqlDataReader reader = sql.ExecuteReader();
                    if (reader.Read() == true)
                    {
                        if (reader.GetString(0).Trim() == pad.Trim())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            
            }
            catch
            {
                return false;
            }
          
        }

    }
}
