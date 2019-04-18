using Dominio.Entidade;
using System;

namespace TestProject.Extentions
{
    public static class CriterioQualificacaoExtention
    {
        public static CriterioQualificacao Criar(this CriterioQualificacao criterioQualificacao)
        {
            criterioQualificacao = new CriterioQualificacao
            {
                IdProduto = 1,
                Titulo = "Certificado de Homologação do INMETRO",
                DtCriacao = DateTime.Now,
                DtAlteracao = DateTime.Now,
                TemControleVencimento = false            
            };

            return criterioQualificacao;
        }

        public static CriterioQualificacao First(this CriterioQualificacao criterioQualificacao)
        {
            criterioQualificacao = new CriterioQualificacao
            {
                IdCriterioQualificacao = 1,
                IdProduto = 1,
                DtCriacao = DateTime.Now,
                DtAlteracao = DateTime.Now,
                TemControleVencimento = false,
                Titulo = "Certificado de Homologação do INMETRO",
            };

            return criterioQualificacao;
        }
    }
}
