using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IUsuarioServico
    {
        void Valido(Usuario usuario, ref List<string> erros);
        void ValidoAtualizarMeusDados(Usuario usuario, ref List<string> erros);
        bool PossuiAcesso(int idUsuario, int idModulo, int idFuncao);
        List<Usuario> ObterUsuariosPorPerfil(int idPerfil);
        void PodeAlterarSenha(Usuario entidade, ref List<string> erros);
        void AlterarSenha(int idUsuario, string novaSenha);
        Usuario ObterUsuarioPorCdIdentificacao(string cdIdentificacao);
        List<Usuario> ObterUsuariosPorCargo(int idCargo);
        List<Funcionalidade> ObterFuncionalidadesPermitidas(int idUsuario);
        bool Excluir(int id, int idUsuarioMigracao);
    }
}
