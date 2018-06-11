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
    public class CustomerApiController : ApiController
    {
        private CustomerRepository service = new CustomerRepository();

        [HttpGet]
        public List<CustomerViewModel> Get()
        {
            var result = service.GetAllCustomer();
            return result;
        }

        [HttpGet]
        public CustomerViewModel Get(int id)
        {
            var result = service.GetCustomerById(id);
            return result;
        }

        [HttpGet]
        public List<CustomerViewModel> SearchByKey(string id)
        {
            string[] Parameters = id.Split('|');

            string param1 = Parameters[0];
            string param2 = Parameters[1];
            string param3 = Parameters[2];

            var result = service.SearchByKey(param1, param2, param3);
            return result;
        }

        [HttpGet]
        public string CekNamaExisting(string id, string id2)
        {
            var result = service.CekNamaExisting(id, id2);
            return result;
        }

        [HttpGet]
        public string CekEmailExisting(string id)
        {
            var result = service.CekEmailExisting(id);
            return result;
        }

        [HttpPost]
        public int Post(CustomerViewModel customer)
        {
            try
            {
                service.AddNewCustomer(customer);
                return 1;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        [HttpPut]
        public int Put(CustomerViewModel customer)
        {
            try
            {
                service.UpdateCustomer(customer);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [HttpDelete]
        public int Delete(int id)
        {
            try
            {
                service.DeleteCustomer(id);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
