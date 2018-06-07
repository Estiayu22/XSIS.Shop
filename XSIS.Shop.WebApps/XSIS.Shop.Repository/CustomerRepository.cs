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
    public class CustomerRepository
    {
        // Select * from Customer
        public List<CustomerViewModel> GetAllCustomer()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var ListCustomers = db.Customer.ToList();
                List<CustomerViewModel> ListVM = new List<CustomerViewModel>();

                foreach (var item in ListCustomers)
                {
                    CustomerViewModel custVM = new CustomerViewModel();
                    custVM.Id = item.Id;
                    custVM.FirstName = item.FirstName;
                    custVM.LastName = item.LastName;
                    custVM.City = item.City;
                    custVM.Country = item.Country;
                    custVM.Phone = item.Phone;
                    custVM.Email = item.Email;
                    ListVM.Add(custVM);
                }

                return ListVM;
            }
        }

        // Select * from Customer Where Id = id
        public CustomerViewModel GetCustomerById(int id)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Customer customer = db.Customer.Find(id);

                CustomerViewModel custVM = new CustomerViewModel();
                custVM.Id = customer.Id;
                custVM.FirstName = customer.FirstName;
                custVM.LastName = customer.LastName;
                custVM.City = customer.City;
                custVM.Country = customer.Country;
                custVM.Phone = customer.Phone;
                custVM.Email = customer.Email;

                return custVM;
            }
        }

        public void AddNewCustomer(CustomerViewModel customer)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Customer model = new Customer();

                model.FirstName = customer.FirstName;
                model.LastName = customer.LastName;
                model.City = customer.City;
                model.Country = customer.Country;
                model.Phone = customer.Phone;
                model.Email = customer.Email;

                db.Customer.Add(model);
                db.SaveChanges();
            }
        }

        public void UpdateCustomer(CustomerViewModel customer)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Customer model = new Customer();

                model.Id = customer.Id;
                model.FirstName = customer.FirstName;
                model.LastName = customer.LastName;
                model.City = customer.City;
                model.Country = customer.Country;
                model.Phone = customer.Phone;
                model.Email = customer.Email;

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteCustomer(int id)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Customer customer = db.Customer.Find(id);
                db.Customer.Remove(customer);
                db.SaveChanges();
            }
        }

        // Filtering Search based on Full Name, City/Country, Email
        public List<CustomerViewModel> SearchByKey(string FullName, string CityCountry, string Email)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                List<CustomerViewModel> listVM = new List<CustomerViewModel>();

                if (FullName == " " && CityCountry == " " && Email == " ")
                {
                    listVM = (from list in db.Customer
                              select new CustomerViewModel
                              {
                                  Id = list.Id,
                                  FirstName = list.FirstName,
                                  LastName = list.LastName,
                                  City = list.City,
                                  Country = list.Country,
                                  Phone = list.Phone,
                                  Email = list.Email
                              }).ToList();
                }
                else if (FullName == null && CityCountry == null && Email == null)
                {
                    listVM = (from list in db.Customer
                              select new CustomerViewModel
                              {
                                  Id = list.Id,
                                  FirstName = list.FirstName,
                                  LastName = list.LastName,
                                  City = list.City,
                                  Country = list.Country,
                                  Phone = list.Phone,
                                  Email = list.Email
                              }).ToList();
                }
                else if (string.IsNullOrWhiteSpace(FullName) && string.IsNullOrWhiteSpace(CityCountry) && string.IsNullOrWhiteSpace(Email))
                {
                    listVM = (from list in db.Customer
                              select new CustomerViewModel
                              {
                                  Id = list.Id,
                                  FirstName = list.FirstName,
                                  LastName = list.LastName,
                                  City = list.City,
                                  Country = list.Country,
                                  Phone = list.Phone,
                                  Email = list.Email
                              }).ToList();
                }
                else if (!string.IsNullOrEmpty(FullName))
                {
                    if (!string.IsNullOrEmpty(CityCountry) && !string.IsNullOrEmpty(Email))
                    {
                        listVM = (from list in db.Customer
                                where (list.FirstName.ToLower().Contains(FullName.ToLower()) || list.LastName.ToLower().Contains(FullName.ToLower())) ||
                                (list.City.ToLower().Contains(CityCountry.ToLower()) || list.Country.ToLower().Contains(FullName.ToLower())) ||
                                (list.Email.ToLower().Contains(Email.ToLower()))
                                select new CustomerViewModel
                                {
                                    Id = list.Id,
                                    FirstName = list.FirstName,
                                    LastName = list.LastName,
                                    City = list.City,
                                    Country = list.Country,
                                    Phone = list.Phone,
                                    Email = list.Email
                                }).ToList();
                    }
                    else if(!string.IsNullOrEmpty(CityCountry) && string.IsNullOrEmpty(Email))
                    {
                        listVM = (from list in db.Customer
                                  where (list.FirstName.ToLower().Contains(FullName.ToLower()) || list.LastName.ToLower().Contains(FullName.ToLower())) ||
                                  (list.City.ToLower().Contains(CityCountry.ToLower()) || list.Country.ToLower().Contains(FullName.ToLower()))
                                  select new CustomerViewModel
                                  {
                                      Id = list.Id,
                                      FirstName = list.FirstName,
                                      LastName = list.LastName,
                                      City = list.City,
                                      Country = list.Country,
                                      Phone = list.Phone,
                                      Email = list.Email
                                  }).ToList();
                    }
                    else if (string.IsNullOrEmpty(CityCountry) && !string.IsNullOrEmpty(Email))
                    {
                        listVM = (from list in db.Customer
                                  where (list.FirstName.ToLower().Contains(FullName.ToLower()) || list.LastName.ToLower().Contains(FullName.ToLower())) ||
                                  (list.Email.ToLower().Contains(Email.ToLower()))
                                  select new CustomerViewModel
                                  {
                                      Id = list.Id,
                                      FirstName = list.FirstName,
                                      LastName = list.LastName,
                                      City = list.City,
                                      Country = list.Country,
                                      Phone = list.Phone,
                                      Email = list.Email
                                  }).ToList();
                    }
                    else
                    {
                        listVM = (from list in db.Customer
                                  where (list.FirstName.ToLower().Contains(FullName.ToLower()) || list.LastName.ToLower().Contains(FullName.ToLower()))
                                  select new CustomerViewModel
                                  {
                                      Id = list.Id,
                                      FirstName = list.FirstName,
                                      LastName = list.LastName,
                                      City = list.City,
                                      Country = list.Country,
                                      Phone = list.Phone,
                                      Email = list.Email
                                  }).ToList();
                    }
                }
                else if(!string.IsNullOrEmpty(CityCountry))
                {
                    if (!string.IsNullOrEmpty(FullName) && !string.IsNullOrEmpty(Email))
                    {
                        listVM = (from list in db.Customer
                                  where (list.FirstName.ToLower().Contains(FullName.ToLower()) || list.LastName.ToLower().Contains(FullName.ToLower())) ||
                                  (list.City.ToLower().Contains(CityCountry.ToLower()) || list.Country.ToLower().Contains(FullName.ToLower())) ||
                                  (list.Email.ToLower().Contains(Email.ToLower()))
                                  select new CustomerViewModel
                                  {
                                      Id = list.Id,
                                      FirstName = list.FirstName,
                                      LastName = list.LastName,
                                      City = list.City,
                                      Country = list.Country,
                                      Phone = list.Phone,
                                      Email = list.Email
                                  }).ToList();
                    }
                    else if (!string.IsNullOrEmpty(FullName) && string.IsNullOrEmpty(Email))
                    {
                        listVM = (from list in db.Customer
                                  where (list.FirstName.ToLower().Contains(FullName.ToLower()) || list.LastName.ToLower().Contains(FullName.ToLower())) ||
                                  (list.City.ToLower().Contains(CityCountry.ToLower()) || list.Country.ToLower().Contains(FullName.ToLower()))
                                  select new CustomerViewModel
                                  {
                                      Id = list.Id,
                                      FirstName = list.FirstName,
                                      LastName = list.LastName,
                                      City = list.City,
                                      Country = list.Country,
                                      Phone = list.Phone,
                                      Email = list.Email
                                  }).ToList();
                    }
                    else if (string.IsNullOrEmpty(FullName) && !string.IsNullOrEmpty(Email))
                    {
                        listVM = (from list in db.Customer
                                  where (list.City.ToLower().Contains(CityCountry.ToLower()) || list.Country.ToLower().Contains(FullName.ToLower())) ||
                                  (list.Email.ToLower().Contains(Email.ToLower()))
                                  select new CustomerViewModel
                                  {
                                      Id = list.Id,
                                      FirstName = list.FirstName,
                                      LastName = list.LastName,
                                      City = list.City,
                                      Country = list.Country,
                                      Phone = list.Phone,
                                      Email = list.Email
                                  }).ToList();
                    }
                    else
                    {
                        listVM = (from list in db.Customer
                                  where (list.City.ToLower().Contains(CityCountry.ToLower()) || list.Country.ToLower().Contains(FullName.ToLower()))
                                  select new CustomerViewModel
                                  {
                                      Id = list.Id,
                                      FirstName = list.FirstName,
                                      LastName = list.LastName,
                                      City = list.City,
                                      Country = list.Country,
                                      Phone = list.Phone,
                                      Email = list.Email
                                  }).ToList();
                    }
                }
                else  if(!string.IsNullOrEmpty(Email))
                {
                    if (!string.IsNullOrEmpty(CityCountry) && !string.IsNullOrEmpty(FullName))
                    {
                        listVM = (from list in db.Customer
                                  where (list.FirstName.ToLower().Contains(FullName.ToLower()) || list.LastName.ToLower().Contains(FullName.ToLower())) ||
                                  (list.City.ToLower().Contains(CityCountry.ToLower()) || list.Country.ToLower().Contains(FullName.ToLower())) ||
                                  (list.Email.ToLower().Contains(Email.ToLower()))
                                  select new CustomerViewModel
                                  {
                                      Id = list.Id,
                                      FirstName = list.FirstName,
                                      LastName = list.LastName,
                                      City = list.City,
                                      Country = list.Country,
                                      Phone = list.Phone,
                                      Email = list.Email
                                  }).ToList();
                    }
                    else if (!string.IsNullOrEmpty(CityCountry) && string.IsNullOrEmpty(FullName))
                    {
                        listVM = (from list in db.Customer
                                  where (list.City.ToLower().Contains(CityCountry.ToLower()) || list.Country.ToLower().Contains(FullName.ToLower())) ||
                                  (list.Email.ToLower().Contains(Email.ToLower()))
                                  select new CustomerViewModel
                                  {
                                      Id = list.Id,
                                      FirstName = list.FirstName,
                                      LastName = list.LastName,
                                      City = list.City,
                                      Country = list.Country,
                                      Phone = list.Phone,
                                      Email = list.Email
                                  }).ToList();
                    }
                    else if (string.IsNullOrEmpty(CityCountry) && !string.IsNullOrEmpty(FullName))
                    {
                        listVM = (from list in db.Customer
                                  where (list.FirstName.ToLower().Contains(FullName.ToLower()) || list.LastName.ToLower().Contains(FullName.ToLower())) ||
                                  (list.Email.ToLower().Contains(Email.ToLower()))
                                  select new CustomerViewModel
                                  {
                                      Id = list.Id,
                                      FirstName = list.FirstName,
                                      LastName = list.LastName,
                                      City = list.City,
                                      Country = list.Country,
                                      Phone = list.Phone,
                                      Email = list.Email
                                  }).ToList();
                    }
                    else
                    {
                        listVM = (from list in db.Customer
                                  where (list.Email.ToLower().Contains(Email.ToLower()))
                                  select new CustomerViewModel
                                  {
                                      Id = list.Id,
                                      FirstName = list.FirstName,
                                      LastName = list.LastName,
                                      City = list.City,
                                      Country = list.Country,
                                      Phone = list.Phone,
                                      Email = list.Email
                                  }).ToList();
                    }
                }
                else
                {
                    listVM = (from list in db.Customer
                              select new CustomerViewModel
                              {
                                  Id = list.Id,
                                  FirstName = list.FirstName,
                                  LastName = list.LastName,
                                  City = list.City,
                                  Country = list.Country,
                                  Phone = list.Phone,
                                  Email = list.Email
                              }).ToList();
                }

                return listVM;
            }
        }
    }
}
