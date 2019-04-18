using Dominio.Entidade;
using Dominio.Interface.Repositorio;

namespace Dominio.Especificacao.Plais
{
    public class PossuiPlainMesAnteriorEspecificacao
    {
        private readonly IPlaiRepositorio _plaiRepositorio;

        public PossuiPlainMesAnteriorEspecificacao(IPlaiRepositorio plaiRepositorio)
        {
            _plaiRepositorio = plaiRepositorio;
        }

        public bool IsSatisfiedBy(Plai plai)
        {
            var mes = plai.Mes - 1;

            var plaiContext = _plaiRepositorio.Get(x => x.IdPai == plai.IdPai && x.Mes == mes);

            if (plaiContext == null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
