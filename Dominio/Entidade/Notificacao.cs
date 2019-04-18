using System;

namespace Dominio.Entidade
{
    public class Notificacao
    {
        public Notificacao()
        {

        }

        public Notificacao(string descricao, DateTime? dtEnvioFilaDisparo,
            DateTime dtVencimento, int idFuncionalidade,
            int ? idProcesso, int idRegistroDaBase,
            int idSite, int nuDiasAntecedencia,
            string tpNotificacao, int idUsuarioReceptorNotificacao
            )
        {
            Descricao = descricao;
            DtEnvioFilaDisparo = dtEnvioFilaDisparo;
            DtVencimento = dtVencimento;
            IdFuncionalidade = idFuncionalidade;
            IdProcesso = idProcesso;
            IdRelacionado = idRegistroDaBase;
            IdSite = idSite;
            NuDiasAntecedencia = nuDiasAntecedencia;
            TpNotificacao = tpNotificacao;
            IdUsuario = idUsuarioReceptorNotificacao;
        }


        public int IdNotificacao { get; set; }
        public int IdUsuario { get; set; }
        public int? IdProcesso { get; set; }
        public int IdSite { get; set; }
        public int IdRelacionado { get; set; }
        public int IdFuncionalidade { get; set; }
        public string TpNotificacao { get; set; }
        public DateTime? DtVencimento { get; set; }
        public int NuDiasAntecedencia { get; set; }
        public DateTime? DtEnvioFilaDisparo { get; set; }
        public string Descricao { get; set; }
        public string FlEtapa { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Funcionalidade Funcionalidade { get; set; }
        public virtual Processo Processo { get; set; }
        public virtual Site Site { get; set; }
        
    }
}
