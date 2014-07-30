using System.Web.Mvc;
using Persistencia;

namespace Event.App_Start.NHibernate
{
    public class SessionPerRequest : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            NHibernateHelper.DisposeSession();
        }

    }
}