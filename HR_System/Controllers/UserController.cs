using HR_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR_System.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        User user = new User();
        public ActionResult viewAttendanceLog(FormCollection fc)
        {
            int employee_id = 10;
            
            string start_date = (fc["start_date"]);
            string end_date = (fc["end_date"]);
            var attendanceLog = user.viewAttendanceDetails(start_date, end_date, employee_id);
            return View(attendanceLog);
        }
    }
}