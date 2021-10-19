using Dominio.Entidade;

namespace Dominio.Interface.Repositorio
{
    public interface IAnaliseCriticaRepositorio : IBaseRepositorio<AnaliseCritica>
    {
        void SalvarAnaliseCritica(AnaliseCritica analiseCritica);
        void AtualizaAnaliseCriticaTema(AnaliseCritica analiseCritica);
        void AtualizaAnaliseCriticaFuncionario(AnaliseCritica analiseCritica);
        void AtualizaAnaliseCritica(AnaliseCritica analiseCritica);
    }
}
