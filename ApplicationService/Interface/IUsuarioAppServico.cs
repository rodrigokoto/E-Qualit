using Dominio.Entidade;
using System;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IUsuarioAppServico : IBaseServico<Usuario>
    {
        List<Usuario> ObterFuncionalidadesPorUsuario(int idSite, int idFuncao, int idProcesso, int idCargo);
        List<Usuario> ObterUsuariosPorFuncionalidade(int idProcesso, int idSite, int idFuncionalidade);
        List<Usuario> ObterUsuariosPorFuncao(int ? idProcesso = null, int ? idSite = null, int ? idFuncao = null);
        List<Usuario> ObterUsuariosPorFuncao(int idSite, int idFuncao);
        bool PossuiAcesso(int idUsuario, int idModulo, int idFuncao);
        List<Usuario> ObterUsuariosPorPerfil(int idPerfil);
        void PodeAlterarSenha(Usuario entidade, ref List<string> erros);
        void AlterarSenha(int idUsuario, string novaSenha);
        void NovaSenhaRandomica(Usuario usuario, string novaSenha);
        Usuario ObterUsuarioPorCdIdentificacao(string cdIdentificacao);

        Usuario ObterUsuarioPorIdeSenha(string cdIdentificacao, string senha);
        List<Usuario> ObterUsuariosPorCargo(int idCargo);
        List<Funcionalidade> ObterFuncionalidadesPermitidas(int idUsuario);
        List<Funcionalidade> ObterFuncionalidadesPermitidasPorSite(int idUsuario);
        //List<Usuario> ObterUsuariosPorCliente(int idCliente);

        bool AtivarInativar(int idUsuario);
        bool BloqueiaDesbloqueia(int idUsuario);
        bool RecebeNaoRecebeEmail(int idUsuario);


        void AtualizarCadastro(Usuario usuario);
        void AtualizarMeusDados(Usuario usuario);
        void EnviaEmailEsqueciASenha(Usuario usuario, Guid token);
        void EnviaEmailNovoUsuario(Usuario usuario);
        List<Usuario> ObterUsuariosPorPerfilESite(int idSite, int idPerfil, int idPerfilLogado);



    }
}
