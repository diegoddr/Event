using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Dependencia;
using Dominio.Entidades;
using Dominio.Servicos;
using Event.Models;
using Event.ViewModels;

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
            TempData["SucessoSair"] = TempData["SucessoSair"];
            TempData["SucessoEntrar"] = TempData["SucessoEntrar"];
            var usuario = _usuarioServicos.ObterPorId((int) System.Web.HttpContext.Current.Session["Usuario"]);
            return RedirectToAction("IndexCracha", "Home", new {cracha = usuario.Cracha.ToString()});
        }

        public ActionResult IndexCracha(string cracha)
        {
            TempData["SucessoSair"] = TempData["SucessoSair"];
            TempData["SucessoEntrar"] = TempData["SucessoEntrar"];
            var usuario = _usuarioServicos.ObterPorId((int) System.Web.HttpContext.Current.Session["Usuario"]);
            var dados = new DadosModulo();
            dados.Usuario = usuario;
            dados.ModulosHoje =
                _moduloServicos.Listar(e => e.Evento.Organizador == usuario && e.Data.Date == DateTime.Now.Date);
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
                    if (DateTime.Now.TimeOfDay >= dezMinutos.TimeOfDay &&
                        DateTime.Now.TimeOfDay <= maisDezMinutos.TimeOfDay)
                    {
                        moduloDaHora =
                            _inscricaoModuloServicos.ObterPorFiltro(
                                e => e.Usuario.Cracha.ToString() == cracha && e.Modulo == i);
                        break;
                    }
                }
                if (moduloDaHora != null)
                {
                    if (moduloDaHora.Entrada != null)
                    {
                        moduloDaHora.Saida = DateTime.Now.TimeOfDay.ToString();
                        _inscricaoModuloServicos.Cadastrar(moduloDaHora);
                        TempData["SucessoSair"] = "Horário de saida salvo";
                        return RedirectToAction("Redirecionando");
                    }
                    else
                    {
                        moduloDaHora.Entrada = DateTime.Now.TimeOfDay.ToString();
                        _inscricaoModuloServicos.Cadastrar(moduloDaHora);
                        TempData["SucessoEntrar"] = "Horário de entrada salvo";
                        return RedirectToAction("Redirecionando");
                    }

                }
                return View(dados);
            }

            return View(dados);

        }

    }
}
