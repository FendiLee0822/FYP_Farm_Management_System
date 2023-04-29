using CsvHelper;
using Farm_Management_System.farm_Management_SystemModel;
using Farm_Management_System.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm_Management_System.Controllers
{
    public class PlantController : Controller
    {
        //------------------------------Admin Part---------------------------------------

        //Show all plant information for admin
        public IActionResult plant_Information_Page()
        {
            using (var db = new farm_Management_SystemContext())
            {
                var plants = db.Plants
                    .ToList();

                return View(plants);
            }
        }

        //-------------------------------------------------------------------------------

        //Plant Create Page
        [HttpGet]
        public IActionResult plant_Create_Page()
        {
            using (var db = new farm_Management_SystemContext())
            {
                var userId = HttpContext.Session.GetInt32("Key1");
                var admin_Update = db.Admins
                    .Where(a => a.UserId == (userId ?? 0))
                    .ToList();

                var model = new plant_Admin_Model();
                model.Admins = admin_Update;
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult plant_Create_Page(plant_Admin_Model model)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Add Plant Information
                db.Plants.Add(model.Plant);

                //Add Update Time
                DateTime update_Time = DateTime.Now;
                model.Plant.PlantUpdateTime = update_Time;

                db.SaveChanges();
            }
            return RedirectToAction("plant_Information_Page");
        }

        //-------------------------------------------------------------------------------

        //Plant Detail Page
        public IActionResult plant_Detail_Page(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var db = new farm_Management_SystemContext())
            {
                var plants = db.Plants
                    .FirstOrDefault(s => s.PlantId == id);

                return View(plants);
            }
        }

        //-------------------------------------------------------------------------------

        //Plant Update Page
        [HttpGet]
        public IActionResult plant_Update_Page(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Show selected plant information
                var selected_Plant = db.Plants
                    .Where(s => s.PlantId == id)
                    .SingleOrDefault();

                //Assign ADMIN ID into ADMIN ID UPDATE selection
                var userId = HttpContext.Session.GetInt32("Key1");
                var admin_Update = db.Admins
                    .Where(a => a.UserId == (userId ?? 0))
                    .ToList();

                //Assign both into model
                var model = new plant_Admin_Model();
                model.Admins = admin_Update;
                model.Plant = selected_Plant;

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult plant_Update_Page(plant_Admin_Model model)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Update selected plant's other information
                db.Entry(model.Plant).State = EntityState.Modified;

                //Update UPDATE TIME
                DateTime update_Time = DateTime.Now;
                model.Plant.PlantUpdateTime = update_Time;

                db.SaveChanges();
            }
            return RedirectToAction("plant_Information_page");
        }

        //-------------------------------------------------------------------------------

        //User Delete Page
        [HttpGet]
        public IActionResult plant_Delete_Page(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var db = new farm_Management_SystemContext())
            {
                var plants = db.Plants
                    .FirstOrDefault(s => s.PlantId == id);

                return View(plants);
            }
        }

        [HttpPost]
        public IActionResult plant_Delete_Page(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var selected_Plant = db.Plants
                    .Find(id);

                //Remove SELECTED PLANT from PLANT TABLE
                db.Remove(selected_Plant);
                db.SaveChanges();

                return RedirectToAction("plant_Information_Page");
            }
        }

        //-------------------------------------------------------------------------------

        //Plant Import Page
        [HttpGet]
        public IActionResult plant_Import_Page(List<Plant> plants = null)
        {
            plants = plants == null ? new List<Plant>() : plants;
            return View(plants);
        }

        [HttpPost]
        [Obsolete]
        public IActionResult plant_Import_Page(IFormFile file, [FromServices] IHostingEnvironment hostingEnvironment)
        {
            #region Upload CSV
            string filename = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";
            using (FileStream fileStream = System.IO.File.Create(filename))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            #endregion

            var plants = this.get_Csv_List(file.FileName);

            using (var db = new farm_Management_SystemContext())
            {
                #region Save to Database
                foreach (var plant in plants)
                {
                    db.Plants.Add(plant);
                }
                db.SaveChanges();
                #endregion
            }


            return plant_Import_Page(plants);
        }

        private List<Plant> get_Csv_List(string filename)
        {
            List<Plant> plants = new List<Plant>();

            #region Read CSV
            var path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + filename;
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<csvmap_Plant>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var plant = csv.GetRecord<Plant>();
                    plant.PlantId = 0;
                    plants.Add(plant);
                }
            }
            #endregion
            return plants;
        }

        //-------------------------------------------------------------------------------

        //Export PLANT DATA to CSV File
        [HttpPost]
        public FileResult export_Plant_CSV()
        {
            using (var db = new farm_Management_SystemContext())
            {
                List<object> plants = (from plant in db.Plants.ToList().Take(10)
                                      select new[]
{
                    plant.PlantId.ToString(),
                    plant.PlantName,
                    plant.PlantBugTime,
                    plant.PlantFloweringTime,
                    plant.PlantSetTime,
                    plant.PlantGrowthTime,
                    plant.PlantRipeningTime,
                    plant.AdminUpdate.ToString(),
                    plant.PlantUpdateTime.ToString(),
                }).ToList<object>();

                plants.Insert(0, new string[9] { "PlantId", "PlantName", "PlantBugTime", "PlantFloweringTime", "PlantSetTime", "PlantGrowthTime", "PlantRipeningTime", "AdminUpdate", "PlantUpdateTime"});

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < plants.Count; i++)
                {
                    string[] plant_Data = (string[])plants[i];
                    for (int j = 0; j < plant_Data.Length; j++)
                    {
                        //Append data with separator.
                        sb.Append(plant_Data[j] + ',');
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }
                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Plant.csv");
            }
        }

        //-------------------------------------------------------------------------------

        //Search Plant
        public ActionResult plant_Search_Page(string searchBy, string search)
        {
            using (var db = new farm_Management_SystemContext())
            {
                switch (searchBy)
                {
                    case "PlantName":
                        return View(db.Plants.Where(x => x.PlantName.StartsWith(search) || search == null).ToList());
                    case "PlantBugTime":
                        return View(db.Plants.Where(x => x.PlantBugTime.StartsWith(search)).ToList());
                    case "PlantFloweringTime":
                        return View(db.Plants.Where(x => x.PlantFloweringTime.StartsWith(search)).ToList());
                    case "PlantSetTime":
                        return View(db.Plants.Where(x => x.PlantSetTime.StartsWith(search)).ToList());
                    case "PlantGrowthTime":
                        return View(db.Plants.Where(x => x.PlantGrowthTime.StartsWith(search)).ToList());
                    case "PlantRipeningTime":
                        return View(db.Plants.Where(x => x.PlantRipeningTime.StartsWith(search)).ToList());
                    default:
                        return View(db.Plants.ToList());
                }
            }
        }

        //-------------------------------------------------------------------------------

        //Plant Attribute Selection Page
        public IActionResult plant_Attribute_Selection_View()
        {
            return View(new plant_Column_Model() { Plant = new Plant() });
        }

        [HttpPost]
        public IActionResult plant_Attribute_Selection_View(plant_Column_Selection_Model model)
        {
            return RedirectToAction("plant_Created_View", new { selected_Plant_Attribute = model.selected_Plant_Attribute });
        }

        //Created View Page
        public IActionResult plant_Created_View(List<string> selected_Plant_Attribute)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var plants = db.Plants
                    .ToList();

                var model = new plant_Column_Selection_Model();

                model.selected_Plant_Attribute = selected_Plant_Attribute;
                model.Plants = plants;

                return View(model);
            }
        }

        //------------------------------Employee Part---------------------------------------

        //Show all plant information for admin
        public IActionResult plant_Information_Page_Employee()
        {
            using (var db = new farm_Management_SystemContext())
            {
                var plants = db.Plants
                    .ToList();

                return View(plants);
            }
        }

        //-------------------------------------------------------------------------------

        //Search Plant
        public ActionResult plant_Search_Page_Employee(string searchBy, string search)
        {
            using (var db = new farm_Management_SystemContext())
            {
                switch (searchBy)
                {
                    case "PlantName":
                        return View(db.Plants.Where(x => x.PlantName.StartsWith(search) || search == null).ToList());
                    case "PlantBugTime":
                        return View(db.Plants.Where(x => x.PlantBugTime.StartsWith(search)).ToList());
                    case "PlantFloweringTime":
                        return View(db.Plants.Where(x => x.PlantFloweringTime.StartsWith(search)).ToList());
                    case "PlantSetTime":
                        return View(db.Plants.Where(x => x.PlantSetTime.StartsWith(search)).ToList());
                    case "PlantGrowthTime":
                        return View(db.Plants.Where(x => x.PlantGrowthTime.StartsWith(search)).ToList());
                    case "PlantRipeningTime":
                        return View(db.Plants.Where(x => x.PlantRipeningTime.StartsWith(search)).ToList());
                    default:
                        return View(db.Plants.ToList());
                }
            }
        }

        //-------------------------------------------------------------------------------

        //Plant Attribute Selection Page
        public IActionResult plant_Attribute_Selection_View_Employee()
        {
            return View(new plant_Column_Model() { Plant = new Plant() });
        }

        [HttpPost]
        public IActionResult plant_Attribute_Selection_View_Employee(plant_Column_Selection_Model model)
        {
            return RedirectToAction("plant_Created_View_Employee", new { selected_Plant_Attribute = model.selected_Plant_Attribute });
        }

        //Created View Page
        public IActionResult plant_Created_View_Employee(List<string> selected_Plant_Attribute)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var plants = db.Plants
                    .ToList();

                var model = new plant_Column_Selection_Model();

                model.selected_Plant_Attribute = selected_Plant_Attribute;
                model.Plants = plants;

                return View(model);
            }
        }
    }
}
