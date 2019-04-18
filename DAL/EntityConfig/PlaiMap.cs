using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class PlaiMap : EntityTypeConfiguration<Plai>
    {
        public PlaiMap()
        {
            Ignore(x => x.TrocouMes);

            ToTable("Plai");

            HasKey(x => x.IdPlai);

            Property(x => x.IdPai)
                .IsRequired();

            //Property(x => x.IdArquivo)
            //    .IsOptional();

            //Property(x => x.IdElaborador)
            //    .IsRequired();

            Property(x => x.IdRepresentanteDaDirecao)
                .IsRequired();

            Property(x => x.Bloqueado)
                .IsRequired();

            Property(x => x.Agendado);

            Property(x => x.EnviouEmail);

            Property(x => x.Mes);
            
            Property(x => x.DataReuniaoAbertura)
                .IsRequired();

            Property(x => x.DataReuniaoEncerramento)
                .IsRequired();

            Property(x => x.DataAlteracao);

            Property(x => x.DataCadastro);

            #region Relacionamento

            HasRequired(x => x.Pai)
               .WithMany()
               .HasForeignKey(x => x.IdPai);

            //HasRequired(x => x.Elaborador)
            // .WithMany()
            // .HasForeignKey(x => x.IdPai);

            //HasOptional(x => x.Arquivo)
            //  .WithMany(y=>y.Plais)
            //  .HasForeignKey(x => x.IdArquivo);

            #endregion
        }
    }
}
