using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Plateforme.AlloTabib.ServiceLayer.Filters;

namespace Plateforme.AlloTabib.ServiceLayer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterGlobalApiFilters(HttpFilterCollection filters)
        {
            filters.Add(new NhSessionManagementAttribute());
            filters.Add(new ApiResponseExceptionFilterAttribute());
        }
    }
}