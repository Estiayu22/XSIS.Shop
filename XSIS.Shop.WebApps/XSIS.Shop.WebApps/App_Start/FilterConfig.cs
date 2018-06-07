using System.Web;
using System.Web.Mvc;

namespace XSIS.Shop.WebApps
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
