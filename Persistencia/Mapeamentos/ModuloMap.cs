using Dominio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Persistencia.Mapeamentos
{
    public class ModuloMap : ClassMapping<Modulo>
    {
        public ModuloMap()
        {
            Table("Modulo");
            Id(p => p.Id, m =>
            {
                m.Column("ID");
                m.Generator(Generators.Identity);
            });
            Property(p => p.Nome, m => m.Column("NOME"));
            Property(p => p.Descricao, m => m.Column("DESCRICAO"));
            Property(p => p.Data, m => m.Column("DATA"));
            Property(p => p.Inicio, m => m.Column("INICIO"));
            Property(p => p.Fim, m => m.Column("FIM"));
            Property(p => p.Vagas, m => m.Column("VAGAS"));
            Property(p => p.Observacao, m => m.Column("OBSERVACAO"));
            Property(p => p.Ativo, m => m.Column("ATIVO"));
            ManyToOne(p => p.Evento, m =>
            {
                m.Column("EVENTO");
                m.Lazy(LazyRelation.Proxy);
                m.Fetch(FetchKind.Join);
                m.Cascade(Cascade.None);
            });
            Bag<Usuario>(p => p.Usuarios, m =>
            {
                m.Table("inscricaoModulo");
                m.Cascade(Cascade.None);
                m.Lazy(CollectionLazy.Lazy);
                m.Fetch(CollectionFetchMode.Join);
                m.Key(k => k.Column("idModulo"));
            }, map => map.ManyToMany(p => p.Column("idUsuario")));
        }
    }
}
