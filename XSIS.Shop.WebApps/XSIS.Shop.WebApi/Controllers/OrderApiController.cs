using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using XSIS.Shop.Repository;
using XSIS.Shop.ViewModels;

namespace XSIS.Shop.WebApi.Controllers
{
    public class OrderApiController : ApiController
    {
        private OrderRepository service = new OrderRepository();

        [HttpGet]
        public List<OrderViewModel> Get()
        {
            var result = service.GetAllOrder();
            return result;
        }

        [HttpGet]
        public List<OrderViewModel> SearchByKey(string id)
        {
            string[] Parameters = id.Split('|');

            string param1 = Parameters[0];
            string param2 = Parameters[1];
            string param3 = Parameters[2];

            var result = service.SearchByKey(param1, param2, param3);
            return result;
        }

        [HttpGet]
        public OrderViewModel Get(int id)
        {
            var result = service.GetDetailOrderById(id);
            return result;
        }

        [HttpPost]
        public List<OrderItemViewModel> GroupListItem(List<OrderItemViewModel> ListItem)
        {
            var result = service.GroupListItem(ListItem);
            return result;
        }

        [HttpPost]
        public List<OrderItemViewModel> RemoveItem(OrderRemoveViewModel OrderRemoveItem)
        {
            var result = service.RemoveItem(OrderRemoveItem);
            return result;
        }

        [HttpGet]
        public int GetLatestOrderID()
        {
            var result = service.GetLatestOrderID();
            return result;
        }

        [HttpPost]
        public int Post(OrderViewModel order)
        {
            try
            {
                service.AddNewOrder(order);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
