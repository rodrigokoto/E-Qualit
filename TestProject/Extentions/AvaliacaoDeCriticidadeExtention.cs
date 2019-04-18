using Dominio.Entidade;

namespace TestProject.Extentions
{
    public static class AvaliacaoDeCriticidadeExtention
    {
        public static AvaliacaoCriticidade Criar(this AvaliacaoCriticidade avaliacaoDeCriticidade)
        {
            return avaliacaoDeCriticidade = new AvaliacaoCriticidade
            {
                Titulo = "Qualificado",             
            };
        }

        public static AvaliacaoCriticidade First(this AvaliacaoCriticidade avaliacaoDeCriticidade)
        {
            return avaliacaoDeCriticidade = new AvaliacaoCriticidade
            {
                Titulo = "Qualificado",
                IdProduto = 1,
                IdAvaliacaoCriticidade = 1,                
            };
        }
    }
}
