using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class RegistroGutMap : EntityTypeConfiguration<RegistroGut>
    {
        public RegistroGutMap()
        {
            ToTable("RegistroGut");


            HasKey(t => t.IdGut);

            Property(t => t.Gravidade)
                .HasColumnName("Gravidade");


            Property(t => t.Urgencia)
                .HasColumnName("Urgencia");


            Property(t => t.Tendencia)
                .HasColumnName("Tendencia");

            

            #region Relacionamentos


            #endregion

        }
    }
}
