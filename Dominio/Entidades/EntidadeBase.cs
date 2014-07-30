namespace Dominio.Entidade
{
    public abstract class EntidadeBase
    {
        public virtual int Id { get; set; }
        public virtual string Status { get; set; }
    }
}