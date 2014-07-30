using System;
using System.Collections.Generic;
using Dominio.Entidade;

namespace Dominio.Entidades
{
    public class Modulo : EntidadeBase
    {
        public virtual string Nome { get; set; }
        public virtual string Descricao { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual string Inicio { get; set; }
        public virtual string Fim { get; set; }
        public virtual int Vagas { get; set; }
        public virtual string Observacao { get; set; }
        public virtual Evento Evento { get; set; }
        public virtual string Ativo { get; set; }
        public virtual IList<Usuario> Usuarios { get; set; }

        public Modulo()
        {
            Usuarios = new List<Usuario>();
        }

    }
}
