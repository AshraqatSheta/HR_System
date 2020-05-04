using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["HRCon"].ToString();
            con = new SqlConnection(constring);
        }

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
        // check if the employee has set his arrival time before
        public int checkDuplicatesArrivals(int employeeId,DateTime date)
        {
            connection();
            SqlCommand cmd = new SqlCommand("checkDuplicatesArrivals", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employeeId", employeeId);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.Add("@count", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            con.Open();
            cmd.ExecuteNonQuery();
            int count = Convert.ToInt32(cmd.Parameters["@count"].Value);
            return count;
        }
        private int setArrivalTime (int id)
        {
            DateTime now = DateTime.Now;
            int success = 0;
            if (checkDuplicatesArrivals(id, now.Date)==0)
            {
                connection();
                SqlCommand cmd = new SqlCommand("setArrivalTime", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employeeId", id);
                cmd.Parameters.AddWithValue("@date", now.Date);
                cmd.Parameters.AddWithValue("@time", now.TimeOfDay);
                
                con.Open();
                success=cmd.ExecuteNonQuery();
                con.Close();
                return success;
            }
            return success;
        }
        // check whether the employee has already set his leave time before
        public int checkDuplicatesLeave(int employeeId, DateTime date)
        {
            connection();
            SqlCommand cmd = new SqlCommand("checkDuplicatesLeave", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employeeId", employeeId);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.Add("@count", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            con.Open();
            cmd.ExecuteNonQuery();
            int count = Convert.ToInt32(cmd.Parameters["@count"].Value);
            return count;
        }
        private int setLeaveTime(int id )
        {
            DateTime now = DateTime.Now;
            int success = 0;
            if (checkDuplicatesLeave(id, now.Date) == 0)
            {
                connection();
                SqlCommand cmd = new SqlCommand("setLeaveTime", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employeeId", id);
                cmd.Parameters.AddWithValue("@date", now.Date);
                cmd.Parameters.AddWithValue("@time", now.TimeOfDay);

                con.Open();
                success = cmd.ExecuteNonQuery();
                con.Close();
                return success;
            }
            return success;
        }
        //if start_date and end_date == null then view the attendance of the previous month
        //start_date == null view attendance of a month before end_date
        //end_date == null view attendance of start_date only
        public List<Attendance> viewAttendance(int id ,DateTime start_date,DateTime end_date)
        {
            List<Attendance> attendancesList = new List<Attendance>();
            start_date = start_date.Date;
            end_date = end_date.Date;
           
            
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
                end_date = start_date;
            }
            connection();
            SqlCommand cmd = new SqlCommand("viewEmployeeAttendance", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                Attendance attendance = new Attendance();
                attendance.Date = Convert.ToDateTime(dr["day_date"]).Date;
                attendance.ArrivalTime= Convert.ToDateTime(dr["arrival_time"]);
                attendance.ArrivalTime = Convert.ToDateTime(dr["leave_time"]);

                attendancesList.Add(attendance);
            }


            return attendancesList;
        }
        private List<string> viewProfile(int id)
        {
            List<string> profileInfo = new List<string>();

            return profileInfo;
        }
    }
}