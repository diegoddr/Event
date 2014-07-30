using System;
using System.Collections.Generic;
using Dominio.Entidade;

namespace Dominio.Entidades
{
    public class Evento : EntidadeBase
    {
        public virtual string Nome { get; set; }
        public virtual string Descricao { get; set; }
        public virtual DateTime Inicio { get; set; }
        public virtual DateTime Fim { get; set; }
        public virtual Usuario Organizador { get; set; }
        public virtual string Ativo { get; set; }
        public virtual IList<Usuario> Usuarios { get; set; }

        public Evento()
        {
            Usuarios = new List<Usuario>();
        }
    }
}
