using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dominio.Entidade;
using Dominio.Repositorio;
using NHibernate;
using NHibernate.Linq;

namespace Persistencia
{
    public abstract class BaseRepositorio<T> : IBaseRepositorio<T> where T : EntidadeBase
    {
        protected ISession Sessao
        {
            get { return NHibernateHelper.GetCurrentSession(); }
        }

        //Salva ou Atualiza
        public bool Armazenar(T entidade)
        {
            try
            {
                //this.Sessão.SaveOrUpdate(entidade);
                this.Sessao.Merge(entidade);
                return true;
            }
            catch (Exception e)
            {
                this.Sessao.Transaction.Rollback();
                return false;
            }
        }

        public bool Remover(T entidade)
        {
            try
            {
                Sessao.Delete(entidade);
                return true;
            }
            catch (Exception e)
            {
                Sessao.Transaction.Rollback();
                return false;
            }
        }

        public IList<T> Listar(Expression<Func<T, bool>> filtro)
        {
            return this.Sessao.Query<T>().Where(filtro).ToList();
        }

        public T ObterPorId(int id)
        {
            return this.Sessao.Query<T>().FirstOrDefault(e => e.Id == id);
        }

        public T ObterPorFiltro(Expression<Func<T, bool>> filtro)
        {
            return this.Sessao.Query<T>().FirstOrDefault(filtro);
        }
    }
}
