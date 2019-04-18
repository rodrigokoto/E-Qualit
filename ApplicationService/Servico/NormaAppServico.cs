using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;

namespace ApplicationService.Servico
{
    public class NormaAppServico : BaseServico<Norma>, INormaAppServico
    {
        private readonly INormaRepositorio _normaRepositorio;

        public NormaAppServico(INormaRepositorio normaRepositorio) : base(normaRepositorio)
        {
            _normaRepositorio = normaRepositorio;
        }

        public bool AtivarInativar(int id)
        {
            try
            {
                var norma = _normaRepositorio.GetById(id);

                norma.Ativo = !norma.Ativo;
                _normaRepositorio.Update(norma);

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }  

}
