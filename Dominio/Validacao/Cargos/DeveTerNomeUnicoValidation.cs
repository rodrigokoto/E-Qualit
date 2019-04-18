using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.Cargos;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.Cargos
{
    public class DeveTerNomeUnicoValidation : Validator<Cargo>
    {
        private readonly ICargoRepositorio _cargoRepositorio;
        public DeveTerNomeUnicoValidation(ICargoRepositorio cargoRepositorio)
        {
            _cargoRepositorio = cargoRepositorio;
           
            var deveTerNomeUnica = new DeveTerNomeUnicoPorSiteSpecification(_cargoRepositorio);

            base.Add(Traducao.Resource.MsgErroCadastro, new Rule<Cargo>(deveTerNomeUnica, Traducao.Resource.MsgErroNomeUnico));
        }
    }
}
