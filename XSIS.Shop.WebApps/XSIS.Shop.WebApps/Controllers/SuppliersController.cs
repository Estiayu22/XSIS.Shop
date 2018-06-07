using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using XSIS.Shop.WebApps.Models;
using XSIS.Shop.WebApps.ViewModels;

namespace XSIS.Shop.WebApps.Controllers
{
    public class SuppliersController : Controller
    {
        private ShopEntities db = new ShopEntities();

        public ActionResult Index()
        {
            var ListSupplier = db.Supplier.ToList();
            List<SupplierViewModel> listVM = new List<SupplierViewModel>();

            foreach(var item in ListSupplier)
            {
                SupplierViewModel ViewModel = new SupplierViewModel();
                ViewModel.Id = item.Id;
                ViewModel.CompanyName = item.CompanyName;
                ViewModel.ContactName = item.ContactName;
                ViewModel.ContactTitle = item.ContactTitle;
                ViewModel.City = item.City;
                ViewModel.Country = item.Country;
                ViewModel.Phone = item.Phone;
                ViewModel.Fax = item.Fax;
                listVM.Add(ViewModel);
            }

            return View(listVM);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Supplier supplier = db.Supplier.Find(id);

            if (supplier == null)
            {
                return HttpNotFound();
            }

            SupplierViewModel ViewModel = new SupplierViewModel();
            ViewModel.Id = supplier.Id;
            ViewModel.CompanyName = supplier.CompanyName;
            ViewModel.ContactName = supplier.ContactName;
            ViewModel.ContactTitle = supplier.ContactTitle;
            ViewModel.City = supplier.City;
            ViewModel.Country = supplier.Country;
            ViewModel.Phone = supplier.Phone;
            ViewModel.Fax = supplier.Fax;

            return View(ViewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SupplierViewModel supplier)
        {
            if (ModelState.IsValid)
            {
                Supplier model = new Supplier();

                model.CompanyName = supplier.CompanyName;
                model.ContactName = supplier.ContactName;
                model.ContactTitle = supplier.ContactTitle;
                model.City = supplier.City;
                model.Country = supplier.Country;
                model.Phone = supplier.Phone;
                model.Fax = supplier.Fax;

                db.Supplier.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Supplier supplier = db.Supplier.Find(id);

            if (supplier == null)
            {
                return HttpNotFound();
            }

            SupplierViewModel ViewModel = new SupplierViewModel();
            ViewModel.Id = supplier.Id;
            ViewModel.CompanyName = supplier.CompanyName;
            ViewModel.ContactName = supplier.ContactName;
            ViewModel.ContactTitle = supplier.ContactTitle;
            ViewModel.City = supplier.City;
            ViewModel.Country = supplier.Country;
            ViewModel.Phone = supplier.Phone;
            ViewModel.Fax = supplier.Fax;

            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SupplierViewModel supplier)
        {
            if (ModelState.IsValid)
            {
                Supplier model = new Supplier();
                model.Id = supplier.Id;
                model.CompanyName = supplier.CompanyName;
                model.ContactName = supplier.ContactName;
                model.ContactTitle = supplier.ContactTitle;
                model.City = supplier.City;
                model.Country = supplier.Country;
                model.Phone = supplier.Phone;
                model.Fax = supplier.Fax;

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Supplier supplier = db.Supplier.Find(id);

            if (supplier == null)
            {
                return HttpNotFound();
            }

            SupplierViewModel ViewModel = new SupplierViewModel();
            ViewModel.Id = supplier.Id;
            ViewModel.CompanyName = supplier.CompanyName;
            ViewModel.ContactName = supplier.ContactName;
            ViewModel.ContactTitle = supplier.ContactTitle;
            ViewModel.City = supplier.City;
            ViewModel.Country = supplier.Country;
            ViewModel.Phone = supplier.Phone;
            ViewModel.Fax = supplier.Fax;

            return View(ViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = db.Supplier.Find(id);
            db.Supplier.Remove(supplier);
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
