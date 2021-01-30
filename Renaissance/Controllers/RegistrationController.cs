using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Renaissance.Models;

namespace Renaissance.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        [HttpGet]
        public ActionResult Registration(int id=0)
        {
            User userModel = new User();
            return View(userModel);
        }
        /*
        [HttpGet]
        public ActionResult addOrEdit(int id=0)
        {
            User userModel = new User();
            return View(userModel);
        }
        */
        [HttpPost]
        public ActionResult Registration(User userMoldel)
        {
            using (dbModels dbModel = new dbModels())
            {
                if (dbModel.Users.Any(x => x.userName == userMoldel.userName))
                {
                    ViewBag.DuplicateMessage = "Username already exist.";
                    return View("Registration", userMoldel);
                }
                dbModel.Users.Add(userMoldel);
                dbModel.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successful";
            return View("Registration", new User());
        }
      
    }
}