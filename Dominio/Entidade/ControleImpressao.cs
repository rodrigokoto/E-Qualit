using System;

namespace Dominio.Entidade
{
    public class ControleImpressao
    {
        public int Id { get; set; }
        public int IdFuncionalidade { get; set; }
        public string CodigoReferencia { get; set; }
        public bool CopiaControlada { get; set; }
        public int? IdUsuarioDestino { get; set; }
        public DateTime DataImpressao { get; set; }
        public int IdUsuarioIncluiu { get; set; }
        public DateTime DataInclusao { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Usuario UsuarioDestino { get; set; }
        public virtual Funcionalidade Funcionalidade { get; set; }
    }
}
