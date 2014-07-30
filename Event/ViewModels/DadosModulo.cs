using System.Collections.Generic;
using Dominio.Entidades;

namespace Event.ViewModels
{
    public class DadosModulo
    {
        public IList<Evento> Eventos { get; set; }
        public IList<Modulo> Modulos { get; set; }
        public Modulo Modulo { get; set; }
        public IList<Usuario> Usuarios { get; set; }
        public Usuario Usuario { get; set; }
    }
}