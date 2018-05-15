using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Trace3.Models;

namespace Trace3.Controllers
{
    public class ImportDatasController : Controller
    {
        private TestDataEntities db = new TestDataEntities();

        // GET: ImportDatas
        public ActionResult Index()
        {
            var importData = db.ImportData.Include(i => i.Product);
            return View(importData.ToList());
        }

        // GET: ImportDatas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportData importData = db.ImportData.Find(id);
            if (importData == null)
            {
                return HttpNotFound();
            }
            return View(importData);
        }

        // GET: ImportDatas/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "ProductName");
            return View();
        }

        // POST: ImportDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ImportDataID,ProductID,MFD,EXP,Lisence,Declaration,Item,PurchaseNum")] ImportData importData)
        {
            if (ModelState.IsValid)
            {
                db.ImportData.Add(importData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "ProductName", importData.ProductID);
            return View(importData);
        }

        // GET: ImportDatas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportData importData = db.ImportData.Find(id);
            if (importData == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "ProductName", importData.ProductID);
            return View(importData);
        }

        // POST: ImportDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ImportDataID,ProductID,MFD,EXP,Lisence,Declaration,Item,PurchaseNum")] ImportData importData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(importData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "ProductName", importData.ProductID);
            return View(importData);
        }

        // GET: ImportDatas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportData importData = db.ImportData.Find(id);
            if (importData == null)
            {
                return HttpNotFound();
            }
            return View(importData);
        }

        // POST: ImportDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ImportData importData = db.ImportData.Find(id);
            db.ImportData.Remove(importData);
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
    }
}
