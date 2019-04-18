using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.DocDocumentos;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.DocDocumentos
{
    public class AptoParaCadastroAtualizarValidation : Validator<DocDocumento>
    {
        private readonly IDocUsuarioVerificaAprovaRepositorio _docUsuarioVerificaAprovaRepositorio;

        public AptoParaCadastroAtualizarValidation(IDocUsuarioVerificaAprovaRepositorio docUsuarioVerificaAprovaRepositorio)
        {
            _docUsuarioVerificaAprovaRepositorio = docUsuarioVerificaAprovaRepositorio;


            //var possuiElaboracao = new DeveSerElaboracaoEspecification();
            var possuiWorkFlowAprovador = new PossuiWorkFlowDeveTerAprovadoresEspecification();
            var possuiWorkFlowVerificador = new PossuiWorkFlowDeveTerVerificadoresEspecification();

            //base.Add("Erro no Status Colaboração", new Rule<DocDocumento>(possuiElaboracao, "E necessário o status estar como colaboração."));
            base.Add(Traducao.Resource.MsgErroAprovador, new Rule<DocDocumento>(possuiWorkFlowAprovador, Traducao.Resource.MsgErroInfAprovador));
            base.Add(Traducao.Resource.MsgErroVerificador, new Rule<DocDocumento>(possuiWorkFlowVerificador, Traducao.Resource.MsgErroInfVerificador));
        }
    }
}
