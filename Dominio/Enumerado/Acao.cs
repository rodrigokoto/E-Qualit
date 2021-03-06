﻿using System.ComponentModel;

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
        [Description("Servico Mensageiro")]
        ServicoMensageiro = 8,
        [Description("Salvar")]
        Salvar = 9,
        [Description("Alterar")]
        Alterar = 10,
        [Description("Excluir")]
        Excluir = 11,
        [Description("Impressão")]
        Impressao = 12,

    }
}
