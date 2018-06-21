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
    }
}
