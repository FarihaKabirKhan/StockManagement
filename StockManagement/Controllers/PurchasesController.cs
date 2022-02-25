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
    public class PurchasesController : Controller
    {
        private inventoryDBEntities db = new inventoryDBEntities();

        // GET: Purchases
        public ActionResult Index()
        {
            var tblPurchases = db.tblPurchases.Include(t => t.tblProduct).Include(t => t.tblSupplier).Include(t => t.tblWarehouse);
            return View(tblPurchases.ToList());
        }

      
        // GET: Purchases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPurchase tblPurchase = db.tblPurchases.Find(id);
            if (tblPurchase == null)
            {
                return HttpNotFound();
            }
            return View(tblPurchase);
        }

        // GET: Purchases/Create
        public ActionResult Create()
        {
            ViewBag.productID = new SelectList(db.tblProducts, "productID", "productName");
            ViewBag.supplierID = new SelectList(db.tblSuppliers, "supplierID", "supplierName");
            ViewBag.warehouseID = new SelectList(db.tblWarehouses, "warehouseID", "warehouseName");
            return View();
        }

        // POST: Purchases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "purchaseID,productID,supplierID,warehouseID,purchasePrice,quantity,totalPrice,purchaseDate,description")] tblPurchase tblPurchase)
        {
            //if (ModelState.IsValid)
            //{
            //    db.tblPurchases.Add(tblPurchase);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.productID = new SelectList(db.tblProducts, "productID", "productName", tblPurchase.productID);
            //ViewBag.supplierID = new SelectList(db.tblSuppliers, "supplierID", "supplierName", tblPurchase.supplierID);
            //ViewBag.warehouseID = new SelectList(db.tblWarehouses, "warehouseID", "warehouseName", tblPurchase.warehouseID);
            //return View(tblPurchase);


            if (ModelState.IsValid)
            {
                tblPurchase oReceive = new tblPurchase();
                db.tblPurchases.Add(tblPurchase);
                var oStock = (from o in db.tblStocks where o.productID == tblPurchase.productID select o).FirstOrDefault();
                if (oStock == null)
                {
                    oStock = new tblStock();
                    oStock.productID = tblPurchase.productID;
                    oStock.quantity = tblPurchase.quantity;
                    oStock.status = "Receive";
                    db.tblStocks.Add(oStock);
                }
                else
                {
                    oStock.quantity += tblPurchase.quantity;
                    oStock.status = "Receive";
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblPurchase);
        }


        // GET: Purchases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPurchase tblPurchase = db.tblPurchases.Find(id);
            if (tblPurchase == null)
            {
                return HttpNotFound();
            }
            ViewBag.productID = new SelectList(db.tblProducts, "productID", "productName", tblPurchase.productID);
            ViewBag.supplierID = new SelectList(db.tblSuppliers, "supplierID", "supplierName", tblPurchase.supplierID);
            ViewBag.warehouseID = new SelectList(db.tblWarehouses, "warehouseID", "warehouseName", tblPurchase.warehouseID);
            return View(tblPurchase);
        }

        // POST: Purchases/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "purchaseID,productID,supplierID,warehouseID,purchasePrice,quantity,totalPrice,purchaseDate,description")] tblPurchase tblPurchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblPurchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productID = new SelectList(db.tblProducts, "productID", "productName", tblPurchase.productID);
            ViewBag.supplierID = new SelectList(db.tblSuppliers, "supplierID", "supplierName", tblPurchase.supplierID);
            ViewBag.warehouseID = new SelectList(db.tblWarehouses, "warehouseID", "warehouseName", tblPurchase.warehouseID);
            return View(tblPurchase);

            //if (ModelState.IsValid)
            //{
            //    tblPurchase oReceive = new tblPurchase();

            //    db.Entry(tblPurchase).State = EntityState.Modified;
            //    var oStock = (from o in db.tblStocks where o.productID == tblPurchase.productID select o).FirstOrDefault();
            //    if (oStock == null)
            //    {
            //        oStock = new tblStock();
            //        oStock.productID = tblPurchase.productID;
            //        oStock.quantity = tblPurchase.quantity;
            //        oStock.status = "Receive";
            //        db.tblStocks.Add(oStock);
            //    }
            //    else
            //    {
            //        oStock.quantity += tblPurchase.quantity;
            //        //db.Entry(tblPurchase).State = EntityState.Modified;
            //        oStock.status = "Receive";
            //    }
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(tblPurchase);

        }

        // GET: Purchases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPurchase tblPurchase = db.tblPurchases.Find(id);
            if (tblPurchase == null)
            {
                return HttpNotFound();
            }
            return View(tblPurchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblPurchase tblPurchase = db.tblPurchases.Find(id);
            db.tblPurchases.Remove(tblPurchase);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //For Price Show
        [HttpPost]
        public JsonResult GetPrice(int productID)
        {
            decimal? price = db.tblProducts.Where(x => x.productID == productID).Select(X => X.purchasePrice).FirstOrDefault();
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
            localreport.ReportPath = Server.MapPath("~/Reports/PurchaseReport.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "PurchaseDataSet";
            reportDataSource.Value = db.tblPurchases.ToList();
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


            Response.AddHeader("content-disposition", "attachment;fileName = PurchaseReport." + fileNameExtension);
            return File(renderdByte, fileNameExtension);
        }
        #endregion

    }
}
