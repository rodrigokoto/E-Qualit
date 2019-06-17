using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidade
{
    public class DocIndicadores
    {
        public int IdIndicadores { get; set; }
        public int IdDocumento { get; set; }


        public string Objetivo { get; set; }
        public long IdResponsavel { get; set; }
        public decimal IndicadoresMeta { get; set; }
        public string Indicadores { get; set; }
        public string IndicadoresUnidadeMeta { get; set; }
        public bool IndicadoresMetaMaximaMinima { get; set; }
        [NotMapped]
        public string ResponsavelNomeCompleto { get; set; }

        public virtual DocDocumento Documento { get; set; }


    }
}
