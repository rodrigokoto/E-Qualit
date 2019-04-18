using Dominio.Entidade;
using System;

namespace TestProject.Extentions
{
    public static class ProdutoExtention
    {
        public static Produto CriaProdutoValido(this Produto prod)
        {
            prod = new Produto
            {
                Nome = "Desenvolvedor .Net",
                Especificacao = "O Desenvolvedor têm seus código 100% cobertos pelos teste",
                Tags = "Dev",
                IdResponsavel = 1,
                IdSite = 1,
                               

            };
            return prod;
        }

        public static Produto First(this Produto prod)
        {
            prod = new Produto
            {
                Nome = "Desenvolvedor .Net",
                Especificacao = "O Desenvolvedor têm seus código 100% cobertos pelos teste",
                Tags = "Dev",
                IdResponsavel = 2,
                IdSite = 5,
                IdProduto = 1


            };
            return prod;
        }

        public static AvaliacaoCriticidade CriarAvaliacaoCriticidade(this AvaliacaoCriticidade avaliacaoCriticidade)
        {
            avaliacaoCriticidade = new AvaliacaoCriticidade
            {
                DtAlteracao = DateTime.Now,
                DtCriacao = DateTime.Now,
                Titulo = "Nova Avaliação de Criticidade"
            };

            return avaliacaoCriticidade;
        }

    }
}
