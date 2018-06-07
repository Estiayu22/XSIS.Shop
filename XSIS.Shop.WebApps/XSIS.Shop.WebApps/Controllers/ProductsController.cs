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
using XSIS.Shop.Repository;

namespace XSIS.Shop.WebApps.Controllers
{
    public class ProductsController : Controller
    {
        private ShopEntities db = new ShopEntities();
        private ProductRepository service = new ProductRepository();

        public ActionResult Index()
        {
            var result = service.GetAllProduct();
            var product = db.Product.Include(p => p.Supplier);
            List<ProductViewModel> ListVM = new List<ProductViewModel>();

            foreach(var item in product)
            {
                ProductViewModel ViewModel = new ProductViewModel();
                ViewModel.Id = item.Id;
                ViewModel.ProductName = item.ProductName;
                ViewModel.SupplierId = item.SupplierId;
                ViewModel.UnitPrice = item.UnitPrice;
                ViewModel.Package = item.Package;
                ViewModel.IsDiscontinued = item.IsDiscontinued;
                ViewModel.SupplierName = item.Supplier.CompanyName;
                ListVM.Add(ViewModel);
            }

            return View(ListVM);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Product.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            ProductViewModel ViewModel = new ProductViewModel();
            ViewModel.Id = product.Id;
            ViewModel.ProductName = product.ProductName;
            ViewModel.SupplierId = product.SupplierId;
            ViewModel.UnitPrice = product.UnitPrice;
            ViewModel.Package = product.Package;
            ViewModel.IsDiscontinued = product.IsDiscontinued;
            ViewModel.SupplierName = product.Supplier.CompanyName;

            return View(ViewModel);
        }

        public ActionResult Create()
        {
            ViewBag.SupplierId = new SelectList(db.Supplier, "Id", "CompanyName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                Product model = new Product();

                model.ProductName = product.ProductName;
                model.SupplierId = product.SupplierId;
                model.UnitPrice = product.UnitPrice;
                model.Package = product.Package;
                model.IsDiscontinued = product.IsDiscontinued;

                db.Product.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SupplierId = new SelectList(db.Supplier, "Id", "CompanyName", product.SupplierId);
            return View(product);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Product.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            ProductViewModel ViewModel = new ProductViewModel();
            ViewModel.Id = product.Id;
            ViewModel.ProductName = product.ProductName;
            ViewModel.SupplierId = product.SupplierId;
            ViewModel.UnitPrice = product.UnitPrice;
            ViewModel.Package = product.Package;
            ViewModel.IsDiscontinued = product.IsDiscontinued;
            ViewModel.SupplierName = product.Supplier.CompanyName;

            ViewBag.SupplierId = new SelectList(db.Supplier, "Id", "CompanyName", product.SupplierId);
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                Product model = new Product();
                model.Id = product.Id;
                model.ProductName = product.ProductName;
                model.SupplierId = product.SupplierId;
                model.UnitPrice = product.UnitPrice;
                model.Package = product.Package;
                model.IsDiscontinued = product.IsDiscontinued;

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SupplierId = new SelectList(db.Supplier, "Id", "CompanyName", product.SupplierId);
            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Product.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            ProductViewModel ViewModel = new ProductViewModel();
            ViewModel.Id = product.Id;
            ViewModel.ProductName = product.ProductName;
            ViewModel.SupplierId = product.SupplierId;
            ViewModel.UnitPrice = product.UnitPrice;
            ViewModel.Package = product.Package;
            ViewModel.IsDiscontinued = product.IsDiscontinued;
            ViewModel.SupplierName = product.Supplier.CompanyName;

            return View(ViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
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
