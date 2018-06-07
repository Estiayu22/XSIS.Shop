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
                var ListProducts = db.Product.Include(p => p.Supplier);
                List<ProductViewModel> ListVM = new List<ProductViewModel>();
                return ListVM;
            }
        }
    }
}
