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
    public class ProductRepository
    {
        public List<ProductViewModel> GetAllProduct()
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

        public ProductViewModel GetProductById(int id)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Product product = db.Product.Find(id);
                ProductViewModel ViewModel = new ProductViewModel();
                ViewModel.Id = product.Id;
                ViewModel.ProductName = product.ProductName;
                ViewModel.SupplierId = product.SupplierId;
                ViewModel.UnitPrice = product.UnitPrice;
                ViewModel.Package = product.Package;
                ViewModel.IsDiscontinued = product.IsDiscontinued;
                ViewModel.SupplierName = product.Supplier.CompanyName;
                return ViewModel;
            }
        }

        public List<Supplier> GetSupplierDDL()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var result = db.Supplier.ToList();
                return result;
            }
        }

        public void AddNewProduct(ProductViewModel product)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Product model = new Product();

                model.ProductName = product.ProductName;
                model.SupplierId = product.SupplierId;
                model.UnitPrice = product.UnitPrice;
                model.Package = product.Package;
                model.IsDiscontinued = product.IsDiscontinued;

                db.Product.Add(model);
                db.SaveChanges();
            }
        }

        public void UpdateProduct(ProductViewModel product)
        {
            using (ShopDBEntities db = new ShopDBEntities())
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
            }
        }

        public void DeleteProduct(int id)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Product product = db.Product.Find(id);
                db.Product.Remove(product);
                db.SaveChanges();
            }
        }
    }
}
