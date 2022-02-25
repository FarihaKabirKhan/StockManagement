using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StockManagement.Controllers
{
    public class UsersController : Controller
    {
        private inventoryDBEntities db = new inventoryDBEntities();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.tblUsers.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = db.tblUsers.Find(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            return View(tblUser);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Username,UserPass,UserType")] tblUser tblUser)
        {
            if (ModelState.IsValid)
            {
                db.tblUsers.Add(tblUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblUser);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = db.tblUsers.Find(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            return View(tblUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Username,UserPass,UserType")] tblUser tblUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblUser);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = db.tblUsers.Find(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            return View(tblUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblUser tblUser = db.tblUsers.Find(id);
            db.tblUsers.Remove(tblUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //User Permission
        public async Task<ActionResult> UserRole(int userId)
        {
            var listTblUserRole = await db.tblUserRoles.Where(o => o.UserID == userId).ToListAsync();
            var oTblUser = await db.tblUsers.Where(o => o.UserID == userId).FirstOrDefaultAsync();
            TempData["Username"] = oTblUser.Username;

            #region create menu at view page
            var listUserRole = new List<tblUserRole>();

            #region Products
            var oUserRole = listTblUserRole.Where(o => o.UserID == userId && o.PageName == "Products").FirstOrDefault();
            if (oUserRole == null)
            {
                oUserRole = new tblUserRole();
                oUserRole.UserID = userId;
                oUserRole.PageName = "Products"; // controller name
                oUserRole.IsCreate = false;
                oUserRole.IsRead = false;
                oUserRole.IsUpdate = false;
                oUserRole.IsDelete = false;

                listUserRole.Add(oUserRole);
            }
            else
            {
                listUserRole.Add(oUserRole);
            }
            #endregion

            #region Category
            oUserRole = listTblUserRole.Where(o => o.UserID == userId && o.PageName == "ProductCategories").FirstOrDefault();
            if (oUserRole == null)
            {
                oUserRole = new tblUserRole();
                oUserRole.UserID = userId;
                oUserRole.PageName = "ProductCategories";    // controller name
                oUserRole.IsCreate = false;
                oUserRole.IsRead = false;
                oUserRole.IsUpdate = false;
                oUserRole.IsDelete = false;

                listUserRole.Add(oUserRole);
            }
            else
            {
                listUserRole.Add(oUserRole);
            }
            #endregion

            #region Suppliers
            oUserRole = listTblUserRole.Where(o => o.UserID == userId && o.PageName == "Suppliers").FirstOrDefault();
            if (oUserRole == null)
            {
                oUserRole = new tblUserRole();
                oUserRole.UserID = userId;
                oUserRole.PageName = "Suppliers"; // controller name
                oUserRole.IsCreate = false;
                oUserRole.IsRead = false;
                oUserRole.IsUpdate = false;
                oUserRole.IsDelete = false;

                listUserRole.Add(oUserRole);
            }
            else
            {
                listUserRole.Add(oUserRole);
            }
            #endregion

            #region Warehouse
            oUserRole = listTblUserRole.Where(o => o.UserID == userId && o.PageName == "Warehouses").FirstOrDefault();
            if (oUserRole == null)
            {
                oUserRole = new tblUserRole();
                oUserRole.UserID = userId;
                oUserRole.PageName = "Warehouses"; // controller name
                oUserRole.IsCreate = false;
                oUserRole.IsRead = false;
                oUserRole.IsUpdate = false;
                oUserRole.IsDelete = false;

                listUserRole.Add(oUserRole);
            }
            else
            {
                listUserRole.Add(oUserRole);
            }
            #endregion

            #region Customers
            oUserRole = listTblUserRole.Where(o => o.UserID == userId && o.PageName == "Customers").FirstOrDefault();
            if (oUserRole == null)
            {
                oUserRole = new tblUserRole();
                oUserRole.UserID = userId;
                oUserRole.PageName = "Customers"; // controller name
                oUserRole.IsCreate = false;
                oUserRole.IsRead = false;
                oUserRole.IsUpdate = false;
                oUserRole.IsDelete = false;

                listUserRole.Add(oUserRole);
            }
            else
            {
                listUserRole.Add(oUserRole);
            }
            #endregion

            #region Purchase
            oUserRole = listTblUserRole.Where(o => o.UserID == userId && o.PageName == "Purchases").FirstOrDefault();
            if (oUserRole == null)
            {
                oUserRole = new tblUserRole();
                oUserRole.UserID = userId;
                oUserRole.PageName = "Purchases"; // controller name
                oUserRole.IsCreate = false;
                oUserRole.IsRead = false;
                oUserRole.IsUpdate = false;
                oUserRole.IsDelete = false;

                listUserRole.Add(oUserRole);
            }
            else
            {
                listUserRole.Add(oUserRole);
            }
            #endregion

            #region Sales
            oUserRole = listTblUserRole.Where(o => o.UserID == userId && o.PageName == "Sales").FirstOrDefault();
            if (oUserRole == null)
            {
                oUserRole = new tblUserRole();
                oUserRole.UserID = userId;
                oUserRole.PageName = "Sales"; // controller name
                oUserRole.IsCreate = false;
                oUserRole.IsRead = false;
                oUserRole.IsUpdate = false;
                oUserRole.IsDelete = false;

                listUserRole.Add(oUserRole);
            }
            else
            {
                listUserRole.Add(oUserRole);
            }
            #endregion

            #region Stock
            oUserRole = listTblUserRole.Where(o => o.UserID == userId && o.PageName == "Stocks").FirstOrDefault();
            if (oUserRole == null)
            {
                oUserRole = new tblUserRole();
                oUserRole.UserID = userId;
                oUserRole.PageName = "Stocks"; // controller name
                oUserRole.IsCreate = false;
                oUserRole.IsRead = false;
                oUserRole.IsUpdate = false;
                oUserRole.IsDelete = false;

                listUserRole.Add(oUserRole);
            }
            else
            {
                listUserRole.Add(oUserRole);
            }
            #endregion

            #region Home
            oUserRole = listTblUserRole.Where(o => o.UserID == userId && o.PageName == "Home").FirstOrDefault();
            if (oUserRole == null)
            {
                oUserRole = new tblUserRole();
                oUserRole.UserID = userId;
                oUserRole.PageName = "Home"; // controller name
                oUserRole.IsCreate = false;
                oUserRole.IsRead = false;
                oUserRole.IsUpdate = false;
                oUserRole.IsDelete = false;

                listUserRole.Add(oUserRole);
            }
            else
            {
                listUserRole.Add(oUserRole);
            }
            #endregion

            #endregion

            return View(listUserRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserRole(tblUserRole[] tblUserRoles)
        {
            var userRoleIdMax = db.tblUserRoles.Max(o => (int?)o.UserRoleID) ?? 0;
            for (var i = 0; i < tblUserRoles.Length; i++)
            {
                int userRoleId = tblUserRoles[i].UserRoleID;
                var oTblUserRole = await db.tblUserRoles.Where(o => o.UserRoleID == userRoleId).FirstOrDefaultAsync();
                if (oTblUserRole == null) // insert
                {
                    oTblUserRole = new tblUserRole();
                    oTblUserRole.UserRoleID = ++userRoleIdMax;
                    oTblUserRole.UserID = tblUserRoles[i].UserID;
                    oTblUserRole.PageName = tblUserRoles[i].PageName;
                    oTblUserRole.IsCreate = tblUserRoles[i].IsCreate;
                    oTblUserRole.IsRead = tblUserRoles[i].IsRead;
                    oTblUserRole.IsUpdate = tblUserRoles[i].IsUpdate;
                    oTblUserRole.IsDelete = tblUserRoles[i].IsDelete;
                    db.tblUserRoles.Add(oTblUserRole);
                }
                else // update
                {
                    oTblUserRole.IsCreate = tblUserRoles[i].IsCreate;
                    oTblUserRole.IsRead = tblUserRoles[i].IsRead;
                    oTblUserRole.IsUpdate = tblUserRoles[i].IsUpdate;
                    oTblUserRole.IsDelete = tblUserRoles[i].IsDelete;
                }
            }
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}