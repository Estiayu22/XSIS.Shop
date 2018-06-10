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
    public class HomeController : Controller
    {
        private HomeRepository service = new HomeRepository();

        public ActionResult Index()
        {
            int TotalCustomers = service.DashboardGetTotalCustomers();
            int TotalSuppliers = service.DashboardGetTotalSuppliers();
            int TotalProducts = service.DashboardGetTotalProducts();
            int TotalOrders = service.DashboardGetTotalOrders();

            List<ProductViewModel> ListProduct = new List<ProductViewModel>();
            ListProduct = service.DashboardGetAllProduct();

            List<OrderViewModel> ListOrder = new List<OrderViewModel>();
            ListOrder = service.DashboardGetAllOrders();

            ViewBag.TotalCustomers = TotalCustomers;
            ViewBag.TotalSuppliers = TotalSuppliers;
            ViewBag.TotalProducts = TotalProducts;
            ViewBag.TotalOrders = TotalOrders;
            ViewBag.ListProducts = ListProduct;
            ViewBag.ListOrders = ListOrder;

            return View();
        }
    }
}