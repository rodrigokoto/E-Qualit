using System.Collections.Generic;

namespace Entidade
{
    public class PermissaoProcesso: BaseEntidade
   {
      public int IdProcesso { get; set; }
      public string NmProcesso { get; set; }
      public bool FlSelecionado { get; set; }

      public Cargo Cargo { get; set; }
      public List<PermissaoModulo> LvModulos { get; set; }

      public PermissaoProcesso()
      {
         LvModulos = new List<PermissaoModulo>();
      }

   }
}
