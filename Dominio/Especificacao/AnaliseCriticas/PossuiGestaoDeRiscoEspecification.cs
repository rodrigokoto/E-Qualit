using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using System;

namespace Dominio.Especificacao.AnaliseCriticas
{
    public class PossuiGestaoDeRiscoEspecification : ISpecification<AnaliseCritica>
    {
        public bool IsSatisfiedBy(AnaliseCritica customer)
        {
            //customer => customer).NotEmpty();
            //RuleFor(customer => customer.Forename).NotEmpty().WithMessage("Please specify a first name");
            //RuleFor(customer => customer.Discount).NotEqual(0).When(customer => customer.HasDiscount);
            //RuleFor(customer => customer.Address).Length(20, 250);
            //RuleFor(customer => customer.Postcode).Must(BeAValidPostcode).WithMessage("Please specify a valid postcode");


        //        public static IRuleBuilderOptions<T, TProperty> NotEmpty<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        //{

            throw new NotImplementedException();
        }


    }
}
