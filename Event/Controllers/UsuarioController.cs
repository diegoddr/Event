using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dependencia;
using Dominio.Entidades;
using Dominio.Servicos;
using Event.Models;
using NHibernate.Criterion;
using NHibernate.Linq;


namespace Event.Controllers
{
    public class UsuarioController : Controller
    {
        //
        // GET: /Usuario/
        private readonly InscricaoModuloServicos _inscricaoModuloServicos;
        private readonly ModuloServicos _moduloServicos;
        private readonly UsuarioServicos _usuarioServicos;
        public UsuarioController()
        {
            _usuarioServicos = Dependencias.Resolver<UsuarioServicos>();
            _moduloServicos = Dependencias.Resolver<ModuloServicos>();
            _inscricaoModuloServicos = Dependencias.Resolver<InscricaoModuloServicos>();
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CadastrarUsuario()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CadastrarUsuario(FormCollection f)
        {
            var usuario = new Usuario();
            TryUpdateModel(usuario);
            if (f["Masculino"] == "on")
            {
                usuario.Sexo = "M";
            }
            else
            {
                usuario.Sexo = "F";
            }

            Random rdn = new Random();
            bool existe = true;
            while (existe)
            {
                usuario.Cracha = (rdn.Next(1000000000));
                var existeCracha = _usuarioServicos.Listar(e => e.Cracha == usuario.Cracha);
                if (!existeCracha.Any()) existe = false;
            }
            _usuarioServicos.Cadastrar(usuario);
            return RedirectToAction("Login","Login");
        }
        public JsonResult validarLogin(string id)
        {
            var usuario = _usuarioServicos.Listar(e => e.Login.ToLower() == id.ToLower());
            if (usuario.Any()) return Json("Existe", JsonRequestBehavior.AllowGet);
            return Json("Disponivel", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AlterarUsuario(int id)
        {
            var usuario = _usuarioServicos.ObterPorId(id);
            return View(usuario);
        }
        [HttpPost]
        public ActionResult AlterarUsuario(FormCollection f, int id)
        {
            var usuario = _usuarioServicos.ObterPorId(id);
            usuario.Nome = f["Nome"];
            usuario.Senha = f["Senha"];
            usuario.Telefone = f["Telefone"];
            return RedirectToAction("IndexCracha", "Home");
        }
        public JsonResult StatusUsuarioModulo(int id)
        {
            var modulo = _moduloServicos.ObterPorId(id);
            var lista = new List<UsuarioPresente>();
            foreach (var i in modulo.Usuarios)
            {
                var inscricaoModulo = _inscricaoModuloServicos.ObterPorFiltro(e => e.Modulo == modulo && e.Usuario == i);
                lista.Add(new UsuarioPresente
                {
                    Id = inscricaoModulo.Id,
                    Nome = i.Nome,
                    HoraEntrada = inscricaoModulo.Entrada??"- - : - -",
                    HoraSaida = inscricaoModulo.Saida ?? "- - : - -"
                });
            }
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public void zerarSaida(int id)
        {
            var saidaUsuario = _inscricaoModuloServicos.ObterPorId(id);
            saidaUsuario.Saida = null;
            _inscricaoModuloServicos.Cadastrar(saidaUsuario);
        }
        public void zerarEntrada(int id)
        {
            var entradaUsuario = _inscricaoModuloServicos.ObterPorId(id);
            entradaUsuario.Entrada = null;
            _inscricaoModuloServicos.Cadastrar(entradaUsuario);
        }
    }
}

