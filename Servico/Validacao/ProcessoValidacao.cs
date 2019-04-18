using Entidade;
using System.Collections.Generic;

namespace Servico.Validacao
{
    public class ProcessoValidacao
    {

        public bool PossuiProcesso(List<Processo> processos)
        {
            if (processos == null || processos.Count == 0)
            {
                return false;
            }
            return true;
        }

        public bool PossuiPermissaoProcesso(List<PermissaoProcesso> permissaoProcessos)
        {
            if (permissaoProcessos == null || permissaoProcessos.Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}
