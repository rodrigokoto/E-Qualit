
using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IPlaiAppServico : IBaseServico<Plai>
    {
        
        List<Plai> ObterPorPai(int idPai);
        void InsereMensagemPlaisVencidos();
        void Atualizar(Plai plai);
    }
}
