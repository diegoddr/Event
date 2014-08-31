using System.Web.Mvc;
using System.Web.Security;
using Dominio.Servicos;
using Event.Models;

namespace Event.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioServicos _usuarioServicos;
        public LoginController()
        {
            _usuarioServicos = Dependencia.Dependencias.Resolver<UsuarioServicos>();

        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string returnUrl, LoginModel model, FormCollection f)
        {
            if (ModelState.IsValid && Autenticar(model.UserName, model.Password))
            {
                var usuario = _usuarioServicos.ObterPorLogin(model.UserName);
                FormsAuthentication.SetAuthCookie(usuario.Nome, false);
                Session.Add("Usuario", usuario.Id);
                return RedirectToAction("Redirecionando", "Home");
            }
            else
            {
                TempData["Login"] = model.UserName;
                return View(model);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("IndexCracha", "Home");
        }
        public bool Autenticar(string usuario, string senha)
        {
            var user = _usuarioServicos.ObterPorLogin(usuario);
            if (user != null && user.Senha == senha)
                return true;
            return false;
        }
        public ActionResult Logout()
        {
            Session["usuario"] = null;
            FormsAuthentication.SignOut();
            return Redirect("Login");
        }
    }
}

