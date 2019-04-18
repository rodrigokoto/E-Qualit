using DomainValidation.Interfaces.Specification;
using Dominio.Entidade.RH;

namespace Dominio.Especificacao.RH.Funcionarios
{
    public class DeveInformarCamposObrigatorios : ISpecification<Funcionario>
    {
        public bool IsSatisfiedBy(Funcionario funcionario)
        {
            if (funcionario.Nome == null)
            {
                return false;
            }

            if (funcionario.Sexo == null)
            {
                return false;
            }

            if (funcionario.DataNascimento == null)
            {
                return false;
            }

            if (funcionario.DataVencimentoFerias == null)
            {
                return false;
            }

            if (funcionario.DataUltimoPrazo == null)
            {
                return false;
            }

            if (funcionario.NumeroRegistro == 0)
            {
                return false;
            }

            if (funcionario.EstadoCivil == null)
            {
                return false;
            }

            if (funcionario.Endereco == null)
            {
                return false;
            }

            if (funcionario.Bairro == null)
            {
                return false;
            }

            if (funcionario.Cep == null)
            {
                return false;
            }

            if (funcionario.Cidade == null)
            {
                return false;
            }

            if (funcionario.Uf == null)
            {
                return false;
            }

            return true;
        }
    }
}
