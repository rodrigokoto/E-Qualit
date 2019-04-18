using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class PaiMap : EntityTypeConfiguration<Pai>
    {
        public PaiMap()
        {
            ToTable("Pai");

            HasKey(x => x.IdPai);

            Ignore(x => x.Plais);
            Ignore(x => x.Processos);

            Property(x => x.Ano)
                .IsRequired()
                .HasColumnName("Ano");

            Property(x => x.DataCadastro)
                .IsRequired()
                .HasColumnName("DataCadastro");

            Property(x => x.IdGestor)
                .IsRequired()
                .HasColumnName("IdGestor");
            
            HasRequired(x => x.Usuario)
              .WithMany()
              .HasForeignKey(x => x.IdGestor);

            HasRequired(x => x.Site)
              .WithMany()
              .HasForeignKey(x => x.IdSite);
        }
    }
}
