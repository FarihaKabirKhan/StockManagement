using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using StockManagement.Filters;
using StockManagement.Models;

namespace StockManagement.Controllers
{
    [TestActionFilter]
    public class SalesController : Controller
    {
        private inventoryDBEntities db = new inventoryDBEntities();

        // GET: Sales
        public ActionResult Index()
        {
            var tblSales = db.tblSales.Include(t => t.tblCustomer).Include(t => t.tblProduct);
            return View(tblSales.ToList());
        }

        // GET: Sales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSale tblSale = db.tblSales.Find(id);
            if (tblSale == null)
            {
                return HttpNotFound();
            }
            return View(tblSale);
        }

        // GET: Sales/Create
        public ActionResult Create()
        {
            ViewBag.customerID = new SelectList(db.tblCustomers, "customerID", "customerName");
            ViewBag.productID = new SelectList(db.tblPurchases.Where(p=>p.productID==p.tblProduct.productID).Select(p=>p.tblProduct), "productID", "productName");
            return View();
        }

        // POST: Sales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "salesID,productID,customerID,productStatus,quantity,rate,totalPrice,purchaseDate,vat,discount,netTotal,transactionType,transactionDate,invoiceNo")] tblSale tblSale)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                int qty = db.tblStocks.Where(q => q.productID == tblSale.productID).Select(q => q.quantity).FirstOrDefault();
                if (qty > tblSale.quantity)
                {
                    tblSale oReceive = new tblSale();
                    db.tblSales.Add(tblSale);
                    var oStock = (from o in db.tblStocks where o.productID == tblSale.productID select o).FirstOrDefault();
                    if (oStock == null)
                    {
                        oStock = new tblStock();
                        oStock.productID = tblSale.productID;
                        oStock.quantity = tblSale.quantity;
                        oStock.status = "Sale";
                        db.tblStocks.Add(oStock);
                    }
                    else
                    {
                        oStock.quantity -= tblSale.quantity;
                        oStock.status = "Sale";
                    }
                    db.SaveChanges();
                }
                else
                {
                    message = "Sorry! your product is exceed the Sales Limit.";
                }
                ViewBag.message = message;

            }
            return RedirectToAction("Index");

        }

        // GET: Sales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSale tblSale = db.tblSales.Find(id);
            if (tblSale == null)
            {
                return HttpNotFound();
            }
            ViewBag.customerID = new SelectList(db.tblCustomers, "customerID", "customerName", tblSale.customerID);
            ViewBag.productID = new SelectList(db.tblProducts, "productID", "productName", tblSale.productID);
            return View(tblSale);
        }

        // POST: Sales/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "salesID,productID,customerID,productStatus,quantity,rate,totalPrice,purchaseDate,vat,discount,netTotal,transactionType,transactionDate,invoiceNo")] tblSale tblSale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerID = new SelectList(db.tblCustomers, "customerID", "customerName", tblSale.customerID);
            ViewBag.productID = new SelectList(db.tblProducts, "productID", "productName", tblSale.productID);
            return View(tblSale);

            //if (ModelState.IsValid)
            //{
            //    tblSale oReceive = new tblSale();
            //    db.Entry(tblSale).State = EntityState.Modified;
            //    var oStock = (from o in db.tblStocks where o.productID == tblSale.productID select o).FirstOrDefault();
            //    if (oStock == null)
            //    {
            //        oStock = new tblStock();
            //        oStock.productID = tblSale.productID;
            //        oStock.quantity = tblSale.quantity;
            //        oStock.status = "Sale";
            //        db.tblStocks.Add(oStock);
            //    }
            //    else
            //    {
            //        oStock.quantity -= tblSale.quantity;
            //        oStock.status = "Sale";
            //    }
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(tblSale);

        }

        // GET: Sales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSale tblSale = db.tblSales.Find(id);
            if (tblSale == null)
            {
                return HttpNotFound();
            }
            return View(tblSale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSale tblSale = db.tblSales.Find(id);
            db.tblSales.Remove(tblSale);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //For Price Show
        [HttpPost]
        public JsonResult GetPrice(int productID)
        {
            decimal price = db.tblProducts.Where(x => x.productID == productID).Select(X => X.salesPrice).FirstOrDefault();
            return Json(price);
        }

        #region //For Report
        public ActionResult StockList()
        {
            return View(db.tblStocks.ToList());
        }

        public ActionResult Reports(string ReportType)
        {
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Reports/SalesReport.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "SalesDataSet";
            reportDataSource.Value = db.tblSales.ToList();
            localreport.DataSources.Add(reportDataSource);

            string reportType = ReportType;
            string mimeType;
            string encoding;
            string fileNameExtension;
            if (reportType == "Excel")
            {
                fileNameExtension = "xlsx";
            }
            else if (reportType == "PDF")
            {
                fileNameExtension = "pdf";

            }

            else if (reportType == "Word")
            {
                //fileNameExtension = "docsx";
                fileNameExtension = "docx";
            }
            else
            {
                fileNameExtension = "png";
            }

            string[] streams;
            Warning[] warnings;
            byte[] renderdByte;

            renderdByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension, out streams, out warnings);


            Response.AddHeader("content-disposition", "attachment;fileName = SalesReport." + fileNameExtension);
            return File(renderdByte, fileNameExtension);
        }
        #endregion
    }
}
