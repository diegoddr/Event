using System;
using System.Linq.Expressions;
using Dominio.Entidade;

namespace Dominio.Repositorio
{
    public interface IBaseRepositorio<T>
        where T : EntidadeBase
    {
        bool Armazenar(T entidade);
        System.Collections.Generic.IList<T> Listar(System.Linq.Expressions.Expression<Func<T, bool>> filtro);
        T ObterPorId(int id);
        bool Remover(T entidade);
        T ObterPorFiltro(Expression<Func<T, bool>> filtro);
    }
}
