using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Employee
    {
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
    }
}