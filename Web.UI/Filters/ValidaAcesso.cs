using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Web.UI.Filters
{
    public class ValidaAcesso : AuthorizeAttribute
    {
        public ValidaAcesso()
        {
            
        }
    }
}