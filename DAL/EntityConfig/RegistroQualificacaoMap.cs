using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class RegistroQualificacaoMap : EntityTypeConfiguration<Dominio.Entidade.RegistroQualificacao>
    {
        public RegistroQualificacaoMap()
        {
            ToTable("RegistroQualificacao");

            HasKey(x => x.IdRegQualificacao);

            Property(x => x.GuidAvaliacao).IsRequired();

            Property(x => x.IdUsuarioAvaliacao).IsRequired();

            Property(x => x.IdFornecedor).IsRequired();

            Property(x => x.DtInclusao).IsRequired();

            Property(x => x.IdFilaEnvio).IsRequired();

            Property(x => x.IdUsuarioInclusao).IsRequired();


        }
    }
}
