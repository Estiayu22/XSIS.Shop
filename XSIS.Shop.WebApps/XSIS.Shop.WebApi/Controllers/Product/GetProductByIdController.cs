using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using XSIS.Shop.Repository;
using XSIS.Shop.ViewModels;

namespace XSIS.Shop.WebApi.Controllers.Product
{
    public class GetProductByIdController : ApiController
    {
        private ProductRepository service = new ProductRepository();

        [HttpGet]
        public ProductViewModel GetProductById(int id)
        {
            var result = service.GetProductById(id);
            return result;
        }
    }
}
