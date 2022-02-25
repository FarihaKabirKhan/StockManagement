using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using StockManagement.Filters;
using StockManagement.Models;
using StockManagement.ViewModels;

namespace StockManagement.Controllers
{
    [TestActionFilter]
    public class ProductsController : Controller
    {
        public ActionResult Index(int? id)
        {
            var ctx = new inventoryDBEntities();

            var categoryWiseProductQty = from p in ctx.tblProducts
                                         group p by p.categoryID into g
                                         select new
                                         {
                                             g.FirstOrDefault().categoryID
                                             //Qty = g.Sum(s => s.Quantity)
                                         };
            var listCategory = (from c in ctx.tblProductCategories
                                join cwpq in categoryWiseProductQty on c.categoryID equals cwpq.categoryID
                                select new VmCategory
                                {
                                    categoryName = c.categoryName,
                                    categoryID = cwpq.categoryID
                                    //Quantity = cwpq.Qty
                                }).ToList();
            var listProduct = (from p in ctx.tblProducts
                               join c in ctx.tblProductCategories on p.categoryID equals c.categoryID
                               where p.categoryID == id
                               select new VmProduct
                               {
                                   categoryID = p.categoryID,
                                   categoryName = c.categoryName,
                                   ImagePath = p.ImagePath,
                                   salesPrice = p.salesPrice,
                                   purchasePrice = p.purchasePrice,
                                   productID = p.productID,
                                   productName = p.productName,
                                   status = p.status
                               }).ToList();

            var oCategoryWiseProduct = new VmCategoryWiseProduct();
            oCategoryWiseProduct.CategoryList = listCategory;
            oCategoryWiseProduct.ProductList = listProduct;
            oCategoryWiseProduct.categoryID = listProduct.Count > 0 ? listProduct[0].categoryID : 0;
            oCategoryWiseProduct.categoryName = listProduct.Count > 0 ? listProduct[0].categoryName : "";

            return View(oCategoryWiseProduct);
        }

        public ActionResult Create()
        {
            var model = new VmProductCategory();
            var ctx = new inventoryDBEntities();
            model.CategoryList = ctx.tblProductCategories.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(tblProductCategory model, string[] ProductName, decimal[] salesPrice, decimal[] purchasePrice, string[] status, HttpPostedFileBase[] imgFile)
        {
            var ctx = new inventoryDBEntities();
            var oCatetory = (from c in ctx.tblProductCategories where c.categoryName == model.categoryName.Trim() select c).FirstOrDefault();
            if (oCatetory == null)
            {
                ctx.tblProductCategories.Add(model);
                ctx.SaveChanges();
            }
            else
            {
                model.categoryID = oCatetory.categoryID;
            }

            var listProduct = new List<tblProduct>();
            for (int i = 0; i < ProductName.Length; i++)
            {
                string imgPath = "";
                if (imgFile[i] != null && imgFile[i].ContentLength > 0)
                {
                    var fileName = Path.GetFileName(imgFile[i].FileName);
                    string fileLocation = Path.Combine(
                        Server.MapPath("~/uploads"), fileName);
                    imgFile[i].SaveAs(fileLocation);

                    imgPath = "/uploads/" + imgFile[i].FileName;
                }

                var newProduct = new tblProduct();
                newProduct.productName = ProductName[i];
                newProduct.salesPrice = salesPrice[i];
                newProduct.purchasePrice = purchasePrice[i];
                newProduct.ImagePath = imgPath;
                newProduct.status = status[i];
                newProduct.categoryID = model.categoryID;
                listProduct.Add(newProduct);
            }
            ctx.tblProducts.AddRange(listProduct);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            var ctx = new inventoryDBEntities();
            var oProduct = (from p in ctx.tblProducts
                            join c in ctx.tblProductCategories on p.categoryID equals c.categoryID
                            where p.productID == id
                            select new VmProduct
                            {
                                categoryID = p.categoryID,
                                categoryName = c.categoryName,
                                ImagePath = p.ImagePath,
                                salesPrice = p.salesPrice,
                                purchasePrice = p.purchasePrice,
                                productID = p.productID,
                                productName = p.productName,
                                status = p.status
                            }).FirstOrDefault();
            oProduct.CategoryList = ctx.tblProductCategories.ToList();
            return View(oProduct);
        }

        [HttpPost]
        public ActionResult Edit(VmProduct model)
        {
            var ctx = new inventoryDBEntities();

            string imgPath = "";
            if (model.ImgFile != null && model.ImgFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(model.ImgFile.FileName);
                string fileLocation = Path.Combine(
                    Server.MapPath("~/uploads"), fileName);
                model.ImgFile.SaveAs(fileLocation);

                imgPath = "/uploads/" + model.ImgFile.FileName;
            }

            var oProduct = ctx.tblProducts.Where(w => w.productID == model.productID).FirstOrDefault();
            if (oProduct != null)
            {
                oProduct.productName = model.productName;
                oProduct.salesPrice = model.salesPrice;
                oProduct.purchasePrice = model.purchasePrice;
                oProduct.categoryID = model.categoryID;
                if (!string.IsNullOrEmpty(imgPath))
                {
                    var fileName = Path.GetFileName(oProduct.ImagePath);
                    string fileLocation = Path.Combine(Server.MapPath("~/uploads"), fileName);
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }
                }
                oProduct.ImagePath = imgPath == "" ? oProduct.ImagePath : imgPath;

                ctx.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult EditMultiple(int id)
        {
            var ctx = new inventoryDBEntities();
            var oCategoryWiseProduct = new VmCategoryWiseProduct();
            var listProduct = (from p in ctx.tblProducts
                               join c in ctx.tblProductCategories on p.categoryID equals c.categoryID
                               where p.categoryID == id
                               select new VmProduct
                               {
                                   categoryID = p.categoryID,
                                   categoryName = c.categoryName,
                                   ImagePath = p.ImagePath,
                                   salesPrice = p.salesPrice,
                                   purchasePrice = p.purchasePrice,
                                   productID = p.productID,
                                   productName = p.productName,
                                   status = p.status
                               }).ToList();
            oCategoryWiseProduct.ProductList = listProduct;

            oCategoryWiseProduct.CategoryList = (from c in ctx.tblProductCategories
                                                 select new VmCategory
                                                 {
                                                     categoryID = c.categoryID,
                                                     categoryName = c.categoryName
                                                 }).ToList();
            oCategoryWiseProduct.categoryID = listProduct.Count > 0 ? listProduct[0].categoryID : 0;
            oCategoryWiseProduct.categoryName = listProduct.Count > 0 ? listProduct[0].categoryName : "";
            return View(oCategoryWiseProduct);
        }

        [HttpPost]
        public ActionResult EditMultiple(tblProductCategory model, int[] productID, string[] productName, decimal[] salesPrice, decimal[] purchasePrice, string[] status, HttpPostedFileBase[] imgFile)
        {
            var ctx = new inventoryDBEntities();
            var listProduct = new List<tblProduct>();
            for (int i = 0; i < productName.Length; i++)
            {
                if (productID[i] > 0)
                {
                    string imgPath = "";
                    if (imgFile[i] != null && imgFile[i].ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(imgFile[i].FileName);
                        string fileLocation = Path.Combine(
                            Server.MapPath("~/uploads"), fileName);
                        imgFile[i].SaveAs(fileLocation);

                        imgPath = "/uploads/" + imgFile[i].FileName;
                    }
                    int pid = productID[i];
                    var oProduct = ctx.tblProducts.Where(w => w.productID == pid).FirstOrDefault();
                    if (oProduct != null)
                    {
                        oProduct.productName = productName[i];
                        oProduct.salesPrice = salesPrice[i];
                        oProduct.purchasePrice = purchasePrice[i];
                        oProduct.status = status[i];
                        oProduct.categoryID = model.categoryID;
                        if (!string.IsNullOrEmpty(imgPath))
                        {
                            var fileName = Path.GetFileName(oProduct.ImagePath);
                            string fileLocation = Path.Combine(Server.MapPath("~/uploads"), fileName);
                            if (System.IO.File.Exists(fileLocation))
                            {
                                System.IO.File.Delete(fileLocation);
                            }
                        }
                        oProduct.ImagePath = imgPath == "" ? oProduct.ImagePath : imgPath;
                        ctx.SaveChanges();
                    }
                }
                else if (!string.IsNullOrEmpty(productName[i]))
                {
                    string imgPath = "";
                    if (imgFile[i] != null && imgFile[i].ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(imgFile[i].FileName);
                        string fileLocation = Path.Combine(
                            Server.MapPath("~/uploads"), fileName);
                        imgFile[i].SaveAs(fileLocation);

                        imgPath = "/uploads/" + imgFile[i].FileName;
                    }

                    var newProduct = new tblProduct();
                    newProduct.productName = productName[i];
                    newProduct.salesPrice = salesPrice[i];
                    newProduct.purchasePrice = purchasePrice[i]; 
                    newProduct.ImagePath = imgPath;
                    newProduct.status = status[i];
                    newProduct.categoryID = model.categoryID;
                    ctx.tblProducts.Add(newProduct);
                    ctx.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var ctx = new inventoryDBEntities();
            var oProduct = ctx.tblProducts.Where(p => p.productID == id).FirstOrDefault();
            if (oProduct != null)
            {
                ctx.tblProducts.Remove(oProduct);
                ctx.SaveChanges();

                var fileName = Path.GetFileName(oProduct.ImagePath);
                string fileLocation = Path.Combine(
                    Server.MapPath("~/uploads"), fileName);

                if (System.IO.File.Exists(fileLocation))
                {

                    System.IO.File.Delete(fileLocation);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteMultiple(int id)
        {
            var ctx = new inventoryDBEntities();
            var listProduct = ctx.tblProducts.Where(p => p.categoryID == id).ToList();
            foreach (var oProduct in listProduct)
            {
                if (oProduct != null)
                {
                    ctx.tblProducts.Remove(oProduct);
                    ctx.SaveChanges();

                    var fileName = Path.GetFileName(oProduct.ImagePath);
                    string fileLocation = Path.Combine(
                        Server.MapPath("~/uploads"), fileName);
                    if (System.IO.File.Exists(fileLocation))
                    {

                        System.IO.File.Delete(fileLocation);
                    }
                }
            }

            var oCategory = ctx.tblProductCategories.Where(c => c.categoryID == id).FirstOrDefault();
            ctx.tblProductCategories.Remove(oCategory);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        #region //For Report
        private inventoryDBEntities db = new inventoryDBEntities();
        public ActionResult StockList()
        {
            return View(db.tblProducts.ToList());
        }

        public ActionResult Reports(string ReportType)
        {
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Reports/ProductReport.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "ProductDataSet";
            reportDataSource.Value = db.tblProducts.ToList();
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


            Response.AddHeader("content-disposition", "attachment;fileName = Product." + fileNameExtension);
            return File(renderdByte, fileNameExtension);
        }
        #endregion


    }
}
