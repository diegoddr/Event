using System;
using System.Web.Mvc;
using Dominio.Entidades;
using Dominio.Servicos;

namespace Event.Controllers
{
    public class EventoController : Controller
    {
        private readonly Usuario _usuario;
        private readonly EventoServicos _eventoServicos;
        private readonly UsuarioServicos _usuarioServicos;
        public EventoController()
        {
            _eventoServicos = Dependencia.Dependencias.Resolver<EventoServicos>();
            _usuarioServicos = Dependencia.Dependencias.Resolver<UsuarioServicos>();
            _usuario = _usuarioServicos.ObterPorId((int)System.Web.HttpContext.Current.Session["Usuario"]);
        }
        public ActionResult CadastrarEvento()
        {
            var listarMeusEventos = _eventoServicos.Listar(e => e.Organizador == _usuario && e.Fim >= DateTime.Now && e.Ativo.Equals("S"));
            return View(listarMeusEventos);
        }
        [HttpPost]
        public ActionResult CadastrarEvento(FormCollection f)
        {
            var evento = new Evento();
            TryUpdateModel(evento);
            evento.Organizador = _usuario;
            evento.Usuarios.Add(_usuario);
            evento.Ativo = "S";
            _eventoServicos.Cadastrar(evento);
            return RedirectToAction("CadastrarEvento");
        }
        public ActionResult ListarTodosEventos()
        {
            var listarTodosEventos =
                _eventoServicos.Listar(e => (e.Usuarios.Contains(_usuario) || e.Organizador == _usuario) && e.Fim >= DateTime.Now && e.Ativo.Equals("S"));
            return View(listarTodosEventos);
        }
        public ActionResult ListarMeusEventos()
        {
            var listarMeusEventos = _eventoServicos.Listar(e => e.Organizador == _usuario && e.Fim >= DateTime.Now && e.Ativo.Equals("S"));
            return View(listarMeusEventos);
        }
        public ActionResult ListarEventosAntigos()
        {
            var listarEventosAntigos =
                _eventoServicos.Listar(
                    e => (e.Usuarios.Contains(_usuario) || e.Organizador == _usuario) && (e.Fim < DateTime.Now || e.Ativo.Equals("N")));
            return View(listarEventosAntigos);
        }
        public ActionResult AlterarEvento(int id)
        {
            var evento = _eventoServicos.ObterPorId(id);
            return View(evento);
        }
        [HttpPost]
        public ActionResult AlterarEvento(FormCollection f, int id)
        {
            var evento = _eventoServicos.ObterPorId(id);
            evento.Nome = f["Nome"];
            evento.Descricao = f["Descricao"];
            evento.Inicio = Convert.ToDateTime(f["Inicio"]);
            evento.Fim = Convert.ToDateTime(f["Fim"]);
            _eventoServicos.Cadastrar(evento);
            return RedirectToAction("CadastrarEvento");
        }
        public void InativarEvento(int id)
        {
            var evento = _eventoServicos.ObterPorId(id);
            evento.Ativo = "N";
            _eventoServicos.Cadastrar(evento);
            Response.Redirect("/Evento/CadastrarEvento");
        }
    }
}
