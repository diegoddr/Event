using Dominio.Entidades;
using Dominio.Repositorio;

namespace Dominio.IRepositorios
{
    public interface IUsuarioRepositorio : IBaseRepositorio<Usuario>
    {
        Usuario ObterPorLogin(string login);
    }
}
