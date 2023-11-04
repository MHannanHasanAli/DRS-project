using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using DRS.Models;
using DRS.Services;
using DRS.ViewModels;
using DRS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using Microsoft.AspNet.Identity.EntityFramework;
using OfficeOpenXml.Sorting;

namespace DRS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private AMSignInManager _signInManager;
        private AMUserManager _userManager;
        public AMSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<AMSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public AMUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AMUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private AMRolesManager _rolesManager;
        public AMRolesManager RolesManager
        {
            get
            {
                return _rolesManager ?? HttpContext.GetOwinContext().GetUserManager<AMRolesManager>();
            }
            private set
            {
                _rolesManager = value;
            }
        }

        public UserController()
        {
        }
        public UserController(AMUserManager userManager, AMSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        // GET: User
        public ActionResult Index(string searchterm)
        {
            Session["ACTIVER"] = "User";

            UsersListingViewModel model = new UsersListingViewModel();
            model.Users = SearchUsers(searchterm);
            model.Roles = RolesManager.Roles.ToList();
            return View(model);
        }
        public ActionResult ExportToExcel()
        {

            var users = UserManager.Users.AsQueryable();


            System.Data.DataTable tableData = new System.Data.DataTable();
            tableData.Columns.Add("ID", typeof(string)); // Replace "Column1" with the actual column name
            tableData.Columns.Add("Name", typeof(string)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Surname", typeof(string)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Branch", typeof(string)); // Replace "Column2" with the actual column nam
            tableData.Columns.Add("Profile", typeof(string)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Password", typeof(string)); // Replace "Column2" with the actual column name

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            foreach (var item in users)
            {
                System.Data.DataRow row = tableData.NewRow();
                row["ID"] = item.Id;
                row["Name"] = item.Name;
                row["Surname"] = item.Surname;
                row["Branch"] = item.Branch;
                row["Profile"] = item.Role;
                row["Password"] = item.Password;

                tableData.Rows.Add(row);
            }





            // Create the Excel package
            using (ExcelPackage package = new ExcelPackage())
            {
                // Create a new worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");

                // Set the column names
                for (int i = 0; i < tableData.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = tableData.Columns[i].ColumnName;
                }

                // Set the row data
                for (int row = 0; row < tableData.Rows.Count; row++)
                {
                    for (int col = 0; col < tableData.Columns.Count; col++)
                    {
                        worksheet.Cells[row + 2, col + 1].Value = tableData.Rows[row][col];
                    }
                }

                // Auto-fit columns for better readability
                worksheet.Cells.AutoFitColumns();

                // Convert the Excel package to a byte array
                byte[] excelBytes = package.GetAsByteArray();

                // Return the Excel file as a downloadable file
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Users.xlsx");
            }
        }
        [HttpGet]
        public ActionResult Import()
        {
            Session["ACTIVER"] = "User Import";

            return View();
        }
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select Excel File";
                return View();
            }
            else
            {
                var items = new List<RegisterViewModel>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // or LicenseContext.Commercial

                if (excelfile != null && excelfile.ContentLength > 0)
                {
                    using (var package = new ExcelPackage(excelfile.InputStream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        var rolesList = RolesManager.Roles.ToList();
                        for (int row = 2; row <= rowCount; row++) // Assuming the first row is header
                        {

                            RegisterViewModel user = new RegisterViewModel();

                            if (worksheet.Cells[row, 1].Value == null || worksheet.Cells[row, 2].Value == null || worksheet.Cells[row, 3].Value == null || worksheet.Cells[row, 4].Value == null || worksheet.Cells[row, 5].Value == null)
                            {
                                continue;
                            }

                            if (worksheet.Cells[row, 1].Value != null)
                            {
                                user.Name = worksheet.Cells[row, 1].Value.ToString();
                            }

                            //ProductName
                            if (worksheet.Cells[row, 2].Value != null)
                            {
                                user.Surname = worksheet.Cells[row, 2].Value.ToString();
                            }
                      

                            if (worksheet.Cells[row, 3].Value != null)
                            {
                                user.Branch = worksheet.Cells[row, 3].Value.ToString();
                            }
                            
                            if (worksheet.Cells[row, 4].Value != null)
                            {
                                string roleName = worksheet.Cells[row, 4].Value.ToString();

                                var RoleOfUser = rolesList.FirstOrDefault(r => r.Name == roleName);
                                if (RoleOfUser != null)
                                {
                                    user.RoleID = RoleOfUser.Id;
                                }

                            }
                           
                            if (worksheet.Cells[row, 5].Value != null)
                            {
                                user.Password = worksheet.Cells[row, 5].Value.ToString();
                            }
                           

                            var role = RolesManager.FindByIdAsync(user.RoleID);
                            var UserToBeSaved = new User { UserName = user.Name, Email = user.Name + "@DRS.com", Surname = user.Surname, Branch = user.Branch, Name = user.Name, Role = role.Result.Name, Password = user.Password, Image = user.Image };
                            var rolesAll = rolesList.Select(x => x.Id).ToList();
                            if (rolesAll.Contains(UserToBeSaved.Role))
                            {
                                UserManager.CreateAsync(UserToBeSaved, user.Password);

                                UserManager.AddToRoleAsync(UserToBeSaved.Id, role.Result.Name);
                                items.Add(user);
                            }
                        }
                       
                    }
                    ViewBag.Products = items;
                    return RedirectToAction("Index","User");

                }



                else
                {
                    ViewBag.Error = "Incorrect File";

                    return View();
                }
            }

            //var Prcoess = Process.GetProcessesByName("EXCEL.EXE").FirstOrDefault();
            //Prcoess.Kill();

        }

      

        public IEnumerable<User> SearchUsers(string searchTerm)
        {
            var users = UserManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(a => a.Email.ToLower().Contains(searchTerm.ToLower()));
            }

            users.OrderBy(b => b.Name);

            return users;
        }

        public ActionResult Register(RegisterViewModel model)
        {
            model.Roles = RolesManager.Roles.ToList();
            return PartialView("_Register", model);
        }

        [HttpGet]
        public async Task<ActionResult> Action(string ID)
        {
            UserActionModel model = new UserActionModel();
            model.Roles = RolesManager.Roles.ToList();
            if (!string.IsNullOrEmpty(ID))
            {
                Session["ACTIVER"] = "User Edit";
            }
            else
            {
                Session["ACTIVER"] = "Add User";

            }

            if (!string.IsNullOrEmpty(ID))
            {
                var user = await UserManager.FindByIdAsync(ID);
                model.ID = user.Id;
                model.Name = user.Name;
                model.Image = user.Image;
                //model.Contact = user.PhoneNumber;
                //model.Email = user.Email;
                model.Surname = user.Surname;
                model.Branch = user.Branch;

                model.Role = user.Role;
                model.Password = user.Password;
            }
            return PartialView("_Action", model);
        }



        [HttpPost]
        public async Task<JsonResult> Action(UserActionModel model)
        {
            JsonResult json = new JsonResult();
            IdentityResult result = null;
            if (!string.IsNullOrEmpty(model.ID)) //update record
            {
                var user = await UserManager.FindByIdAsync(model.ID);

                user.Id = model.ID;
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Branch = model.Branch;
                user.Image = model.Image;
                //user.PhoneNumber = model.Contact;
                //user.Email = model.Email;
                user.Role = model.Role;
                var token = await UserManager.GeneratePasswordResetTokenAsync(model.ID);
                var result2 = await UserManager.ResetPasswordAsync(model.ID, token, model.Password);
                result = await UserManager.UpdateAsync(user);

            }
            else
            {
                var User = new User();
                User.Name = model.Name;
                User.Surname = model.Surname;
                User.Branch = model.Branch;
                User.Image = model.Image;

                //User.PhoneNumber = model.Contact;
                //User.Email = model.Email;
                User.Password = model.Password;
                User.Role = model.Role;
                //User.UserName = model.Email;
                result = await UserManager.CreateAsync(User);

            }

            json.Data = new { Success = result.Succeeded, Message = string.Join(", ", result.Errors) };

            return json;
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string ID)
        {
            UserActionModel model = new UserActionModel();

            var user = await UserManager.FindByIdAsync(ID);

            model.ID = user.Id;

            return PartialView("_Delete", model);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(UserActionModel model)
        {
            JsonResult json = new JsonResult();

            IdentityResult result = null;

            if (!string.IsNullOrEmpty(model.ID)) //we are trying to delete a record
            {
                var user = await UserManager.FindByIdAsync(model.ID);

                result = await UserManager.DeleteAsync(user);

                json.Data = new { Success = result.Succeeded, Message = string.Join(", ", result.Errors) };
            }
            else
            {
                json.Data = new { Success = false, Message = "Invalid user." };
            }

            return json;
        }



        [HttpGet]
        public async Task<ActionResult> UserRoles(string ID)
        {
            UserRoleModel model = new UserRoleModel();
            model.UserID = ID;
            var user = await UserManager.FindByIdAsync(ID);
            var userRoleIDs = user.Roles.Select(x => x.RoleId).ToList();


            model.UserRoles = RolesManager.Roles.Where(x => userRoleIDs.Contains(x.Id)).ToList();
            model.Roles = RolesManager.Roles.Where(x => !userRoleIDs.Contains(x.Id)).ToList();
            return PartialView("_UserRoles", model);
        }



        [HttpPost]
        public async Task<JsonResult> UserRoles(UserActionModel model)
        {
            JsonResult json = new JsonResult();
            IdentityResult result = null;
            if (!string.IsNullOrEmpty(model.ID)) //update record
            {
                var user = await UserManager.FindByIdAsync(model.ID);

                user.Id = model.ID;
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Branch = model.Branch;
                //user.PhoneNumber = model.Contact;
                //user.Email = model.Email;
                user.Password = model.Password;
                user.Role = model.Role;
                result = await UserManager.UpdateAsync(user);

            }
            else
            {
                var User = new User();
                User.Name = model.Name;
                User.Surname = model.Surname;
                User.Branch = model.Branch;
                //User.PhoneNumber = model.Contact;
                //User.Email = model.Email;
                User.Password = model.Password;
                //User.UserName = model.Email;
                User.Role = model.Role;
                result = await UserManager.CreateAsync(User);

            }

            json.Data = new { Success = result.Succeeded, Message = string.Join(", ", result.Errors) };

            return json;
        }




        [HttpPost]
        public async Task<JsonResult> UserRoleOperation(string userID, string roleID, bool isDelete = false)
        {
            JsonResult json = new JsonResult();

            var user = await UserManager.FindByIdAsync(userID);
            var role = await RolesManager.FindByIdAsync(roleID);

            if (user != null && role != null)
            {
                IdentityResult result = null;
                if (!isDelete)
                {
                    result = await UserManager.AddToRoleAsync(userID, role.Name);
                }
                else
                {
                    result = await UserManager.RemoveFromRoleAsync(userID, role.Name);
                }
                json.Data = new { Success = result.Succeeded, Message = string.Join(", ", result.Errors) };

            }
            else
            {
                json.Data = new { Success = false, Message = "Invalid Operation" };

            }


            return json;
        }
    }
}