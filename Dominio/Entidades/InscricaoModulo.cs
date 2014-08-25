using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entidade;

namespace Dominio.Entidades
{
    public class InscricaoModulo : EntidadeBase
    {
        public virtual Usuario Usuario { get; set; }
        public virtual Modulo Modulo { get; set; }
        public virtual string Entrada { get; set; }
        public virtual string Saida { get; set; }
    }
}
