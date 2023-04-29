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
    public class GrowthController : Controller
    {
        //------------------------------Admin Part---------------------------------------

        //Show all growth information for admin
        public IActionResult growth_Information_Page()
        {
            using (var db = new farm_Management_SystemContext())
            {
                var growths = db.Growths
                    .ToList();

                return View(growths);
            }
        }

        //-------------------------------------------------------------------------------

        //Growth Create Page
        [HttpGet]
        public IActionResult growth_Create_Page()
        {
            using (var db = new farm_Management_SystemContext())
            {
                var model = new growth_Creation_Model();

                var project_List = db.Projects.ToList();
                model.Projects = project_List;

                var plant_List = db.Plants.ToList();
                model.Plants = plant_List;

                var field_List = db.Fields.ToList();
                model.Fields = field_List;

                var userId = HttpContext.Session.GetInt32("Key1");
                var user_Update = db.Users
                    .Where(a => a.UserId == (userId ?? 0))
                    .ToList();
                model.Users = user_Update;

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult growth_Create_Page(growth_Creation_Model model)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Add growth Information
                db.Growths.Add(model.Growth);

                //Add Update Time
                DateTime update_Time = DateTime.Now;
                model.Growth.GrowthUpdateTime = update_Time;

                db.SaveChanges();
            }
            return RedirectToAction("growth_Information_Page");
        }

        //-------------------------------------------------------------------------------

        //Growth Detail Page
        public IActionResult growth_Detail_Page(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var db = new farm_Management_SystemContext())
            {
                var growths = db.Growths
                    .FirstOrDefault(s => s.GrowthId == id);

                return View(growths);
            }
        }

        //-------------------------------------------------------------------------------

        //Growth Update Page
        [HttpGet]
        public IActionResult growth_Update_Page(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var model = new growth_Creation_Model();

                //Show selected plant information
                var selected_Growth = db.Growths
                    .Where(s => s.GrowthId == id)
                    .SingleOrDefault();
                model.Growth = selected_Growth;

                var project_List = db.Projects.ToList();
                model.Projects = project_List;

                var plant_List = db.Plants.ToList();
                model.Plants = plant_List;

                var field_List = db.Fields.ToList();
                model.Fields = field_List;

                //Assign ADMIN ID into ADMIN ID UPDATE selection
                var userId = HttpContext.Session.GetInt32("Key1");
                var user_Update = db.Users
                    .Where(a => a.UserId == (userId ?? 0))
                    .ToList();
                model.Users = user_Update;

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult growth_Update_Page(growth_Creation_Model model)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Update selected plant's other information
                db.Entry(model.Growth).State = EntityState.Modified;

                //Update UPDATE TIME
                DateTime update_Time = DateTime.Now;
                model.Growth.GrowthUpdateTime = update_Time;

                db.SaveChanges();
            }
            return RedirectToAction("growth_Information_page");
        }

        //-------------------------------------------------------------------------------

        //Growth Delete Page
        [HttpGet]
        public IActionResult growth_Delete_Page(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var db = new farm_Management_SystemContext())
            {
                var growths = db.Growths
                    .FirstOrDefault(s => s.GrowthId == id);

                return View(growths);
            }
        }

        [HttpPost]
        public IActionResult growth_Delete_Page(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var selected_Growth = db.Growths
                    .Find(id);

                //Remove SELECTED GROWTH from GROWTH TABLE
                db.Remove(selected_Growth);
                db.SaveChanges();

                return RedirectToAction("growth_Information_Page");
            }
        }

        //-------------------------------------------------------------------------------

        //Search Growth
        public ActionResult growth_Search_Page(string searchBy, string search)
        {
            using (var db = new farm_Management_SystemContext())
            {
                switch (searchBy)
                {
                    case "GrowthProject":
                        int projectId;
                        if (int.TryParse(search, out projectId))
                        {
                            return View(db.Growths.Where(x => x.GrowthProject == projectId).ToList());
                        }
                        else
                        {
                            return View(db.Growths.ToList());
                        }
                    case "GrowthPlant":
                        int plantId;
                        if (int.TryParse(search, out plantId))
                        {
                            return View(db.Growths.Where(x => x.GrowthPlant == plantId).ToList());
                        }
                        else
                        {
                            return View(db.Growths.ToList());
                        }
                    case "GrowthField":
                        int FieldId;
                        if (int.TryParse(search, out FieldId))
                        {
                            return View(db.Growths.Where(x => x.GrowthField == FieldId).ToList());
                        }
                        else
                        {
                            return View(db.Growths.ToList());
                        }
                    case "GrowthStatus":
                        return View(db.Growths.Where(x => x.GrowthStatus.StartsWith(search) || search == null).ToList());
                    default:
                        return View(db.Growths.ToList());
                }
            }
        }

        //-------------------------------------------------------------------------------

        //Growth Attribute Selection Page
        public IActionResult growth_Attribute_Selection_View()
        {
            return View(new growth_Column_Model() { Growth = new Growth() });
        }

        [HttpPost]
        public IActionResult growth_Attribute_Selection_View(growth_Column_Selection_Model model)
        {
            return RedirectToAction("growth_Created_View", new { selected_Growth_Attribute = model.selected_Growth_Attribute });
        }

        //Created View Page
        public IActionResult growth_Created_View(List<string> selected_Growth_Attribute)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var growths = db.Growths
                    .ToList();

                var model = new growth_Column_Selection_Model();

                model.selected_Growth_Attribute = selected_Growth_Attribute;
                model.Growths = growths;

                return View(model);
            }
        }
    }
}
