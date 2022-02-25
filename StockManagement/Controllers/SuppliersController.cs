using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StockManagement.Filters;
using StockManagement.Models;
using PagedList;
using PagedList.Mvc;
using Microsoft.Reporting.WebForms;

namespace StockManagement.Controllers
{
    //[TestActionFilter]
    public class SuppliersController : Controller
    {
        private inventoryDBEntities db = new inventoryDBEntities();

        // GET: Suppliers
        public ActionResult Index(string searchBy, string search, int? page, string sortBy)
        {
            ViewBag.SortNameParameter = string.IsNullOrEmpty(sortBy) ? "supplierName desc" : "";
            ViewBag.SortManagerParameter = sortBy == "managerName" ? "managerName desc" : "managerName";

            //if (searchBy == "managerName")
            //{
            //    return View(db.tblSuppliers.Where(x => x.managerName == search || search == null)
            //     .ToList().ToPagedList(page ?? 1, 3));
            //}
            //else
            //{
            //    return View(db.tblSuppliers.Where(x => x.supplierName.StartsWith(search) || search == null)
            //     .ToList().ToPagedList(page ?? 1, 3));
            //}

            //For sorting
            var suppliers = db.tblSuppliers.AsQueryable();

            if (searchBy == "managerName")
            {
                suppliers = suppliers.Where(x => x.managerName == search || search == null);        
            }
            else
            {
                suppliers = suppliers.Where(x => x.supplierName.StartsWith(search) || search == null);       
            }

            switch (sortBy)
            {
                case "supplierName desc":
                    suppliers = suppliers.OrderByDescending(x => x.supplierName);
                    break;
                case "managerName desc":
                    suppliers = suppliers.OrderByDescending(x => x.managerName);
                    break;
                case "managerName":
                    suppliers = suppliers.OrderBy(x => x.managerName);
                    break;
                default:
                    suppliers = suppliers.OrderBy(x => x.supplierName);
                    break;
            }
            return View(suppliers.ToPagedList(page ?? 1, 3));
        }

        [HttpPost]
        public ActionResult Index(string search)
        {
            inventoryDBEntities db = new inventoryDBEntities();
            List<tblSupplier> tblSuppliers;
            if(string.IsNullOrEmpty(search))
            {
                tblSuppliers = db.tblSuppliers.ToList();
            }
            else
            {
                tblSuppliers = db.tblSuppliers.Where(x => x.supplierName.StartsWith(search)).ToList();
            }
            return View(tblSuppliers);
        }

        public JsonResult GetSuppliers(string term)
        {
            inventoryDBEntities db = new inventoryDBEntities();
            List<string> tblSuppliers;

            tblSuppliers = db.tblSuppliers.Where(x => x.supplierName.StartsWith(term))
             .Select(y => y.supplierName).ToList();
            
            return Json(tblSuppliers,JsonRequestBehavior.AllowGet);
        }

        // GET: Suppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSupplier tblSupplier = db.tblSuppliers.Find(id);
            if (tblSupplier == null)
            {
                return HttpNotFound();
            }
            return View(tblSupplier);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "supplierID,supplierName,mobileNumber,address,balance,managerName")] tblSupplier tblSupplier)
        {
            if (ModelState.IsValid)
            {
                db.tblSuppliers.Add(tblSupplier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblSupplier);
        }

        // GET: Suppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSupplier tblSupplier = db.tblSuppliers.Find(id);
            if (tblSupplier == null)
            {
                return HttpNotFound();
            }
            return View(tblSupplier);
        }

        // POST: Suppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "supplierID,supplierName,mobileNumber,address,balance,managerName")] tblSupplier tblSupplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSupplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblSupplier);
        }

        // GET: Suppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSupplier tblSupplier = db.tblSuppliers.Find(id);
            if (tblSupplier == null)
            {
                return HttpNotFound();
            }
            return View(tblSupplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSupplier tblSupplier = db.tblSuppliers.Find(id);
            db.tblSuppliers.Remove(tblSupplier);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region //For Report
        public ActionResult SupplierList()
        {
            return View(db.tblSuppliers.ToList());
        }

        public ActionResult Reports(string ReportType)
        {
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Reports/Supplier.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "Supplier";
            reportDataSource.Value = db.tblSuppliers.ToList();
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


            Response.AddHeader("content-disposition", "attachment;fileName = SupplierReport." + fileNameExtension);
            return File(renderdByte, fileNameExtension);
        }
        #endregion
    }
}
