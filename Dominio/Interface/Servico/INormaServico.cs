using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface INormaServico 
    {
        void Valido(Norma norma, ref List<string> erros);
    }
}
