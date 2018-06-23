using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using XSIS.Shop.Repository;
using XSIS.Shop.ViewModels;

namespace XSIS.Shop.WebApps.Controllers
{
    public class SuppliersController : Controller
    {

        private SupplierRepository service = new SupplierRepository();

        [HttpGet]
        public ActionResult Index(string FullName, string CityCountry, string PhoneNo)
        {
            List<SupplierViewModel> list = null;
            var result = list;

            if (!string.IsNullOrEmpty(FullName) || !string.IsNullOrEmpty(CityCountry) || !string.IsNullOrEmpty(PhoneNo) ||
                !string.IsNullOrWhiteSpace(FullName) || !string.IsNullOrWhiteSpace(CityCountry) || !string.IsNullOrWhiteSpace(PhoneNo)
                || FullName != " " || CityCountry != " " || PhoneNo != " ")

            {
                result = service.SearchByKey(FullName, CityCountry, PhoneNo);
            }
            else
            {
                result = service.GetAllSupplier();
            }

            return View(result.ToList());
        }

        public ActionResult List()
        {
            string search = string.Empty;
            return PartialView("_List", service.GetAllSupplier().ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int idx = id.HasValue ? id.Value : 0;
            SupplierViewModel supplier = service.GetDetailSupplierById(idx);

            if (supplier == null)
            {
                return HttpNotFound();
            }

            return PartialView("_Details", supplier);
        }

        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SupplierViewModel supplier)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.AddNewSupplier(supplier);
                    return Json(new { success = true, from = "create" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { success = false, msg = e.Message }, JsonRequestBehavior.AllowGet);
                }

            }

            return PartialView("_Create", supplier);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int idx = id.HasValue ? id.Value : 0;
            SupplierViewModel supplier = service.GetSupplierById(idx);

            if (supplier == null)
            {
                return HttpNotFound();
            }

            return PartialView("_Edit",supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SupplierViewModel supplier)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.UpdateSupplier(supplier);
                    return Json(new { success = true, from = "edit" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { success = false, msg = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }

            return PartialView("_Edit", supplier);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int idx = id.HasValue ? id.Value : 0;
            SupplierViewModel supplier = service.GetSupplierById(idx);

            if (supplier == null)
            {
                return HttpNotFound();
            }

            return PartialView("_Delete", supplier);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                service.DeleteSupplier(id);
                return Json(new { success = true, from = "delete" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
