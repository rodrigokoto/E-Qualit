using Dominio.Entidade;
using FluentValidation;
using System.Linq;

namespace Dominio.Validacao.RegistroConformidades.AcaoCorretivas
{
    public class CamposObrigatoriosAcaoCorretivaEncerrada : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosAcaoCorretivaEncerrada()
        {
            
        }
    }
}
