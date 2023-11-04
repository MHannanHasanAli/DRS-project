using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using DRS.Entities;
using DRS.Services;
using DRS.ViewModels;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;

namespace DRS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SupplierController : Controller
    {
        // GET: Supplier
        public ActionResult Index(string SearchTerm = "")
        {
            Session["ACTIVER"] = "Supplier";

            SupplierListingViewModel model = new SupplierListingViewModel();
            model.Suppliers = SupplierServices.Instance.GetSuppliers();
            return View("Index", model);
        }


        [HttpGet]
        public ActionResult Action(int ID = 0)
        {
            if (ID != 0)
            {
                Session["ACTIVER"] = "Supplier Edit";
            }
            else
            {
                Session["ACTIVER"] = "Add Supplier";

            }
            SupplierActionViewModel model = new SupplierActionViewModel();
            if (ID != 0)
            {
                var Supplier = SupplierServices.Instance.GetSupplierById(ID);
                model.ID = Supplier.ID;
                model.Description = Supplier.Description;
                model.Email = Supplier.Email;
                model.Telephone = Supplier.Telephone;
                model.Whatsapp = Supplier.Whatsapp;
                model.Note = Supplier.Note;
                model.Contact = Supplier.Contact;
                model.Logo = Supplier.Logo;

            }
            return View("Action", model);
        }
        public ActionResult ExportToExcel()
        {

            var supplier = SupplierServices.Instance.GetSuppliers();


            System.Data.DataTable tableData = new System.Data.DataTable();
            tableData.Columns.Add("ID", typeof(int)); // Replace "Column1" with the actual column name
            tableData.Columns.Add("Description", typeof(string)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Telephone", typeof(string)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Whatsapp", typeof(string)); // Replace "Column2" with the actual column nam
            tableData.Columns.Add("Email", typeof(string)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Note", typeof(string)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Contact", typeof(string)); // Replace "Column1" with the actual column name
            
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                foreach (var item in supplier)
                {
                        DataRow row = tableData.NewRow();
                        row["ID"] = item.ID;
                        row["Description"] = item.Description;
                        row["Telephone"] = item.Telephone;
                        row["Whatsapp"] = item.Whatsapp;
                        row["Email"] = item.Email;
                        row["Note"] = item.Note;
                        row["Contact"] = item.Contact;

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
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Suppliers.xlsx");
            }
        }
        [HttpGet]
        public ActionResult Import()
        {
            Session["ACTIVER"] = "Supplier Import";

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
                var items = new List<Supplier>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // or LicenseContext.Commercial

                if (excelfile != null && excelfile.ContentLength > 0)
                {
                    using (var package = new ExcelPackage(excelfile.InputStream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;


                        for (int row = 2; row <= rowCount; row++) // Assuming the first row is header
                        {

                            var Supplier = new Supplier();

                            if (worksheet.Cells[row, 1].Value == null)
                            {
                                continue;
                            }

                            if (worksheet.Cells[row, 1].Value != null)
                            {
                                Supplier.Description = worksheet.Cells[row, 1].Value.ToString();
                            }

                            //ProductName
                            if (worksheet.Cells[row, 2].Value != null)
                            {
                                Supplier.Telephone = worksheet.Cells[row, 2].Value.ToString();
                            }
                            else
                            {
                                Supplier.Telephone = "Not Specified";
                            }

                            if (worksheet.Cells[row, 3].Value != null)
                            {
                                Supplier.Whatsapp = worksheet.Cells[row, 3].Value.ToString();
                            }
                            else
                            {
                                Supplier.Whatsapp = "Not Specified";
                            }
                            if (worksheet.Cells[row, 4].Value != null)
                            {
                                Supplier.Email = worksheet.Cells[row, 4].Value.ToString();
                            }
                            else
                            {
                                Supplier.Email = "Not Specified";
                            }
                            if (worksheet.Cells[row, 5].Value != null)
                            {
                                Supplier.Note = worksheet.Cells[row, 5].Value.ToString();
                            }
                            else
                            {
                                Supplier.Note = "Not Specified";
                            }
                            if (worksheet.Cells[row, 6].Value != null)
                            {
                                Supplier.Contact = worksheet.Cells[row, 6].Value.ToString();
                            }
                            else
                            {
                                Supplier.Contact = "Not Specified";
                            }

                            items.Add(Supplier);
                            SupplierServices.Instance.CreateSupplier(Supplier);
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
        [HttpPost]
        public ActionResult Action(SupplierActionViewModel model)
        {

            if (model.ID != 0)
            {
                var Supplier = SupplierServices.Instance.GetSupplierById(model.ID);
                Supplier.ID = model.ID;
                Supplier.Description = model.Description;
                Supplier.Email = model.Email;
                Supplier.Telephone = model.Telephone;
                Supplier.Whatsapp = model.Whatsapp;
                Supplier.Note = model.Note;
                Supplier.Contact = model.Contact;
                Supplier.Logo = model.Logo;


                SupplierServices.Instance.UpdateSupplier(Supplier);

            }
            else
            {
                var Supplier = new Entities.Supplier();
                Supplier.Description = model.Description;
                Supplier.Email = model.Email;
                Supplier.Telephone = model.Telephone;
                Supplier.Whatsapp = model.Whatsapp;
                Supplier.Note = model.Note;
                Supplier.Contact = model.Contact;
                Supplier.Logo = model.Logo;

                SupplierServices.Instance.CreateSupplier(Supplier);

            }


            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult Delete(int ID)
        {
            SupplierActionViewModel model = new SupplierActionViewModel();
            var Supplier = SupplierServices.Instance.GetSupplierById(ID);
            model.ID = Supplier.ID;
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(SupplierActionViewModel model)
        {
            var supplierbrand = Supplier_BrandServices.Instance.GetSupplier_Brand().Where(x => x.IDSupplier == model.ID);
            if (supplierbrand.Count() != 0)
            {
                return Json(new { success = false, Message = "Impossibile eliminare il fornitore perché è presente nel Fornitore/Marchio attivo. Elimina prima la relazione fornitore/marchio!" });
            }

            var orders = OrderServices.Instance.GetOrder().Where(x => x.IDSupplier == model.ID);
            if (orders.Count() != 0)
            {
                return Json(new { success = false, Message = "Impossibile eliminare il fornitore poiché è presente nell'ordine attivo. Elimina prima l'ordine!" });
            }

            if (model.ID != 0)
            {
                var Supplier = SupplierServices.Instance.GetSupplierById(model.ID);

                SupplierServices.Instance.DeleteSupplier(Supplier.ID);
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}