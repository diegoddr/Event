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
        private readonly InscricaoModuloServicos _inscricaoModuloServicos;
        public ModuloController()
        {
            _moduloServicos = Dependencia.Dependencias.Resolver<ModuloServicos>();
            _moduloServicos = Dependencia.Dependencias.Resolver<ModuloServicos>();
            _eventoServicos = Dependencia.Dependencias.Resolver<EventoServicos>();
            _usuarioServicos = Dependencia.Dependencias.Resolver<UsuarioServicos>();
            _inscricaoModuloServicos = Dependencia.Dependencias.Resolver<InscricaoModuloServicos>();
            _usuario = _usuarioServicos.ObterPorId((int)System.Web.HttpContext.Current.Session["Usuario"]);
        }

        public ActionResult CadastrarModulo(int? id)
        {
            var dados = new DadosModulo();
            dados.Eventos = _eventoServicos.Listar(e => e.Organizador == _usuario && e.Fim >= DateTime.Now);
            dados.Modulos = _moduloServicos.Listar(e => e.Evento.Organizador == _usuario && e.Data.Date >= DateTime.Now.Date && e.Ativo.Equals("S"));
            dados.Usuario = _usuario;
            dados.IdEvento = id.HasValue ? id.Value : 0;
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
            dados.Modulos = _moduloServicos.Listar(e => (e.Usuarios.Contains(_usuario) || e.Evento.Organizador == _usuario) && e.Data.Date >= DateTime.Now.Date && e.Ativo.Equals("S"));
            dados.Usuario = _usuario;
            return View(dados);
        }
        public ActionResult ListarMeusModulos()
        {
            var dados = new DadosModulo();
            dados.Modulos = _moduloServicos.Listar(e => e.Evento.Organizador == _usuario && e.Data.Date >= DateTime.Now.Date && e.Ativo.Equals("S"));
            return View(dados);
        }
        public ActionResult ListarModulosAntigos()
        {
            var dados = new DadosModulo();
            dados.Modulos =
                _moduloServicos.Listar(
                    e => (e.Usuarios.Contains(_usuario) || e.Evento.Organizador == _usuario) && e.Data.Date < DateTime.Now.Date);
            return View(dados);
        }
        public ActionResult AlterarModulo(int id)
        {
            var dadosModulo = new DadosModulo();
            dadosModulo.Modulo = _moduloServicos.ObterPorId(id);
            dadosModulo.Eventos = _eventoServicos.Listar(e => e.Organizador == _usuario && e.Fim >= DateTime.Now);
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
            modulo.Evento = _eventoServicos.ObterPorId(Convert.ToInt16(f["Evento"]));

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
        public JsonResult AceitarConvite(int id)
        {

            var modulo = _moduloServicos.ObterPorId(id);
            if (modulo.Usuarios.Count < modulo.Vagas)
            {
                var modulosDoDia =
                        _moduloServicos.Listar(e => e.Usuarios.Contains(_usuario) && e.Data.Date == DateTime.Now.Date);
                InscricaoModulo moduloDaHora = new InscricaoModulo();
                foreach (var i in modulosDoDia)
                {
                    var dezMinutos = DateTime.Parse(i.Inicio).AddMinutes(-10);
                    var maisDezMinutos = DateTime.Parse(i.Fim).AddMinutes(10);
                    if (DateTime.Parse(modulo.Inicio).TimeOfDay >= dezMinutos.TimeOfDay && DateTime.Parse(modulo.Fim).TimeOfDay <= maisDezMinutos.TimeOfDay)
                    {
                        return Json("Existente", JsonRequestBehavior.AllowGet);
                    }
                } 
                //Logica que salva.
                var usuario = _usuario;
                usuario.Modulos.Remove(modulo);
                _usuarioServicos.Cadastrar(usuario);
                var confirmaInscricao =
                    _inscricaoModuloServicos.ObterPorFiltro(e => e.Usuario == usuario && e.Modulo == modulo) ??
                    new InscricaoModulo();
                confirmaInscricao.Usuario = usuario;
                confirmaInscricao.Modulo = modulo;
                _inscricaoModuloServicos.Cadastrar(confirmaInscricao);

                return Json("Aceito", JsonRequestBehavior.AllowGet);
            }
            return Json("Cheio", JsonRequestBehavior.AllowGet);

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
            var result = new { dataInicio = dataInicioEvento.ToShortDateString(), dataFim = dataFimEvento.ToShortDateString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void SairModulo(int id)
        {
            var inscricao = _moduloServicos.ObterPorId(id);
            var convite = _usuarioServicos.ObterPorId(id);
            var usuario = _usuario;
            inscricao.Usuarios.Remove(usuario);
            _moduloServicos.Cadastrar(inscricao);
            usuario.Modulos.Add(inscricao);
            _usuarioServicos.Cadastrar(usuario);
        }
    }
}

