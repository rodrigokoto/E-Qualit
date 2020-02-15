using Dominio.Entidade;
using Dominio.Enumerado;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dominio.Interface.Servico
{
    public interface IRegistroConformidadesServico 
    {
        RegistroConformidade SalvarPrimeiraEtapa(RegistroConformidade registroConformidade);

        RegistroConformidade SalvarSegundaEtapa(RegistroConformidade registroConformidade, Funcionalidades funcionalidade);
        
        RegistroConformidade ObtemAcaoConformidadePorNaoConformidade(RegistroConformidade naoConformidade);

        List<RegistroConformidade> ObtemListaRegistroConformidadePorSite(int idSite, string tipoRegistro, ref int numeroUltimoRegistro);
        void GestaoDeRiscoValida(RegistroConformidade gestaoDeRisco, ref List<string> erros);
        RegistroConformidade SalvarGestaoDeRisco(RegistroConformidade gestaoDeRisco);

        DateTime ObtemUltimaDataEmissao(int idSite, string tipoRegistro);

        RegistroConformidade DestravarEtapa(RegistroConformidade registroConformidade);

        void ValidaNaoConformidade(RegistroConformidade naoConformidade, int idUsuarioLogado, ref List<string> erros);
        void ValidaGestaoDeRisco(RegistroConformidade gestaoDeRisco, int IdUsuario, ref List<string> erros);
        void ValidaAcaoCorretiva(RegistroConformidade acaoCorretiva, int IdUsuario, ref List<string> erros);
        void ValidaGestaoMelhoria(RegistroConformidade gestaoMelhoria, int IdUsuario, ref List<string> erros);

        RegistroConformidade CriarAcaoCorretivaApartirDeNaoConformidade(RegistroConformidade naoConformidade);

        void ValidarCampos(RegistroConformidade naoConformidade, ref List<string> erros);
        void ValidarCamposAcoesImediata(ICollection<RegistroAcaoImediata> acoesImediatas, RegistroConformidade registro, int idUsuarioLogado, ref List<string> erros);
        void ValidoParaExclusaoNaoConformidade(RegistroConformidade naoConformidade, ref List<string> erros);
        void ValidoParaExclusaoAcaoCorretiva(RegistroConformidade acaoCorretiva, ref List<string> erros);
        void ValidaDestravamento(int idPerfil, ref List<string> erros);
        void ValidaUsuarioPorSegundaEtapa(RegistroConformidade naoConformidade, Usuario usuarioLogado, ref List<string> erros);

        Int64 GeraProximoNumeroRegistro(string tipoRegistro, int idSite, int ? idProcesso = null);

        DataTable RetornarDadosGrafico(DateTime dtDe, DateTime dtAte, int? idTipoNaoConformidade, int? idCliente, int idSite, int tipoGrafico);        
    }
}
