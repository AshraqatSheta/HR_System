using HR.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class HrAdmin
    {
        private SqlConnection con;
        private SqlConnection conDW;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["HRCon"].ToString();
            con = new SqlConnection(constring);
        }
        private void DWconnection()
        {
            string constring = ConfigurationManager.ConnectionStrings["DwCon"].ToString();
            conDW = new SqlConnection(constring);
        }

        // return 0 if no duplicates and return 1 otherwise
        private int checkDuplicatesEmployees(string userName)
        {
            connection();
            SqlCommand cmd = new SqlCommand("checkUserName", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userName", userName);
            cmd.Parameters.Add("@count", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            con.Open();
            cmd.ExecuteNonQuery();
            int count = Convert.ToInt32(cmd.Parameters["@count"].Value);
            return count;
        }
        // returns 0 in failure and 1 or greater if success
        public int addEmployee(User_Info userInfo)
        {
           int success;
           
                connection();
                SqlCommand cmd = new SqlCommand("add_employee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user_name", userInfo.UserName);
                cmd.Parameters.AddWithValue("@password", Encrypt.Encode(userInfo.Password));
                cmd.Parameters.AddWithValue("@email", userInfo.Email);
                cmd.Parameters.AddWithValue("@full_name", userInfo.FullName);
                cmd.Parameters.AddWithValue("@address", userInfo.Address);
                cmd.Parameters.AddWithValue("@ssd", userInfo.Ssn);
                cmd.Parameters.AddWithValue("@salary", userInfo.Salary);
                cmd.Parameters.AddWithValue("@start_date", userInfo.StartDate);
                cmd.Parameters.AddWithValue("@phone_number", userInfo.PhoneNumber);
                cmd.Parameters.AddWithValue("@gender", userInfo.Gendre);
                cmd.Parameters.AddWithValue("@birth_date", userInfo.BirthDate);
                cmd.Parameters.AddWithValue("@educational_degree", userInfo.EducationalDrgree);
                cmd.Parameters.AddWithValue("@graduation_date", userInfo.GraduationDate);
                cmd.Parameters.AddWithValue("@notes", userInfo.Notes);
                con.Open();
                success= cmd.ExecuteNonQuery();
                con.Close();
         return success;
                  
        }

        private List<Attendance> getAttendanceForEmployee(int employeeId, DateTime start_date,DateTime end_date)
        {
            List<Attendance> attendancesList = new List<Attendance>();
            start_date = start_date.Date;
            end_date = end_date.Date;


            if (start_date == null && end_date == null)
            {
                end_date = DateTime.Today.Date;
                start_date = end_date.AddDays(-30);
            }
            else if (start_date == null)
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
                attendance.ArrivalTime =(TimeSpan) Convert.ToDateTime(dr["arrival_time"]).TimeOfDay;
                attendance.ArrivalTime =(TimeSpan) Convert.ToDateTime(dr["leave_time"]).TimeOfDay;

                attendancesList.Add(attendance);
            }
            return attendancesList;
        }
        private List<Attendance> getAttendanceOnDate(DateTime date)
        {
            
            List<Attendance> attendanceList = new List<Attendance>();
            
            connection();
            SqlCommand cmd = new SqlCommand("getAttendanceOnDate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@date", date.Date);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Attendance attendance = new Attendance();
                attendance.EmployeeId = Convert.ToInt32(dr["employee_id"]);
                attendance.EmployeeName = Convert.ToString(dr["full_name"]);
                attendance.ArrivalTime =(TimeSpan) Convert.ToDateTime(dr["arrival_time"]).TimeOfDay;
                attendance.LeaveTime =(TimeSpan) Convert.ToDateTime(dr["leave_time"]).TimeOfDay;

                attendanceList.Add(attendance);
            }

            return attendanceList;
        }
        private List<int> getEmployeeWithDepartment(int departmentId)
        {
            List<int> employeeIdList = new List<int>();

            connection();
            SqlCommand cmd = new SqlCommand("getEmployeeWithDepartment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@department_id", departmentId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                
                int employeeId = Convert.ToInt32(dr["employee_id"]);
                employeeIdList.Add(employeeId);
               
            }

            return employeeIdList;
        }
        private List<Attendance> getAttendanceForDepartmentOnDate(int departmentId,DateTime date)
        {

            List<Attendance> attendanceList = new List<Attendance>();
            List<int> employeeIdList ;
            employeeIdList = getEmployeeWithDepartment(departmentId);
            foreach (int id in employeeIdList){
                Attendance attendance ;
                attendance = getAttendanceForEmployeeOnDate(id,date);
                attendanceList.Add(attendance);
            }
            
            return attendanceList;
        }

        private Attendance getAttendanceForEmployeeOnDate(int id, DateTime date)
        {
            Attendance attendance = new Attendance();
            connection();
            SqlCommand cmd = new SqlCommand("getAttendanceForEmployeeOnDate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
               
                attendance.EmployeeName = Convert.ToString(dr["full_name"]);
                attendance.ArrivalTime = (TimeSpan)Convert.ToDateTime(dr["arrival_time"]).TimeOfDay;
                attendance.ArrivalTime =(TimeSpan) Convert.ToDateTime(dr["leave_time"]).TimeOfDay;

                
            }
            return attendance;
        }

        private List<List<string>> getPerformancReports(int id)
        {
            List<List<string>> performance = new List<List<string>>();

            return performance;
        }
        private List<List<string>> getPerformancReports()
        {
            List<List<string>> performance = new List<List<string>>();

            return performance;
        }
        public int addHoliday(Holiday holiday,int hr_id)
        {
            int success;

            connection();
            SqlCommand cmd = new SqlCommand("addHoliday", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employee_id", holiday.EmployeeId);
            cmd.Parameters.AddWithValue("@start_date", holiday.StartDate.Date);
            cmd.Parameters.AddWithValue("@end_date",holiday.EndDate.Date);
            cmd.Parameters.AddWithValue("@cause", holiday.Cause);
            cmd.Parameters.AddWithValue("@hr_id", hr_id);
           
            con.Open();
            success = cmd.ExecuteNonQuery();
            con.Close();
            return success;

        }
        public int addPermission(Permission permission,int hr_id)
        {

            int success;

            connection();
            SqlCommand cmd = new SqlCommand("add_permession", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employee_id", permission.EmployeeId);
            cmd.Parameters.AddWithValue("@start_date", permission.StartTime.TimeOfDay);
            cmd.Parameters.AddWithValue("@end_date", permission.EndTime.TimeOfDay);
            cmd.Parameters.AddWithValue("@cause", permission.Cause);
            cmd.Parameters.AddWithValue("@hr_id", hr_id);

            con.Open();
            success = cmd.ExecuteNonQuery();
            con.Close();
            return success;


        }
        private int validate_vacation(DateTime startDate, DateTime endDate)
        {
            connection();
            SqlCommand cmd = new SqlCommand("validate_vacation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@startDate", startDate.Date);
            cmd.Parameters.AddWithValue("@endDate", endDate.Date);
            cmd.Parameters.Add("@count", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
            con.Open();
            cmd.ExecuteNonQuery();
            int count = Convert.ToInt32(cmd.Parameters["@count"].Value);
            return count;
        }
        public int addVacation(Official_Vacation vacation)
        {

            int success;

            connection();
            SqlCommand cmd = new SqlCommand("addVacation", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@vacationName", vacation.VacationName);
            cmd.Parameters.AddWithValue("@startDate",vacation.StartDate.Date);
            cmd.Parameters.AddWithValue("@endDate", vacation.EndDate.Date);

            con.Open();
            success = cmd.ExecuteNonQuery();
            con.Close();
            return success;


        }
        public void setBounaceCriteria(float minPerformanceRate, int minExperiencYear)
        {

        }
        public List<List<string>> viewCompatibleCandidatesToBounus(int bounusId)
        {
            List<List<string>> candidates = new List<List<string>>();
            return candidates;
        }
        public int insertTraining(Training training)
        {
            int trainingId=0;
            connection();
            SqlCommand cmd = new SqlCommand("insertTraining", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@trainingName", training.TrainingName);
            cmd.Parameters.AddWithValue("@startDate", training.StartDate.Date);
            cmd.Parameters.AddWithValue("@endDate", training.EndDate.Date);
            cmd.Parameters.AddWithValue("@location", training.Location);
            cmd.Parameters.AddWithValue("@maxNumberOfParticipants", training.MaxNumOfParticipants);
            cmd.Parameters.AddWithValue("@hoursPerDay", training.HoursPerDay);
            cmd.Parameters.AddWithValue("@skillId", training.SkillId);
            cmd.Parameters.AddWithValue("@maxRank", training.MaxRank);
            cmd.Parameters.AddWithValue("@positionId", training.PositionId);
            cmd.Parameters.AddWithValue("@dapartmentId", training.DepartmentId);
            cmd.Parameters.Add("@trainingId", SqlDbType.Int).Direction = ParameterDirection.Output;
            con.Open();
            cmd.ExecuteNonQuery();
            trainingId = Convert.ToInt32(cmd.Parameters["@trainingId"].Value);
            con.Close();
            return trainingId;
        }
        public List<int> viewEmployeesForTraining(int trainingId)
        {
            List<int> employeesList= new List<int>();
            connection();
            SqlCommand cmd = new SqlCommand("getRecommendedEmployeesForTraining", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@trainingId",trainingId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
               int  employeeID = Convert.ToInt32(dr["employee_id"]);

                employeesList.Add(employeeID);
            }


            return employeesList;
        }
        // checks if there is an empty place for an employee in a given trainig returns 1 if there is an empty place 
        // and 0 otherwise
        private int checkEmptyTrainingPlace(int trainingId)
        {
            int currentNum=0;
            int maxNum=0;
            connection();
            SqlCommand cmd = new SqlCommand("getCurrentNumberOfParticipants", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@trainingId", trainingId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                currentNum = Convert.ToInt32(dr["number_of_participants"]);
                maxNum = Convert.ToInt32(dr["max_number_of_participants"]);

            }
            if(currentNum < maxNum)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }
        
        public int assignTrainingToEmloyee(int trainingId,int employeeId)
        {
           
            
            if (checkEmptyTrainingPlace(trainingId) == 1)
            {
                DateTime date = new Training().getTrainingStartDate(trainingId);
                // if date is not set yet assign employees without constrains
                if (date == null)
                {
                    connection();
                    SqlCommand cmd = new SqlCommand("assignTraining", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@trainingId", trainingId);
                    cmd.Parameters.AddWithValue("@employeeId", employeeId);


                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    // assigned successfully
                    return 1;
                }
                // if date is set check first if employees available 
                else
                {
                    connection();
                    SqlCommand cmd = new SqlCommand("isEmployeeAvailableOnDate", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@employeeId",employeeId);
                    cmd.Parameters.AddWithValue("@start_date", date);
                    cmd.Parameters.AddWithValue("@end_date", new Training().getTrainingEndDate(trainingId));
                    cmd.Parameters.Add("@count", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    int count = Convert.ToInt32(cmd.Parameters["@count"].Value);
                    if(count > 0)
                    {
                        // employee is not available at the training time
                        return 0;
                    }
                    // employee available 
                    else
                    {
                        //assign 
                        connection();
                        SqlCommand cmd_1 = new SqlCommand("assignTraining", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@trainingId", trainingId);
                        cmd.Parameters.AddWithValue("@employeeId", employeeId);


                        con.Open();
                        cmd_1.ExecuteNonQuery();
                        con.Close();
                        // assigned successfully
                       
                        return 1;

                    }
                    
                }

            }
            else
            {
                return 2;
            }
            

        }
        private DataTable trainingReportForAllEmployees(int trainingId)
        {

           

            connection();
            SqlCommand cmd = new SqlCommand("trainingReportForAllEmployees", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@trainingId", trainingId);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            // dt has 3 columns : employee_id (int), skill_rank_before_training (int),skill_rank (int)
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

           
            return dt;

        }
        private DataTable employeeTrainingReport (int employeeId)
        {
            connection();
            SqlCommand cmd = new SqlCommand("employeeTrainingReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employeeId", employeeId);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            // dt has 3 columns : training_id(int), skill_rank_before_training (int),skill_rank (int)
            DataTable dt = new DataTable();
            
            con.Open();
            sd.Fill(dt);
            con.Close();


            return dt;
        }
        private DateTime getSuitableTrainingStartTime(List<int> employeeId)
        {
            DateTime startTime = DateTime.MinValue;
            foreach (int id in employeeId)
            {
                DateTime date = new DateTime();
                connection();
                SqlCommand cmd = new SqlCommand("getLastTaskEndDate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employeeId", id);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                con.Open();
                sd.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    date = Convert.ToDateTime(dr["end_time"]);

                }
                if(startTime < date)
                {
                    startTime = date;
                }

            }
            return startTime;
        }
        public List<int> getUnAvailableEmployees(int trainingId, DateTime desiredStartDate,DateTime desiredEndDate)
        {
            List<int> employeesList = new Training().getAssignedEmployeesForTraining(trainingId);
            List<int> unAvailableList = new List<int>();
            foreach(int id in employeesList)
            {
                connection();
                SqlCommand cmd = new SqlCommand("isEmployeeAvailableOnDate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employeeId", id);
                cmd.Parameters.AddWithValue("@start_date", desiredStartDate);
                cmd.Parameters.AddWithValue("@end_date", desiredEndDate);
                cmd.Parameters.Add("@count", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                int count = Convert.ToInt32(cmd.Parameters["@count"].Value);
                if(count > 0)
                {
                    unAvailableList.Add(id);
                }
            }
            return unAvailableList;

        }
        public int unAssignEmployeeFromTraining(int trainingId,int employeeId)
        {
            int success;
            connection();
            SqlCommand cmd = new SqlCommand("removeEmployeeFromTraining", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employeeId", employeeId);
            cmd.Parameters.AddWithValue("@trainingId",trainingId);
            con.Open();
            success= cmd.ExecuteNonQuery();
            con.Close();
            return success;
        }
        public int removeTraining(int trainingId)
        {
            int success;
            connection();
            SqlCommand cmd = new SqlCommand("removeTraining", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@trainingId", trainingId);
            con.Open();
            success = cmd.ExecuteNonQuery();
            con.Close();
            return success;
        }
        public int setTrainingTime(DateTime start_date ,DateTime end_date,int trainingId)
        {
            int success;
            connection();
            SqlCommand cmd = new SqlCommand("setTrainingTime", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@trainingId", trainingId);
            cmd.Parameters.AddWithValue("@start_date", start_date.Date);
            cmd.Parameters.AddWithValue("@end_date", end_date.Date);


            con.Open();
            success= cmd.ExecuteNonQuery();
            con.Close();

            return success;

        }
        public Training viewTraining (int trainingId)
        {
            Training training = new Training();

            connection();
            SqlCommand cmd = new SqlCommand("viewTraining", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@trainingId", trainingId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                training.TrainingId = Convert.ToInt32(dr["training_id"]);
                training.TrainingName = Convert.ToString(dr["name"]);
                if (DBNull.Value.Equals(dr["start_date"]))
                {
                    training.StartDate=new DateTime();
                }
                else
                {

                    training.StartDate = Convert.ToDateTime(dr["start_date"]).Date;
                }
                if (DBNull.Value.Equals(dr["end_date"]))
                {
                    training.EndDate = new DateTime();
                }
                else
                {

                    training.EndDate = Convert.ToDateTime(dr["end_date"]).Date;
                }
                training.Location = Convert.ToString(dr["location"]);
                training.ParticipationsNum = Convert.ToInt32(dr["number_of_participants"]);
                training.HoursPerDay = Convert.ToInt32(dr["hours_per_day"]);
                training.SkillId = Convert.ToInt32(dr["skill_id"]);
                if (DBNull.Value.Equals(dr["maxRank"]))
                {
                    training.MaxRank = 0;
                }
                else
                {
                    training.MaxRank = Convert.ToInt32(dr["maxRank"]);
                }
                if (DBNull.Value.Equals(dr["positionId"]))
                {
                    training.PositionId = 0;
                }
                else
                {
                    training.PositionId = Convert.ToInt32(dr["positionId"]);
                }
                if (DBNull.Value.Equals(dr["departmentId"]))
                {
                    training.DepartmentId = 0;
                }
                else
                {
                    training.DepartmentId = Convert.ToInt32(dr["departmentId"]);
                }

                training.MaxNumOfParticipants = Convert.ToInt32(dr["max_number_of_participants"]);


            }
            return training;
        }
        public List<Training> viewListOfTraining()
        {
            List<Training> trainingList = new List<Training>();
            List<int> trainingId = new List<int>();
            connection();
            SqlCommand cmd = new SqlCommand("viewListOfTraining", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                //int id = Convert.ToInt32(dr["training_id"]);
                //trainingId.Add(id);
                Training training = new Training();
                training.TrainingId = Convert.ToInt32(dr["training_id"]);
                training.TrainingName = Convert.ToString(dr["name"]);
                training.StartDate = Convert.ToDateTime(dr["start_date"]).Date;
                training.EndDate = Convert.ToDateTime(dr["end_date"]).Date;
                training.Location = Convert.ToString(dr["location"]);
                training.ParticipationsNum = Convert.ToInt32(dr["number_of_participants"]);
                training.HoursPerDay = Convert.ToInt32(dr["hours_per_day"]);
                training.SkillId = Convert.ToInt32(dr["skill_id"]);
                if (DBNull.Value.Equals(dr["maxRank"]))
                {
                    training.MaxRank = 0;
                }
                else {
                    training.MaxRank = Convert.ToInt32(dr["maxRank"]);
                }
                if (DBNull.Value.Equals(dr["positionId"] ))
                {
                    training.PositionId = 0;
                }
                else
                {
                    training.PositionId = Convert.ToInt32(dr["positionId"]);
                }
                if (DBNull.Value.Equals(dr["departmentId"]))
                {
                    training.DepartmentId = 0;
                }
                else
                {
                    training.DepartmentId = Convert.ToInt32(dr["departmentId"]);
                }

                training.MaxNumOfParticipants = Convert.ToInt32(dr["max_number_of_participants"]);
                trainingList.Add(training);
            }
           /* foreach(int id in trainingId)
            {
                Training training = viewTraining(id);
                trainingList.Add(training);
            }*/
            return trainingList;
        }
        public DataTable employeeEnrolledInTraining(int training_id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("getEmployeeEnrolledInTraining", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@training_id", training_id);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            return dt;
        }
        public List<List<string>> getEmployeePerformanceBeforeAndAfterTraining(List<int> compatibleCandidatesId)
        {
            List<List<string>> performanceReport = new List<List<string>>();

            return performanceReport;
        }
        public List<List<string>> viewSkillsNeeded(int minSkillRankThreshold)
        {
            List<List<string>> skillsNeeded = new List<List<string>>();

            return skillsNeeded;
        }
        public List<List<string>> viewCandidateApplicantsForJob(List<int>neededSkills)
        {
            List<List<string>> suitableApplicants = new List<List<string>>();

            return suitableApplicants;
        }

        ///Attendanc Reporting 
        ///
        public DataTable viewAttendanceReportProgress(string category,string category_value,DateTime from,DateTime to)
        {
            DWconnection();
            if (category=="Employee")
            {
                SqlCommand cmd = new SqlCommand("Report_attendance_by_employee", conDW);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_name", category_value);
                cmd.Parameters.AddWithValue("@startmonth", from.Month);
                cmd.Parameters.AddWithValue("@endmonth", to.Month);
                cmd.Parameters.AddWithValue("@startyear",from.Year);
                cmd.Parameters.AddWithValue("@endyear", to.Year);

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conDW.Open();
                sd.Fill(dt);
                conDW.Close();

                return dt;
            }
            else if (category =="Department")
            {
                SqlCommand cmd = new SqlCommand("Report_attendance_by_department", conDW);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@department_name", category_value);
                cmd.Parameters.AddWithValue("@startmonth", from.Month);
                cmd.Parameters.AddWithValue("@endmonth", to.Month);
                cmd.Parameters.AddWithValue("@startyear", from.Year);
                cmd.Parameters.AddWithValue("@endyear", to.Year);

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conDW.Open();
                sd.Fill(dt);
                conDW.Close();

                return dt;
            }
            else if (category =="Position")
            {
                SqlCommand cmd = new SqlCommand("Report_attendance_by_position", conDW);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@position_name", category_value);
                cmd.Parameters.AddWithValue("@startmonth", from.Month);
                cmd.Parameters.AddWithValue("@endmonth", to.Month);
                cmd.Parameters.AddWithValue("@startyear", from.Year);
                cmd.Parameters.AddWithValue("@endyear", to.Year);

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conDW.Open();
                sd.Fill(dt);
                conDW.Close();

                return dt;
            }
            else
            {
                DataTable dt = new DataTable();
                
                return dt;
            }
            
        }

        public DataTable viewAttendanceReportComparison(string category,  DateTime date)
        {
            DWconnection();
            if (category=="Employee")
            {
                SqlCommand cmd = new SqlCommand("Report_attendance_by_employees_com", conDW);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", date.Month);
                cmd.Parameters.AddWithValue("@year", date.Year);
                

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conDW.Open();
                sd.Fill(dt);
                conDW.Close();

                return dt;
            }
            else if (category=="Department")
            {
                SqlCommand cmd = new SqlCommand("Report_attendance_by_departments_com", conDW);
                cmd.CommandType = CommandType.StoredProcedure;
                int month = date.Month;
                int year = date.Year;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conDW.Open();
                sd.Fill(dt);
                conDW.Close();

                return dt;
            }
            else if (category=="Position")
            {
                SqlCommand cmd = new SqlCommand("Report_attendance_by_positions_com", conDW);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", date.Month);
                cmd.Parameters.AddWithValue("@year", date.Year);

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conDW.Open();
                sd.Fill(dt);
                conDW.Close();

                return dt;
            }
            else
            {
                DataTable dt = new DataTable();
                return dt;
            }
            
        }

        public DataTable viewPerformanceReportProgress(string category, string category_value, DateTime from, DateTime to)
        {
            DWconnection();
            if (category == "Employee")
            {
                SqlCommand cmd = new SqlCommand("Report_performance_by_employee", conDW);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_name", category_value);
                cmd.Parameters.AddWithValue("@startmonth", from.Month);
                cmd.Parameters.AddWithValue("@endmonth", to.Month);
                cmd.Parameters.AddWithValue("@startyear", from.Year);
                cmd.Parameters.AddWithValue("@endyear", to.Year);

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conDW.Open();
                sd.Fill(dt);
                conDW.Close();

                return dt;
            }
            else if (category == "Department")
            {
                SqlCommand cmd = new SqlCommand("Report_performance_by_department", conDW);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@department_name", category_value);
                cmd.Parameters.AddWithValue("@startmonth", from.Month);
                cmd.Parameters.AddWithValue("@endmonth", to.Month);
                cmd.Parameters.AddWithValue("@startyear", from.Year);
                cmd.Parameters.AddWithValue("@endyear", to.Year);

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conDW.Open();
                sd.Fill(dt);
                conDW.Close();

                return dt;
            }
            else if (category == "Position")
            {
                SqlCommand cmd = new SqlCommand("Report_performance_by_position", conDW);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@position_name", category_value);
                cmd.Parameters.AddWithValue("@startmonth", from.Month);
                cmd.Parameters.AddWithValue("@endmonth", to.Month);
                cmd.Parameters.AddWithValue("@startyear", from.Year);
                cmd.Parameters.AddWithValue("@endyear", to.Year);

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conDW.Open();
                sd.Fill(dt);
                conDW.Close();

                return dt;
            }
            else
            {
                DataTable dt = new DataTable();

                return dt;
            }

        }

        public DataTable viewPerformanceReportComparison(string category, DateTime date)
        {
            DWconnection();
            if (category == "Employee")
            {
                SqlCommand cmd = new SqlCommand("Report_performance_by_employees_com", conDW);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", date.Month);
                cmd.Parameters.AddWithValue("@year", date.Year);


                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conDW.Open();
                sd.Fill(dt);
                conDW.Close();

                return dt;
            }
            else if (category == "Department")
            {
                SqlCommand cmd = new SqlCommand("Report_performance_by_departments_com", conDW);
                cmd.CommandType = CommandType.StoredProcedure;
                int month = date.Month;
                int year = date.Year;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);


                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conDW.Open();
                sd.Fill(dt);
                conDW.Close();

                return dt;
            }
            else if (category == "Position")
            {
                SqlCommand cmd = new SqlCommand("Report_performance_by_positions_com", conDW);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@month", date.Month);
                cmd.Parameters.AddWithValue("@year", date.Year);

                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conDW.Open();
                sd.Fill(dt);
                conDW.Close();

                return dt;
            }
            else
            {
                DataTable dt = new DataTable();
                return dt;
            }

        }

    }
}
