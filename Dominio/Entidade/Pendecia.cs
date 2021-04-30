using DomainValidation.Interfaces.Validation;
using DomainValidation.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidade
{
    public class Pendencia
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int IdResponsavel { get; set; }
        public string Modulo { get; set; }

        public string Url { get; set; }

    }
}
