using Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EntityConfig
{
    public class ClienteContratoMap : EntityTypeConfiguration<ClienteContrato>
    {
        public ClienteContratoMap()
        {
            HasKey(x => x.IdClienteContrato);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.IdCliente)
                .IsRequired();

            #region Relacionamento

            HasRequired(s => s.Cliente)
                .WithMany(x => x.Contratos)
                .HasForeignKey(s => s.IdCliente)
                .WillCascadeOnDelete(true);

            HasRequired(s => s.Contrato)
                .WithMany(s => s.ClientesContratos)
                .HasForeignKey(s => s.IdAnexo)
                .WillCascadeOnDelete(true);

            #endregion
        }
    }
}
