using CustomerAVS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using PagedList;  
using PagedList.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using CustomerAVS.Helpers;


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

        public ActionResult ExportToExcel()
        {
            var customers = customerDAL.GetAllCustomers(); 

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Clientes");

                // Agregar título
                ws.Cell(1, 1).Value = "Lista de Clientes";
                ws.Range("A1:G1").Merge().Style.Font.SetBold().Font.FontSize = 16;
                ws.Cell(2, 1).Value = "Fecha de Exportación: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ws.Range("A2:G2").Merge().Style.Font.SetItalic();

                // Encabezados
                string[] headers = { "ID", "Nombre", "Apellido", "Teléfono", "Compañía", "Última Visita", "Cuenta Abierta" };
                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cell(4, i + 1).Value = headers[i];
                    ws.Cell(4, i + 1).Style.Font.SetBold();
                    ws.Cell(4, i + 1).Style.Fill.BackgroundColor = XLColor.Gray;
                    ws.Cell(4, i + 1).Style.Font.FontColor = XLColor.White;
                }

                // Llenado de datos
                int row = 5;
                foreach (var item in customers)
                {
                    ws.Cell(row, 1).Value = item.ID;
                    ws.Cell(row, 2).Value = item.FirstName;
                    ws.Cell(row, 3).Value = item.LastName;
                    ws.Cell(row, 4).Value = item.PhoneNumber;
                    ws.Cell(row, 5).Value = item.Company;
                    ws.Cell(row, 6).Value = item.LastVisit.ToShortDateString();
                    ws.Cell(row, 7).Value = item.AccountOpened.ToShortDateString();
                    row++;
                }

                ws.Columns().AdjustToContents(); 

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Clientes.xlsx");
                }
            }
        }



        public ActionResult ExportToPDF()
        {
            var customers = customerDAL.GetAllCustomers();
            MemoryStream workStream = new MemoryStream();
            Document doc = new Document(PageSize.A4, 10f, 10f, 20f, 20f);
            PdfWriter writer = PdfWriter.GetInstance(doc, workStream);

            writer.PageEvent = new PdfPageEventHelperCustom();

            doc.Open();

            // Agregar título
            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            Paragraph title = new Paragraph("Lista de Clientes", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            doc.Add(title);

            // Agregar fecha de exportación
            Font dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.GRAY);
            Paragraph date = new Paragraph("Fecha de Exportación: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dateFont)
            {
                Alignment = Element.ALIGN_RIGHT
            };
            doc.Add(date);
            doc.Add(new Paragraph("\n"));

            // Crear la tabla
            PdfPTable table = new PdfPTable(7) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 8, 15, 15, 15, 20, 15, 15 });

            // Definir las fuentes
            Font fontHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);
            Font fontCell = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.BLACK);

            // Encabezados
            string[] headers = { "ID", "Nombre", "Apellido", "Teléfono", "Compañía", "Última Visita", "Cuenta Abierta" };
            foreach (var header in headers)
            {
                PdfPCell cell = new PdfPCell(new Phrase(header, fontHeader))
                {
                    BackgroundColor = BaseColor.DARK_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                };
                table.AddCell(cell);
            }

            // Datos
            foreach (var item in customers)
            {
                table.AddCell(new PdfPCell(new Phrase(item.ID.ToString(), fontCell)));
                table.AddCell(new PdfPCell(new Phrase(item.FirstName, fontCell)));
                table.AddCell(new PdfPCell(new Phrase(item.LastName, fontCell)));
                table.AddCell(new PdfPCell(new Phrase(item.PhoneNumber, fontCell)));
                table.AddCell(new PdfPCell(new Phrase(item.Company, fontCell)));
                table.AddCell(new PdfPCell(new Phrase(item.LastVisit.ToShortDateString(), fontCell)));
                table.AddCell(new PdfPCell(new Phrase(item.AccountOpened.ToShortDateString(), fontCell)));
            }

            doc.Add(table);
            doc.Close();

            byte[] byteArray = workStream.ToArray();
            return File(byteArray, "application/pdf", "Clientes.pdf");
        }

    }
}