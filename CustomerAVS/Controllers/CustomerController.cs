using CustomerAVS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CustomerAVS.Models;
using System.Data.SqlClient;
using System.Data;

namespace CustomerAVS.Controllers
{
    public class CustomerController : Controller
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["DBCustomer"].ToString();

        public static List<customer> olista = new List<customer>();
        // GET: Customer
        public ActionResult Index()
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlConnection cmd = new 
            }
        }
    }
}