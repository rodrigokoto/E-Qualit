using DomainValidation.Interfaces.Validation;
using System;
using System.Collections.Generic;
using DomainValidation.Validation;
using System.Linq;

namespace Dominio.Entidade
{
    public class Indicador : ISelfValidator
    {
        public Indicador()
        {
            PeriodicidadeDeAnalises = new List<PeriodicidaDeAnalise>();
        }
        public int Id { get; set; }
        public string Objetivo { get; set; }
        public string Descricao { get; set; }
        public string Unidade { get; set; }
        public int Direcao { get; set; }

        public string MetaAnual { get; set; }

        public int Ano { get; set; }
        public int Periodicidade { get; set; }
        public int PeriodicidadeMedicao { get; set; }
        public int IdSite { get; set; }
        public int IdUsuarioIncluiu { get; set; }
        public DateTime DataAlteracao { get; set; }
        public DateTime DataInclusao { get; set; }
        public byte StatusRegistro { get; set; }
        
        public virtual List<PeriodicidaDeAnalise> PeriodicidadeDeAnalises { get; set; }

        public int IdProcesso { get; set; }
        public virtual Processo Processo { get; set; }

        public int IdResponsavel { get; set; }
        public virtual Usuario Responsavel { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public int Medida { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }

        public void Teste()
        {
            foreach (var periodicidaDeAnalise in PeriodicidadeDeAnalises)
            {
                var countPositivo = new List<DateTime>();

                for (int i = 0; i < periodicidaDeAnalise.PlanoDeVoo.Count; i++)
                {
                    var mesMeta = periodicidaDeAnalise.PlanoDeVoo[i].DataReferencia;

                    periodicidaDeAnalise.MetasRealizadas.Where(x => x.DataReferencia == mesMeta);

                    if (true)
                    {

                    }
                };
            }

        }
    }
}
