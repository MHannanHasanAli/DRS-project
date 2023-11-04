using DRS.Entities;
using DRS.Services;
using DRS.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace DRS.Controllers
{
    [Authorize]
    public class OrderController : Controller
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
        public OrderController()
        {
        }



        public OrderController(AMUserManager userManager, AMSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public static int CalculateDaysBetweenDates(DateTime startDate)
        {
            DateTime endDate = DateTime.Now;
            TimeSpan timeSpan = endDate - startDate;
            int daysDifference = (int)timeSpan.TotalDays;
            return daysDifference;
        }
        [HttpGet]
        public ActionResult Import()
        {
            Session["ACTIVER"] = "Order Import";
            return View();
        }
        // GET: Order
        [HttpPost] // You can use [HttpGet] if appropriate
        public ActionResult GetSuppliersByBrand(int brandID)
        {
            var supplierList = new List<Supplier>();
            var supplierids = new List<int>();
            var suppliers = Supplier_BrandServices.Instance.GetSupplier_BrandsByBrandID(brandID);
            
            foreach (var item in suppliers)
            {
                if(item.Default == "on")
                {
                    supplierids.Add(item.IDSupplier);
                }
            }
            foreach (var item in suppliers)
            {
                if (item.Default == "off")
                {
                    supplierids.Add(item.IDSupplier);
                }
            }
            foreach (var item in supplierids)
            {
                var data = SupplierServices.Instance.GetSupplierById(item);
                supplierList.Add(data);
            }
            // Return the supplier data as JSON
            return Json(supplierList, JsonRequestBehavior.AllowGet);
        }

        static int checker = 0;
        public ActionResult ActionProducts(string products)
        {
            if(checker == 1)
            {
                checker = 0;
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            var LastOrder = OrderServices.Instance.GetLastOrderId();
            int flag = 0;
            var ListOfInventory = JsonConvert.DeserializeObject<List<OrderProductModel>>(products);
            var OrderItem = new Order_Item();
            foreach (var item in ListOfInventory)
            {
                if (item.ItemId == "" && item.Name == "" && item.ItemId == "" && item.Quantity == "")
                {
                    continue;
                }

                    if (item.ItemId == "")
                {
                    item.ItemId = "0";
                }
                if (item.Name == "")
                {
                    item.Name = "No Name";
                }
                if (flag == 0)
                {
                    OrderItem.IDOrder = LastOrder.ID;
                    OrderItem.ItemCode = item.ItemId;
                    OrderItem.Note = item.Note;
                    OrderItem.Description = item.Name;
                    if (item.Quantity == "")
                    {
                        OrderItem.Quantity = 0;
                    }
                    else
                    {
                        OrderItem.Quantity = int.Parse(item.Quantity);

                    }


                    Order_ItemServices.Instance.CreateOrder_Item(OrderItem);
                    flag = 1;
                }
                else
                {
                    var user = UserManager.FindById(User.Identity.GetUserId());

                    var Order = new Entities.Order();
                    Order.IDBranch = LastOrder.IDBranch;
                    Order.IDCustomer = LastOrder.IDCustomer;
                    Order.IDBrand = LastOrder.IDBrand;
                    Order.IDSupplier = LastOrder.IDSupplier;
                    Order.Note = LastOrder.Note;
                    Order.Plate = LastOrder.Plate;
                    Order.Chassis = LastOrder.Chassis;
                    Order.IDUser = user.Name;
                    Order.Date = DateTime.Now;
                    Order.Unavailability = CalculateDaysBetweenDates(Order.Date);
                    OrderServices.Instance.CreateOrder(Order);

                    var ModifiedOrder = OrderServices.Instance.GetLastOrderId();


                    OrderItem.IDOrder = ModifiedOrder.ID;
                    OrderItem.ItemCode = item.ItemId;
                    OrderItem.Note = item.Note;
                    OrderItem.Description = item.Name;
                    if (item.Quantity == "")
                    {
                        OrderItem.Quantity = 0;
                    }
                    else
                    {
                        OrderItem.Quantity = int.Parse(item.Quantity);

                    }


                    Order_ItemServices.Instance.CreateOrder_Item(OrderItem);
                    flag = 1;
                }
               

               

            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index(string SearchTerm = "")
        {
            Session["ACTIVER"] = "Order";
            OrderListingViewModel model = new OrderListingViewModel();

            var AllOrders = OrderServices.Instance.GetOrders();
            var AllItems = Order_ItemServices.Instance.GetOrder_Items();

            foreach (var order in AllOrders)
            {
                foreach (var item in AllItems)
                {
                    if (order.ID == item.IDOrder)
                    {
                        var Supplier = SupplierServices.Instance.GetSupplierById(order.IDSupplier);
                        var Customer = CustomerServices.Instance.GetCustomerById(order.IDCustomer);
                        var Branch = BranchServices.Instance.GetBranchById(order.IDBranch);
                        var Brand = BrandServices.Instance.GetBrandById(order.IDBrand);
                        var user = UserManager.FindById(order.IDUser);
                        if (order.Received == null)
                        {
                            order.Received = "off";
                        }
                            var modelfiller = new OrderIndex
                        {
                            Supplier = Supplier?.Description,
                            Customer = Customer?.Description,
                            Alias = Customer?.Alias,
                            Branch = Branch?.Description,
                            Brand = Brand?.Description,
                            // User = user?.Name, // Uncomment this line when needed
                            Chassis = order.Chassis,
                            Plate = order.Plate,
                            Note = order.Note,
                            Date = order.Date,
                            DeliveryDate = order.DeliveryDate,
                            Reminder1 = order.Reminder1,
                            Reminder2 = order.Reminder2,
                            Reminder3 = order.Reminder3,
                            IDOrder = order.ID,
                            IDItem = item.ID,
                            ItemCode = item.ItemCode,
                            Description = item.Description,
                            Quantity = item.Quantity,
                            NoteItem = item.Note,
                            Attachment = order.Attachment,
                            AlternativeCode = order.AlternativeCode,
                            Unavailability = CalculateDaysBetweenDates(order.Date),
                            Received = order.Received,
                            User = order.IDUser,
                            Photo = order.File
                    };

                        if (model.Order == null)
                        {
                            model.Order = new List<OrderIndex>();
                        }

                        model.Order.Add(modelfiller);
                    }
                }
            }

            return View("Index", model);
        }
        public ActionResult ReceivedOrders()
        {
            Session["ACTIVER"] = "Ricevuto Order";
            OrderListingViewModel model = new OrderListingViewModel();

            var AllOrders = OrderServices.Instance.GetReceivedOrders();
            var AllItems = Order_ItemServices.Instance.GetOrder_Items();

            foreach (var order in AllOrders)
            {
                foreach (var item in AllItems)
                {
                    if (order.ID == item.IDOrder)
                    {
                        var Supplier = SupplierServices.Instance.GetSupplierById(order.IDSupplier);
                        var Customer = CustomerServices.Instance.GetCustomerById(order.IDCustomer);
                        var Branch = BranchServices.Instance.GetBranchById(order.IDBranch);
                        var Brand = BrandServices.Instance.GetBrandById(order.IDBrand);
                        var user = UserManager.FindById(order.IDUser);
                        if (order.Received == null)
                        {
                            order.Received = "off";
                        }
                        var modelfiller = new OrderIndex
                        {
                            Supplier = Supplier?.Description,
                            Customer = Customer?.Description,
                            Alias = Customer?.Alias,
                            Branch = Branch?.Description,
                            Brand = Brand?.Description,
                            // User = user?.Name, // Uncomment this line when needed
                            Chassis = order.Chassis,
                            Plate = order.Plate,
                            Note = order.Note,
                            Date = order.Date,
                            DeliveryDate = order.DeliveryDate,
                            Reminder1 = order.Reminder1,
                            Reminder2 = order.Reminder2,
                            Reminder3 = order.Reminder3,
                            IDOrder = order.ID,
                            IDItem = item.ID,
                            ItemCode = item.ItemCode,
                            Description = item.Description,
                            Quantity = item.Quantity,
                            NoteItem = item.Note,
                            Attachment = order.Attachment,
                            AlternativeCode = order.AlternativeCode,
                            Unavailability = CalculateDaysBetweenDates(order.Date),
                            Received = order.Received,
                            User = order.IDUser,
                            Photo = order.File
                        };

                        if (model.Order == null)
                        {
                            model.Order = new List<OrderIndex>();
                        }

                        model.Order.Add(modelfiller);
                    }
                }
            }

            return View("ReceivedOrders", model);
        }

        public ActionResult ConfirmedOrders()
        {
            Session["ACTIVER"] = "Confirmed Order";
            OrderListingViewModel model = new OrderListingViewModel();

            var AllOrders = OrderServices.Instance.GetConfirmedOrders();
            var AllItems = Order_ItemServices.Instance.GetOrder_Items();

            foreach (var order in AllOrders)
            {
                foreach (var item in AllItems)
                {
                    if (order.ID == item.IDOrder)
                    {
                        var Supplier = SupplierServices.Instance.GetSupplierById(order.IDSupplier);
                        var Customer = CustomerServices.Instance.GetCustomerById(order.IDCustomer);
                        var Branch = BranchServices.Instance.GetBranchById(order.IDBranch);
                        var Brand = BrandServices.Instance.GetBrandById(order.IDBrand);
                        var user = UserManager.FindById(order.IDUser);
                        if (order.Received == null)
                        {
                            order.Received = "off";
                        }
                        var modelfiller = new OrderIndex
                        {
                            Supplier = Supplier?.Description,
                            Customer = Customer?.Description,
                            Alias = Customer?.Alias,
                            Branch = Branch?.Description,
                            Brand = Brand?.Description,
                            // User = user?.Name, // Uncomment this line when needed
                            Chassis = order.Chassis,
                            Plate = order.Plate,
                            Note = order.Note,
                            Date = order.Date,
                            DeliveryDate = order.DeliveryDate,
                            Reminder1 = order.Reminder1,
                            Reminder2 = order.Reminder2,
                            Reminder3 = order.Reminder3,
                            IDOrder = order.ID,
                            IDItem = item.ID,
                            ItemCode = item.ItemCode,
                            Description = item.Description,
                            Quantity = item.Quantity,
                            NoteItem = item.Note,
                            Attachment = order.Attachment,
                            AlternativeCode = order.AlternativeCode,
                            Unavailability = CalculateDaysBetweenDates(order.Date),
                            Received = order.Received,
                            User = order.IDUser,
                            Photo = order.File
                        };

                        if (model.Order == null)
                        {
                            model.Order = new List<OrderIndex>();
                        }

                        model.Order.Add(modelfiller);
                    }
                }
            }

            return View("ConfirmedOrders", model);
        }
        [HttpGet]
        public ActionResult Action(int ID = 0)
        {
            if (ID != 0)
            {
                Session["ACTIVER"] = "Order Edit";
            }
            else
            {
                Session["ACTIVER"] = "Add Order";
            }
            OrderActionViewModel model = new OrderActionViewModel();
            if (ID != 0)
            {
                var Order = OrderServices.Instance.GetOrderById(ID);
                var OrderItem = Order_ItemServices.Instance.GetOrder_Items().Where(x => x.IDOrder == Order.ID);

                model.ID = Order.ID;              
                model.IDBranch = Order.IDBranch;
                model.IDCustomer = Order.IDCustomer;
                model.IDBrand = Order.IDBrand;
                model.IDSupplier = Order.IDSupplier;
                model.Note = Order.Note;
                model.Plate = Order.Plate;
                model.Chassis = Order.Chassis;
                model.IDUser = Order.IDUser;
                model.Date = Order.Date;
                model.Photo = Order.File;
                
                if(model.Date != null)
                {
                    model.Unavailability = CalculateDaysBetweenDates(model.Date);
                }
                model.Received = Order.Received;
                model.Attachment = Order.Attachment;
                model.Reminder1 = Order.Reminder1;
                model.Reminder2 = Order.Reminder2;
                model.Reminder3 = Order.Reminder3;
                model.DeliveryDate = Order.DeliveryDate;
                model.Received = Order.Received;
                model.Attachment = Order.Attachment;
                model.IDUser = Order.IDUser;
                foreach (var item in OrderItem)
                {
                    model.ItemCode = item.ItemCode;
                    model.Description = item.Description;
                    model.Quantity = item.Quantity;
                    model.NoteItem = item.Note;
                }
               
            }
            model.Branches = BranchServices.Instance.GetBranchs();
            model.Customers = CustomerServices.Instance.GetCustomers();
            model.Brands = BrandServices.Instance.GetBrands();

            return View("Action", model);
        }

      
        public ActionResult Confirm(int ID)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (ID != 0)
            {
                var Order = OrderServices.Instance.GetOrderById(ID);

                Order.Confirmation = "Yes";
                
                OrderServices.Instance.UpdateOrder(Order);

            }

            return RedirectToAction("ConfirmedOrders");
        }


        [HttpPost]
        public ActionResult Action(OrderActionViewModel model)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (model.ID != 0)
            {
                var Order = OrderServices.Instance.GetOrderById(model.ID);
               
                Order.ID = model.ID;
                Order.IDBranch = model.IDBranch;
                Order.IDCustomer = model.IDCustomer;
                Order.IDBrand = model.IDBrand;
                Order.IDSupplier = model.IDSupplier;
                Order.Note = model.Note;
                Order.Plate = model.Plate;
                Order.Chassis = model.Chassis;
                Order.IDUser = user.Name;
                Order.Date = DateTime.Now;
                Order.DeliveryDate = model.DeliveryDate;
                Order.Reminder1 = model.Reminder1;
                Order.Reminder2 = model.Reminder2;
                Order.Reminder3 = model.Reminder3;
                Order.Attachment = model.Attachment;
                Order.Received = model.Received;
                Order.AlternativeCode = model.AlternativeCode;
                Order.File = model.Photo;
                Order.Unavailability = CalculateDaysBetweenDates(Order.Date);
                checker = 1;
                OrderServices.Instance.UpdateOrder(Order);

            }
            else
            {
                var Order = new Entities.Order();
                Order.IDBranch = model.IDBranch;
                Order.IDCustomer = model.IDCustomer;
                Order.IDBrand = model.IDBrand;
                Order.IDSupplier = model.IDSupplier;
                Order.Note = model.Note;
                Order.Plate = model.Plate;
                Order.Chassis = model.Chassis;
                Order.IDUser = user.Name;               
                Order.Date = DateTime.Now;

                Order.Unavailability = CalculateDaysBetweenDates(Order.Date);
                OrderServices.Instance.CreateOrder(Order);

            }
            //if(model.IDSupplier != 0)
            //{
            //    var supplier = SupplierServices.Instance.GetSupplierById(model.IDSupplier);
            //    if (supplier.Contact == "Telephone")
            //    {
            //        OrderActionViewModel data = new OrderActionViewModel();
            //        data.ID = 1;
            //        data.Note = supplier.Contact;
            //        data.Plate = supplier.Telephone;
            //        return PartialView("_Confirmation", model);
            //    }
            //    else if (supplier.Contact == "Whatsapp")
            //    {
            //        OrderActionViewModel data = new OrderActionViewModel();
            //        data.ID = 1;
            //        data.Note = supplier.Contact;
            //        data.Plate = supplier.Whatsapp;
            //        return PartialView("_Confirmation", model);
            //    }
            //    else
            //    {

            //    }
            //}
            

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSupplierInfo(int supplierId)
        {
            // Replace this with your actual data retrieval logic
            var supplierInfo = SupplierServices.Instance.GetSupplierById(supplierId);

            return Json(supplierInfo, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Confirmation(int ID)
        {
            OrderActionViewModel model = new OrderActionViewModel();
            var Order = OrderServices.Instance.GetOrderById(ID);
            model.ID = Order.ID;
            return PartialView("_Confirmation", model);
        }

        [HttpGet]
        public ActionResult Delete(int ID)
        {
            OrderActionViewModel model = new OrderActionViewModel();
            var Order = OrderServices.Instance.GetOrderById(ID);
            model.ID = Order.ID;
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(OrderActionViewModel model)
        {
            if (model.ID != 0)
            {
                var products = Order_ItemServices.Instance.GetOrder_Items().Where(x => x.IDOrder == model.ID);
                foreach (var item in products)
                {
                    Order_ItemServices.Instance.DeleteOrder_Item(item.ID);
                }
                    var Order = OrderServices.Instance.GetOrderById(model.ID);

                OrderServices.Instance.DeleteOrder(Order.ID);
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportToExcel()
        {


            var orders = OrderServices.Instance.GetOrders();
            var products = new List<Order_Item>();
            foreach (var item in orders)
            {
                var data = Order_ItemServices.Instance.GetItemsByOrderID(item.ID);
                products.Add(data);
            }
           

            // Create a DataTable and populate it with the site data
            System.Data.DataTable tableData = new System.Data.DataTable();
            tableData.Columns.Add("Order Code", typeof(int)); // Replace "Column1" with the actual column name
            tableData.Columns.Add("#N", typeof(int)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Fornitore", typeof(string)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Data Ordine", typeof(string)); // Replace "Column2" with the actual column nam
            tableData.Columns.Add("Marca", typeof(string)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Targa", typeof(string)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Telaio", typeof(string)); // Replace "Column1" with the actual column name
            tableData.Columns.Add("Codice", typeof(string)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Codice Alternativo", typeof(string)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Descrizione", typeof(string)); // Replace "Column2" with the actual column nam
            tableData.Columns.Add("Qnt", typeof(int)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Data Consegna Prevists", typeof(string)); // Replace "Column2" with the actual column name
            tableData.Columns.Add("Alias", typeof(string)); // Replace "Column2" with the actual column nam
            tableData.Columns.Add("Cliente", typeof(string));
            tableData.Columns.Add("Branch", typeof(string)); // Replace "Column2" with the actual column nam
            tableData.Columns.Add("Glorni Indisponibilita", typeof(int));// Replace "Column2" with the actual column name
            tableData.Columns.Add("Allegato", typeof(string)); // Replace "Column2" with the actual column nam
            tableData.Columns.Add("Note", typeof(string));
            tableData.Columns.Add("User", typeof(string)); // Replace "Column2" with the actual column nam
            tableData.Columns.Add("Sol 1", typeof(string));
            tableData.Columns.Add("Sol 2", typeof(string));
            tableData.Columns.Add("Sol 3", typeof(string));
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            foreach (var item in products)
            {
                foreach (var order in orders) {
                   
                    if(order.ID == item.IDOrder)
                    {
                        DataRow row = tableData.NewRow();
                        row["Order Code"] = order.ID;
                        row["#N"] = item.ID;

                        var supplier = SupplierServices.Instance.GetSupplierById(order.IDSupplier);

                       
                        row["Fornitore"] = supplier.Description;
                        row["Data Ordine"] = order.Date;

                        var brand = BrandServices.Instance.GetBrandById(order.IDBrand);

                        row["Marca"] = brand.Description;
                        row["Targa"] = order.Plate;
                        row["Telaio"] = order.Chassis;
                        row["Codice"] = item.ItemCode;
                        row["Codice Alternativo"] = order.AlternativeCode;
                        row["Descrizione"] = item.Description;
                        row["Qnt"] = item.Quantity;
                        if(order.DeliveryDate != null)
                        {
                            row["Data Consegna Prevists"] = order.DeliveryDate.Value.ToShortDateString();
                        }
                        

                        var customer = CustomerServices.Instance.GetCustomerById(order.IDCustomer);

                        row["Alias"] = customer.Alias;
                        row["Cliente"] = customer.Description;

                        var branch = BranchServices.Instance.GetBranchById(order.IDBranch);

                        row["Branch"] = branch.Description;
                        row["Glorni Indisponibilita"] = order.Unavailability;
                        if(order.Attachment == "on")
                        {
                            row["Allegato"] = "Yes";
                        }
                        else
                        {
                            row["Allegato"] = "No";
                        }
                        row["Note"] = item.Note;

                        var user = UserManager.FindById(order.IDUser);

                        //row["User"] = user.Name;
                        if(order.Reminder1 != null)
                        {
                            row["Sol 1"] = order.Reminder1.Value.ToShortDateString();

                        }
                        
                        if (order.Reminder2 != null)
                        {
                            row["Sol 2"] = order.Reminder2.Value.ToShortDateString();

                        }
                        
                        if (order.Reminder3 != null)
                        {
                            row["Sol 3"] = order.Reminder3.Value.ToShortDateString();

                        }
                        
                       
                        tableData.Rows.Add(row);
                    }
                   
                }
                
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
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ActiveOrders.xlsx");
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
                var items = new List<Order_Item>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // or LicenseContext.Commercial

                if (excelfile != null && excelfile.ContentLength > 0)
                {
                    using (var package = new ExcelPackage(excelfile.InputStream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++) // Assuming the first row is header
                        {
                           
                                var order = new Order();
                            var orderitem = new Order_Item();
                            if (worksheet.Cells[row, 7].Value == null)
                            {
                                continue;
                            }

                            if (worksheet.Cells[row, 1].Value != null)
                            {
                                var Supplier = SupplierServices.Instance.GetSupplier(worksheet.Cells[row, 1].Value.ToString()).FirstOrDefault(); ;
                                if(Supplier != null)
                                {
                                    order.IDSupplier = Supplier.ID;
                                }
                                else
                                {
                                    var supplier = new Supplier();
                                    supplier.Description = worksheet.Cells[row, 1].Value.ToString();
                                    SupplierServices.Instance.CreateSupplier(supplier);
                                    var NewSupplier = SupplierServices.Instance.GetLastEntryId();
                                    order.IDSupplier = NewSupplier;
                                }
                            }
                          order.Date = DateTime.Now;
                            if (worksheet.Cells[row, 2].Value != null)
                            {
                                var brand = BrandServices.Instance.GetBrand(worksheet.Cells[row, 2].Value.ToString()).FirstOrDefault();
                                if (brand != null)
                                {
                                    order.IDBrand = brand.ID;
                                }
                                else
                                {
                                    var Brand = new Brand();
                                    Brand.Description = worksheet.Cells[row, 2].Value.ToString();
                                    BrandServices.Instance.CreateBrand(Brand);
                                    var NewBrand = BrandServices.Instance.GetLastEntryId();
                                    order.IDBrand = NewBrand;
                                }
                            }

                            
                            //ProductName
                            if (worksheet.Cells[row, 3].Value != null)
                            {
                                order.Plate = worksheet.Cells[row, 3].Value.ToString();
                            }
                            else
                            {
                                order.Plate = "Not Specified";
                            }

                            if (worksheet.Cells[row, 4].Value != null)
                            {
                               order.Chassis = worksheet.Cells[row, 4].Value.ToString();
                            }
                            else
                            {
                                order.Chassis = "Not Specified";
                            }
                            if (worksheet.Cells[row, 5].Value != null)
                            {
                                orderitem.ItemCode = worksheet.Cells[row, 5].Value.ToString();
                            }
                            else
                            {
                                orderitem.ItemCode = "Not Specified";
                            }
                            if (worksheet.Cells[row, 6].Value != null)
                            {
                                order.AlternativeCode = worksheet.Cells[row, 6].Value.ToString();
                            }
                            else
                            {
                                order.AlternativeCode = "Not Specified";
                            }
                            if (worksheet.Cells[row, 7].Value != null)
                            {
                                orderitem.Description = worksheet.Cells[row, 7].Value.ToString();
                            }
                            else
                            {
                                orderitem.Description = "Not Specified";
                            }
                            if (worksheet.Cells[row, 8].Value != null)
                            {
                                orderitem.Quantity = int.Parse(worksheet.Cells[row, 8].Value.ToString());
                            }
                            else
                            {
                                orderitem.Quantity = 0;
                            }
                           
                            
                            if (worksheet.Cells[row, 10].Value != null)
                            {
                                 var customer =CustomerServices.Instance.GetCustomer(worksheet.Cells[row, 10].Value.ToString()).FirstOrDefault();
                                if (customer != null)
                                {
                                    order.IDCustomer = customer.ID;
                                    
                                }
                                else
                                {
                                    var Customer = new Customer();
                                    Customer.Description = worksheet.Cells[row, 10].Value.ToString();
                                    Customer.Alias = worksheet.Cells[row, 9].Value.ToString();
                                    CustomerServices.Instance.CreateCustomer(Customer);
                                    var NewCustomer = CustomerServices.Instance.GetLastEntryId();
                                    order.IDCustomer = NewCustomer;
                                }
                            }
                            if (worksheet.Cells[row, 11].Value != null)
                            {
                                var branch = BranchServices.Instance.GetBranch(worksheet.Cells[row, 11].Value.ToString()).FirstOrDefault();
                                if (branch != null)
                                {
                                    order.IDBranch = branch.ID;

                                }
                                else
                                {
                                    var Branch = new Branch();
                                    Branch.Description = worksheet.Cells[row, 11].Value.ToString();
                                    BranchServices.Instance.CreateBranch(Branch);                                    
                                    var NewBranch = BranchServices.Instance.GetLastEntryId();
                                    order.IDBranch= NewBranch;
                                }
                            }
                           


                            if (worksheet.Cells[row, 12].Value != null)
                            {
                                if (worksheet.Cells[row, 12].Value.ToString() == "Yes")
                                {
                                    order.Attachment = "on";
                                }
                                
                            }
                            

                            if (worksheet.Cells[row, 13].Value != null)
                            {
                                orderitem.Note = worksheet.Cells[row, 13].Value.ToString(); //SKU

                            }
                            else
                            {
                                orderitem.Note = "Not Specified";
                            }

                           
                           
                            items.Add(orderitem);
                            OrderServices.Instance.CreateOrder(order);
                            var lastid = OrderServices.Instance.GetLastOrderId();
                            orderitem.IDOrder = lastid.ID;
                            Order_ItemServices.Instance.CreateOrder_Item(orderitem);


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


    }
}