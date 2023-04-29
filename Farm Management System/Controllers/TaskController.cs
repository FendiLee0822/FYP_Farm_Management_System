using Farm_Management_System.farm_Management_SystemModel;
using Farm_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farm_Management_System.Controllers
{
    public class TaskController : Controller
    {
        //------------------------------Admin Part---------------------------------------

        //Show all task information for admin
        public IActionResult task_Information_Page()
        {
            using (var db = new farm_Management_SystemContext())
            {
                var tasks = db.Tasks
                    .ToList();

                return View(tasks);
            }
        }

        //-------------------------------------------------------------------------------

        //Task Create Page
        [HttpGet]
        public IActionResult task_Create_Page()
        {
            using (var db = new farm_Management_SystemContext())
            {
                var model = new task_Creation_Model();

                var project_List = db.Projects.ToList();
                model.Projects = project_List;

                var field_List = db.Fields.ToList();
                model.Fields = field_List;

                var plant_List = db.Plants.ToList();
                model.Plants = plant_List;

                var employee_List = db.Employees.ToList();
                model.Employees = employee_List;

                var userId = HttpContext.Session.GetInt32("Key1");
                var admin_Assign = db.Admins
                    .Where(a => a.UserId == (userId ?? 0))
                    .ToList();
                model.Admins = admin_Assign;

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult task_Create_Page(task_Creation_Model model)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Add Plant Information
                db.Tasks.Add(model.Task);

                //Add Create & Update Time
                DateTime create_Update_Time = DateTime.Now;
                model.Task.TaskCreateTime = create_Update_Time;
                model.Task.TaskUpdateTime = create_Update_Time;

                db.SaveChanges();
            }
            return RedirectToAction("task_Information_Page");
        }

        //-------------------------------------------------------------------------------

        //Task Detail Page
        public IActionResult task_Detail_Page(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var db = new farm_Management_SystemContext())
            {
                var tasks = db.Tasks
                    .FirstOrDefault(s => s.TaskId == id);

                return View(tasks);
            }
        }

        //-------------------------------------------------------------------------------

        //Task Update Page
        [HttpGet]
        public IActionResult task_Update_Page(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var model = new task_Creation_Model();

                //Show selected task information
                var selected_Task = db.Tasks
                    .Where(s => s.TaskId == id)
                    .SingleOrDefault();
                model.Task = selected_Task;

                var project_List = db.Projects.ToList();
                model.Projects = project_List;

                var field_List = db.Fields.ToList();
                model.Fields = field_List;

                var plant_List = db.Plants.ToList();
                model.Plants = plant_List;

                var employee_List = db.Employees.ToList();
                model.Employees = employee_List;

                var userId = HttpContext.Session.GetInt32("Key1");
                var admin_Assign = db.Admins
                    .Where(a => a.UserId == (userId ?? 0))
                    .ToList();
                model.Admins = admin_Assign;

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult task_Update_Page(task_Creation_Model model)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //var task = db.Tasks.Where(t => t.TaskId == model.Task.TaskId).FirstOrDefault();

                DateTime create_Update_Time = DateTime.Now;
                model.Task.TaskUpdateTime = create_Update_Time;
                
               // model.Task.TaskCreateTime = create_Update_Time;

                //Update selected task's other information
                db.Entry(model.Task).State = EntityState.Modified;


                db.SaveChanges();
            }
            return RedirectToAction("task_Information_page");
        }

        //-------------------------------------------------------------------------------

        //Task Delete Page
        [HttpGet]
        public IActionResult task_Delete_Page(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var db = new farm_Management_SystemContext())
            {
                var tasks = db.Tasks
                    .FirstOrDefault(s => s.TaskId == id);

                return View(tasks);
            }
        }

        [HttpPost]
        public IActionResult task_Delete_Page(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var selected_Task = db.Tasks
                    .Find(id);

                //Remove SELECTED PLANT from PLANT TABLE
                db.Remove(selected_Task);
                db.SaveChanges();

                return RedirectToAction("task_Information_Page");
            }
        }

        //-------------------------------------------------------------------------------

        //Search Task
        public ActionResult task_Search_Page(string searchBy, string search)
        {
            using (var db = new farm_Management_SystemContext())
            {
                switch (searchBy)
                {
                    case "TaskDescription":
                        return View(db.Tasks.Where(x => x.TaskDescription.StartsWith(search) || search == null).ToList());
                    case "TaskProject":
                        int projectId;
                        if (int.TryParse(search, out projectId))
                        {
                            return View(db.Tasks.Where(x => x.TaskProject == projectId).ToList());
                        }
                        else
                        {
                            return View(db.Tasks.ToList());
                        }
                    case "TaskField":
                        int FieldId;
                        if (int.TryParse(search, out FieldId))
                        {
                            return View(db.Tasks.Where(x => x.TaskField == FieldId).ToList());
                        }
                        else
                        {
                            return View(db.Tasks.ToList());
                        }
                    case "TaskPlant":
                        int plantId;
                        if (int.TryParse(search, out plantId))
                        {
                            return View(db.Tasks.Where(x => x.TaskPlant == plantId).ToList());
                        }
                        else
                        {
                            return View(db.Tasks.ToList());
                        }
                    case "TaskStatus":
                        return View(db.Tasks.Where(x => x.TaskStatus.StartsWith(search) || search == null).ToList());
                    default:
                        return View(db.Tasks.ToList());
                }
            }
        }

        //-------------------------------------------------------------------------------

        //Task Attribute Selection Page
        public IActionResult task_Attribute_Selection_View()
        {
            return View(new task_Column_Model() { Task = new Farm_Management_System.farm_Management_SystemModel.Task() });
        }

        [HttpPost]
        public IActionResult task_Attribute_Selection_View(task_Column_Selection_Model model)
        {
            return RedirectToAction("task_Created_View", new { selected_Task_Attribute = model.selected_Task_Attribute });
        }

        //Created View Page
        public IActionResult task_Created_View(List<string> selected_Task_Attribute)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var tasks = db.Tasks
                    .ToList();

                var model = new task_Column_Selection_Model();

                model.selected_Task_Attribute = selected_Task_Attribute;
                model.Tasks = tasks;

                return View(model);
            }
        }

        //------------------------------Employee Part---------------------------------------

        //Show employees' own task information
        public IActionResult task_Information_Page_Employee()
        {
            using (var db = new farm_Management_SystemContext())
            {
                var userId = HttpContext.Session.GetInt32("Key1");
                var employee_Login = db.Employees
                    .Where(a => a.UserId == (userId ?? 0))
                    .FirstOrDefault();

                var employee_task = db.Tasks
                    .Where(a => a.EmployeeIncharge == (employee_Login.EmployeeId))
                    .ToList();

                return View(employee_task);
            }
        }

        //-------------------------------------------------------------------------------

        //Task Update Page
        [HttpGet]
        public IActionResult task_Update_Page_Employee(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var model = new task_Creation_Model();

                //Show selected task information
                var selected_Task = db.Tasks
                    .Where(s => s.TaskId == id)
                    .SingleOrDefault();
                model.Task = selected_Task;

                var project_List = db.Projects.ToList();
                model.Projects = project_List;

                var field_List = db.Fields.ToList();
                model.Fields = field_List;

                var plant_List = db.Plants.ToList();
                model.Plants = plant_List;

                var userId = HttpContext.Session.GetInt32("Key1");
                var employee_Incharge = db.Employees
                    .Where(a => a.UserId == (userId ?? 0))
                    .ToList();
                model.Employees = employee_Incharge;

                var admin_Assign = db.Admins
                    .Where(a => a.AdminId == selected_Task.AdminAssigned)
                    .ToList();
                model.Admins = admin_Assign;

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult task_Update_Page_Employee(task_Creation_Model model)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Update selected task's other information
                db.Entry(model.Task).State = EntityState.Modified;

                DateTime create_Update_Time = DateTime.Now;
                //model.Task.TaskCreateTime = create_Update_Time;
                model.Task.TaskUpdateTime = create_Update_Time;

                db.SaveChanges();
            }
            return RedirectToAction("task_Information_page_Employee");
        }
    }
}
