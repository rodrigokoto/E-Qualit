using Dominio.Enumerado;
using System;
using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class RegistroAcaoImediata
    {
        public RegistroAcaoImediata()
        {
            DtInclusao = DateTime.Now;
            ArquivoEvidencia = new List<ArquivoDeEvidenciaAcaoImediata>();
        }

        public int IdAcaoImediata { get; set; }
        
        public DateTime? DtPrazoImplementacao { get; set; }

        public DateTime? DtEfetivaImplementacao { get; set; }

        public string Descricao { get; set; }


        public int? IdResponsavelImplementar { get; set; }

        public int? IdUsuarioIncluiu { get; set; }

        public DateTime DtInclusao { get; set; }
                
        public int? IdRegistroConformidade { get; set; }

        public EstadoObjetoEF Estado { get; set; }

        public bool? Aprovado { get; set; }

        public Anexo ArquivoEvidenciaAux { get; set; }

        public virtual ICollection<ArquivoDeEvidenciaAcaoImediata> ArquivoEvidencia { get; set; }

        public virtual Usuario ResponsavelImplementar { get; set; }
        public virtual RegistroConformidade Registro { get; set; }

    }
}
