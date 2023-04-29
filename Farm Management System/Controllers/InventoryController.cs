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
    public class InventoryController : Controller
    {
        //------------------------------Admin Part---------------------------------------

        //Show all inventory information for admin
        public IActionResult inventory_Information_Page()
        {
            using (var db = new farm_Management_SystemContext())
            {
                var inventory = db.Inventories
                    .ToList();

                return View(inventory);
            }
        }

        //-------------------------------------------------------------------------------

        //Inventory Create Page
        [HttpGet]
        public IActionResult inventory_Create_Page()
        {
            using (var db = new farm_Management_SystemContext())
            {
                var userId = HttpContext.Session.GetInt32("Key1");
                var user_Update = db.Users
                    .Where(a => a.UserId == (userId ?? 0))
                    .ToList();

                var model = new inventory_User_Model();
                model.Users = user_Update;
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult inventory_Create_Page(inventory_User_Model model)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Add Inventory Information
                db.Inventories.Add(model.Inventory);

                //Add Update Time
                DateTime update_Time = DateTime.Now;
                model.Inventory.InventoryUpdateTime = update_Time;

                db.SaveChanges();
            }
            return RedirectToAction("inventory_Information_Page");
        }

        //-------------------------------------------------------------------------------

        //Plant Detail Page
        public IActionResult inventory_Detail_Page(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var db = new farm_Management_SystemContext())
            {
                var inventorys = db.Inventories
                    .FirstOrDefault(s => s.InventoryId == id);

                return View(inventorys);
            }
        }

        //-------------------------------------------------------------------------------

        //Inventory Update Page
        [HttpGet]
        public IActionResult inventory_Update_Page(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Show selected inventory information
                var selected_Inventory = db.Inventories
                    .Where(s => s.InventoryId == id)
                    .SingleOrDefault();

                //Assign ADMIN ID into ADMIN ID UPDATE selection
                var userId = HttpContext.Session.GetInt32("Key1");
                var user_Update = db.Users
                    .Where(a => a.UserId == (userId ?? 0))
                    .ToList();

                //Assign both into model
                var model = new inventory_User_Model();
                model.Users= user_Update;
                model.Inventory = selected_Inventory;

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult inventory_Update_Page(inventory_User_Model model)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Update selected inventory's other information
                db.Entry(model.Inventory).State = EntityState.Modified;

                //Update UPDATE TIME
                DateTime update_Time = DateTime.Now;
                model.Inventory.InventoryUpdateTime = update_Time;

                db.SaveChanges();
            }
            return RedirectToAction("inventory_Information_Page");
        }

        //-------------------------------------------------------------------------------

        //User Delete Page
        [HttpGet]
        public IActionResult inventory_Delete_Page(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var db = new farm_Management_SystemContext())
            {
                var inventorys = db.Inventories
                    .FirstOrDefault(s => s.InventoryId == id);

                return View(inventorys);
            }
        }

        [HttpPost]
        public IActionResult inventory_Delete_Page(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var selected_Inventory = db.Inventories
                    .Find(id);

                //Remove SELECTED INVENTORY from INVENTORY TABLE
                db.Remove(selected_Inventory);
                db.SaveChanges();

                return RedirectToAction("inventory_Information_Page");
            }
        }

        //-------------------------------------------------------------------------------

        //Inventory Import Page
        [HttpGet]
        public IActionResult inventory_Import_Page(List<Inventory> inventorys = null)
        {
            inventorys = inventorys == null ? new List<Inventory>() : inventorys;
            return View(inventorys);
        }

        [HttpPost]
        [Obsolete]
        public IActionResult inventory_Import_Page(IFormFile file, [FromServices] IHostingEnvironment hostingEnvironment)
        {
            #region Upload CSV
            string filename = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";
            using (FileStream fileStream = System.IO.File.Create(filename))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            #endregion

            var inventorys = this.get_Csv_List(file.FileName);

            using (var db = new farm_Management_SystemContext())
            {
                #region Save to Database
                foreach (var inventory in inventorys)
                {
                    db.Inventories.Add(inventory);
                }
                db.SaveChanges();
                #endregion
            }

            return inventory_Import_Page(inventorys);
        }

        private List<Inventory> get_Csv_List(string filename)
        {
            List<Inventory> inventorys = new List<Inventory>();

            #region Read CSV
            var path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + filename;
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<csvmap_Inventory>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var inventory = csv.GetRecord<Inventory>();
                    inventory.InventoryId = 0;
                    inventorys.Add(inventory);
                }
            }
            #endregion
            return inventorys;
        }

        //-------------------------------------------------------------------------------

        //Export INVENTORY DATA to CSV File
        [HttpPost]
        public FileResult export_Inventory_CSV()
        {
            using (var db = new farm_Management_SystemContext())
            {
                List<object> inventorys = (from inventory in db.Inventories.ToList().Take(10)
                                       select new[]
 {
                    inventory.InventoryId.ToString(),
                    inventory.InventoryName,
                    inventory.InventoryAmount.ToString(),
                    inventory.UserUpdate.ToString(),
                    inventory.InventoryUpdateTime.ToString(),
                }).ToList<object>();

                inventorys.Insert(0, new string[5] { "InventoryId", "InventoryName", "InventoryAmount", "UserUpdate", "InventoryUpdateTime" });

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < inventorys.Count; i++)
                {
                    string[] inventory_Data = (string[])inventorys[i];
                    for (int j = 0; j < inventory_Data.Length; j++)
                    {
                        //Append data with separator.
                        sb.Append(inventory_Data[j] + ',');
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }
                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Inventory.csv");
            }
        }

        //-------------------------------------------------------------------------------

        //Search Inventory
        public ActionResult inventory_Search_Page(string search)
        {
            using (var db = new farm_Management_SystemContext())
            {
                return View(db.Inventories.Where(x => x.InventoryName.StartsWith(search) || search == null).ToList());
            }
        }

        //-------------------------------------------------------------------------------

        //Inventory Attribute Selection Page
        public IActionResult inventory_Attribute_Selection_View()
        {
            return View(new inventory_Column_Model() { Inventory = new Inventory() });
        }

        [HttpPost]
        public IActionResult inventory_Attribute_Selection_View(inventory_Column_Selection_Model model)
        {
            return RedirectToAction("inventory_Created_View", new { selected_Inventory_Attribute = model.selected_Inventory_Attribute });
        }

        //Created View Page
        public IActionResult inventory_Created_View(List<string> selected_Inventory_Attribute)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var inventorys = db.Inventories
                    .ToList();

                var model = new inventory_Column_Selection_Model();

                model.selected_Inventory_Attribute = selected_Inventory_Attribute;
                model.Inventorys = inventorys;

                return View(model);
            }
        }

        //------------------------------Employee Part---------------------------------------

        //Show all inventory information for admin
        public IActionResult inventory_Information_Page_Employee()
        {
            using (var db = new farm_Management_SystemContext())
            {
                var inventorys = db.Inventories
                    .ToList();

                return View(inventorys);
            }
        }

        //-------------------------------------------------------------------------------

        //Inventory Update Page
        [HttpGet]
        public IActionResult inventory_Update_Page_Employee(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Show selected inventory information
                var selected_Inventory = db.Inventories
                    .Where(s => s.InventoryId == id)
                    .SingleOrDefault();

                //Assign ADMIN ID into ADMIN ID UPDATE selection
                var userId = HttpContext.Session.GetInt32("Key1");
                var user_Update = db.Users
                    .Where(a => a.UserId == (userId ?? 0))
                    .ToList();

                //Assign both into model
                var model = new inventory_User_Model();
                model.Users = user_Update;
                model.Inventory = selected_Inventory;

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult inventory_Update_Page_Employee(inventory_User_Model model)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Update selected inventory's other information
                db.Entry(model.Inventory).State = EntityState.Modified;

                //Update UPDATE TIME
                DateTime update_Time = DateTime.Now;
                model.Inventory.InventoryUpdateTime = update_Time;

                db.SaveChanges();
            }
            return RedirectToAction("inventory_Information_Page_Employee");
        }

        //-------------------------------------------------------------------------------

        //Search Inventory
        public ActionResult inventory_Search_Page_Employee(string search)
        {
            using (var db = new farm_Management_SystemContext())
            {
                return View(db.Inventories.Where(x => x.InventoryName.StartsWith(search) || search == null).ToList());
            }
        }

        //-------------------------------------------------------------------------------

        //Inventory Attribute Selection Page
        public IActionResult inventory_Attribute_Selection_View_Employee()
        {
            return View(new inventory_Column_Model() { Inventory = new Inventory() });
        }

        [HttpPost]
        public IActionResult inventory_Attribute_Selection_View_Employee(inventory_Column_Selection_Model model)
        {
            return RedirectToAction("inventory_Created_View_Employee", new { selected_Inventory_Attribute = model.selected_Inventory_Attribute });
        }

        //Created View Page
        public IActionResult inventory_Created_View_Employee(List<string> selected_Inventory_Attribute)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var inventorys = db.Inventories
                    .ToList();

                var model = new inventory_Column_Selection_Model();

                model.selected_Inventory_Attribute = selected_Inventory_Attribute;
                model.Inventorys = inventorys;

                return View(model);
            }
        }
    }
}
