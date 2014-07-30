using System.Web.Mvc;
using Event.App_Start.NHibernate;

namespace Event
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new TransactionPerRequest());
        }
    }
}