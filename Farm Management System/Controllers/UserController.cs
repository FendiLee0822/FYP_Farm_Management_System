using CsvHelper;
using Farm_Management_System.farm_Management_SystemModel;
using Farm_Management_System.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm_Management_System.Controllers
{
    public class UserController : Controller
    {

        //Login Page
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        public ActionResult login_Page()
        {
            return View();
        }
        void connectionString()
        {
            con.ConnectionString = "data source=(localdb)\\MSSQLLocalDB; database=farm_Management_System; integrated security=SSPI;";
        }

        [HttpPost]
        public ActionResult login_Verify(User user)
        {
            User currentUser = null;

            connectionString();
            con.Open();
            com.Connection = con;

            HttpContext.Session.SetInt32("Key1", user.UserId);

            com.CommandText = "select * from [User] where user_Id='" + user.UserId + "'and user_Pwd='" + user.UserPwd + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                HttpContext.Session.SetString("CurrentUser", JsonConvert.SerializeObject(currentUser));

                if (Convert.ToString(dr["user_Role"]) == "Admin")
                {
                    //Set key for ROLE to use different layout
                    HttpContext.Session.SetInt32("user_Role", 1);

                    con.Close();
                    return RedirectToAction("user_Information_Page");
                }
                if (Convert.ToString(dr["user_Role"]) == "Employee")
                {
                    HttpContext.Session.SetInt32("user_Role", 2);

                    con.Close();
                    return RedirectToAction("user_Personal_Page");
                }
                else
                {
                    con.Close();
                    return View("login_Error");
                }
            }
            else
            {
                con.Close();
                return View("login_Error");
            }
        }

        //------------------------------Admin Part---------------------------------------

        //Show all user information
        public IActionResult user_Information_Page()
        {
            using (var db = new farm_Management_SystemContext())
            {
                var users = db.Users
                    .ToList();

                return View(users);
            }
        }

        //-------------------------------------------------------------------------------

        //User Create Page
        [HttpGet]
        public IActionResult user_Create_Page()
        {
            return View();
        }

        [HttpPost]
        public IActionResult user_Create_Page(User user)
        {
            using (var db = new farm_Management_SystemContext())
            {
                db.Users.Add(user);
                db.SaveChanges();

                //Add the USER into respective ROLE TABLE
                if (user.UserRole == "Admin")
                {
                    db.Admins.Add(new Admin { UserId = user.UserId });
                    db.SaveChanges();
                }
                else if (user.UserRole == "Employee")
                {
                    db.Employees.Add(new Employee { UserId = user.UserId });
                    db.SaveChanges();
                }
            }
            return RedirectToAction("user_Information_Page");
        }

        //-------------------------------------------------------------------------------

        //User Detail Page
        public IActionResult user_Detail_Page(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var db = new farm_Management_SystemContext())
            {
                var users = db.Users
                    .FirstOrDefault(s => s.UserId == id);

                if (User == null)
                {
                    return NotFound();
                }
                return View(users);
            }
        }

        //-------------------------------------------------------------------------------

        //User Update Page
        [HttpGet]
        public IActionResult user_Update_Page(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var user = db.Users
                    .Where(s => s.UserId == id)
                    .SingleOrDefault();

                return View(user);
            }
        }

        [HttpPost]
        public IActionResult user_Update_Page(User user)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var user_Selected = db.Users
                    .Where(s => s.UserId == user.UserId)
                    .SingleOrDefault();

                //Update the DATA in ROLE TABLE while USER ROLE change
                if (user.UserRole != user_Selected.UserRole)
                {
                    if (user.UserRole == "Admin")
                    {
                        db.Employees.RemoveRange(db.Employees.Where(s => s.UserId == user.UserId));
                        db.Admins.Add(new Admin { UserId = user.UserId });
                        db.SaveChanges();
                    }
                    else if (user.UserRole == "Employee")
                    {
                        db.Admins.RemoveRange(db.Admins.Where(a => a.UserId == user.UserId));
                        db.Employees.Add(new Employee { UserId = user.UserId });
                        db.SaveChanges();
                    }
                }

                var local = db.Set<User>()
                    .Local
                    .FirstOrDefault(entry => entry.UserId.Equals(user.UserId));
                db.Entry(local).State = EntityState.Detached;

                //Update other information of selected user
                db.Users.Update(user);
                db.SaveChanges();
            }
            return RedirectToAction("user_Information_page");
        }

        //-------------------------------------------------------------------------------

        //User Delete Page
        [HttpGet]
        public IActionResult user_Delete_Page(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var db = new farm_Management_SystemContext())
            {
                var users = db.Users
                    .FirstOrDefault(m => m.UserId == id);

                if (User == null)
                {
                    return NotFound();
                }

                return View(users);
            }
        }

        [HttpPost]
        public IActionResult user_Delete_Page(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var users = db.Users
                    .Find(id);

                //Delete USER from ROLE TABLE
                if (users.UserRole == "Admin")
                {
                    db.Admins.RemoveRange(db.Admins.Where(a => a.UserId == users.UserId));
                    db.SaveChanges();
                }
                else if (users.UserRole == "Employee")
                {
                    db.Employees.RemoveRange(db.Employees.Where(a => a.UserId == users.UserId));
                    db.SaveChanges();
                }

                //Remove USER from USER TABLE
                db.Remove(users);
                db.SaveChanges();

                return RedirectToAction("user_Information_Page");
            }
        }

        //-------------------------------------------------------------------------------

        //User Import Page
        [HttpGet]
        public IActionResult user_Import_Page(List<User> users = null)
        {
            users = users == null ? new List<User>() : users;
            return View(users);
        }

        [HttpPost]
        [Obsolete]
        public IActionResult user_Import_Page(IFormFile file, [FromServices] IHostingEnvironment hostingEnvironment)
        {
            #region Upload CSV
            string filename = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";
            using (FileStream fileStream = System.IO.File.Create(filename))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            #endregion

            var users = this.get_Csv_List(file.FileName);
            return user_Import_Page(users);
        }

        private List<User> get_Csv_List(string filename)
        {
            List<User> users = new List<User>();

            #region Read CSV
            var path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + filename;
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<csvmap_User>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var user = csv.GetRecord<User>();
                    user.UserId = 0;
                    users.Add(user);
                }
            }
            #endregion
            #region Create CSV

            using (var db = new farm_Management_SystemContext())
            {
                db.Users.AddRange(users);
                db.SaveChanges();

                //Add USER to respective ROLE TABLE
                var admins = users
                    .Where(u => u.UserRole == "Admin")
                    .Select(u => new Admin
                    {
                        UserId = u.UserId
                    });
                db.Admins.AddRange(admins);

                var employees = users
                    .Where(u => u.UserRole == "Employee")
                    .Select(u => new Employee
                    {
                        UserId = u.UserId
                    });
                db.Employees.AddRange(employees);

                db.SaveChanges();
            }

            #endregion
            return users;
        }

        //-------------------------------------------------------------------------------

        //Export USER DATA to CSV File
        [HttpPost]
        public FileResult export_User_CSV()
        {
            using (var db = new farm_Management_SystemContext())
            {
                List<object> users = (from user in db.Users.ToList().Take(10)
                                      select new[]
{
                    user.UserId.ToString(),
                    user.UserFname,
                    user.UserLname,
                    user.UserGender,
                    user.UserDob.ToString(),
                    user.UserPwd,
                    user.UserRole
                }).ToList<object>();

                users.Insert(0, new string[7] { "UserId", "UserFname", "UserLname", "UserGender", "UserDob", "UserPwd", "UserRole" });

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < users.Count; i++)
                {
                    string[] user_Data = (string[])users[i];
                    for (int j = 0; j < user_Data.Length; j++)
                    {
                        //Append data with separator.
                        sb.Append(user_Data[j] + ',');
                    }
                    //Append new line character.
                    sb.Append("\r\n");
                }
                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "User.csv");
            }
        }

        //-------------------------------------------------------------------------------

        //Search User 
        public ActionResult user_Search_Page(string searchBy, string search)
        {
            using (var db = new farm_Management_SystemContext())
            {
                switch (searchBy)
                {
                    case "UserGender":
                        return View(db.Users.Where(x => x.UserGender.StartsWith(search) || search == null).ToList());
                    case "UserRole":
                        return View(db.Users.Where(x => x.UserRole.StartsWith(search)).ToList());
                    case "UserFname":
                        return View(db.Users.Where(x => x.UserFname.StartsWith(search) || search == null).ToList());
                    case "UserLname":
                        return View(db.Users.Where(x => x.UserLname.StartsWith(search) || search == null).ToList());
                    default:
                        return View(db.Users.ToList());
                }
            }
        }


        //-------------------------------------------------------------------------------

        //User Attribute Selection Page
        public IActionResult user_Attribute_Selection_View()
        {
            return View(new user_Column_Model() { User = new User() });
        }

        [HttpPost]
        public IActionResult user_Attribute_Selection_View(user_Column_Selection_Model model)
        {
            return RedirectToAction("user_Created_View", new { selected_User_Attribute = model.selected_User_Attribute });
        }

        //Created View Page
        public IActionResult user_Created_View(List<string> selected_User_Attribute)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var users = db.Users
                    .ToList();

                var model = new user_Column_Selection_Model();

                model.selected_User_Attribute = selected_User_Attribute;
                model.Users = users;

                return View(model);
            }
        }

        //------------------------------Employee Part------------------------------------

        //User Peronal Page
        public IActionResult user_Personal_Page(User user)
        {
            using (var db = new farm_Management_SystemContext())
            {

                var userId = HttpContext.Session.GetInt32("Key1");
                var dbuser = db.Users
                    .Where(s => s.UserId == (userId ?? 0))
                    .FirstOrDefault(s => s.UserId == (userId ?? 0));

                if (user == null)
                {
                    return NotFound();
                }

                return View(dbuser);
            }
        }

        //-------------------------------------------------------------------------------

        //User Update Page
        [HttpGet]
        public IActionResult personal_Update_Page(int id)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var userId = HttpContext.Session.GetInt32("Key1");
                var user = db.Users
                    .Where(s => s.UserId == (userId ?? 0))
                    .FirstOrDefault(s => s.UserId == (userId ?? 0));

                return View(user);
            }
        }

        [HttpPost]
        public IActionResult personal_Update_Page(User user)
        {
            using (var db = new farm_Management_SystemContext())
            {
                var userId = HttpContext.Session.GetInt32("Key1");
                var dbuser = db.Users
                    .Where(s => s.UserId == (userId ?? 0))
                    .FirstOrDefault(s => s.UserId == (userId ?? 0));

                //Update ROLE data in ROLE TABLE
                if (user.UserRole != dbuser.UserRole)
                {
                    if (user.UserRole == "Admin")
                    {
                        db.Employees.RemoveRange(db.Employees.Where(a => a.UserId == user.UserId));
                        db.Admins.Add(new Admin { UserId = user.UserId });
                        db.SaveChanges();
                    }
                    else if (user.UserRole == "Employee")
                    {
                        db.Admins.RemoveRange(db.Admins.Where(a => a.UserId == user.UserId));
                        db.Employees.Add(new Employee { UserId = user.UserId });
                        db.SaveChanges();
                    }
                }

                var local = db.Set<User>()
                    .Local
                    .FirstOrDefault(entry => entry.UserId.Equals(user.UserId));
                db.Entry(local).State = EntityState.Detached;

                db.Users.Update(user);
                db.SaveChanges();
            }
            return RedirectToAction("user_Personal_Page");
        }
    }
}
