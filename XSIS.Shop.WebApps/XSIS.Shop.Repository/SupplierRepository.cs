﻿using System;
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

        public SupplierViewModel GetDetailSupplierById(int id)
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

                List<ProductViewModel> ListProduct = new List<ProductViewModel>();
                ListProduct = (from d in db.Product
                               where d.SupplierId == ViewModel.Id
                               select new ProductViewModel
                               {
                                   Id = d.Id,
                                   ProductName = d.ProductName,
                                   SupplierId = d.SupplierId,
                                   UnitPrice = d.UnitPrice,
                                   Package = d.Package,
                                   IsDiscontinued = d.IsDiscontinued
                               }).ToList();

                if(ListProduct == null)
                {
                    ViewModel.ListProductViewModel = null;
                }
                else
                {
                    ViewModel.ListProductViewModel = ListProduct;
                }

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

        // Filtering Search based on CompanyName/EmployeeName, City/Country, PhoneNo
        public List<SupplierViewModel> SearchByKey(string FullName, string CityCountry, string PhoneNo)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                List<SupplierViewModel> listVM = new List<SupplierViewModel>();

                if (FullName == " " && CityCountry == " " && PhoneNo == " ")
                {
                    listVM = (from item in db.Supplier
                              select new SupplierViewModel
                              {
                                Id = item.Id,
                                CompanyName = item.CompanyName,
                                ContactName = item.ContactName,
                                ContactTitle = item.ContactTitle,
                                City = item.City,
                                Country = item.Country,
                                Phone = item.Phone,
                                Fax = item.Fax
                              }).ToList();
                }
                else if (FullName == null && CityCountry == null && PhoneNo == null)
                {
                    listVM = (from item in db.Supplier
                              select new SupplierViewModel
                              {
                                  Id = item.Id,
                                  CompanyName = item.CompanyName,
                                  ContactName = item.ContactName,
                                  ContactTitle = item.ContactTitle,
                                  City = item.City,
                                  Country = item.Country,
                                  Phone = item.Phone,
                                  Fax = item.Fax
                              }).ToList();
                }
                else if (string.IsNullOrWhiteSpace(FullName) && string.IsNullOrWhiteSpace(CityCountry) && string.IsNullOrWhiteSpace(PhoneNo))
                {
                    listVM = (from item in db.Supplier
                              select new SupplierViewModel
                              {
                                  Id = item.Id,
                                  CompanyName = item.CompanyName,
                                  ContactName = item.ContactName,
                                  ContactTitle = item.ContactTitle,
                                  City = item.City,
                                  Country = item.Country,
                                  Phone = item.Phone,
                                  Fax = item.Fax
                              }).ToList();
                }
                else if (!string.IsNullOrEmpty(FullName))
                {
                    if (!string.IsNullOrEmpty(CityCountry) && !string.IsNullOrEmpty(PhoneNo))
                    {
                        listVM = (from item in db.Supplier
                                  where (item.CompanyName.ToLower().Contains(FullName.ToLower()) || item.ContactName.ToLower().Contains(FullName.ToLower())) ||
                                  (item.City.ToLower().Contains(CityCountry.ToLower()) || item.Country.ToLower().Contains(CityCountry.ToLower())) ||
                                  (item.Phone.ToLower().Contains(PhoneNo.ToLower()))
                                  select new SupplierViewModel
                                  {
                                      Id = item.Id,
                                      CompanyName = item.CompanyName,
                                      ContactName = item.ContactName,
                                      ContactTitle = item.ContactTitle,
                                      City = item.City,
                                      Country = item.Country,
                                      Phone = item.Phone,
                                      Fax = item.Fax
                                  }).ToList();
                    }
                    else if (!string.IsNullOrEmpty(CityCountry) && string.IsNullOrEmpty(PhoneNo))
                    {
                        listVM = (from item in db.Supplier
                                  where (item.CompanyName.ToLower().Contains(FullName.ToLower()) || item.ContactName.ToLower().Contains(FullName.ToLower())) ||
                                  (item.City.ToLower().Contains(CityCountry.ToLower()) || item.Country.ToLower().Contains(CityCountry.ToLower()))
                                  select new SupplierViewModel
                                  {
                                      Id = item.Id,
                                      CompanyName = item.CompanyName,
                                      ContactName = item.ContactName,
                                      ContactTitle = item.ContactTitle,
                                      City = item.City,
                                      Country = item.Country,
                                      Phone = item.Phone,
                                      Fax = item.Fax
                                  }).ToList();
                    }
                    else if (string.IsNullOrEmpty(CityCountry) && !string.IsNullOrEmpty(PhoneNo))
                    {
                        listVM = (from item in db.Supplier
                                  where (item.CompanyName.ToLower().Contains(FullName.ToLower()) || item.ContactName.ToLower().Contains(FullName.ToLower())) ||
                                  (item.Phone.ToLower().Contains(PhoneNo.ToLower()))
                                  select new SupplierViewModel
                                  {
                                      Id = item.Id,
                                      CompanyName = item.CompanyName,
                                      ContactName = item.ContactName,
                                      ContactTitle = item.ContactTitle,
                                      City = item.City,
                                      Country = item.Country,
                                      Phone = item.Phone,
                                      Fax = item.Fax
                                  }).ToList();
                    }
                    else
                    {
                        listVM = (from item in db.Supplier
                                  where (item.CompanyName.ToLower().Contains(FullName.ToLower()) || item.ContactName.ToLower().Contains(FullName.ToLower()))
                                  select new SupplierViewModel
                                  {
                                      Id = item.Id,
                                      CompanyName = item.CompanyName,
                                      ContactName = item.ContactName,
                                      ContactTitle = item.ContactTitle,
                                      City = item.City,
                                      Country = item.Country,
                                      Phone = item.Phone,
                                      Fax = item.Fax
                                  }).ToList();
                    }
                }
                else if (!string.IsNullOrEmpty(CityCountry))
                {
                    if (!string.IsNullOrEmpty(FullName) && !string.IsNullOrEmpty(PhoneNo))
                    {
                        listVM = (from item in db.Supplier
                                  where (item.CompanyName.ToLower().Contains(FullName.ToLower()) || item.ContactName.ToLower().Contains(FullName.ToLower())) ||
                                  (item.City.ToLower().Contains(CityCountry.ToLower()) || item.Country.ToLower().Contains(CityCountry.ToLower())) ||
                                  (item.Phone.ToLower().Contains(PhoneNo.ToLower()))
                                  select new SupplierViewModel
                                  {
                                      Id = item.Id,
                                      CompanyName = item.CompanyName,
                                      ContactName = item.ContactName,
                                      ContactTitle = item.ContactTitle,
                                      City = item.City,
                                      Country = item.Country,
                                      Phone = item.Phone,
                                      Fax = item.Fax
                                  }).ToList();
                    }
                    else if (!string.IsNullOrEmpty(FullName) && string.IsNullOrEmpty(PhoneNo))
                    {
                        listVM = (from item in db.Supplier
                                  where (item.CompanyName.ToLower().Contains(FullName.ToLower()) || item.ContactName.ToLower().Contains(FullName.ToLower())) ||
                                  (item.City.ToLower().Contains(CityCountry.ToLower()) || item.Country.ToLower().Contains(CityCountry.ToLower()))
                                  select new SupplierViewModel
                                  {
                                      Id = item.Id,
                                      CompanyName = item.CompanyName,
                                      ContactName = item.ContactName,
                                      ContactTitle = item.ContactTitle,
                                      City = item.City,
                                      Country = item.Country,
                                      Phone = item.Phone,
                                      Fax = item.Fax
                                  }).ToList();
                    }
                    else if (string.IsNullOrEmpty(FullName) && !string.IsNullOrEmpty(PhoneNo))
                    {
                        listVM = (from item in db.Supplier
                                  where (item.City.ToLower().Contains(CityCountry.ToLower()) || item.Country.ToLower().Contains(CityCountry.ToLower())) ||
                                  (item.Phone.ToLower().Contains(PhoneNo.ToLower()))
                                  select new SupplierViewModel
                                  {
                                      Id = item.Id,
                                      CompanyName = item.CompanyName,
                                      ContactName = item.ContactName,
                                      ContactTitle = item.ContactTitle,
                                      City = item.City,
                                      Country = item.Country,
                                      Phone = item.Phone,
                                      Fax = item.Fax
                                  }).ToList();
                    }
                    else
                    {
                        listVM = (from item in db.Supplier
                                  where (item.City.ToLower().Contains(CityCountry.ToLower()) || item.Country.ToLower().Contains(CityCountry.ToLower()))
                                  select new SupplierViewModel
                                  {
                                      Id = item.Id,
                                      CompanyName = item.CompanyName,
                                      ContactName = item.ContactName,
                                      ContactTitle = item.ContactTitle,
                                      City = item.City,
                                      Country = item.Country,
                                      Phone = item.Phone,
                                      Fax = item.Fax
                                  }).ToList();
                    }
                }
                else if (!string.IsNullOrEmpty(PhoneNo))
                {
                    if (!string.IsNullOrEmpty(CityCountry) && !string.IsNullOrEmpty(FullName))
                    {
                        listVM = (from item in db.Supplier
                                  where (item.CompanyName.ToLower().Contains(FullName.ToLower()) || item.ContactName.ToLower().Contains(FullName.ToLower())) ||
                                  (item.City.ToLower().Contains(CityCountry.ToLower()) || item.Country.ToLower().Contains(CityCountry.ToLower())) ||
                                  (item.Phone.ToLower().Contains(PhoneNo.ToLower()))
                                  select new SupplierViewModel
                                  {
                                      Id = item.Id,
                                      CompanyName = item.CompanyName,
                                      ContactName = item.ContactName,
                                      ContactTitle = item.ContactTitle,
                                      City = item.City,
                                      Country = item.Country,
                                      Phone = item.Phone,
                                      Fax = item.Fax
                                  }).ToList();
                    }
                    else if (!string.IsNullOrEmpty(CityCountry) && string.IsNullOrEmpty(FullName))
                    {
                        listVM = (from item in db.Supplier
                                  where (item.City.ToLower().Contains(CityCountry.ToLower()) || item.Country.ToLower().Contains(CityCountry.ToLower())) ||
                                  (item.Phone.ToLower().Contains(PhoneNo.ToLower()))
                                  select new SupplierViewModel
                                  {
                                      Id = item.Id,
                                      CompanyName = item.CompanyName,
                                      ContactName = item.ContactName,
                                      ContactTitle = item.ContactTitle,
                                      City = item.City,
                                      Country = item.Country,
                                      Phone = item.Phone,
                                      Fax = item.Fax
                                  }).ToList();
                    }
                    else if (string.IsNullOrEmpty(CityCountry) && !string.IsNullOrEmpty(FullName))
                    {
                        listVM = (from item in db.Supplier
                                  where (item.CompanyName.ToLower().Contains(FullName.ToLower()) || item.ContactName.ToLower().Contains(FullName.ToLower())) ||
                                  (item.Phone.ToLower().Contains(PhoneNo.ToLower()))
                                  select new SupplierViewModel
                                  {
                                      Id = item.Id,
                                      CompanyName = item.CompanyName,
                                      ContactName = item.ContactName,
                                      ContactTitle = item.ContactTitle,
                                      City = item.City,
                                      Country = item.Country,
                                      Phone = item.Phone,
                                      Fax = item.Fax
                                  }).ToList();
                    }
                    else
                    {
                        listVM = (from item in db.Supplier
                                  where (item.Phone.ToLower().Contains(PhoneNo.ToLower()))
                                  select new SupplierViewModel
                                  {
                                      Id = item.Id,
                                      CompanyName = item.CompanyName,
                                      ContactName = item.ContactName,
                                      ContactTitle = item.ContactTitle,
                                      City = item.City,
                                      Country = item.Country,
                                      Phone = item.Phone,
                                      Fax = item.Fax
                                  }).ToList();
                    }
                }
                else
                {
                    listVM = (from item in db.Supplier
                              select new SupplierViewModel
                              {
                                  Id = item.Id,
                                  CompanyName = item.CompanyName,
                                  ContactName = item.ContactName,
                                  ContactTitle = item.ContactTitle,
                                  City = item.City,
                                  Country = item.Country,
                                  Phone = item.Phone,
                                  Fax = item.Fax
                              }).ToList();
                }

                return listVM;
            }
        }

    }
}
