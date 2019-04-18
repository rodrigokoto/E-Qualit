using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class AnaliseCriticaTemaMap : EntityTypeConfiguration<AnaliseCriticaTema>
    {
        public AnaliseCriticaTemaMap()
        {
            ToTable("AnaliseCriticaTema");

            HasKey(x => x.IdTema);

            Ignore(x => x.ValidationResult);

            Property(x => x.IdTema)
                .IsRequired()
                .HasColumnName("IdTema");

            Property(x => x.Descricao)
                .IsRequired()
                .HasColumnName("Descricao");

            Property(x => x.PossuiGestaoRisco)
                 .IsRequired()
                 .HasColumnName("PossuiGestaoRisco");
            Property(x => x.CorRisco)
                .IsRequired();

            Property(x => x.IdControladorCategoria)
                .IsRequired()
                .HasColumnName("IdControladorCategoria");

            Property(x => x.IdGestaoDeRisco)
                .HasColumnName("IdRegistro")
                .IsOptional();

            Property(x => x.DataCadastro)
                .HasColumnName("DataCadastro");

            Property(x => x.Ativo)
                .HasColumnName("Ativo");

            HasRequired(X => X.ControladorCategoria)
                .WithMany()
                .HasForeignKey(x => x.IdControladorCategoria);

            HasOptional(X => X.GestaoDeRisco)
                .WithMany()
                .HasForeignKey(x => x.IdGestaoDeRisco);

            HasRequired(X => X.AnaliseCritica)
                .WithMany(x => x.Temas)
                .HasForeignKey(x => x.IdAnaliseCritica);
        }
    }
}
