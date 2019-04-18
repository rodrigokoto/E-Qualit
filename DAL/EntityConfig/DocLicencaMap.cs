//using Dominio.Entidade;
//using System.Data.Entity.ModelConfiguration;

//namespace DAL.EntityConfig
//{
//    public class DocLicencaMap : EntityTypeConfiguration<DocLicenca>
//    {
//        public DocLicencaMap()
//        {
//            HasKey(x => x.IdDocLicenca);

//            Property(x => x.IdAnexo)
//                .IsRequired();

//            Property(x => x.IdDocumento)
//                .IsRequired();

//            #region Relacionamento

//            HasRequired(s => s.Licenca)
//                .WithMany(s => s.Licencas)
//                .HasForeignKey(s => s.IdAnexo)
//                .WillCascadeOnDelete(true);

//            HasRequired(s => s.DocDocumento)
//                .WithMany(x => x.Licencas)
//                .HasForeignKey(s => s.IdDocumento)
//                .WillCascadeOnDelete(true);


//            #endregion
//        }
//    }
//}
