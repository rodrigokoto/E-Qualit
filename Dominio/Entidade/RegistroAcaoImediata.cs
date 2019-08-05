using Dominio.Enumerado;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entidade
{
    public class RegistroAcaoImediata
    {
        public RegistroAcaoImediata()
        {
            DtInclusao = DateTime.Now;
            ArquivoEvidencia = new List<ArquivoDeEvidenciaAcaoImediata>();
            ComentariosAcaoImediata = new List<ComentarioAcaoImediata>();
        }

        public int IdAcaoImediata { get; set; }

        public DateTime? DtPrazoImplementacao { get; set; }

        public DateTime? DtEfetivaImplementacao { get; set; }

        public string Descricao { get; set; }

        public string Observacao { get; set; }


        public int? IdResponsavelImplementar { get; set; }

        public int? IdUsuarioIncluiu { get; set; }

        public DateTime DtInclusao { get; set; }

        public int? IdRegistroConformidade { get; set; }

        public EstadoObjetoEF Estado { get; set; }

        public bool? Aprovado { get; set; }

        public Anexo ArquivoEvidenciaAux { get; set; }

        public long? IdFilaEnvio { get; set; }
        [NotMapped]
        public string Motivo { get; set; }
        [NotMapped]
        public string Orientacao { get; set; }

        public virtual ICollection<ArquivoNaoConformidadeAnexo> ArquvioAcaoCorretivaAnexo { get; set; }
        public virtual ICollection<ArquivoDeEvidenciaAcaoImediata> ArquivoEvidencia { get; set; }

        public virtual Usuario ResponsavelImplementar { get; set; }
        public virtual RegistroConformidade Registro { get; set; }

        public virtual List<ComentarioAcaoImediata> ComentariosAcaoImediata { get; set; }
        //public int IdComentarioAcaoImediata { get; set; }
    }
}