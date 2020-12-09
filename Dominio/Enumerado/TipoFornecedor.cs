using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traducao;

namespace Dominio.Enumerado
{
    public enum TipoFornecedor
    {
        [Description("Fornecedor")]
        Fornecedor = 1,
        [Description("Fabricante")]
        Fabricante = 2 
    }
}
