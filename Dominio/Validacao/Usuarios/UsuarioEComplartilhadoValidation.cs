using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.Usuarios;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.Usuarios
{
    public class UsuarioEComplartilhadoValidation : Validator<Usuario>
    {
        public UsuarioEComplartilhadoValidation(IUsuarioRepositorio usuarioRepositorio)
        {
            var contaPodeNaoCompartilhada = new DeveTerContaNaoCompartilhada(usuarioRepositorio);

            base.Add(Traducao.Resource.ContaNaoPodeCompartilhar, new Rule<Usuario>(contaPodeNaoCompartilhada, Traducao.Login.ResourceLogin.RecuperaSenha_msg_Usuario_Compartilhado));
        }
    }
}
