using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using DRS.Services;
using DRS.ViewModels;
using DRS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DRS.Controllers
{
    public class AdminController : Controller
    {
        private AMSignInManager _signInManager;
        private AMRolesManager _rolesManager;
        private AMUserManager _userManager;
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
        // GET: Admin
        public ActionResult Index()
        {
            AdminViewModel model = new AdminViewModel();
            var user = UserManager.FindById(User.Identity.GetUserId());
            model.Name = user.Name;
            return View(model);
        }


        public ActionResult Dashboard()
        {
            AdminViewModel model = new AdminViewModel();
            var user = UserManager.FindById(User.Identity.GetUserId());
            model.SignedInUser = user;
            return View(model);
        }



        [HttpPost]
        public ActionResult Dashboard(string SearchTerm)
        {
            AdminViewModel model = new AdminViewModel();
            var user = UserManager.FindById(User.Identity.GetUserId());
            model.SignedInUser = user;
            return View(model);
        }


    }
}