using Dominio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Persistencia.Mapeamentos
{
    public class IncricaoModuloMap : ClassMapping<InscricaoModulo>
    {
        public IncricaoModuloMap()
        {
            Table("inscricaoModulo");
            Id(p => p.Id, m =>
            {
                m.Column("ID");
                m.Generator(Generators.Identity);
            });
            this.ManyToOne(p => p.Modulo, m =>
            {
                m.Column("idModulo");
                m.Lazy(LazyRelation.Proxy);
                m.Cascade(Cascade.None);
            });
            this.ManyToOne(p => p.Usuario, m =>
            {
                m.Column("idUsuario");
                m.Lazy(LazyRelation.Proxy);
                m.Cascade(Cascade.None);
            });
            Property(p => p.Entrada, m => m.Column("entrada"));
            Property(p => p.Saida, m => m.Column("saida"));
        }
    }
}
