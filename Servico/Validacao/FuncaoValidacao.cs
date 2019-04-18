using Entidade;
using System.Collections.Generic;

namespace Servico.Validacao
{
    public class FuncaoValidacao
    {
        public bool PossuiFuncao(List<Funcao> funcao)
        {
            if (funcao == null || funcao.Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}
