using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Servico
{
    public class BaseService
    {
        public Exception Error;
        public bool Sucesso { get; set; }

        public BaseService()
        {
            AutoMapperConfig.RegisterMappings();
        }
    }
}
