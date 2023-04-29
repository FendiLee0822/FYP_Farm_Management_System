using Farm_Management_System.farm_Management_SystemModel;
using Farm_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farm_Management_System.Controllers
{
/*    [ApiController]
    [Route("api/[controller]")]*/
    public class ProjectController : Controller
    {
        //------------------------------Admin Part---------------------------------------

        //Show all project information for admin
        public IActionResult project_Information_Page()
        {
            using (var db = new farm_Management_SystemContext())
            {
                var projects = db.Projects
                    .ToList();

                return View(projects);
            }
        }

        //-------------------------------------------------------------------------------

        //Project Create Page
        [HttpGet]
        public IActionResult project_Create_Page()
        {
            using (var db = new farm_Management_SystemContext())
            {
                var userId = HttpContext.Session.GetInt32("Key1");
                var admin_Update = db.Admins
                    .Where(a => a.UserId == (userId ?? 0))
                    .ToList();

                var model = new project_Admin_Model();
                model.Admins = admin_Update;
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult project_Create_Page(project_Admin_Model model)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Add Project Information
                db.Projects.Add(model.Project);

                //Add Create Time
                DateTime create_Time = DateTime.Now;
                model.Project.ProjectCreateTime = create_Time;

                db.SaveChanges();
            }
            return RedirectToAction("project_Information_Page");
        }

        //-------------------------------------------------------------------------------

        //Project Detail Page
        public IActionResult project_Detail_Page(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var db = new farm_Management_SystemContext())
            {
                var projects = db.Projects
                    .FirstOrDefault(s => s.ProjectId == id);

                var project_Tasks = db.Tasks
                    .Where(a => a.TaskProject == id)
                    .ToList();

                var project_Growths = db.Growths
                    .Where(a => a.GrowthProject == id)
                    .ToList();

                var project_Financials = db.Financials
                    .Where(a => a.TransactionProject == id)
                    .ToList();

                var model = new project_Detail_Model();
                model.Tasks = project_Tasks;
                model.Growths = project_Growths;
                model.Financials = project_Financials;
                model.Projects = projects;
                return View(model);

            }
        }

        //-------------------------------------------------------------------------------

        //Project Update Page
        [HttpGet]
        public IActionResult project_Update_Page(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Show selected project information
                var selected_Project = db.Projects
                    .Where(s => s.ProjectId == id)
                    .SingleOrDefault();

                //Assign ADMIN ID into ADMIN ID UPDATE selection
                var userId = HttpContext.Session.GetInt32("Key1");
                var admin_Update = db.Admins
                    .Where(a => a.UserId == (userId ?? 0))
                    .ToList();

                //Assign both into model
                var model = new project_Admin_Model();
                model.Admins = admin_Update;
                model.Project = selected_Project;

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult project_Update_Page(project_Admin_Model model)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Update selected project's other information
                db.Entry(model.Project).State = EntityState.Modified;

                //Update UPDATE TIME
                DateTime update_Time = DateTime.Now;
                model.Project.ProjectCreateTime = update_Time;

                db.SaveChanges();
            }
            return RedirectToAction("project_Information_page");
        }

        //-------------------------------------------------------------------------------

        //Project Delete Page
        [HttpGet]
        public IActionResult project_Delete_Page(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var db = new farm_Management_SystemContext())
            {
                var projects = db.Projects
                    .FirstOrDefault(s => s.ProjectId == id);

                return View(projects);
            }
        }

        [HttpPost]
        public IActionResult project_Delete_Page(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var selected_project = db.Projects
                    .Find(id);

                db.Remove(selected_project);
                db.SaveChanges();

                return RedirectToAction("project_Information_Page");
            }
        }

        //-------------------------------------------------------------------------------

        //Search Project
        public ActionResult project_Search_Page(string searchBy, string search)
        {
            using (var db = new farm_Management_SystemContext())
            {
                switch (searchBy)
                {
                    case "ProjectName":
                        return View(db.Projects.Where(x => x.ProjectName.StartsWith(search) || search == null).ToList());
                    default:
                        return View(db.Projects.ToList());
                }
            }
        }

        //-------------------------------------------------------------------------------

        //Project Attribute Selection Page
        public IActionResult project_Attribute_Selection_View()
        {
            return View(new project_Column_Model() { Project = new Project() });
        }

        [HttpPost]
        public IActionResult project_Attribute_Selection_View(project_Column_Selection_Model model)
        {
            return RedirectToAction("project_Created_View", new { selected_Project_Attribute = model.selected_Project_Attribute });
        }

        //Created View Page
        public IActionResult project_Created_View(List<string> selected_Project_Attribute)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var projects = db.Projects
                    .ToList();

                var model = new project_Column_Selection_Model();

                model.selected_Project_Attribute = selected_Project_Attribute;
                model.Projects = projects;

                return View(model);
            }
        }

        //-------------------------------------------------------------------------------

        /*public interface IServiceClientProvider
        {
            Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, string endpoint);
            Task<IDisposable> CreateClientAsync();
        }


        private readonly ILogger<ProjectController> _logger;
        private readonly IServiceClientProvider _serviceClientProvider;

        public ProjectController(ILogger<ProjectController> logger, IServiceClientProvider serviceClientProvider)
        {
            _logger = logger;
            _serviceClientProvider = serviceClientProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetFieldDetails(string entityLogicalName, string fieldName)
        {
            using (var svc = await _serviceClientProvider.CreateClientAsync())
            {
                var request = new RetrieveAttributeRequest
                {
                    EntityLogicalName = entityLogicalName,
                    LogicalName = fieldName,
                    RetrieveAsIfPublished = true
                };
                var response = (RetrieveAttributeResponse)svc.Execute(request);
                var attributeMetadata = (EnumAttributeMetadata)response.AttributeMetadata;

                // Here, you can return the attributeMetadata object, or extract specific properties from it
                return Ok(attributeMetadata);
            }
        }*/
    }
}
