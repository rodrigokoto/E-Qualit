using System;

namespace Servico
{
    public partial class Service
    {

        public void LoginEsqueciMinhaSenha(Entidade.EsqueciMinhaSenha login)
        {
            try
            {
                // valida se o usuário está ativo/válido
                Entidade.CtrlUsuario Usuario = CtrlUsuarioGetByIdentificacao(login.CdIdentificacao);
                if (Usuario == null)
                {
                    Sucesso = false;
                    Mensagem = Traducao.Resource.login_usuario_senha_invalido;
                }
                else
                {
                    if (!Usuario.FlAtivo)
                    {
                        // Usuário inativo
                        Sucesso = false;
                        Mensagem = Traducao.Resource.login_usuario_senha_invalido;
                    }
                    else if (Usuario.FlBloqueado)
                    {
                        // Usuario bloqueado
                        Sucesso = false;
                        Mensagem = Traducao.Resource.login_usuario_bloqueado;
                    }
                    else if (Usuario.FlCompartilhado)
                    {
                        // Usuario compartilhado
                        Sucesso = false;
                        Mensagem = Traducao.Resource.alterar_senha_usuario_compartilhado;
                    }
                    else
                    {
                        // Troca de senha
                        string email = CtrlUsuarioRedefinirSenha(login.CdIdentificacao);
                        string[] _email = email.Split('@');

                        if (email.Length > 0)
                        {
                            email = "xxxxxxxxxxx@" + _email[1];
                        }

                        if (Sucesso)
                        {
                            Mensagem = "OK";
                        }
                        else
                        {
                            Mensagem = Error.Message;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Sucesso = false;
                Mensagem = ex.Message;
            }
        }

        public void LoginTrocaSenha(Entidade.TrocaSenha login)
        {

        }
    }
}
