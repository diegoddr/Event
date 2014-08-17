using System;
using System.Linq;
using System.Web.Mvc;
using Dependencia;
using Dominio.Entidades;
using Dominio.Servicos;
using NHibernate.Criterion;
using NHibernate.Linq;


namespace Event.Controllers
{
    public class UsuarioController : Controller
    {
        //
        // GET: /Usuario/
        private readonly UsuarioServicos _usuarioServicos;
        public UsuarioController()
        {
            _usuarioServicos = Dependencias.Resolver<UsuarioServicos>();
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
    }
}

