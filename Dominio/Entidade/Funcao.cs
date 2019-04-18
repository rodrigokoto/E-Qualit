using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class Funcao
    {
        public Funcao()
        {
            CargoProcessos = new List<CargoProcesso>();
        }

        public int IdFuncao { get; set; }
        public int IdFuncionalidade { get; set; }
        public string NmNome { get; set; }
        public int NuOrdem { get; set; }

        public virtual ICollection<CargoProcesso> CargoProcessos { get; set; }
        public virtual Funcionalidade Funcionalidade { get; set; }
    }
}
