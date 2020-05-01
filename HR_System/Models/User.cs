using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class User
    {
        private int user_id;
        private int department_id;
        private string position;
        private string name;
        private string email;
        private string number;
        private string ssn;
        private string address;

        SqlConnection con = new SqlConnection("Data Source=developmentserver.database.windows.net;Initial Catalog=HR;User ID=afm2020;Password=********;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        public int User_id { get => user_id; set => user_id = value; }

        private void updateProfile(int user_id, LinkedList<string> newValues)
        {

        }
        private int logIn (string userName,string password)
        {
            int id=0;

            return id;

        }
        private void logOut(int id)
        {

        }
        private void setArrivalTime (int id, TimeSpan currentTime)
        {
            
        }
        private void setLeaveTime(int id ,TimeSpan currentTime)
        {

        }
        public string viewAttendance(int id ,DateTime start_date,DateTime end_date)
        {
            id = User_id;
            start_date = start_date.Date;
            end_date = end_date.Date;
           // List<string> attendance = new List<string>();
            var cmd = new SqlCommand("dbo.viewttendance", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            
            if (start_date==null && end_date== null)
            {
                end_date = DateTime.Today.Date;
                start_date = end_date.AddDays(-30);
            }else if (start_date == null)
            {
                start_date = end_date.AddDays(-30);
            }
            else if (end_date == null)
            {
                end_date = start_date.AddDays(30);
            }
            cmd.Parameters.Add(new SqlParameter("@id", id));
            cmd.Parameters.Add(new SqlParameter("@start_date", start_date));
            cmd.Parameters.Add(new SqlParameter("@end_date", end_date));
            con.Open();
            string attendance = cmd.ExecuteScalar().ToString();
            con.Close();
            
            return attendance;
        }
        private List<string> viewProfile(int id)
        {
            List<string> profileInfo = new List<string>();

            return profileInfo;
        }
    }
}