using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Training
    {
        private int trainingId;
        private string trainingName;
        private DateTime startDate;
        private DateTime endDate;
        private string location;
        private List<int> skills;
        private int participationsNum;
        private int hoursPerDay;

        public int TrainingId { get => trainingId; set => trainingId = value; }
        public string TrainingName { get => trainingName; set => trainingName = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public string Location { get => location; set => location = value; }
        public List<int> Skills { get => skills; set => skills = value; }
        public int ParticipationsNum { get => participationsNum; set => participationsNum = value; }
        public int HoursPerDay { get => hoursPerDay; set => hoursPerDay = value; }
    }
}