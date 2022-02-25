using Microsoft.Reporting.WebForms;
using StockManagement.Libs.Utilities;
using StockManagement.Models;
using StockManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManagement.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DashboardViewModel dashboard = new DashboardViewModel();

            dashboard.customer_count = db.tblCustomers.Count();
            dashboard.product_count = db.tblProducts.Count();
            dashboard.supplier_count = db.tblSuppliers.Count();
            dashboard.warehouse_count = db.tblWarehouses.Count();
            dashboard.purchase_count = db.tblPurchases.Count();
            dashboard.sales_count = db.tblSales.Count();
            dashboard.stock_count = db.tblStocks.Count();

            return View(dashboard);
        }

        #region Login
        private inventoryDBEntities db = new inventoryDBEntities();
        public ActionResult Login()
        {
            return View();
        }

        // POST: HomeController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Username,UserPass")] tblUser model)
        {
            try
            {
                var otblUser = db.tblUsers.Where(o => o.Username == model.Username && o.UserPass == model.UserPass).FirstOrDefault();
                if (otblUser != null)
                {
                    var listtblUserRole = db.tblUserRoles.Where(o => o.UserID == otblUser.UserID).ToList();
                    Session["tblUsers"] = otblUser;
                    Session["tblUserRoles"] = listtblUserRole;
                    if (otblUser.UserType == UserType.SuperAdmin.ToString())
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (otblUser.UserType == UserType.GeneralUser.ToString())
                    {
                        return RedirectToAction("Index", "Products");
                    }
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Remove("tblUsers");
            return RedirectToAction("Login", "Home");
        }
        #endregion
    }
}