using Dominio.Entidade;

namespace TestProject.Extentions
{
    public static class CriterioAvaliacaoExtention
    {
        public static CriterioAvaliacao Criar(this CriterioAvaliacao qualificaAvaliacaoCriticidade)
        {
            qualificaAvaliacaoCriticidade = new CriterioAvaliacao
            {
                Titulo = "Prazo",
                IdProduto = 1,

            };

            return qualificaAvaliacaoCriticidade;
        }

        public static CriterioAvaliacao First(this CriterioAvaliacao qualificaAvaliacaoCriticidade)
        {
            qualificaAvaliacaoCriticidade = new CriterioAvaliacao
            {
                Titulo = "Prazo",
                IdProduto = 1,
                IdCriterioAvaliacao = 1,

            };

            return qualificaAvaliacaoCriticidade;
        }
    }
}
