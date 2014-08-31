using System;
using System.Linq;
using System.Web.Mvc;
using Dependencia;
using Dominio.Entidades;
using Dominio.Servicos;

namespace Event.Controllers
{
    public class HomeController : Controller
    {
        private readonly UsuarioServicos _usuarioServicos;
        private readonly ModuloServicos _moduloServicos;
        private readonly InscricaoModuloServicos _inscricaoModuloServicos;

        public HomeController()
        {
            _usuarioServicos = Dependencias.Resolver<UsuarioServicos>();
            _moduloServicos = Dependencias.Resolver<ModuloServicos>();
            _inscricaoModuloServicos = Dependencias.Resolver<InscricaoModuloServicos>();
        }
        public ActionResult Redirecionando()
        {
            var usuario = _usuarioServicos.ObterPorId((int)System.Web.HttpContext.Current.Session["Usuario"]);

            return RedirectToAction("IndexCracha", "Home", new { cracha = usuario.Cracha.ToString() });
        }

        public ActionResult IndexCracha(string cracha)
        {
            var usuario = _usuarioServicos.ObterPorId((int)System.Web.HttpContext.Current.Session["Usuario"]);

            if (cracha != usuario.Cracha.ToString())
            {
                var obterModulos =
                _inscricaoModuloServicos.ObterPorFiltro(e => e.Usuario.Cracha.ToString() == cracha.ToString()
                    && e.Modulo.Data.Date == DateTime.Now.Date);

                if (obterModulos != null)
                {
                    obterModulos.Entrada = DateTime.Now.TimeOfDay.ToString();
                    _inscricaoModuloServicos.Cadastrar(obterModulos);
                }

                return View(usuario);
            }
            return View(usuario);

        }
    }
}