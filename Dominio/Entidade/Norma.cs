using System;
using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class Norma
    {
        public int IdNorma { get; set; }
        public int ? Numero { get; set; }
        public int IdSite { get; set; }
        public int IdUsuarioIncluiu { get; set; }
        public virtual List<NormaProcesso> Processos { get; set; }
        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
