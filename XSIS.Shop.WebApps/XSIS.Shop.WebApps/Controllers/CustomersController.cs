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
using System.Web.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace XSIS.Shop.WebApps.Controllers
{
    public class CustomersController : Controller
    {
        private string ApiUrl = WebConfigurationManager.AppSettings["XSIS.Shop.API"];

        [HttpGet]
        public ActionResult Index(string FullName, string CityCountry, string Email)
        {
            List<CustomerViewModel> list = null;
            var result = list;

            if (!string.IsNullOrEmpty(FullName) || !string.IsNullOrEmpty(CityCountry) || !string.IsNullOrEmpty(Email) ||
                !string.IsNullOrWhiteSpace(FullName) || !string.IsNullOrWhiteSpace(CityCountry) || !string.IsNullOrWhiteSpace(Email)
                || FullName != " " || CityCountry != " " || Email != " ")

            {
                // Get All Customer API Akses http://localhost:2099/api/CustomerApi/FullName/CityCountry/Email with parameter
                string ApiEndPoint = ApiUrl + "api/CustomerApi/SearchByKey/" + (FullName + "|" + CityCountry + "|" + Email);
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

                string ListResult = response.Content.ReadAsStringAsync().Result.ToString();
                result = JsonConvert.DeserializeObject<List<CustomerViewModel>>(ListResult);
            }
            else
            {
                // Get All Customer API Akses http://localhost:2099/api/CustomerApi/ without parameter
                string ApiEndPoint = ApiUrl + "api/CustomerApi/";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

                string ListResult = response.Content.ReadAsStringAsync().Result.ToString();
                result = JsonConvert.DeserializeObject<List<CustomerViewModel>>(ListResult);
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

            // Details API Akses http://localhost:2099/api/CustomerApi/1
            string ApiEndPoint = ApiUrl + "api/CustomerApi/" + idx;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string result = response.Content.ReadAsStringAsync().Result.ToString();
            CustomerViewModel custVM = JsonConvert.DeserializeObject<CustomerViewModel>(result);

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
                // Check Nama API Akses http://localhost:2099/api/CustomerApi/CekNamaExisting?param=Maria&param2=Anders
                string ApiEndPoint = ApiUrl + "api/CustomerApi/CekNamaExisting/" + customer.FirstName + "/" + customer.LastName;
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

                string NamaLengkap = response.Content.ReadAsStringAsync().Result.ToString().Replace("\\", "").Replace("\"", "");

                // Cek Email Identik
                string Email = string.Empty;

                if(!string.IsNullOrEmpty(customer.Email))
                {
                    // Check Email API Akses http://localhost:2099/api/CustomerApi/CheckNamaExisting/Email
                    string ApiEndPoint1 = ApiUrl + "api/CustomerApi/CekEmailExisting/" + customer.Email;
                    HttpClient client1 = new HttpClient();
                    HttpResponseMessage response1 = client1.GetAsync(ApiEndPoint1).Result;

                    Email = response1.Content.ReadAsStringAsync().Result.ToString().Replace("\\", "").Replace("\"", "");
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
                    // Create API Akses http://localhost:2099/api/CustomerApi/Customer

                    string json = JsonConvert.SerializeObject(customer);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    string ApiEndPoint2 = ApiUrl + "api/CustomerApi/";
                    HttpClient client2 = new HttpClient();

                    HttpResponseMessage response2 = client2.PostAsync(ApiEndPoint2, byteContent).Result;

                    string result = response2.Content.ReadAsStringAsync().Result.ToString();
                    int success = int.Parse(result);

                    if(success == 1)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(customer);
                    }
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

            // Details API Akses http://localhost:2099/api/CustomerApi/1
            string ApiEndPoint = ApiUrl + "api/CustomerApi/" + idx;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string result = response.Content.ReadAsStringAsync().Result.ToString();
            CustomerViewModel custVM = JsonConvert.DeserializeObject<CustomerViewModel>(result);

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
                // Update API Akses http://localhost:2099/api/CustomerApi/Customer

                string json = JsonConvert.SerializeObject(customer);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                string ApiEndPoint = ApiUrl + "api/CustomerApi/";
                HttpClient client = new HttpClient();

                HttpResponseMessage response = client.PutAsync(ApiEndPoint, byteContent).Result;

                string result = response.Content.ReadAsStringAsync().Result.ToString();
                int success = int.Parse(result);

                if (success == 1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(customer);
                }
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

            // Details API Akses http://localhost:2099/api/CustomerApi/1
            string ApiEndPoint = ApiUrl + "api/CustomerApi/" + idx;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string result = response.Content.ReadAsStringAsync().Result.ToString();
            CustomerViewModel custVM = JsonConvert.DeserializeObject<CustomerViewModel>(result);

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
            // Delete API Akses http://localhost:2099/api/CustomerApi/1
            string ApiEndPoint = ApiUrl + "api/CustomerApi/" + id;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.DeleteAsync(ApiEndPoint).Result;

            string result = response.Content.ReadAsStringAsync().Result.ToString();
            int success = int.Parse(result);

            if (success == 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}
