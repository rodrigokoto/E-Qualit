using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface ILoginServico 
    {
        void ValidoParaLogar(Usuario usuario, ref List<string> erros);
        void ValidoParaAcessar(Usuario usuario, ref List<string> erros);
        void ValidoParaEsqueciSenha(Usuario usuario, ref List<string> erros);
        void ValidoParaAlterarSenhaViaEmail(Usuario usuario, ref List<string> erros);
    }
}
