using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.Instrumentos;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.Instrumentos
{
    public class AptoParaEditarInstrumentoValidation : Validator<Instrumento>
    {
        public AptoParaEditarInstrumentoValidation(IInstrumentoRepositorio repositorio)
        {
            var devePossuirNomeUnicoPorModelo = new DevePossuirNomeUnicoPorModeloSpecification(repositorio);
            //var devePossuirNumeroUnicoPorModelo = new DevePossuirNumeroUnicoPorModeloSpecification(repositorio);

            base.Add(Traducao.Resource.NomeEquipamento, new Rule<Instrumento>(devePossuirNomeUnicoPorModelo, Traducao.Instrumentos.ResourceInstrumentos.IN_Valida_Nome));
            //base.Add("Numero do Equipamento", new Rule<Instrumento>(devePossuirNumeroUnicoPorModelo, Traducao.Instrumentos.ResourceInstrumentos.IN_Valida_Numero));

        }
    }
}
