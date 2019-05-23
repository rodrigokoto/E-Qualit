using System;

namespace Dominio.Entidade
{
    public partial class DocTemplate
    {
        public int IdDocTemplate { get; set; }
        public int IdDocumento { get; set; }
        public string TpTemplate { get; set; }
        public bool? Ativo { get; set; }
        public virtual DocDocumento DocDocumento { get; set; }

        public static String FluxoTemplate { get { return "F"; } }
        public static String TextoTemplate { get { return "T"; } }
        public static String RegistrosTemplate { get { return "R"; } }
        public static String GestaoDeRiscoTemplate { get { return "RI"; } }
        public static String RecursosTemplate { get { return "RE"; } }
        public static String RotinaTemplate { get { return "RO"; } }
        //public static String UploadTemplate { get { return "U"; } }
        public static String LicencaTemplate { get { return "L"; } }
        public static String DocExternoTemplate { get { return "DE"; } }
        public static String ResiduoTemplate { get { return "RES"; } }
        public static String IndicadoresTemplate { get { return "IN"; } }
    }
}
