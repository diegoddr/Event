using System;
using System.Linq;
using System.Web.Mvc;
using Dominio.Entidades;
using Dominio.Servicos;
using Event.ViewModels;

namespace Event.Controllers
{
    public class ModuloController : Controller
    {
        private readonly Usuario _usuario;
        private readonly ModuloServicos _moduloServicos;
        private readonly EventoServicos _eventoServicos;
        private readonly UsuarioServicos _usuarioServicos;
        public ModuloController()
        {
            _moduloServicos = Dependencia.Dependencias.Resolver<ModuloServicos>();
            _moduloServicos = Dependencia.Dependencias.Resolver<ModuloServicos>();
            _eventoServicos = Dependencia.Dependencias.Resolver<EventoServicos>();
            _usuarioServicos = Dependencia.Dependencias.Resolver<UsuarioServicos>();
            _usuario = _usuarioServicos.ObterPorId((int)System.Web.HttpContext.Current.Session["Usuario"]);
        }

        public ActionResult CadastrarModulo()
        {
            var dados = new DadosModulo();
            dados.Eventos = _eventoServicos.Listar(e => e.Organizador == _usuario && e.Fim >= DateTime.Now);
            dados.Modulos = _moduloServicos.Listar(e => e.Evento.Organizador == _usuario && e.Data >= DateTime.Now && e.Ativo.Equals("S"));
            dados.Usuario = _usuario;
            return View(dados);
        }

        [HttpPost]
        public ActionResult CadastrarModulo(FormCollection f)
        {
            var modulo = new Modulo();
            TryUpdateModel(modulo);
            modulo.Evento = _eventoServicos.ObterPorId(Convert.ToInt16(f["Evento"]));
            modulo.Ativo = "S";
            _moduloServicos.Cadastrar(modulo);
            return RedirectToAction("CadastrarModulo");
        }
        public ActionResult ListarTodosModulos()
        {
            var dados = new DadosModulo();
            dados.Modulos =
                _moduloServicos.Listar(
                    e => (e.Usuarios.Contains(_usuario) || e.Evento.Organizador == _usuario) && e.Data >= DateTime.Now && e.Ativo.Equals("S"));
            dados.Usuario = _usuario;
            return View(dados);
        }
        public ActionResult ListarMeusModulos()
        {
            var dados = new DadosModulo();
            dados.Modulos = _moduloServicos.Listar(e => e.Evento.Organizador == _usuario && e.Data >= DateTime.Now && e.Ativo.Equals("S"));
            return View(dados);
        }
        public ActionResult ListarModulosAntigos()
        {
            var dados = new DadosModulo();
            dados.Modulos =
                _moduloServicos.Listar(
                    e => (e.Usuarios.Contains(_usuario) || e.Evento.Organizador == _usuario) && e.Data < DateTime.Now);
            return View(dados);
        }
        public ActionResult AlterarModulo(int id)
        {
            var dadosModulo = new DadosModulo();
            dadosModulo.Modulo = _moduloServicos.ObterPorId(id);
            dadosModulo.Eventos = _eventoServicos.Listar(e => true);
            return View(dadosModulo);
        }
        [HttpPost]
        public ActionResult AlterarModulo(FormCollection f, int id)
        {
            var modulo = _moduloServicos.ObterPorId(id);
            modulo.Nome = f["Nome"];
            modulo.Descricao = f["Descricao"];
            modulo.Vagas = Convert.ToInt16(f["Vagas"]);
            modulo.Data = Convert.ToDateTime(f["Data"]);
            modulo.Inicio = f["Inicio"];
            modulo.Fim = f["Fim"];
            modulo.Observacao = f["Observacao"];
            //modulo.Evento = _eventoServicos.ObterPorId(Convert.ToInt16(f["Evento"]));
            _moduloServicos.Cadastrar(modulo);
            return RedirectToAction("AlterarModulo");
        }
        public ActionResult ConvidarUsuario(int id)
        {
            var listarUsuarios = new DadosModulo();
            listarUsuarios.Usuarios = _usuarioServicos.Listar(e => true);
            listarUsuarios.Modulo = _moduloServicos.ObterPorId(id);
            return View(listarUsuarios);
        }
        [HttpPost]
        public ActionResult ConvidarUsuario(FormCollection f)
        {
            var listarUsuarios = _usuarioServicos.Listar(e => true).OrderBy(e => e.Nome).ToList();
            var modulo = _moduloServicos.ObterPorId(Convert.ToInt16(f["Modulo"]));
            foreach (var i in listarUsuarios)
            {
                var formUsuario = i.Id + "usu";
                if (f[formUsuario] == "on")
                {
                    i.Modulos.Add(modulo);
                    _usuarioServicos.Cadastrar(i);
                }
            }
            return RedirectToAction("CadastrarModulo");
        }

        public void RecusarConvite(int id)
        {
            var modulo = _moduloServicos.ObterPorId(id);
            var usuario = _usuario;
            usuario.Modulos.Remove(modulo);
            _usuarioServicos.Cadastrar(usuario);
        }

        public void AceitarConvite(int id)
        {
            var modulo = _moduloServicos.ObterPorId(id);
            var usuario = _usuario;
            usuario.Modulos.Remove(modulo);
            _usuarioServicos.Cadastrar(usuario);
            modulo.Usuarios.Add(usuario);
            _moduloServicos.Cadastrar(modulo);
        }
        public void InativarModulo(int id)
        {
            var modulo = _moduloServicos.ObterPorId(id);
            modulo.Ativo = "N";
            _moduloServicos.Cadastrar(modulo);
            Response.Redirect("/Modulo/CadastrarModulo");
        }

        public JsonResult validarDataModulo(int id)
        {
            var evento = _eventoServicos.ObterPorId(id);
            var dataInicioEvento = evento.Inicio;
            var dataFimEvento = evento.Fim;
            var result = new {dataInicio = dataInicioEvento.ToShortDateString(), dataFim = dataFimEvento.ToShortDateString()};
            return Json(result,JsonRequestBehavior.AllowGet);
        }

    }
}

