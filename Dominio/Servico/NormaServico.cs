using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.Normas;
using System.Collections.Generic;

namespace Dominio.Servico
{
    public class NormaServico : INormaServico
    {
        private readonly INormaRepositorio _normaRepositorio;

        public NormaServico(INormaRepositorio normaRepositorio)
        {
            _normaRepositorio = normaRepositorio;
        }

        public void Valido(Norma norma, ref List<string> erros)
        {
            var validacao = new AptoParaCadastroValidation().Validate(norma);

            if (!validacao.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validacao.Errors));
            }
        }
    }
}
