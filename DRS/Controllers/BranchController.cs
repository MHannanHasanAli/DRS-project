using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DRS.Entities;
using DRS.Services;
using DRS.ViewModels;
using OfficeOpenXml;

namespace DRS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BranchController : Controller
    {
        // GET: Branch
        public ActionResult Index(string SearchTerm = "")
        {
            Session["ACTIVER"] = "Branch";

            BranchListingViewModel model = new BranchListingViewModel();
            model.Branches = BranchServices.Instance.GetBranchs();
            return View("Index", model);
        }
        [HttpGet]
        public ActionResult Import()
        {
            Session["ACTIVER"] = "Branch Import";

            return View();
        }
        public ActionResult ExportToExcel()
        {

            var branches = BranchServices.Instance.GetBranchs();


            System.Data.DataTable tableData = new System.Data.DataTable();
            tableData.Columns.Add("ID", typeof(int)); // Replace "Column1" with the actual column name
            tableData.Columns.Add("Description", typeof(string));
            tableData.Columns.Add("Telephone", typeof(string)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Whatsapp", typeof(string)); // Replace "Column2" with the actual column nam
            tableData.Columns.Add("Email", typeof(string)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Note", typeof(string)); // Replace "Column2" with the actual column name

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            foreach (var item in branches)
            {
                DataRow row = tableData.NewRow();
                row["ID"] = item.ID;
                row["Description"] = item.Description;
                row["Telephone"] = item.Telephone;
                row["Whatsapp"] = item.Whatsapp;
                row["Email"] = item.Email;
                row["Note"] = item.Note;


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
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Branches.xlsx");
            }
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
                var items = new List<Branch>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // or LicenseContext.Commercial

                if (excelfile != null && excelfile.ContentLength > 0)
                {
                    using (var package = new ExcelPackage(excelfile.InputStream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;


                        for (int row = 2; row <= rowCount; row++) // Assuming the first row is header
                        {

                            var Branch = new Branch();

                            if (worksheet.Cells[row, 1].Value == null)
                            {
                                continue;
                            }

                            if (worksheet.Cells[row, 1].Value != null)
                            {
                                Branch.Description = worksheet.Cells[row, 1].Value.ToString();
                            }                          

                            //ProductName
                            if (worksheet.Cells[row, 2].Value != null)
                            {
                                Branch.Telephone = worksheet.Cells[row, 2].Value.ToString();
                            }
                            else
                            {
                                Branch.Telephone = "Not Specified";
                            }

                            if (worksheet.Cells[row, 3].Value != null)
                            {
                                Branch.Whatsapp = worksheet.Cells[row, 3].Value.ToString();
                            }
                            else
                            {
                                Branch.Whatsapp = "Not Specified";
                            }
                            if (worksheet.Cells[row, 4].Value != null)
                            {
                                Branch.Email = worksheet.Cells[row, 4].Value.ToString();
                            }
                            else
                            {
                                Branch.Email = "Not Specified";
                            }
                            if (worksheet.Cells[row, 5].Value != null)
                            {
                                Branch.Note = worksheet.Cells[row, 5].Value.ToString();
                            }
                            else
                            {
                                Branch.Note = "Not Specified";
                            }

                            items.Add(Branch);
                            BranchServices.Instance.CreateBranch(Branch);
                        }

                    }
                    ViewBag.Products = items;
                    return View();

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
        [HttpGet]
        public ActionResult Action(int ID = 0)
        {
            if (ID != 0)
            {
                Session["ACTIVER"] = "Branch Edit";
            }
            else
            {
                Session["ACTIVER"] = "Add Branch";

            }
            BranchActionViewModel model = new BranchActionViewModel();
            if (ID != 0)
            {
                var Branch = BranchServices.Instance.GetBranchById(ID);
                model.ID = Branch.ID;
                model.Description = Branch.Description;
                model.Email = Branch.Email;
                model.Telephone = Branch.Telephone;
                model.Whatsapp = Branch.Whatsapp;
                model.Note = Branch.Note;
            

            }
            return View("Action", model);
        }


        [HttpPost]
        public ActionResult Action(BranchActionViewModel model)
        {

            if (model.ID != 0)
            {
                var Branch = BranchServices.Instance.GetBranchById(model.ID);
                Branch.ID = model.ID;
                Branch.Description = model.Description;
                Branch.Email = model.Email;
                Branch.Telephone = model.Telephone;
                Branch.Whatsapp = model.Whatsapp;
                Branch.Note = model.Note;
            

                BranchServices.Instance.UpdateBranch(Branch);

            }
            else
            {
                var Branch = new Entities.Branch();
                Branch.Description = model.Description;
                Branch.Email = model.Email;
                Branch.Telephone = model.Telephone;
                Branch.Whatsapp = model.Whatsapp;
                Branch.Note = model.Note;
             

                BranchServices.Instance.CreateBranch(Branch);

            }


            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult Delete(int ID)
        {
            BranchActionViewModel model = new BranchActionViewModel();
            var Branch = BranchServices.Instance.GetBranchById(ID);
            model.ID = Branch.ID;
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(BranchActionViewModel model)
        {
            if (model.ID != 0)
            {
                var Branch = BranchServices.Instance.GetBranchById(model.ID);

                BranchServices.Instance.DeleteBranch(Branch.ID);
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}