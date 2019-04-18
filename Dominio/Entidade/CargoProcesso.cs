using Dominio.Enumerado;

namespace Dominio.Entidade
{
    public partial class CargoProcesso
    {
        public int IdCargoProcesso { get; set; }
        public int IdProcesso { get; set; }
        public int IdCargo { get; set; }
        public int IdFuncao { get; set; }

        public virtual Cargo Cargo { get; set; }
        public virtual Funcao Funcao { get; set; }
        public virtual Processo Processo { get; set; }
    }
}
 