using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominio.Entidades;
using Dominio.IRepositorios;

namespace Dominio.Servicos
{
    public class ModuloServicos
    {
        private readonly IModuloRepositorio _moduloRepositorio;

        public ModuloServicos(IModuloRepositorio moduloRepositorio)
        {
            _moduloRepositorio = moduloRepositorio;
        }

        public void Cadastrar(Modulo modulo)
        {
            _moduloRepositorio.Armazenar(modulo);
        }

        public void Excluir(Modulo modulo)
        {
            _moduloRepositorio.Remover(modulo);
        }

        public Modulo ObterPorId(int id)
        {
            return _moduloRepositorio.ObterPorId(id);
        }

        public IList<Modulo> Listar(Expression<Func<Modulo, bool>> filtro)
        {
            return _moduloRepositorio.Listar(filtro);
        }
        public Modulo ObterPorFiltro(Expression<Func<Modulo, bool>> filtro)
        {
            return _moduloRepositorio.ObterPorFiltro(filtro);
        }
    }
}

