using System.Web.Mvc;
using Dependencia;
using Dominio.Servicos;

namespace Event.Controllers
{
    public class HomeController : Controller
    {
        private readonly UsuarioServicos _usuarioServicos;

        public HomeController()
        {
            _usuarioServicos = Dependencias.Resolver<UsuarioServicos>();
        }
        //public ActionResult Redirecionando()
        //{
        //    var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

        //    return RedirectToAction("IndexCracha", "Home", new { id = usuario.Cracha });
        //}

        public ActionResult IndexCracha(int? cracha)
        {
            var usuario = _usuarioServicos.ObterPorId((int)System.Web.HttpContext.Current.Session["Usuario"]);
            return View(usuario);
        }
    }
}
