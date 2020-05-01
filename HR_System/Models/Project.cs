using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Project
    {
        private int projectId;
        private string projectName;
        private int departmentId;
        private TeamLeader teamLeader;
        private string clientName;
        private int performance;
        private DateTime startDate;
        private DateTime endDate;
        private List<Task> task;

        public int ProjectId { get => projectId; set => projectId = value; }
        public string ProjectName { get => projectName; set => projectName = value; }
        public int DepartmentId { get => departmentId; set => departmentId = value; }
        public TeamLeader TeamLeader { get => teamLeader; set => teamLeader = value; }
        public string ClientName { get => clientName; set => clientName = value; }
        public int Performance { get => performance; set => performance = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public List<Task> Task { get => task; set => task = value; }
    }
}