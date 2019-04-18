using System.Collections.Generic;

namespace Entidade
{
    public class PermissaoModulo:BaseEntidade
   {
      public int IdModulo { get; set; }
      public string NmModulo { get; set; }
      public bool FlSelecionado { get; set; }

      public List<Funcao> LvFuncao { get; set; }

      public PermissaoModulo()
      {
         LvFuncao = new List<Funcao>();
      }
   }
}
