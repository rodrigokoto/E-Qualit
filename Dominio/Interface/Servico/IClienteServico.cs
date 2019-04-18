
using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IClienteServico 
    {
        Cliente ObterPorUrl(string url);
        IEnumerable<Cliente> ObterClientesPorUsuario(int idUsuario);
        bool AtivarInativar(int id);
        bool Excluir(int id);
        void ValidaCriacao(Cliente cliente, ref List<string> erros);
        void ValidaEdicao(Cliente cliente, ref List<string> erros);
        Cliente ObjterClienteById(int IdCliente);
    }
}
