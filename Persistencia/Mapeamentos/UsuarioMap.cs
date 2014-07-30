using Dominio.Entidades;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Persistencia.Mapeamentos
{
    public class UsuarioMap : ClassMapping<Usuario>
    {
        public UsuarioMap()
        {
            Table("USUARIO");
            Id(p => p.Id, m =>
            {
                m.Column("ID");
                m.Generator(Generators.Identity);
            });
            Property(p => p.Cracha, m => m.Column("CRACHA"));
            Property(p => p.Nome, m => m.Column("NOME"));
            Property(p => p.Login, m => m.Column("LOGIN"));
            Property(p => p.Senha, m => m.Column("SENHA"));
            Property(p => p.Email, m => m.Column("EMAIL"));
            Property(p => p.Telefone, m => m.Column("TELEFONE"));
            Property(p => p.Sexo, m => m.Column("SEXO"));
            Property(p => p.Nascimento, m => m.Column("nascimento"));

            Bag<Modulo>(p => p.Modulos, m =>
            {
                m.Table("conviteModulo");
                m.Cascade(Cascade.None);
                m.Lazy(CollectionLazy.Lazy);
                m.Fetch(CollectionFetchMode.Join);
                m.Key(k => k.Column("idUsuario"));
            }, map => map.ManyToMany(p => p.Column("idModulo")));
        }
    }
}

