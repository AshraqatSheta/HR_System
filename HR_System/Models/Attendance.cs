using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Attendance
    {
        private int employeeId;
        private DateTime arrivalTime;
        private DateTime leaveTime;
        private DateTime date;
        private string employeeName;

        public int EmployeeId { get => employeeId; set => employeeId = value; }
        public DateTime ArrivalTime { get => arrivalTime; set => arrivalTime = value; }
        public DateTime LeaveTime { get => leaveTime; set => leaveTime = value; }
        public DateTime Date { get => date; set => date = value; }
        public string EmployeeName { get => employeeName; set => employeeName = value; }
    }
}