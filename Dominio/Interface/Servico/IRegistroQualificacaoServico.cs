using Dominio.Entidade;

namespace Dominio.Interface.Servico
{
    public interface IRegistroQualificacaoServico
    {
        void InserirEmail(RegistroQualificacao regQuali);
        void AtualizaEmail(RegistroQualificacao regQuali);
        void ExcluiEmail(RegistroQualificacao regQuali);
        RegistroQualificacao RetornaRegistro(int avaliaCriterioAvaliacao);
        
    }
}
