using Farm_Management_System.farm_Management_SystemModel;
using Farm_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm_Management_System.Controllers
{
    public class FinancialController : Controller
    {
        //------------------------------Admin Part---------------------------------------

        //Show all financial information for admin
        public IActionResult financial_Information_Page()
        {
            using (var db = new farm_Management_SystemContext())
            {
                var financials = db.Financials
                    .ToList();

                // calculate total income and total expense
                var totalIncome = financials.Sum(f => f.IncomeAmount);
                var totalExpense = financials.Sum(f => f.ExpenseAmount);

                // calculate net value
                var netValue = totalIncome - totalExpense;

                // pass the total values to the view
                ViewData["TotalIncome"] = totalIncome;
                ViewData["TotalExpense"] = totalExpense;
                ViewData["NetValue"] = netValue;

                return View(financials);
            }
        }

        //-------------------------------------------------------------------------------

        //Financial Create Page
        [HttpGet]
        public IActionResult financial_Create_Page()
        {
            using (var db = new farm_Management_SystemContext())
            {
                var model = new financial_Creation_Model();

                var userId = HttpContext.Session.GetInt32("Key1");
                var admin_Update = db.Admins
                    .Where(a => a.UserId == (userId ?? 0))
                    .ToList();
                model.Admins = admin_Update;

                var project_List = db.Projects.ToList();
                model.Projects = project_List;

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult financial_Create_Page(financial_Creation_Model model)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Add Financial Information
                db.Financials.Add(model.Financials);

                //Add Update Time
                DateTime update_Time = DateTime.Now;
                model.Financials.TransactionUpdateTime = update_Time;

                db.SaveChanges();
            }
            return RedirectToAction("financial_Information_Page");
        }

        //-------------------------------------------------------------------------------

        //Financial Detail Page
        public IActionResult financial_Detail_Page(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var db = new farm_Management_SystemContext())
            {
                var financials = db.Financials
                    .FirstOrDefault(s => s.TransactionId == id);

                return View(financials);
            }
        }

        //-------------------------------------------------------------------------------

        //Financial Update Page
        [HttpGet]
        public IActionResult financial_Update_Page(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var model = new financial_Creation_Model();

                //Show selected transaction information
                var selected_Transaction = db.Financials
                    .Where(s => s.TransactionId == id)
                    .SingleOrDefault();
                model.Financials = selected_Transaction;

                var userId = HttpContext.Session.GetInt32("Key1");
                var admin_Update = db.Admins
                    .Where(a => a.UserId == (userId ?? 0))
                    .ToList();
                model.Admins = admin_Update;

                var project_List = db.Projects.ToList();
                model.Projects = project_List;

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult financial_Update_Page(financial_Creation_Model model)
        {
            using (var db = new farm_Management_SystemContext())
            {
                //Update selected plant's other information
                db.Entry(model.Financials).State = EntityState.Modified;

                //Update UPDATE TIME
                DateTime update_Time = DateTime.Now;
                model.Financials.TransactionUpdateTime = update_Time;

                db.SaveChanges();
            }
            return RedirectToAction("financial_Information_Page");
        }

        //-------------------------------------------------------------------------------

        //Transaction Delete Page
        [HttpGet]
        public IActionResult financial_Delete_Page(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var db = new farm_Management_SystemContext())
            {
                var financials = db.Financials
                    .FirstOrDefault(s => s.TransactionId == id);

                return View(financials);
            }
        }

        [HttpPost]
        public IActionResult financial_Delete_Page(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var selected_Transaction = db.Financials
                    .Find(id);

                //Remove SELECTED PLANT from PLANT TABLE
                db.Remove(selected_Transaction);
                db.SaveChanges();

                return RedirectToAction("financial_Information_Page");
            }
        }

        //-------------------------------------------------------------------------------

        //Export TRANSACTION DATA to CSV File
        [HttpPost]
        public FileResult export_Financial_CSV()
        {
            using (var db = new farm_Management_SystemContext())
            {
                List<object> financials = (from Financial in db.Financials.ToList().Take(10) select new[]
                {
                    Financial.TransactionId.ToString(),
                    Financial.TransactionName,
                    Financial.TransactionProject.ToString(),
                    Financial.IncomeAmount.ToString(),
                    Financial.ExpenseAmount.ToString(),
                    Financial.AdminUpdate.ToString(),
                    Financial.TransactionUpdateTime.ToString(),
                }).ToList<object>();

                financials.Insert(0, new string[7] { "TransactionId", "TransactionName", "TransactionProject", "IncomeAmount", "ExpenseAmount", "AdminUpdate", "TransactionUpdateTime" });

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < financials.Count; i++)
                {
                    string[] transaction_Data = (string[])financials[i];
                    for (int j = 0; j < transaction_Data.Length; j++)
                    {
                        //Append data with separator.
                        sb.Append(transaction_Data[j] + ',');
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }
                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Financial.csv");
            }
        }

        //-------------------------------------------------------------------------------

        //Search Transaction
        public ActionResult financial_Search_Page(string searchBy, string search)
        {
            using (var db = new farm_Management_SystemContext())
            {
                switch (searchBy)
                {
                    case "TransactionName":
                        return View(db.Financials.Where(x => x.TransactionName.StartsWith(search) || search == null).ToList());
                    case "TransactionProject":
                        int projectId;
                        if (int.TryParse(search, out projectId))
                        {
                            return View(db.Financials.Where(x => x.TransactionProject == projectId).ToList());
                        }
                        else
                        {
                            return View(db.Financials.ToList());
                        }
                    default:
                        return View(db.Financials.ToList());
                }
            }
        }

        //-------------------------------------------------------------------------------

        //Transaction Attribute Selection Page
        public IActionResult financial_Attribute_Selection_View()
        {
            return View(new financial_Column_Model() { Financial = new Financial() });
        }

        [HttpPost]
        public IActionResult financial_Attribute_Selection_View(financial_Column_Selection_Model model)
        {
            return RedirectToAction("financial_Created_View", new { selected_Transaction_Attribute = model.selected_Transaction_Attribute });
        }

        //Created View Page
        public IActionResult financial_Created_View(List<string> selected_Transaction_Attribute)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var financial = db.Financials
                    .ToList();

                var model = new financial_Column_Selection_Model();

                model.selected_Transaction_Attribute = selected_Transaction_Attribute;
                model.Financial = financial;

                return View(model);
            }
        }
    }
}