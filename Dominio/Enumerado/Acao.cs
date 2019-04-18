using System.ComponentModel;

namespace Dominio.Enumerado
{
    public enum Acao
    {
        [Description("Efetuar Login do usuário")]
        LoginDoCliente = 1,
        [Description("Cadastrar usuário")]
        RegistrarUsuario = 2,
        [Description("Efetuar login")]
        Login = 3,
        [Description("Esqueci senha")]
        EsqueceuSenha = 4,
        [Description("Atualizar Email Enviados")]
        AtualizarEmailEnviado = 5,
        [Description("Remover Analise Critica Tema")]
        RemoverAnaliseCritica = 6,
        [Description("Lista Processos Por Usuario")]
        ListaProcessosPorUsuario = 7,
    }
}
