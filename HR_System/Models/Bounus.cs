using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Bounus
    {
        private int bounusId;
        private string description;
        private int minPerformance;
        private string position;
        private int minThresholdTime;

        public int BounusId { get => bounusId; set => bounusId = value; }
        public string Description { get => description; set => description = value; }
        public int MinPerformance { get => minPerformance; set => minPerformance = value; }
        public string Position { get => position; set => position = value; }
        public int MinThresholdTime { get => minThresholdTime; set => minThresholdTime = value; }
    }
}