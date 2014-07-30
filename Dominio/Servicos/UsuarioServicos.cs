using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominio.Entidades;
using Dominio.IRepositorios;

namespace Dominio.Servicos
{
    public class UsuarioServicos
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioServicos(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public void Cadastrar(Usuario usuario)
        {
            _usuarioRepositorio.Armazenar(usuario);
        }

        public void Excluir(Usuario usuario)
        {
            _usuarioRepositorio.Remover(usuario);
        }

        public Usuario ObterPorId(int id)
        {
            return _usuarioRepositorio.ObterPorId(id);
        }

        public IList<Usuario> Listar(Expression<Func<Usuario, bool>> filtro)
        {
            return _usuarioRepositorio.Listar(filtro);
        }

        public Usuario ObterPorLogin(string login)
        {
            return _usuarioRepositorio.ObterPorLogin(login);
        }

    }


}
