using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class DocDocumentoXML
    {
        public string Texto { get; set; }
        public string Fluxo { get; set; }
        public Recurso Recursos { get; set; }
        public List<DocRegistro> Registros { get; set; }
        public List<DocRotina> Rotinas { get; set; }
        public List<Risco> Riscos { get; set; }
        public Residuo Residuo { get; set; }
        //public List<Upload> Upload { get; set; }
        public Licenca Licenca { get; set; }
        public DocExterno DocExterno { get; set; }

        public DocDocumentoXML()
        {
            Registros = new List<DocRegistro>();
            Rotinas = new List<DocRotina>();
            Riscos = new List<Risco>();
            Recursos = new Recurso();
            Residuo = new Residuo();
            //Upload = new List<Upload>();
            Licenca = new Licenca();
            DocExterno = new DocExterno();
        }
    }
}
