using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XSIS.Shop.Models;
using XSIS.Shop.ViewModels;

namespace XSIS.Shop.Repository
{
    public class HomeRepository
    {
        public int DashboardGetTotalCustomers()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var total = db.Customer.Count();
                return total;
            }
        }

        public int DashboardGetTotalSuppliers()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var total = db.Supplier.Count();
                return total;
            }
        }

        public int DashboardGetTotalProducts()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var total = db.Product.Count();
                return total;
            }
        }

        public int DashboardGetTotalOrders()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var total = db.Order.Count();
                return total;
            }
        }

        public List<ProductViewModel> DashboardGetAllProduct()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var product = db.Product.Include(p => p.Supplier);
                List<ProductViewModel> ListVM = new List<ProductViewModel>();

                foreach (var item in product)
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

                return ListVM;
            }
        }

        public List<OrderViewModel> DashboardGetAllOrders()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                List<OrderViewModel> ListOrders = db.Order.Include(p => p.Customer).Select(p => new OrderViewModel
                {
                    Id = p.Id,
                    OrderDate = p.OrderDate.ToString(),
                    OrderNumber = p.OrderNumber,
                    CustomerId = p.CustomerId,
                    CustomerName = (p.Customer.FirstName + " " + p.Customer.LastName),
                    TotalAmount = p.TotalAmount
                }).ToList();

                return ListOrders;
            }
        }
    }
}
