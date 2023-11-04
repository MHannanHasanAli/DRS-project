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
    public class BrandController : Controller
    {
        // GET: Brand
        public ActionResult Index(string SearchTerm = "")
        {
            Session["ACTIVER"] = "Brand";

            BrandListingViewModel model = new BrandListingViewModel();
            model.Brands = BrandServices.Instance.GetBrands();
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult Import()
        {
            Session["ACTIVER"] = "Brand Import";

            return View();
        }
        [HttpGet]
        public ActionResult Action(int ID = 0)
        {
            if (ID != 0)
            {
                Session["ACTIVER"] = "Brand Edit";
            }
            else
            {
                Session["ACTIVER"] = "Add Brand";

            }
            BrandActionViewModel model = new BrandActionViewModel();
            if (ID != 0)
            {
                var Brand = BrandServices.Instance.GetBrandById(ID);
                model.ID = Brand.ID;
                model.Description = Brand.Description;
                model.Note = Brand.Note;
                model.Logo = Brand.Logo;

            }
            return View("Action", model);
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
                var items = new List<Brand>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // or LicenseContext.Commercial

                if (excelfile != null && excelfile.ContentLength > 0)
                {
                    using (var package = new ExcelPackage(excelfile.InputStream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;


                        for (int row = 2; row <= rowCount; row++) // Assuming the first row is header
                        {

                            var Brand = new Brand();

                            if (worksheet.Cells[row, 1].Value == null)
                            {
                                continue;
                            }

                            if (worksheet.Cells[row, 1].Value != null)
                            {
                                Brand.Description = worksheet.Cells[row, 1].Value.ToString();
                            }
                            
                            if (worksheet.Cells[row, 2].Value != null)
                            {
                                Brand.Note = worksheet.Cells[row, 2].Value.ToString();
                            }
                            else
                            {
                                Brand.Note = "Not Specified";
                            }

                            items.Add(Brand);
                            BrandServices.Instance.CreateBrand(Brand);
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
        public ActionResult ExportToExcel()
        {

            var brand = BrandServices.Instance.GetBrands();


            System.Data.DataTable tableData = new System.Data.DataTable();
            tableData.Columns.Add("ID", typeof(int)); // Replace "Column1" with the actual column name
            tableData.Columns.Add("Description", typeof(string)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Note", typeof(string)); // Replace "Column2" with the actual column name

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            foreach (var item in brand)
            {
                DataRow row = tableData.NewRow();
                row["ID"] = item.ID;
                row["Description"] = item.Description;              
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
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Brands.xlsx");
            }
        }
        [HttpPost]
        public ActionResult Action(BrandActionViewModel model)
        {

            if (model.ID != 0)
            {
                var Brand = BrandServices.Instance.GetBrandById(model.ID);
                Brand.ID = model.ID;
                Brand.Description = model.Description;
                Brand.Note = model.Note;
                Brand.Logo = model.Logo;


                BrandServices.Instance.UpdateBrand(Brand);

            }
            else
            {
                var Brand = new Entities.Brand();
                Brand.Description = model.Description;
                Brand.Note = model.Note;
                Brand.Logo = model.Logo;

                BrandServices.Instance.CreateBrand(Brand);

            }


            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult Delete(int ID)
        {
            BrandActionViewModel model = new BrandActionViewModel();
            var Brand = BrandServices.Instance.GetBrandById(ID);
            model.ID = Brand.ID;
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(BrandActionViewModel model)
        {
            if (model.ID != 0)
            {
                var Brand = BrandServices.Instance.GetBrandById(model.ID);

                BrandServices.Instance.DeleteBrand(Brand.ID);
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}