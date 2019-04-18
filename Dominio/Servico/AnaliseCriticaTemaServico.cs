using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.AnaliseCriticas;
using Dominio.Validacao.RegistroConformidades.GestaoDeRiscos;
using System.Collections.Generic;

namespace Dominio.Servico
{
    public class AnaliseCriticaTemaServico : IAnaliseCriticaTemaServico
    {
        private readonly IAnaliseCriticaTemaRepositorio _analiseCriticaTemaRepositorio;
        private readonly ILogRepositorio _logRepositorio;

        public AnaliseCriticaTemaServico(IAnaliseCriticaTemaRepositorio analiseCriticaTemaRepositorio,
                                         ILogRepositorio logRepositorio)
        {
            _analiseCriticaTemaRepositorio = analiseCriticaTemaRepositorio;
            _logRepositorio = logRepositorio;
        }

        private void Valido(AnaliseCriticaTema tema, ref List<string> erros)
        {
            if (GestaoDeRiscoDeveTerCriticidadeMediaOuAltaValidation.PossuiGestaoDeRisco(tema.GestaoDeRisco.CriticidadeGestaoDeRisco.Value, tema.PossuiGestaoRisco))
            {
                tema.ValidationResult = new AptoParaCadastroComGestaoDeRiscoValidacao()
                                                       .Validate(tema.GestaoDeRisco);

                if (!tema.ValidationResult.IsValid)
                {
                    erros.AddRange(UtilsServico.PopularErros(tema.ValidationResult));
                }
            }
        }

        public void Remove(List<AnaliseCriticaTema> temas)
        {

            foreach (var tema in temas)
            {
                _analiseCriticaTemaRepositorio.Remove(tema);
            }
        }
    }
}
