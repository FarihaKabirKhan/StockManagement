using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using PagedList;
using StockManagement.Filters;
using StockManagement.Models;

namespace StockManagement.Controllers
{
    [TestActionFilter]
    public class WarehousesController : Controller
    {
        private inventoryDBEntities db = new inventoryDBEntities();

        // GET: Warehouses
        public ActionResult Index(string searchBy, string search, int? page, string sortBy)
        {
            ViewBag.SortNameParameter = string.IsNullOrEmpty(sortBy) ? "warehouseName desc" : "";
            ViewBag.SortManagerParameter = sortBy == "warehouseManager" ? "warehouseManager desc" : "warehouseManager";

            //For sorting
            var warehouses = db.tblWarehouses.AsQueryable();

            if (searchBy == "warehouseManager")
            {
                warehouses = warehouses.Where(x => x.warehouseManager == search || search == null);
            }
            else
            {
                warehouses = warehouses.Where(x => x.warehouseName.StartsWith(search) || search == null);
            }

            switch (sortBy)
            {
                case "warehouseName desc":
                    warehouses = warehouses.OrderByDescending(x => x.warehouseName);
                    break;
                case "warehouseManager desc":
                    warehouses = warehouses.OrderByDescending(x => x.warehouseManager);
                    break;
                case "warehouseManager":
                    warehouses = warehouses.OrderBy(x => x.warehouseManager);
                    break;
                default:
                    warehouses = warehouses.OrderBy(x => x.warehouseName);
                    break;
            }
            return View(warehouses.ToPagedList(page ?? 1, 3));
        }

        //#region
        //[HttpPost]
        //public ActionResult Index(string searchTerm)
        //{
        //    inventoryDBEntities db = new inventoryDBEntities();
        //    List<tblWarehouse> tblWarehouses;
        //    if (string.IsNullOrEmpty(searchTerm))
        //    {
        //        tblWarehouses = db.tblWarehouses.ToList();
        //    }
        //    else
        //    {
        //        tblWarehouses = db.tblWarehouses.Where(x => x.warehouseName.StartsWith(searchTerm)).ToList();
        //    }
        //    return View(tblWarehouses);
        //}

        //public JsonResult GetWarehouse(string term)
        //{
        //    inventoryDBEntities db = new inventoryDBEntities();
        //    List<string> tblWarehouses;

        //    tblWarehouses = db.tblWarehouses.Where(x => x.warehouseName.StartsWith(term))
        //     .Select(y => y.warehouseName).ToList();

        //    return Json(tblWarehouses, JsonRequestBehavior.AllowGet);
        //}
        //#endregion

        // GET: Warehouses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblWarehouse tblWarehouse = db.tblWarehouses.Find(id);
            if (tblWarehouse == null)
            {
                return HttpNotFound();
            }
            return View(tblWarehouse);
        }

        // GET: Warehouses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Warehouses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "warehouseID,warehouseNo,warehouseName,warehouseManager,address")] tblWarehouse tblWarehouse)
        {
            if (ModelState.IsValid)
            {
                db.tblWarehouses.Add(tblWarehouse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblWarehouse);
        }

        // GET: Warehouses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblWarehouse tblWarehouse = db.tblWarehouses.Find(id);
            if (tblWarehouse == null)
            {
                return HttpNotFound();
            }
            return View(tblWarehouse);
        }

        // POST: Warehouses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "warehouseID,warehouseNo,warehouseName,warehouseManager,address")] tblWarehouse tblWarehouse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblWarehouse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblWarehouse);
        }

        // GET: Warehouses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblWarehouse tblWarehouse = db.tblWarehouses.Find(id);
            if (tblWarehouse == null)
            {
                return HttpNotFound();
            }
            return View(tblWarehouse);
        }

        // POST: Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblWarehouse tblWarehouse = db.tblWarehouses.Find(id);
            db.tblWarehouses.Remove(tblWarehouse);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region //For Report
        public ActionResult WarehouseList()
        {
            return View(db.tblWarehouses.ToList());
        }

        public ActionResult Reports(string ReportType)
        {
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Reports/WarehouseReport.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "Warehouse";
            reportDataSource.Value = db.tblWarehouses.ToList();
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


            Response.AddHeader("content-disposition", "attachment;fileName = WarehouseReport." + fileNameExtension);
            return File(renderdByte, fileNameExtension);
        }
        #endregion
    }
}
