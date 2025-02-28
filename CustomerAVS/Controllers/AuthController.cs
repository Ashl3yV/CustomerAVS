using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerAVS.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
  
            if (username == "admin" && password == "1234")
            {

                Session["User"] = username;


                return RedirectToAction("Index", "Customer");
            }


            ViewBag.Message = "Usuario o contraseña incorrectos";
            return View();
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}