using HR_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR_System.Controllers
{
    public class EmployeeController : Controller
    {
        User user = new User();
        // GET: Employee
      
        [ValidateAntiForgeryToken]
        public ActionResult viewAttendanceList(string start_date, string end_date)
        {
            string var = user.viewAttendance(user.User_id,DateTime.Parse(start_date), DateTime.Parse(end_date));

            return View(var.ToList());
        }

        
    }
}
