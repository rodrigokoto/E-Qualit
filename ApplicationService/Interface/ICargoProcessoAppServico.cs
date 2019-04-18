using Dominio.Entidade;

namespace ApplicationService.Interface
{
    public interface ICargoProcessoAppServico : IBaseServico<CargoProcesso>
    {
        bool PossuiAcessoAFuncao(int idUsuarioLogado, int idFuncao, int ? idProcessoSelecionado = null);
    }
}
