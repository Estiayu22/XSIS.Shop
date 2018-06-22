﻿using System;
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
        private ShopDBEntities db = new ShopDBEntities();
        private ProductRepository service = new ProductRepository();
        private string ApiUrl = WebConfigurationManager.AppSettings["XSIS.Shop.API"];

        public List<OrderItemViewModel> ListItem = new List<OrderItemViewModel>();

        public ActionResult Index(string OrderNumber, string OrderDate, string CustomerId)
        {
            // Get All Customer API Akses http://localhost:2099/api/CustomerApi/ without parameter
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
                // Get All Customer API Akses http://localhost:2099/api/OrderApi/OrderNumber/OrderDate/CustomerId with parameter
                string ApiEndPoint1 = ApiUrl + "api/OrderApi/SearchByKey/" + (OrderNumber + "|" + OrderDate + "|" + CustomerId);
                HttpClient client1 = new HttpClient();
                HttpResponseMessage response1 = client1.GetAsync(ApiEndPoint1).Result;

                string ListResult1 = response1.Content.ReadAsStringAsync().Result.ToString();
                result = JsonConvert.DeserializeObject<List<OrderViewModel>>(ListResult1);
            }
            else
            {
                // Get All Customer API Akses http://localhost:2099/api/CustomerApi/ without parameter
                string ApiEndPoint2 = ApiUrl + "api/OrderApi/";
                HttpClient client2 = new HttpClient();
                HttpResponseMessage response2 = client.GetAsync(ApiEndPoint2).Result;

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

            // Details API Akses http://localhost:2099/api/OrderApi/1
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
            // Get All Customer API Akses http://localhost:2099/api/CustomerApi/ without parameter
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
            // Get All Product API Akses http://localhost:2099//api/Product/GetAllProductDDL without parameter
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

            var DetailProduct = service.GetProductById(ProductId);

            ListItem.Add(new OrderItemViewModel
            {
                ProductId = DetailProduct.Id,
                ProductName = DetailProduct.ProductName,
                UnitPrice = DetailProduct.UnitPrice.HasValue ? DetailProduct.UnitPrice.Value : 0,
                Quantity = OrderQuantity,
                TotalAmount = (DetailProduct.UnitPrice.HasValue ? DetailProduct.UnitPrice.Value : 0) * OrderQuantity
            });

            var ListItemCount = GroupListItem(ListItem);
            Session["ListOrderItem"] = ListItemCount;

            ViewBag.GrandTotal = ListItemCount.Sum(s => s.TotalAmount);
            return PartialView("_ListOrderItem", ListItemCount);
        }

        public List<OrderItemViewModel> GroupListItem(List<OrderItemViewModel> ListItem)
        {
            var CountVarian = (ListItem.GroupBy(x => x.ProductId).Select(a => new OrderItemViewModel
            {
                ProductId = a.Key,
                ProductName = a.First().ProductName,
                UnitPrice = a.First().UnitPrice,
                Quantity = a.Sum(s => s.Quantity),
                TotalAmount = a.Sum(s => s.TotalAmount)
            })).ToList();
            return CountVarian;
        }

        public ActionResult RemoveItemFromCurrentOrder(int ProductId)
        {
            if (Session["ListOrderItem"] != null)
            {
                ListItem = (List<OrderItemViewModel>)Session["ListOrderItem"];
            }

            var RemoveItemvarian = RemoveItem(ProductId, ListItem);
            Session["ListOrderItem"] = RemoveItemvarian;
            ViewBag.GrandTotal = RemoveItemvarian.Sum(s => s.TotalAmount);
            return PartialView("_ListOrderItem", RemoveItemvarian);
        }

        public List<OrderItemViewModel> RemoveItem(int ProductId, List<OrderItemViewModel> ListItem)
        {
            for (int i = 0; i < ListItem.Count; i++)
            {
                if (ListItem[i].ProductId == ProductId)
                {
                    ListItem.Remove(ListItem[i]);
                    break;
                }
            }
            return ListItem;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderViewModel order)
        {
            if (ModelState.IsValid)
            {
                if (Session["ListOrderItem"] == null)
                {
                    ModelState.AddModelError("", "List order item tidak boleh kosong.");
                    return View(order);
                }
                else
                {
                    // Generate Order Number
                    string OrderNum = string.Empty;
                    int latestID = db.Order.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();

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

                    Order model = new Order();
                    model.CustomerId = order.CustomerId;
                    model.OrderDate = oDate;
                    model.OrderNumber = OrderNum;
                    model.TotalAmount = order.TotalAmount;

                    db.Order.Add(model);
                    db.SaveChanges();

                    foreach(var item in ListItem)
                    {
                        OrderItem modelItem = new OrderItem();
                        modelItem.OrderId = model.Id;
                        modelItem.ProductId = item.ProductId;
                        modelItem.Quantity = item.Quantity;
                        modelItem.UnitPrice = item.UnitPrice;

                        db.OrderItem.Add(modelItem);
                        db.SaveChanges();

                    }
                    return RedirectToAction("Index");
                }

            }

            ViewBag.CustomerId = new SelectList(db.Customer, "Id", "FirstName", order.CustomerId);
            return View(order);
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
