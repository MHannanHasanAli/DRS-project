using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using DRS.Services;
using DRS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DRS.Controllers
{
    public class HomeController : Controller
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
        public ActionResult Index()
        {
            AdminViewModel model = new AdminViewModel();
          
            return View(model);
        }


       
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login(string Email, string Password)
        {
            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                return View();
            }
        }



        //[HttpPost]
        //public ActionResult Login(string Email, string Password)
        //{
        //    var user = DBNull;/* AMUserManager(username, password);*/
        //    if (user != null)
        //    {
        //        Session["ID"] = user.ID.ToString();
        //        Session["UserName"] = user.UserName.ToString();
        //        Session["Email"] = user.Email.ToString();
        //        Session["Role"] = user.Role.ToString();
        //        if (user.Role == "Admin")
        //        {
        //            return RedirectToAction("AdminDashboard", "Home");
        //        }
        //        else if (user.Role == "Kitchen Staff")
        //        {
        //            return RedirectToAction("KitchenDashboard", "Home");
        //        }
        //        else if (user.Role == "Cashier")
        //        {
        //            return RedirectToAction("BillingDashboard", "Home");
        //        }

        //        else if (user.Role == "Waiter")
        //        {
        //            return RedirectToAction("WaiterApp", "Home");
        //        }
        //        else if (user.Role == "Kitchen Master")
        //        {
        //            return RedirectToAction("KitchenDashboard", "Home");
        //        }
        //    }
        //    else
        //    {

        //    }
        //    {
        //        string msg = "Invalid Password or UserName";
        //        TempData["ErrorMessage"] = msg;
        //    }

        //    Session["ID"] = null;
        //    Session["UserName"] = null;
        //    Session["Email"] = null;
        //    Session["Role"] = null;
        //    return RedirectToAction("Index", "Login");

        //}


    }
}