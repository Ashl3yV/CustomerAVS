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
using PagedList;  // Agregar esta librería
using PagedList.Mvc;  // Para la paginación en la vista

namespace CustomerAVS.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerDAL customerDAL = new CustomerDAL();

        public ActionResult Index(int? page)
        {
            var customers = customerDAL.GetAllCustomers(); // Obtener todos los clientes
            int pageSize = 10; // Cantidad de registros por página
            int pageNumber = (page ?? 1); // Página actual (si es null, será 1)

            return View(customers.ToPagedList(pageNumber, pageSize));
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(customer newCustomer)
        {
            if (ModelState.IsValid)
            {
                newCustomer.LastUpdated = DateTime.Now;
                customerDAL.AddCustomer(newCustomer);
                return RedirectToAction("Index");
            }
            return View(newCustomer);
        }
    }
}