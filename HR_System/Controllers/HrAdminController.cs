using HR_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR_System.Controllers
{
    public class HrAdminController : Controller
    {
        // GET: HrAdmin
        HrAdmin hr = new HrAdmin();
        Utiles u = new Utiles();
        public ActionResult viewTrainingsList()
        {
            var trainings = new List<Training>();
            try
            {
                trainings = hr.viewListOfTraining();
            }
            catch(Exception e)
            {

            }
            return View(trainings);
        }
        public ActionResult addTraining()
        {
            ViewBag.skills = u.skill_list();
            ViewBag.positions = u.position_list();
            ViewBag.departments = u.department_list();
            return View();
        }

        [HttpPost]
        public ActionResult addTraining(FormCollection fc)
        {
            try
            {
                
                Training training = new Training();
                training.TrainingName = fc["name"];
                training.StartDate = Convert.ToDateTime(fc["start_date"]);
                training.EndDate = Convert.ToDateTime(fc["end_date"]);
                training.Location = fc["location"];
                training.HoursPerDay =Convert.ToInt32(fc["hours_per_day"]);
                training.SkillId = Convert.ToInt32(fc["SkillId"]);
                training.MaxRank = Convert.ToInt32(fc["maxRank"]);
                training.PositionId = Convert.ToInt32(fc["PositionId"]);
                training.DepartmentId = Convert.ToInt32(fc["DepartmentId"]);
                training.MaxNumOfParticipants = Convert.ToInt32(fc["maxNumPart"]);
                int training_id = hr.insertTraining(training);

                TempData["msg"] = "A New Training Has Been Added Successfully !";
            }
            catch
            {

                TempData["msg"] = "An error has been occured ,please try again !";
            }

            return RedirectToAction("addTraining");
        }

       
        public ActionResult attendance_progress (FormCollection fc)
        {

            string category = fc["Category"];
            string category_value = fc["category_value"];
            ViewBag.category_value = category_value;
            DateTime from = Convert.ToDateTime(fc["from"]);
            DateTime to = Convert.ToDateTime(fc["to"]);
            DataTable dt = hr.viewAttendanceReportProgress(category, category_value, from, to);

            return View(dt);
        }

        public ActionResult attendance_comparison(FormCollection fc)
        {

            string category = fc["Category"];
           
            DateTime date = Convert.ToDateTime(fc["date"]);
            
            DataTable dt = hr.viewAttendanceReportComparison(category,date);

            return View(dt);
        }

        public ActionResult performance_progress(FormCollection fc)
        {

            string category = fc["Category"];
            string category_value = fc["category_value"];
            ViewBag.category_value = category_value;
            DateTime from = Convert.ToDateTime(fc["from"]);
            DateTime to = Convert.ToDateTime(fc["to"]);
            DataTable dt = hr.viewPerformanceReportProgress(category, category_value, from, to);

            return View(dt);
        }

        public ActionResult performance_comparison(FormCollection fc)
        {

            string category = fc["Category"];

            DateTime date = Convert.ToDateTime(fc["date"]);

            DataTable dt = hr.viewPerformanceReportComparison(category, date);

            return View(dt);
        }

        public ActionResult viewTraining(int training_id =1)
        {
            Training training = hr.viewTraining(training_id);
            ViewBag.Training = training;
            ViewBag.skill_name = u.skill_name(training.SkillId);
            if(training.DepartmentId > 0)
            {
                ViewBag.Department_name = u.department_name(training.DepartmentId);
            }
            if(training.PositionId > 0)
            {
                ViewBag.Position_name = u.position_info(training.PositionId).PositionName;
            }
            DataTable dt = hr.employeeEnrolledInTraining(training_id);
            return View(dt);
        }
        public ActionResult unassignEmployeeFromTraining(int training_id,int employee_id)
        {
            hr.unAssignEmployeeFromTraining(training_id, employee_id);
            return new EmptyResult();
        }
    }
}