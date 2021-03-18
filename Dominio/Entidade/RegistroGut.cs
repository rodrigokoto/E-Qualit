using DomainValidation.Interfaces.Validation;
using System;
using System.Collections.Generic;
using DomainValidation.Validation;
using Dominio.Enumerado;

namespace Dominio.Entidade
{
    public class RegistroGut 
    {
        public int IdGut { get; set; }
        public int Gravidade { get; set; }
        public int Urgencia { get; set; }
        public int Tendencia { get; set; }
    }
}
