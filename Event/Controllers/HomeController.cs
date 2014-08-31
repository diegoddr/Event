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
                var modulosDoDia =
                    _moduloServicos.Listar(e => e.Usuarios.Any(x => x.Cracha.ToString() == cracha)
                                                        && e.Data.Date == DateTime.Now.Date);
                InscricaoModulo moduloDaHora = new InscricaoModulo();

                foreach (var i in modulosDoDia)
                {
                    var dezMinutos = DateTime.Parse(i.Inicio).AddMinutes(-10);
                    var maisDezMinutos = DateTime.Parse(i.Fim).AddMinutes(10);
                    if (DateTime.Now.TimeOfDay >= dezMinutos.TimeOfDay && DateTime.Now.TimeOfDay <= maisDezMinutos.TimeOfDay)
                    {
                        moduloDaHora =
                            _inscricaoModuloServicos.ObterPorFiltro(e => e.Usuario.Cracha.ToString() == cracha && e.Modulo == i);
                        break;
                    }
                }
                if (moduloDaHora != null)
                {
                    if (moduloDaHora.Entrada != null)
                    {
                        moduloDaHora.Saida = DateTime.Now.TimeOfDay.ToString();
                        _inscricaoModuloServicos.Cadastrar(moduloDaHora);
                        
                    }
                    else
                    {
                        moduloDaHora.Entrada = DateTime.Now.TimeOfDay.ToString();
                        _inscricaoModuloServicos.Cadastrar(moduloDaHora);
                    }

                }
                return View(usuario);
            }
            return View(usuario);

        }
    }
}