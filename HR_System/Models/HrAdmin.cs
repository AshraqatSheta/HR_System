using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class HrAdmin
    {
        private void addEmployee(List<string> empInfo)
        {

        }
        private List<string> viewAttendance(int id, DateTime date)
        {
            List<string> attendance = new List<string>();
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
        public void addHoliday(Holiday holiday)
        { 

       
        }
        public void addPermission(Permission permission)
        {

        }
        public void setBounaceCriteria(float minPerformanceRate, int minExperiancYear)
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

            return trainingId;
        }
        public List<List<string>> viewEmployeesForTraining(int trainingId)
        {
            List<List<string>> employeesList= new List<List<string>>();

            return employeesList;
        }
        public void assignTrainingToEmloyee(int trainingId,int employeeId)
        {

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



    }
}
