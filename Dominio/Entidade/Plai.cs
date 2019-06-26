using System;
using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class Plai
    {
        public int IdPlai { get; set; }
        public int IdPai { get; set; }
        public int IdRepresentanteDaDirecao { get; set; }
        public int IdElaborador { get; set; }


        public int Mes { get; set; }

        public bool Agendado { get; set; }
        public bool Bloqueado { get; set; }

        public bool EnviouEmail { get; set; }

        //aux
        public bool TrocouMes { get; set; }


        public DateTime DataReuniaoAbertura { get; set; }
        public DateTime DataReuniaoEncerramento { get; set; }

        public DateTime? DataAlteracao { get; set; }
        public DateTime DataCadastro { get; set; }

        #region Relacionamento
        public virtual ICollection<ArquivoPlaiAnexo> ArquivoPlai { get; set; }
        public virtual Pai Pai { get; set; }
        public virtual List<PlaiProcessoNorma> PlaiProcessoNorma { get; set; }
        public virtual List<PlaiGerentes> PlaiGerentes { get; set; }
        
        public virtual Usuario Elaborador { get; set; }
        #endregion
    }
}
