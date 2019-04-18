using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System.Collections.Generic;
using Dominio.Validacao.ControleImpressoes;

namespace Dominio.Servico
{
    public class ControleImpressaoServico : IControleImpressaoServico
    {
        private readonly IControleImpressaoRepositorio _controleImpressaoRepositorio;

        public ControleImpressaoServico(IControleImpressaoRepositorio controleImpressao) 
        {
            _controleImpressaoRepositorio = controleImpressao;
        }

        public void Valido(ControleImpressao controleImpressao, ref List<string> erros)
        {
            var camposObrigatorios = new CamposObrigatoriosValidation();

            var validacao = camposObrigatorios.Validate(controleImpressao);

            if (!validacao.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validacao.Errors));
            }
        }
    }
}
