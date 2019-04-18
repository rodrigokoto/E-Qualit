using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ClienteMap : EntityTypeConfiguration<Cliente>
    {
        public ClienteMap()
        {
            Ignore(x => x.ContratosAux);

            HasKey(t => t.IdCliente);

            Ignore(x => x.ValidationResult);
            Ignore(x => x.Site);
            Ignore(x => x.Usuario);
            //Ignore(x => x.ArquivoB64);
            //Ignore(x => x.ArquivoContratoB64);

            Property(t => t.NmFantasia)
                .HasMaxLength(60)
                .IsRequired();

            //Property(t => t.NmLogo);

            //Property(t => t.NmAquivoContrato);

            //Property(t => t.TipoConteudoContrato);
            
            Property(t => t.NmUrlAcesso)
                .HasMaxLength(60)
                .IsRequired();

            Property(t => t.DtValidadeContrato)
                .IsRequired();
            

            Property(t => t.NuDiasTrocaSenha);

            Property(t => t.NuTentativaBloqueioLogin);

            Property(t => t.NuArmazenaSenha);

            Property(t => t.FlExigeSenhaForte)
                .IsRequired();

            Property(t => t.FlTemCaptcha)
                .IsRequired();

            Property(t => t.FlEstruturaAprovador);

            Property(t => t.FlAtivo)
                .IsRequired();

            Property(t => t.IdUsuarioIncluiu);

            Property(t => t.DtInclusao);

            //Property(t => t.TipoConteudo);

            //Property(t => t.Arquivo);

            //Property(t => t.ArquivoContrato);
        }
    }
}
