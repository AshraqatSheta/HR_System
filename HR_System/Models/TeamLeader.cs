using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class TeamLeader
    {
        private int departmentId;
        private Project project;

        public int createProject(Project project)
        {
            int projectId;

            return projectId;

        }
        public int createTask(Task task)
        {
            int taskId;

            return taskId

        }
        public List<List<string>> getEployeeForTask(Task task)
        {
            List<List<string>> employees = new List<List<string>>();

            return employees;
        }
        public void assignEmployeeForTask(int employeeId,Task task)
        {

        }


        public int DepartmentId { get => departmentId; set => departmentId = value; }
        public Project Project { get => project; set => project = value; }
    }
}