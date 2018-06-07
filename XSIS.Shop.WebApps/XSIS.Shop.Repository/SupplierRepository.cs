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
    public class SupplierRepository
    {
        public List<SupplierViewModel> GetAllSupplier()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var ListSupplier = db.Supplier.ToList();
                List<SupplierViewModel> listVM = new List<SupplierViewModel>();

                foreach (var item in ListSupplier)
                {
                    SupplierViewModel ViewModel = new SupplierViewModel();
                    ViewModel.Id = item.Id;
                    ViewModel.CompanyName = item.CompanyName;
                    ViewModel.ContactName = item.ContactName;
                    ViewModel.ContactTitle = item.ContactTitle;
                    ViewModel.City = item.City;
                    ViewModel.Country = item.Country;
                    ViewModel.Phone = item.Phone;
                    ViewModel.Fax = item.Fax;
                    listVM.Add(ViewModel);
                }

                return listVM;
            }
        }

        public SupplierViewModel GetSupplierById(int id)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Supplier supplier = db.Supplier.Find(id);

                SupplierViewModel ViewModel = new SupplierViewModel();
                ViewModel.Id = supplier.Id;
                ViewModel.CompanyName = supplier.CompanyName;
                ViewModel.ContactName = supplier.ContactName;
                ViewModel.ContactTitle = supplier.ContactTitle;
                ViewModel.City = supplier.City;
                ViewModel.Country = supplier.Country;
                ViewModel.Phone = supplier.Phone;
                ViewModel.Fax = supplier.Fax;

                return ViewModel;
            }
        }

        public void AddNewSupplier(SupplierViewModel supplier)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Supplier model = new Supplier();

                model.CompanyName = supplier.CompanyName;
                model.ContactName = supplier.ContactName;
                model.ContactTitle = supplier.ContactTitle;
                model.City = supplier.City;
                model.Country = supplier.Country;
                model.Phone = supplier.Phone;
                model.Fax = supplier.Fax;

                db.Supplier.Add(model);
                db.SaveChanges();
            }
        }

        public void UpdateSupplier(SupplierViewModel supplier)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Supplier model = new Supplier();
                model.Id = supplier.Id;
                model.CompanyName = supplier.CompanyName;
                model.ContactName = supplier.ContactName;
                model.ContactTitle = supplier.ContactTitle;
                model.City = supplier.City;
                model.Country = supplier.Country;
                model.Phone = supplier.Phone;
                model.Fax = supplier.Fax;

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteSupplier(int id)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Supplier supplier = db.Supplier.Find(id);
                db.Supplier.Remove(supplier);
                db.SaveChanges();
            }
        }
    }
}
