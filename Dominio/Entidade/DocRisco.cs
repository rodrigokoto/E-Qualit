using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidade
{
    public class DocRisco
    {
        public int IdDocRisco { get; set; }
        public int IdDocumento { get; set; }

        public bool PossuiGestaoRisco { get; set; }
        public int? CriticidadeGestaoDeRisco { get; set; } //enum.CriticidadeGestaoDeRisco
        public int? IdResponsavelInicarAcaoImediata { get; set; } // Por Adicionar Acoes Imediatas
        public string DescricaoRegistro { get; set; } //o que falhou?
        public string Causa { get; set; }
        //public int? IdRegistroConformidade { get; set; }
        public string DsJustificativa { get; set; }
        [NotMapped]
        public string ResponsavelInicarAcaoImediataNomeCompleto { get; set; }
        public virtual DocDocumento Documento { get; set; }

    }
}
