using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using XSIS.Shop.ViewModels;
using XSIS.Shop.Repository;

namespace XSIS.Shop.WebApps.Controllers
{
    public class ProductsController : Controller
    {
        private ProductRepository service = new ProductRepository();

        public ActionResult Index()
        {
            var result = service.GetAllProduct();
            return View(result.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int idx = id.HasValue ? id.Value : 0;
            ProductViewModel product = service.GetProductById(idx);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        public ActionResult Create()
        {
            ViewBag.ChkDefValue = "true";
            ViewBag.SupplierId = new SelectList(service.GetSupplierDDL(), "Id", "CompanyName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                //service.AddNewProduct(product);
                return RedirectToAction("Index");
            }

            ViewBag.SupplierId = new SelectList(service.GetSupplierDDL(), "Id", "CompanyName", product.SupplierId);
            return View(product);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int idx = id.HasValue ? id.Value : 0;
            ProductViewModel product = service.GetProductById(idx);

            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.ChkValue = product.IsDiscontinued.ToString().ToLower();

            if(product.IsDiscontinued == true)
            {
                ViewBag.ChkSelect = "checked=checked";
            }
            else
            {
                ViewBag.ChkSelect = "";

            }

            ViewBag.SupplierId = new SelectList(service.GetSupplierDDL(), "Id", "CompanyName", product.SupplierId);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                //service.UpdateProduct(product);
                return RedirectToAction("Index");
            }

            ViewBag.SupplierId = new SelectList(service.GetSupplierDDL(), "Id", "CompanyName", product.SupplierId);
            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int idx = id.HasValue ? id.Value : 0;
            ProductViewModel product = service.GetProductById(idx);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            service.DeleteProduct(id);
            return RedirectToAction("Index");
        }
    }
}
