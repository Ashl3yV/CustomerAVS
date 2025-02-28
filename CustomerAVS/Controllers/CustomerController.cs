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
using PagedList;  
using PagedList.Mvc;  

namespace CustomerAVS.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerDAL customerDAL = new CustomerDAL();

        public ActionResult Index(int? page)
        {
            var customers = customerDAL.GetAllCustomers(); 
            int pageSize = 10; 
            int pageNumber = (page ?? 1); 

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

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            var customer = customerDAL.GetAllCustomers().FirstOrDefault(c => c.ID == id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(customer updatedCustomer)
        {
            if (ModelState.IsValid)
            {
                customerDAL.UpdateCustomer(updatedCustomer);
                return RedirectToAction("Index");
            }
            return View(updatedCustomer);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            var customer = customerDAL.GetAllCustomers().FirstOrDefault(c => c.ID == id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            customerDAL.DeleteCustomer(id);
            return RedirectToAction("Index");
        }

        public ActionResult Dashboard()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

    }
}