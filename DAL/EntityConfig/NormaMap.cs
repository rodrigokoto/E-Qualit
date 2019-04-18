using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class NormaMap : EntityTypeConfiguration<Norma>
    {
        public NormaMap()
        {
            HasKey(x => x.IdNorma);

            Property(x => x.Numero);

            Property(x => x.IdSite)
                .IsRequired();

            Property(x => x.IdUsuarioIncluiu)
                .IsRequired();

            Property(x => x.Codigo)
                .IsRequired();

            Property(x => x.Titulo)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.DataAlteracao);
        }
    }
}
