using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Employee
    {

        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["HRCon"].ToString();
            con = new SqlConnection(constring);
        }
        public void updateStartTaskTime(int employeeId,int TaskId ,DateTime startTime)
        {

        }
        public void updateEndTaskTime(int employeeId, int TaskId, DateTime endTime)
        {

        }
        public List<List<string>> viewTasksList (int employeeId)
        {
            List<List<string>> tasks = new List<List<string>>();

            return tasks;
        }
        public List<List<string>> viewPerformanceReport(int employeeId)
        {
            List<List<string>> performance = new List<List<string>>();

            return performance;
        }
        public void giveFeedback(Feedback feedback)
        {

        }

        public List<Training> viewEnrolledInTraining(int employeeId)
        {
            List<Training> trainingList = new List<Training>();
            
            connection();
            SqlCommand cmd = new SqlCommand("viewTrainingForEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employeeId", employeeId);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                Training training = new Training();
                training.TrainingId = Convert.ToInt32(dr["training_id"]);
                training.TrainingName = Convert.ToString(dr["name"]);
                training.StartDate = Convert.ToDateTime(dr["start_date"]).Date;
                training.EndDate = Convert.ToDateTime(dr["end_date"]).Date;
                training.Location = Convert.ToString(dr["location"]);
                training.ParticipationsNum = Convert.ToInt32(dr["number_of_participants"]);
                training.HoursPerDay = Convert.ToInt32(dr["hours_per_day"]);
                training.SkillId = Convert.ToInt32(dr["skill_id"]);
                training.MaxRank = Convert.ToInt32(dr["maxRank"]);
                training.PositionId = Convert.ToInt32(dr["positionId"]);
                training.DepartmentId = Convert.ToInt32(dr["departmentId"]);
                training.MaxNumOfParticipants = Convert.ToInt32(dr["max_number_of_participants"]);
                trainingList.Add(training);
            }
            return trainingList;
        }
    }


}