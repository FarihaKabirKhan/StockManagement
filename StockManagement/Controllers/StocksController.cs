using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using StockManagement.Models;

namespace StockManagement.Controllers
{
    public class StocksController : Controller
    {
        private inventoryDBEntities db = new inventoryDBEntities();

        // GET: Stocks
        public ActionResult Index()
        {
            var tblStocks = db.tblStocks.Include(t => t.tblProduct).Include(t => t.tblPurchase);
            return View(tblStocks.ToList());
        }

        // GET: Stocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblStock tblStock = db.tblStocks.Find(id);
            if (tblStock == null)
            {
                return HttpNotFound();
            }
            return View(tblStock);
        }

        // GET: Stocks/Create
        public ActionResult Create()
        {
            ViewBag.productID = new SelectList(db.tblProducts, "productID", "productName");
            ViewBag.purchaseID = new SelectList(db.tblPurchases, "purchaseID", "purchaseID");
            return View();
        }

        // POST: Stocks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "stockID,productID,purchaseID,quantity,status")] tblStock tblStock)
        {
            if (ModelState.IsValid)
            {
                db.tblStocks.Add(tblStock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.productID = new SelectList(db.tblProducts, "productID", "productName", tblStock.productID);
            ViewBag.purchaseID = new SelectList(db.tblPurchases, "purchaseID", "purchaseID", tblStock.purchaseID);
            return View(tblStock);
        }

        // GET: Stocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblStock tblStock = db.tblStocks.Find(id);
            if (tblStock == null)
            {
                return HttpNotFound();
            }
            ViewBag.productID = new SelectList(db.tblProducts, "productID", "productName", tblStock.productID);
            ViewBag.purchaseID = new SelectList(db.tblPurchases, "purchaseID", "purchaseID", tblStock.purchaseID);
            return View(tblStock);
        }

        // POST: Stocks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "stockID,productID,purchaseID,quantity,status")] tblStock tblStock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblStock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productID = new SelectList(db.tblProducts, "productID", "productName", tblStock.productID);
            ViewBag.purchaseID = new SelectList(db.tblPurchases, "purchaseID", "purchaseID", tblStock.purchaseID);
            return View(tblStock);
        }

        // GET: Stocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblStock tblStock = db.tblStocks.Find(id);
            if (tblStock == null)
            {
                return HttpNotFound();
            }
            return View(tblStock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblStock tblStock = db.tblStocks.Find(id);
            db.tblStocks.Remove(tblStock);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult StockControl(int pID, int quantity)
        {
            string message = "";
            int qty = db.tblStocks.Where(q => q.productID == pID).Select(q => q.quantity).FirstOrDefault();
            if (qty < quantity)
            {
                message = "Sorry! your product is exceed the Sales Limit.";
            }
            return Json(message);
        }

        #region //For Report
        public ActionResult StockList()
        {
            return View(db.tblStocks.ToList());
        }

        public ActionResult Reports(string ReportType)
        {
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Reports/StockReport.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "StockDataSet";
            reportDataSource.Value = db.tblStocks.ToList();
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


            Response.AddHeader("content-disposition", "attachment;fileName = stock." + fileNameExtension);
            return File(renderdByte, fileNameExtension);
        }
        #endregion
    }
}
