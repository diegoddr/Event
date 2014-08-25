
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominio.Entidades;
using Dominio.IRepositorios;

namespace Dominio.Servicos
{
    public class InscricaoModuloServicos
    {
        private readonly IInscricaoModuloRepositorio _inscricaoModuloRepositorio;

        public InscricaoModuloServicos(IInscricaoModuloRepositorio incricaoModuloRepositorio)
        {
            _inscricaoModuloRepositorio = incricaoModuloRepositorio;
        }

        public void Cadastrar(InscricaoModulo inscricaomodulo)
        {
            _inscricaoModuloRepositorio.Armazenar(inscricaomodulo);
        }

        public void Excluir(InscricaoModulo inscricaomodulo)
        {
            _inscricaoModuloRepositorio.Remover(inscricaomodulo);
        }

        public InscricaoModulo ObterPorId(int id)
        {
            return _inscricaoModuloRepositorio.ObterPorId(id);
        }

        public IList<InscricaoModulo> Listar(Expression<Func<InscricaoModulo, bool>> filtro)
        {
            return _inscricaoModuloRepositorio.Listar(filtro);
        }
        public InscricaoModulo ObterPorFiltro(Expression<Func<InscricaoModulo, bool>> filtro)
        {
            return _inscricaoModuloRepositorio.ObterPorFiltro(filtro);
        }
    }
}
