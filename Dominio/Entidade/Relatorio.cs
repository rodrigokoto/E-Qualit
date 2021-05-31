using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidade
{
    public class Relatorio
    {
        [Key]
        public int IdRelatorio { get; set; }
        public string Nome { get; set; }
        public string Url { get; set; }

        public int IdFuncionalidade { get; set; }

        public Dictionary<string , string> Parametros { get; set; }

        public virtual Funcionalidade Funcionalidade { get; set; }
    }
}
