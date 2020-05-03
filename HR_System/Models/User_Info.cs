using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class User_Info
    {
        private int user_id;
        private string userName;
        private string password;
        private int department_id;
        private string position;
        private string fullName;
        private string email;
        private decimal salary;
        private string phoneNumber;
        private string ssn;
        private string address;
        private DateTime startDate;
        private string gendre;
        private DateTime birthDate;
        private string educationalDrgree;
        private DateTime graduationDate;
        private string notes;

        public int User_id { get => user_id; set => user_id = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }
        public int Department_id { get => department_id; set => department_id = value; }
        public string Position { get => position; set => position = value; }
        public string FullName { get => fullName; set => fullName = value; }
        public string Email { get => email; set => email = value; }
        public decimal Salary { get => salary; set => salary = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string Ssn { get => ssn; set => ssn = value; }
        public string Address { get => address; set => address = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public string Gendre { get => gendre; set => gendre = value; }
        public DateTime BirthDate { get => birthDate; set => birthDate = value; }
        public string EducationalDrgree { get => educationalDrgree; set => educationalDrgree = value; }
        public DateTime GraduationDate { get => graduationDate; set => graduationDate = value; }
        public string Notes { get => notes; set => notes = value; }
    }
}