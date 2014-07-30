using System;
using System.Web.Mvc;
using Dependencia;
using Dominio.Entidades;
using Dominio.Servicos;



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
            usuario.Cracha = (rdn.Next(1000000000));

            _usuarioServicos.Cadastrar(usuario);
            return RedirectToAction("CadastrarUsuario");
        }
    }
}

