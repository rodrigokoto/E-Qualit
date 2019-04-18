
using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IPlaiServico 
    {
        void Valido(Plai plai, ref List<string> erros);
        List<Plai> ObterPorPai(int idPai);
        void InsereMensagemPlaisVencidos();
        void Atualizar(Plai plai);
    }
}
