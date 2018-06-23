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

using XSIS.Shop.Models;
using System.Globalization;

namespace XSIS.Shop.WebApps.Controllers
{
    public class OrdersController : Controller
    {
        private string ApiUrl = WebConfigurationManager.AppSettings["XSIS.Shop.API"];
        public List<OrderItemViewModel> ListItem = new List<OrderItemViewModel>();

        public ActionResult Index(string OrderNumber, string OrderDate, string CustomerId)
        {
            List<CustomerViewModel> resultCustomer = new List<CustomerViewModel>();
            string ApiEndPoint = ApiUrl + "api/CustomerApi/";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string ListResult = response.Content.ReadAsStringAsync().Result.ToString();
            resultCustomer = JsonConvert.DeserializeObject<List<CustomerViewModel>>(ListResult);

            ViewBag.CustomerId = new SelectList(resultCustomer, "CustomerName", "CustomerName");

            if (!string.IsNullOrEmpty(OrderDate) || !string.IsNullOrWhiteSpace(OrderDate))
            {
                OrderDate = OrderDate.Replace("/", "-");
            }

            List<OrderViewModel> list = null;
            var result = list;

            if (!string.IsNullOrEmpty(CustomerId) || !string.IsNullOrEmpty(OrderDate) || !string.IsNullOrEmpty(OrderNumber) ||
                !string.IsNullOrWhiteSpace(CustomerId) || !string.IsNullOrWhiteSpace(OrderDate) || !string.IsNullOrWhiteSpace(OrderNumber)
                || CustomerId != " " || OrderDate != " " || OrderNumber != " ")

            {
                string ApiEndPoint1 = ApiUrl + "api/OrderApi/SearchByKey/" + (OrderNumber + "|" + OrderDate + "|" + CustomerId);
                HttpClient client1 = new HttpClient();
                HttpResponseMessage response1 = client1.GetAsync(ApiEndPoint1).Result;

                string ListResult1 = response1.Content.ReadAsStringAsync().Result.ToString();
                result = JsonConvert.DeserializeObject<List<OrderViewModel>>(ListResult1);
            }
            else
            {
                string ApiEndPoint2 = ApiUrl + "api/OrderApi/";
                HttpClient client2 = new HttpClient();
                HttpResponseMessage response2 = client2.GetAsync(ApiEndPoint2).Result;

                string ListResult2 = response2.Content.ReadAsStringAsync().Result.ToString();
                result = JsonConvert.DeserializeObject<List<OrderViewModel>>(ListResult2);
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

            string ApiEndPoint = ApiUrl + "api/OrderApi/Get/" + idx;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string result = response.Content.ReadAsStringAsync().Result.ToString();
            OrderViewModel orderVM = JsonConvert.DeserializeObject<OrderViewModel>(result);

            if (orderVM == null)
            {
                return HttpNotFound();
            }

            return View(orderVM);
        }

        public ActionResult Create()
        {
            List<CustomerViewModel> resultCustomer = new List<CustomerViewModel>();
            string ApiEndPoint = ApiUrl + "api/CustomerApi/";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string ListResult = response.Content.ReadAsStringAsync().Result.ToString();
            resultCustomer = JsonConvert.DeserializeObject<List<CustomerViewModel>>(ListResult);

            ViewBag.CustomerId = new SelectList(resultCustomer, "Id", "CustomerName");
            return View();
        }

        public ActionResult AddItem()
        {
            List<ProductViewModel> resultProducts = new List<ProductViewModel>();
            string ApiEndPoint = ApiUrl + "/api/Product/GetAllProductDDL";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string ListResult = response.Content.ReadAsStringAsync().Result.ToString();
            resultProducts = JsonConvert.DeserializeObject<List<ProductViewModel>>(ListResult);

            ViewBag.ProductId = new SelectList(resultProducts, "Id", "ProductName");
            return PartialView("_AddItem");
        }

        public ActionResult AddItemToCurrentOrder(int ProductId, int OrderQuantity)
        {
            if (Session["ListOrderItem"] != null)
            {
                ListItem = (List<OrderItemViewModel>)Session["ListOrderItem"];
            }

            string ApiEndPoint = ApiUrl + "api/Product/GetProductById/" + ProductId;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string ListResult = response.Content.ReadAsStringAsync().Result.ToString();
            var DetailProduct = JsonConvert.DeserializeObject<ProductViewModel>(ListResult);

            ListItem.Add(new OrderItemViewModel
            {
                ProductId = DetailProduct.Id,
                ProductName = DetailProduct.ProductName,
                UnitPrice = DetailProduct.UnitPrice.HasValue ? DetailProduct.UnitPrice.Value : 0,
                Quantity = OrderQuantity,
                TotalAmount = (DetailProduct.UnitPrice.HasValue ? DetailProduct.UnitPrice.Value : 0) * OrderQuantity
            });

            string json = JsonConvert.SerializeObject(ListItem);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            string ApiEndPoint2 = ApiUrl + "api/OrderApi/GroupListItem/";
            HttpClient client2 = new HttpClient();

            HttpResponseMessage response2 = client2.PostAsync(ApiEndPoint2, byteContent).Result;

            string ListResult2 = response2.Content.ReadAsStringAsync().Result.ToString();
            var ListItemCount = JsonConvert.DeserializeObject<List<OrderItemViewModel>>(ListResult2);

            Session["ListOrderItem"] = ListItemCount;

            ViewBag.GrandTotal = ListItemCount.Sum(s => s.TotalAmount);
            return PartialView("_ListOrderItem", ListItemCount);
        }

        public ActionResult RemoveItemFromCurrentOrder(int ProductId)
        {
            if (Session["ListOrderItem"] != null)
            {
                ListItem = (List<OrderItemViewModel>)Session["ListOrderItem"];
            }

            OrderRemoveViewModel model = new OrderRemoveViewModel();
            model.ProductId = ProductId;
            model.ListOrderItem = ListItem;

            string json = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            string ApiEndPoint2 = ApiUrl + "api/OrderApi/RemoveItem/";
            HttpClient client2 = new HttpClient();

            HttpResponseMessage response2 = client2.PostAsync(ApiEndPoint2, byteContent).Result;

            string ListResult2 = response2.Content.ReadAsStringAsync().Result.ToString();
            var RemoveItemvarian = JsonConvert.DeserializeObject<List<OrderItemViewModel>>(ListResult2);

            Session["ListOrderItem"] = RemoveItemvarian;
            ViewBag.GrandTotal = RemoveItemvarian.Sum(s => s.TotalAmount);
            return PartialView("_ListOrderItem", RemoveItemvarian);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderViewModel order)
        {
            List<CustomerViewModel> resultCustomer = new List<CustomerViewModel>();
            string ApiEndPoint = ApiUrl + "api/CustomerApi/";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string ListResult = response.Content.ReadAsStringAsync().Result.ToString();
            resultCustomer = JsonConvert.DeserializeObject<List<CustomerViewModel>>(ListResult);

            if (ModelState.IsValid)
            {
                if (Session["ListOrderItem"] == null)
                {
                    ModelState.AddModelError("", "List order item tidak boleh kosong.");
                    ViewBag.CustomerId = new SelectList(resultCustomer, "Id", "CustomerName");
                    return View(order);
                }
                else
                {
                    string OrderNum = string.Empty;

                    string ApiEndPoint1 = ApiUrl + "api/OrderApi/GetLatestOrderID/";
                    HttpClient client1 = new HttpClient();
                    HttpResponseMessage response1 = client1.GetAsync(ApiEndPoint1).Result;
                    int latestID = Convert.ToInt32(response1.Content.ReadAsStringAsync().Result.ToString());

                    if(latestID < 10)
                    {
                        OrderNum = "ORD" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + "00" + latestID;
                    }
                    else
                    {
                        OrderNum = "ORD" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + "0" + latestID;
                    }

                    ListItem = (List<OrderItemViewModel>)Session["ListOrderItem"];

                    string[] formats = { "dd/MM/yyyy" };
                    DateTime oDate = DateTime.ParseExact(order.OrderDate, formats, new CultureInfo("en-US"), DateTimeStyles.None);

                    order.OrderNumber = OrderNum;
                    order.ListOrderItem = ListItem;

                    string json = JsonConvert.SerializeObject(order);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    string ApiEndPoint2 = ApiUrl + "api/OrderApi/Post";
                    HttpClient client2 = new HttpClient();

                    HttpResponseMessage response2 = client2.PostAsync(ApiEndPoint2, byteContent).Result;

                    string result = response2.Content.ReadAsStringAsync().Result.ToString();
                    int success = int.Parse(result);

                    if (success == 1)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.CustomerId = new SelectList(resultCustomer, "Id", "CustomerName");
                        return View(order);
                    }
                }
            }

            ViewBag.CustomerId = new SelectList(resultCustomer, "Id", "CustomerName");
            return View(order);
        }
    }
}
