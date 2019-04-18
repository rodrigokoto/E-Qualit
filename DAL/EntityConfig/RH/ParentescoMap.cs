using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class ParentescoMap : EntityTypeConfiguration<Parentesco>
    {
        public ParentescoMap()
        {
            HasKey(x => x.Codigo);

            Property(x => x.Descricao)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();

        }
    }
}
