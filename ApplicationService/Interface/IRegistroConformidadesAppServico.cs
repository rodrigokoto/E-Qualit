using Dominio.Entidade;
using Dominio.Enumerado;
using System;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IRegistroConformidadesAppServico : IBaseServico<RegistroConformidade>
    {
        RegistroConformidade SalvarPrimeiraEtapa(RegistroConformidade registroConformidade);

        RegistroConformidade SalvarSegundaEtapa(RegistroConformidade registroConformidade, Funcionalidades funcionalidade);
        
        RegistroConformidade ObtemAcaoConformidadePorNaoConformidade(RegistroConformidade naoConformidade);

        List<RegistroConformidade> ObtemListaRegistroConformidadePorSite(int idSite, string tipoRegistro, ref int numeroUltimoRegistro, int ? idProcesso = null);
        void GestaoDeRiscoValida(RegistroConformidade gestaoDeRisco, ref List<string> erros);
        RegistroConformidade SalvarGestaoDeRisco(RegistroConformidade gestaoDeRisco);

        DateTime ObtemUltimaDataEmissao(int idSite, string tipoRegistro);

        RegistroConformidade DestravarEtapa(RegistroConformidade registroConformidade);

        RegistroConformidade CriarAcaoCorretivaApartirDeNaoConformidade(RegistroConformidade naoConformidade);
        void CarregarArquivosNaoConformidadeAnexos2(RegistroConformidade naoConformidade, bool carregarArquivosDeEvidenciaAux);
    }
}
