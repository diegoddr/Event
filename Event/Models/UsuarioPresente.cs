using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dominio.Entidades;

namespace Event.Models
{
    public class UsuarioPresente
    {
        public string Nome { get; set; }
        public string HoraEntrada { get; set; }
        public string HoraSaida { get; set; }

    }
}