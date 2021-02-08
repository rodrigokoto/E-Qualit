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
        [Display(Name = "Fornecedor")]
        [Description("Fornecedor")]
        Fornecedor = 1,
        [Display(Name = "Fabricante")]
        [Description("Fabricante")]
        Fabricante = 2,
        [Display(Name = "Fabricante/Fornecedor")]
        [Description("Fabricante/Fornecedor")]
        FabricanteFornecedor = 3
    }
}
