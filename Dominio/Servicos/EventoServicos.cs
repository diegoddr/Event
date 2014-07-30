using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominio.Entidades;
using Dominio.IRepositorios;

namespace Dominio.Servicos
{
    public class EventoServicos
    {
        private readonly IEventoRepositorio _eventoRepositorio;

        public EventoServicos(IEventoRepositorio eventoRepositorio)
        {
            _eventoRepositorio = eventoRepositorio;
        }

        public void Cadastrar(Evento evento)
        {
            _eventoRepositorio.Armazenar(evento);
        }

        public void Excluir(Evento evento)
        {
            _eventoRepositorio.Remover(evento);
        }

        public Evento ObterPorId(int id)
        {
            return _eventoRepositorio.ObterPorId(id);
        }

        public IList<Evento> Listar(Expression<Func<Evento, bool>> filtro)
        {
            return _eventoRepositorio.Listar(filtro);
        }
    }
}

