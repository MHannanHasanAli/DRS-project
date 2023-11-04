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
using OfficeOpenXml.Style;

namespace DRS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class Supplier_BrandController : Controller
    {
        // GET: Supplier_Brand
        public ActionResult Index(string SearchTerm = "")
        {
            Session["ACTIVER"] = "Supplier/Brand";

            Supplier_BrandListingViewModel model = new Supplier_BrandListingViewModel();
            var datalist = new List<DisplayModelForRelation>();
            
            var Supplier_Brands = Supplier_BrandServices.Instance.GetSupplier_Brands();
            foreach (var item in Supplier_Brands)
            {
                DisplayModelForRelation data = new DisplayModelForRelation();
                data.ID = item.ID;
                var branddata = BrandServices.Instance.GetBrandById(item.IDBrand);
                data.BrandLogo = branddata.Logo;
                data.BrandDescription = branddata.Description;

                var supplierdata = SupplierServices.Instance.GetSupplierById(item.IDSupplier);
                data.SupplierDescription = supplierdata.Description;
                data.Default = item.Default;
                data.Note = item.Note;
                datalist.Add(data);
            }
            model.BrandListing = datalist;
            return View("Index", model);
        }
        [HttpGet]
        public ActionResult Import()
        {
            Session["ACTIVER"] = "Supplier/Brand Import";

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
                var items = new List<Supplier_Brand>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // or LicenseContext.Commercial

                if (excelfile != null && excelfile.ContentLength > 0)
                {
                    using (var package = new ExcelPackage(excelfile.InputStream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;


                        for (int row = 2; row <= rowCount; row++) // Assuming the first row is header
                        {

                            var Supplier_Brand = new Supplier_Brand();

                            if (worksheet.Cells[row, 1].Value == null || worksheet.Cells[row, 2].Value == null)
                            {
                                continue;
                            }

                            if (worksheet.Cells[row, 1].Value != null)
                            {
                                var brand = BrandServices.Instance.GetBrand(worksheet.Cells[row, 1].Value.ToString()).FirstOrDefault();
                                if (brand != null)
                                {
                                    Supplier_Brand.IDBrand = brand.ID;
                                }
                                else
                                {
                                    var Brand = new Brand();
                                    Brand.Description = worksheet.Cells[row, 1].Value.ToString();
                                    BrandServices.Instance.CreateBrand(Brand);
                                    var NewBrand = BrandServices.Instance.GetLastEntryId();
                                    Supplier_Brand.IDBrand = NewBrand;
                                }
                            }
                            if (worksheet.Cells[row, 2].Value != null)
                            {
                                var Supplier = SupplierServices.Instance.GetSupplier(worksheet.Cells[row, 2].Value.ToString()).FirstOrDefault(); ;
                                if (Supplier != null)
                                {
                                    Supplier_Brand.IDSupplier = Supplier.ID;
                                }
                                else
                                {
                                    var supplier = new Supplier();
                                    supplier.Description = worksheet.Cells[row, 2].Value.ToString();
                                    SupplierServices.Instance.CreateSupplier(supplier);
                                    var NewSupplier = SupplierServices.Instance.GetLastEntryId();
                                    Supplier_Brand.IDSupplier = NewSupplier;
                                }

                            }
                           
                            if (worksheet.Cells[row, 3].Value != null)
                            {
                                Supplier_Brand.Note = worksheet.Cells[row, 3].Value.ToString();
                            }
                            else
                            {
                                Supplier_Brand.Note = "Not Specified";
                            }

                           

                            items.Add(Supplier_Brand);
                            Supplier_BrandServices.Instance.CreateSupplier_Brand(Supplier_Brand);
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

            var relation = Supplier_BrandServices.Instance.GetSupplier_Brands();


            System.Data.DataTable tableData = new System.Data.DataTable();
            tableData.Columns.Add("ID", typeof(int)); // Replace "Column1" with the actual column name
            tableData.Columns.Add("Marca", typeof(string));
            tableData.Columns.Add("Fornitore", typeof(string)); // Replace "Column1" with the actual column name                                                            // Replace "Column2" with the actual column name
            tableData.Columns.Add("Default", typeof(string)); // Replace "Column2" with the actual column name// Replace "Column2" with the actual column name
            tableData.Columns.Add("Note", typeof(string)); // Replace "Column2" with the actual column name

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            foreach (var item in relation)
            {
                DataRow row = tableData.NewRow();
                row["ID"] = item.ID;

                var brand = BrandServices.Instance.GetBrandById(item.IDBrand);
                row["Marca"] = brand.Description;

                var supplier = SupplierServices.Instance.GetSupplierById(item.IDSupplier);
                row["Fornitore"] = supplier.Description;

                if(item.Default == "on")
                {
                    row["Default"] = "Yes";

                }
                else
                {
                    row["Default"] = "No";

                }

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
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Supplier/Brand.xlsx");
            }
        }
        [HttpGet]
        public ActionResult Action(int ID = 0)
        {
            if (ID != 0)
            {
                Session["ACTIVER"] = "Supplier/Brand Edit";
            }
            else
            {
                Session["ACTIVER"] = "Add Supplier/Brand";

            }
            Supplier_BrandActionViewModel model = new Supplier_BrandActionViewModel();
            if (ID != 0)
            {
                var Supplier_Brand = Supplier_BrandServices.Instance.GetSupplier_BrandById(ID);
                model.ID = Supplier_Brand.ID;
                model.IDBrand = Supplier_Brand.IDBrand;
                model.IDSupplier = Supplier_Brand.IDSupplier;
                model.Default = Supplier_Brand.Default;
                model.Note = Supplier_Brand.Note;
            }
            model.Brands = BrandServices.Instance.GetBrands();
            model.Supplier = SupplierServices.Instance.GetSuppliers();
            return View("Action", model);
        }


        [HttpPost]
        public ActionResult Action(Supplier_BrandActionViewModel model)
        {
            

            if (model.ID != 0)
            {
                
                var Supplier_Brand = Supplier_BrandServices.Instance.GetSupplier_BrandById(model.ID);
                if(Supplier_Brand.Default == null)
                {
                    Supplier_Brand.Default = "off";
                }
                if(model.Default == "on")
                {
                    var data = Supplier_BrandServices.Instance.GetSupplier_Brand();

                    foreach (var item in data)
                    {
                        if (Supplier_Brand.IDBrand == item.IDBrand)
                        {
                            if (item.Default == "on")
                            {
                                item.Default = "off";
                                Supplier_BrandServices.Instance.UpdateSupplier_Brand(item);
                            }
                        }
                    }
                }
                

                Supplier_Brand.ID = model.ID;
                Supplier_Brand.IDBrand = model.IDBrand;
                Supplier_Brand.IDSupplier = model.IDSupplier;
                Supplier_Brand.Default = model.Default;
                Supplier_Brand.Note = model.Note;


                Supplier_BrandServices.Instance.UpdateSupplier_Brand(Supplier_Brand);

            }
            else
            {
                var Supplier_Brand = new Entities.Supplier_Brand();
                if (model.Default == "on")
                {
                    var data = Supplier_BrandServices.Instance.GetSupplier_Brand();

                    foreach (var item in data)
                    {
                        if (model.IDBrand == item.IDBrand)
                        {
                            if (item.Default == "on")
                            {
                                item.Default = "off";
                                Supplier_BrandServices.Instance.UpdateSupplier_Brand(item);
                            }
                        }
                    }
                }
                Supplier_Brand.IDBrand = model.IDBrand;
                Supplier_Brand.IDSupplier = model.IDSupplier;
                Supplier_Brand.Default = model.Default;
                Supplier_Brand.Note = model.Note;

                Supplier_BrandServices.Instance.CreateSupplier_Brand(Supplier_Brand);
            }




            return Json(new { success = true }, JsonRequestBehavior.AllowGet);


        }


        
        



        [HttpGet]
        public ActionResult Delete(int ID)
        {
            Supplier_BrandActionViewModel model = new Supplier_BrandActionViewModel();
            var Supplier_Brand = Supplier_BrandServices.Instance.GetSupplier_BrandById(ID);
            model.ID = Supplier_Brand.ID;
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(Supplier_BrandActionViewModel model)
        {
            if (model.ID != 0)
            {
                var Supplier_Brand = Supplier_BrandServices.Instance.GetSupplier_BrandById(model.ID);

                Supplier_BrandServices.Instance.DeleteSupplier_Brand(Supplier_Brand.ID);
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}