using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XSIS.Shop.Models;
using XSIS.Shop.ViewModels;

namespace XSIS.Shop.Repository
{
    public class OrderRepository
    {
        // Select * from Customer
        public List<OrderViewModel> GetAllOrder()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var order = db.Order.Include(o => o.Customer);

                List<OrderViewModel> orderVM = new List<OrderViewModel>();

                foreach (var item in order)
                {
                    OrderViewModel model = new OrderViewModel();
                    model.Id = item.Id;
                    model.OrderDate = item.OrderDate.ToShortDateString();
                    model.OrderDateFormat = item.OrderDate;
                    model.OrderNumber = item.OrderNumber;
                    model.CustomerName = item.Customer.FirstName + " " + item.Customer.LastName;
                    model.CustomerId = item.CustomerId;
                    model.TotalAmount = item.TotalAmount;

                    orderVM.Add(model);
                }

                return orderVM;
            }
        }

        // Filtering Search based on Full Name, City/Country, Email
        public List<OrderViewModel> SearchByKey(string OrderNumber, string OrderDate, string CustomerId)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                string firstName = string.Empty;
                string lastName = string.Empty;

                if(!string.IsNullOrEmpty(CustomerId) || !string.IsNullOrWhiteSpace(CustomerId))
                {
                    string[] Parameters = CustomerId.Split(' ');

                    firstName = Parameters[0];
                    lastName = Parameters[1];
                }
                else
                {
                    firstName = "";
                    lastName = "";
                }



                List<OrderViewModel> listVM = new List<OrderViewModel>();

                if (OrderNumber == "" && OrderDate == "" && CustomerId == "")
                {
                    listVM = (from listOrd in db.Order
                              join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                              select new OrderViewModel
                              {
                                Id = listOrd.Id,
                                OrderDate = listOrd.OrderDate.ToString(),
                                OrderDateFormat = listOrd.OrderDate,
                                OrderNumber = listOrd.OrderNumber,
                                CustomerName = listCust.FirstName + " " + listCust.LastName,
                                CustomerId = listOrd.CustomerId,
                                TotalAmount = listOrd.TotalAmount,
                            }).ToList();
                }
                else if (OrderNumber == " " && OrderDate == " " && CustomerId == " ")
                {
                    listVM = (from listOrd in db.Order
                              join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                              select new OrderViewModel
                              {
                                  Id = listOrd.Id,
                                  OrderDate = listOrd.OrderDate.ToString(),
                                  OrderDateFormat = listOrd.OrderDate,
                                  OrderNumber = listOrd.OrderNumber,
                                  CustomerName = listCust.FirstName + " " + listCust.LastName,
                                  CustomerId = listOrd.CustomerId,
                                  TotalAmount = listOrd.TotalAmount,
                              }).ToList();
                }
                else if (OrderNumber == null && OrderDate == null && CustomerId == null)
                {
                    listVM = (from listOrd in db.Order
                              join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                              select new OrderViewModel
                              {
                                  Id = listOrd.Id,
                                  OrderDate = listOrd.OrderDate.ToString(),
                                  OrderDateFormat = listOrd.OrderDate,
                                  OrderNumber = listOrd.OrderNumber,
                                  CustomerName = listCust.FirstName + " " + listCust.LastName,
                                  CustomerId = listOrd.CustomerId,
                                  TotalAmount = listOrd.TotalAmount,
                              }).ToList();
                }
                else if (string.IsNullOrWhiteSpace(OrderNumber) && string.IsNullOrWhiteSpace(OrderDate) && string.IsNullOrWhiteSpace(CustomerId))
                {
                    listVM = (from listOrd in db.Order
                              join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                              select new OrderViewModel
                              {
                                  Id = listOrd.Id,
                                  OrderDate = listOrd.OrderDate.ToString(),
                                  OrderDateFormat = listOrd.OrderDate,
                                  OrderNumber = listOrd.OrderNumber,
                                  CustomerName = listCust.FirstName + " " + listCust.LastName,
                                  CustomerId = listOrd.CustomerId,
                                  TotalAmount = listOrd.TotalAmount,
                              }).ToList();
                }
                else if (!string.IsNullOrEmpty(OrderNumber))
                {
                    if (!string.IsNullOrEmpty(OrderDate) && !string.IsNullOrEmpty(CustomerId))
                    {
                        if(OrderDate == "" || OrderDate == " ")
                        {
                            listVM = (from listOrd in db.Order
                                      join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                      where (listCust.FirstName.ToLower().Contains(firstName.ToLower()) || listCust.LastName.ToLower().Contains(lastName.ToLower())) ||
                                      (listOrd.OrderNumber.ToLower().Contains(OrderNumber.ToLower()))
                                      select new OrderViewModel
                                      {
                                          Id = listOrd.Id,
                                          OrderDate = listOrd.OrderDate.ToString(),
                                          OrderDateFormat = listOrd.OrderDate,
                                          OrderNumber = listOrd.OrderNumber,
                                          CustomerName = listCust.FirstName + " " + listCust.LastName,
                                          CustomerId = listOrd.CustomerId,
                                          TotalAmount = listOrd.TotalAmount,
                                      }).ToList();
                        }
                        else
                        {
                            OrderDate = OrderDate.Replace("-", "/");
                            string[] formats = { "dd/MM/yyyy" };
                            DateTime oDate = DateTime.ParseExact(OrderDate, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                            listVM = (from listOrd in db.Order
                                      join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                      where (listCust.FirstName.ToLower().Contains(firstName.ToLower()) || listCust.LastName.ToLower().Contains(lastName.ToLower())) ||
                                      (listOrd.OrderDate == oDate) || (listOrd.OrderNumber.ToLower().Contains(OrderNumber.ToLower()))
                                      select new OrderViewModel
                                      {
                                          Id = listOrd.Id,
                                          OrderDate = listOrd.OrderDate.ToString(),
                                          OrderDateFormat = listOrd.OrderDate,
                                          OrderNumber = listOrd.OrderNumber,
                                          CustomerName = listCust.FirstName + " " + listCust.LastName,
                                          CustomerId = listOrd.CustomerId,
                                          TotalAmount = listOrd.TotalAmount,
                                      }).ToList();
                        }
                    }
                    else if (!string.IsNullOrEmpty(OrderDate) && string.IsNullOrEmpty(CustomerId))
                    {
                        if (OrderDate == "" || OrderDate == " ")
                        {
                            listVM = (from listOrd in db.Order
                                      join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                      where (listOrd.OrderNumber.ToLower().Contains(OrderNumber.ToLower()))
                                      select new OrderViewModel
                                      {
                                          Id = listOrd.Id,
                                          OrderDate = listOrd.OrderDate.ToString(),
                                          OrderDateFormat = listOrd.OrderDate,
                                          OrderNumber = listOrd.OrderNumber,
                                          CustomerName = listCust.FirstName + " " + listCust.LastName,
                                          CustomerId = listOrd.CustomerId,
                                          TotalAmount = listOrd.TotalAmount,
                                      }).ToList();
                        }
                        else
                        {
                            OrderDate = OrderDate.Replace("-", "/");
                            string[] formats = { "dd/MM/yyyy" };
                            DateTime oDate = DateTime.ParseExact(OrderDate, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                            listVM = (from listOrd in db.Order
                                      join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                      where (listOrd.OrderDate == oDate) || (listOrd.OrderNumber.ToLower().Contains(OrderNumber.ToLower()))
                                      select new OrderViewModel
                                      {
                                          Id = listOrd.Id,
                                          OrderDate = listOrd.OrderDate.ToString(),
                                          OrderDateFormat = listOrd.OrderDate,
                                          OrderNumber = listOrd.OrderNumber,
                                          CustomerName = listCust.FirstName + " " + listCust.LastName,
                                          CustomerId = listOrd.CustomerId,
                                          TotalAmount = listOrd.TotalAmount,
                                      }).ToList();
                        }

                    }
                    else if (string.IsNullOrEmpty(OrderDate) && !string.IsNullOrEmpty(CustomerId))
                    {
                        listVM = (from listOrd in db.Order
                                  join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                  where (listCust.FirstName.ToLower().Contains(firstName.ToLower()) || listCust.LastName.ToLower().Contains(lastName.ToLower())) ||
                                  (listOrd.OrderNumber.ToLower().Contains(OrderNumber.ToLower()))
                                  select new OrderViewModel
                                  {
                                      Id = listOrd.Id,
                                      OrderDate = listOrd.OrderDate.ToString(),
                                      OrderDateFormat = listOrd.OrderDate,
                                      OrderNumber = listOrd.OrderNumber,
                                      CustomerName = listCust.FirstName + " " + listCust.LastName,
                                      CustomerId = listOrd.CustomerId,
                                      TotalAmount = listOrd.TotalAmount,
                                  }).ToList();
                    }
                    else
                    {
                        listVM = (from listOrd in db.Order
                                  join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                  where (listOrd.OrderNumber.ToLower().Contains(OrderNumber.ToLower()))
                                  select new OrderViewModel
                                  {
                                      Id = listOrd.Id,
                                      OrderDate = listOrd.OrderDate.ToString(),
                                      OrderDateFormat = listOrd.OrderDate,
                                      OrderNumber = listOrd.OrderNumber,
                                      CustomerName = listCust.FirstName + " " + listCust.LastName,
                                      CustomerId = listOrd.CustomerId,
                                      TotalAmount = listOrd.TotalAmount,
                                  }).ToList();
                    }
                }
                else if (!string.IsNullOrEmpty(OrderDate))
                {
                    if (OrderDate == "" || OrderDate == " ")
                    {
                        if (!string.IsNullOrEmpty(OrderNumber) && !string.IsNullOrEmpty(CustomerId))
                        {
                            listVM = (from listOrd in db.Order
                                      join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                      where (listCust.FirstName.ToLower().Contains(firstName.ToLower()) || listCust.LastName.ToLower().Contains(lastName.ToLower())) ||
                                      (listOrd.OrderNumber.ToLower().Contains(OrderNumber.ToLower()))
                                      select new OrderViewModel
                                      {
                                          Id = listOrd.Id,
                                          OrderDate = listOrd.OrderDate.ToString(),
                                          OrderDateFormat = listOrd.OrderDate,
                                          OrderNumber = listOrd.OrderNumber,
                                          CustomerName = listCust.FirstName + " " + listCust.LastName,
                                          CustomerId = listOrd.CustomerId,
                                          TotalAmount = listOrd.TotalAmount,
                                      }).ToList();
                        }
                        else if (!string.IsNullOrEmpty(OrderNumber) && string.IsNullOrEmpty(CustomerId))
                        {
                            listVM = (from listOrd in db.Order
                                      join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                      where (listOrd.OrderNumber.ToLower().Contains(OrderNumber.ToLower()))
                                      select new OrderViewModel
                                      {
                                          Id = listOrd.Id,
                                          OrderDate = listOrd.OrderDate.ToString(),
                                          OrderDateFormat = listOrd.OrderDate,
                                          OrderNumber = listOrd.OrderNumber,
                                          CustomerName = listCust.FirstName + " " + listCust.LastName,
                                          CustomerId = listOrd.CustomerId,
                                          TotalAmount = listOrd.TotalAmount,
                                      }).ToList();
                        }
                        else if (string.IsNullOrEmpty(OrderNumber) && !string.IsNullOrEmpty(CustomerId))
                        {
                            listVM = (from listOrd in db.Order
                                      join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                      where (listCust.FirstName.ToLower().Contains(firstName.ToLower()) || listCust.LastName.ToLower().Contains(lastName.ToLower()))
                                      select new OrderViewModel
                                      {
                                          Id = listOrd.Id,
                                          OrderDate = listOrd.OrderDate.ToString(),
                                          OrderDateFormat = listOrd.OrderDate,
                                          OrderNumber = listOrd.OrderNumber,
                                          CustomerName = listCust.FirstName + " " + listCust.LastName,
                                          CustomerId = listOrd.CustomerId,
                                          TotalAmount = listOrd.TotalAmount,
                                      }).ToList();
                        }
                        else
                        {
                            (from listOrd in db.Order
                             join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                             select new OrderViewModel
                             {
                                 Id = listOrd.Id,
                                 OrderDate = listOrd.OrderDate.ToString(),
                                 OrderDateFormat = listOrd.OrderDate,
                                 OrderNumber = listOrd.OrderNumber,
                                 CustomerName = listCust.FirstName + " " + listCust.LastName,
                                 CustomerId = listOrd.CustomerId,
                                 TotalAmount = listOrd.TotalAmount,
                             }).ToList();
                        }
                    }
                    else
                    {
                        OrderDate = OrderDate.Replace("-", "/");
                        string[] formats = { "dd/MM/yyyy" };
                        DateTime oDate = DateTime.ParseExact(OrderDate, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                        if (!string.IsNullOrEmpty(OrderNumber) && !string.IsNullOrEmpty(CustomerId))
                        {
                            listVM = (from listOrd in db.Order
                                      join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                      where (listCust.FirstName.ToLower().Contains(firstName.ToLower()) || listCust.LastName.ToLower().Contains(lastName.ToLower())) ||
                                      (listOrd.OrderDate == oDate) || (listOrd.OrderNumber.ToLower().Contains(OrderNumber.ToLower()))
                                      select new OrderViewModel
                                      {
                                          Id = listOrd.Id,
                                          OrderDate = listOrd.OrderDate.ToString(),
                                          OrderDateFormat = listOrd.OrderDate,
                                          OrderNumber = listOrd.OrderNumber,
                                          CustomerName = listCust.FirstName + " " + listCust.LastName,
                                          CustomerId = listOrd.CustomerId,
                                          TotalAmount = listOrd.TotalAmount,
                                      }).ToList();
                        }
                        else if (!string.IsNullOrEmpty(OrderNumber) && string.IsNullOrEmpty(CustomerId))
                        {
                            listVM = (from listOrd in db.Order
                                      join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                      where (listOrd.OrderDate == oDate) || (listOrd.OrderNumber.ToLower().Contains(OrderNumber.ToLower()))
                                      select new OrderViewModel
                                      {
                                          Id = listOrd.Id,
                                          OrderDate = listOrd.OrderDate.ToString(),
                                          OrderDateFormat = listOrd.OrderDate,
                                          OrderNumber = listOrd.OrderNumber,
                                          CustomerName = listCust.FirstName + " " + listCust.LastName,
                                          CustomerId = listOrd.CustomerId,
                                          TotalAmount = listOrd.TotalAmount,
                                      }).ToList();
                        }
                        else if (string.IsNullOrEmpty(OrderNumber) && !string.IsNullOrEmpty(CustomerId))
                        {
                            listVM = (from listOrd in db.Order
                                      join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                      where (listCust.FirstName.ToLower().Contains(firstName.ToLower()) || listCust.LastName.ToLower().Contains(lastName.ToLower())) ||
                                      (listOrd.OrderDate == oDate)
                                      select new OrderViewModel
                                      {
                                          Id = listOrd.Id,
                                          OrderDate = listOrd.OrderDate.ToString(),
                                          OrderDateFormat = listOrd.OrderDate,
                                          OrderNumber = listOrd.OrderNumber,
                                          CustomerName = listCust.FirstName + " " + listCust.LastName,
                                          CustomerId = listOrd.CustomerId,
                                          TotalAmount = listOrd.TotalAmount,
                                      }).ToList();
                        }
                        else
                        {
                            (from listOrd in db.Order
                             join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                             where (listOrd.OrderDate == oDate)
                             select new OrderViewModel
                             {
                                 Id = listOrd.Id,
                                 OrderDate = listOrd.OrderDate.ToString(),
                                 OrderDateFormat = listOrd.OrderDate,
                                 OrderNumber = listOrd.OrderNumber,
                                 CustomerName = listCust.FirstName + " " + listCust.LastName,
                                 CustomerId = listOrd.CustomerId,
                                 TotalAmount = listOrd.TotalAmount,
                             }).ToList();
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(CustomerId))
                {
                    if (!string.IsNullOrEmpty(OrderNumber) && !string.IsNullOrEmpty(OrderDate))
                    {
                        if (OrderDate == "" || OrderDate == " ")
                        {
                            listVM = (from listOrd in db.Order
                                      join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                      where (listCust.FirstName.ToLower().Contains(firstName.ToLower()) || listCust.LastName.ToLower().Contains(lastName.ToLower())) ||
                                      (listOrd.OrderNumber.ToLower().Contains(OrderNumber.ToLower()))
                                      select new OrderViewModel
                                      {
                                          Id = listOrd.Id,
                                          OrderDate = listOrd.OrderDate.ToString(),
                                          OrderDateFormat = listOrd.OrderDate,
                                          OrderNumber = listOrd.OrderNumber,
                                          CustomerName = listCust.FirstName + " " + listCust.LastName,
                                          CustomerId = listOrd.CustomerId,
                                          TotalAmount = listOrd.TotalAmount,
                                      }).ToList();
                        }
                        else
                        {
                            OrderDate = OrderDate.Replace("-", "/");
                            string[] formats = { "dd/MM/yyyy" };
                            DateTime oDate = DateTime.ParseExact(OrderDate, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                            listVM = (from listOrd in db.Order
                                      join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                      where (listCust.FirstName.ToLower().Contains(firstName.ToLower()) || listCust.LastName.ToLower().Contains(lastName.ToLower())) ||
                                      (listOrd.OrderDate == oDate) || (listOrd.OrderNumber.ToLower().Contains(OrderNumber.ToLower()))
                                      select new OrderViewModel
                                      {
                                          Id = listOrd.Id,
                                          OrderDate = listOrd.OrderDate.ToString(),
                                          OrderDateFormat = listOrd.OrderDate,
                                          OrderNumber = listOrd.OrderNumber,
                                          CustomerName = listCust.FirstName + " " + listCust.LastName,
                                          CustomerId = listOrd.CustomerId,
                                          TotalAmount = listOrd.TotalAmount,
                                      }).ToList();
                        }
                    }
                    else if (!string.IsNullOrEmpty(OrderNumber) && string.IsNullOrEmpty(OrderDate))
                    {
                        listVM = (from listOrd in db.Order
                                  join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                  where (listCust.FirstName.ToLower().Contains(firstName.ToLower()) || listCust.LastName.ToLower().Contains(lastName.ToLower())) ||
                                  (listOrd.OrderNumber.ToLower().Contains(OrderNumber.ToLower()))
                                  select new OrderViewModel
                                  {
                                      Id = listOrd.Id,
                                      OrderDate = listOrd.OrderDate.ToString(),
                                      OrderDateFormat = listOrd.OrderDate,
                                      OrderNumber = listOrd.OrderNumber,
                                      CustomerName = listCust.FirstName + " " + listCust.LastName,
                                      CustomerId = listOrd.CustomerId,
                                      TotalAmount = listOrd.TotalAmount,
                                  }).ToList();
                    }
                    else if (string.IsNullOrEmpty(OrderNumber) && !string.IsNullOrEmpty(OrderDate))
                    {
                        if (OrderDate == "" || OrderDate == " ")
                        {
                            listVM = (from listOrd in db.Order
                                      join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                      where (listCust.FirstName.ToLower().Contains(firstName.ToLower()) || listCust.LastName.ToLower().Contains(lastName.ToLower()))
                                      select new OrderViewModel
                                      {
                                          Id = listOrd.Id,
                                          OrderDate = listOrd.OrderDate.ToString(),
                                          OrderDateFormat = listOrd.OrderDate,
                                          OrderNumber = listOrd.OrderNumber,
                                          CustomerName = listCust.FirstName + " " + listCust.LastName,
                                          CustomerId = listOrd.CustomerId,
                                          TotalAmount = listOrd.TotalAmount,
                                      }).ToList();
                        }
                        else
                        {
                            OrderDate = OrderDate.Replace("-", "/");
                            string[] formats = { "dd/MM/yyyy" };
                            DateTime oDate = DateTime.ParseExact(OrderDate, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                            listVM = (from listOrd in db.Order
                                      join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                      where (listCust.FirstName.ToLower().Contains(firstName.ToLower()) || listCust.LastName.ToLower().Contains(lastName.ToLower())) ||
                                      (listOrd.OrderDate == oDate)
                                      select new OrderViewModel
                                      {
                                          Id = listOrd.Id,
                                          OrderDate = listOrd.OrderDate.ToString(),
                                          OrderDateFormat = listOrd.OrderDate,
                                          OrderNumber = listOrd.OrderNumber,
                                          CustomerName = listCust.FirstName + " " + listCust.LastName,
                                          CustomerId = listOrd.CustomerId,
                                          TotalAmount = listOrd.TotalAmount,
                                      }).ToList();
                        }
                    }
                    else
                    {
                        listVM = (from listOrd in db.Order
                                  join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                                  where (listCust.FirstName.ToLower().Contains(firstName.ToLower()) || listCust.LastName.ToLower().Contains(lastName.ToLower()))
                                  select new OrderViewModel
                                  {
                                      Id = listOrd.Id,
                                      OrderDate = listOrd.OrderDate.ToString(),
                                      OrderDateFormat = listOrd.OrderDate,
                                      OrderNumber = listOrd.OrderNumber,
                                      CustomerName = listCust.FirstName + " " + listCust.LastName,
                                      CustomerId = listOrd.CustomerId,
                                      TotalAmount = listOrd.TotalAmount,
                                  }).ToList();
                    }
                }
                else
                {
                    listVM = (from listOrd in db.Order
                              join listCust in db.Customer on listOrd.CustomerId equals listCust.Id
                              select new OrderViewModel
                              {
                                  Id = listOrd.Id,
                                  OrderDate = listOrd.OrderDate.ToString(),
                                  OrderDateFormat = listOrd.OrderDate,
                                  OrderNumber = listOrd.OrderNumber,
                                  CustomerName = listCust.FirstName + " " + listCust.LastName,
                                  CustomerId = listOrd.CustomerId,
                                  TotalAmount = listOrd.TotalAmount,
                              }).ToList();
                }

                return listVM;
            }
        }
        public OrderViewModel GetDetailOrderById(int id)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                OrderViewModel model = new OrderViewModel();
                /* select a.OrderNumber, d.FirstName, c.ProductName
                   from[Order] a JOIN[OrderItem] b on a.Id = b.OrderId
                   JOIN Product c on b.ProductId = c.Id
                   JOIN Customer d on d.Id = a.CustomerId
                    where a.OrderNumber = '542378' */

                // Mapping Master Order
                model = (from a in db.Order
                         join d in db.Customer on a.CustomerId equals d.Id
                         where a.Id == id
                         select new OrderViewModel{
                            Id = a.Id,
                             OrderDateFormat = a.OrderDate,
                            OrderNumber = a.OrderNumber,
                            CustomerName = d.FirstName + " " + d.LastName,
                            CustomerId  = d.Id,
                            TotalAmount = a.TotalAmount
                         }).Single();

                model.OrderDate = model.OrderDateFormat.ToShortDateString();

                // Mapping Detail Order (Order Item)
                model.ListOrderItem = (from a in db.Order
                                       join b in db.OrderItem on a.Id equals b.OrderId
                                       join c in db.Product on b.ProductId equals c.Id
                                       where a.Id == id
                                       select new OrderItemViewModel
                                       {
                                            Id = b.Id,
                                            OrderId = b.OrderId,
                                            OrderNumber = a.OrderNumber,
                                            ProductId  = c.Id,
                                            ProductName = c.ProductName,
                                            UnitPrice = b.UnitPrice,
                                            Quantity = b.Quantity,
                                            TotalAmount = b.UnitPrice * b.Quantity
                                        }).ToList();

                return model;
            }
        }
    }
}
