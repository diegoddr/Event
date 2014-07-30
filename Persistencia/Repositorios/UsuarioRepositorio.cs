using System.Linq;
using Dominio.Entidades;
using Dominio.IRepositorios;
using NHibernate.Linq;

namespace Persistencia.Repositorios
{
    public class UsuarioRepositorio : BaseRepositorio<Usuario>, IUsuarioRepositorio
    {
        public Usuario ObterPorLogin(string login)
        {
            return this.Sessao.Query<Usuario>().FirstOrDefault(e => e.Login == login);
        }
    }
}