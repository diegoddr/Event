using System.Web.Mvc;
using Persistencia;

namespace Event.App_Start.NHibernate
{
    public class TransactionPerRequest : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            NHibernateHelper.BeginTransaction();
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.Exception == null)
            {
                NHibernateHelper.CommitTransaction();
            }
            NHibernateHelper.DisposeSession();
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }
}