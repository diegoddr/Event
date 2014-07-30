using System;
using System.Collections.Generic;
using Dominio.Entidade;

namespace Dominio.Entidades
{
    public class Usuario : EntidadeBase
    {
        public virtual int Cracha { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Login { get; set; }
        public virtual string Senha { get; set; }
        public virtual string Email { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string Sexo { get; set; }
        public virtual DateTime Nascimento { get; set; }
        public virtual IList<Modulo> Modulos { get; set; }

        public Usuario()
        {
            Modulos = new List<Modulo>();
        }
    }
}
