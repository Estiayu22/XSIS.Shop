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
    public class CustomersController : Controller
    {
        private CustomerRepository service = new CustomerRepository();

        [HttpGet]
        public ActionResult Index(string FullName, string CityCountry, string Email)
        {
            List<CustomerViewModel> list = null;
            var result = list;

            if (!string.IsNullOrEmpty(FullName) || !string.IsNullOrEmpty(CityCountry) || !string.IsNullOrEmpty(Email) ||
                !string.IsNullOrWhiteSpace(FullName) || !string.IsNullOrWhiteSpace(CityCountry) || !string.IsNullOrWhiteSpace(Email)
                || FullName != " " || CityCountry != " " || Email != " ")

            {
                result = service.SearchByKey(FullName, CityCountry, Email);
            }
            else
            {
                result = service.GetAllCustomer();
            }

            return View(result.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int idx = id.HasValue ? id.Value : 0;
            CustomerViewModel custVM = service.GetCustomerById(idx);

            if (custVM == null)
            {
                return HttpNotFound();
            }

            return View(custVM);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                bool CheckValidation = true;

                // Cek Nama Depan dan Nama Belakang Identik
                string NamaLengkap = service.CekNamaExisting(customer.FirstName, customer.LastName);

                // Cek Email Identik
                string Email = string.Empty;

                if(!string.IsNullOrEmpty(customer.Email))
                {
                    Email = service.CekEmailExisting(customer.Email);
                }

                if (NamaLengkap == (customer.FirstName + " " + customer.LastName))
                {
                    ModelState.AddModelError("", "Maaf Nama Lengkap sudah ada di database");
                    CheckValidation = false;
                }

                if (Email == customer.Email)
                {
                    ModelState.AddModelError("", "Maaf Email sudah ada di database");
                    CheckValidation = false;
                }

                if(CheckValidation == false)
                {
                    return View(customer);
                }
                else
                {
                    service.AddNewCustomer(customer);
                    return RedirectToAction("Index");
                }
            }

            return View(customer);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int idx = id.HasValue ? id.Value : 0;
            CustomerViewModel custVM = service.GetCustomerById(idx);

            if (custVM == null)
            {
                return HttpNotFound();
            }

            return View(custVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                service.UpdateCustomer(customer);
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int idx = id.HasValue ? id.Value : 0;
            CustomerViewModel custVM = service.GetCustomerById(idx);

            if (custVM == null)
            {
                return HttpNotFound();
            }

            return View(custVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            service.DeleteCustomer(id);
            return RedirectToAction("Index");
        }
    }
}
