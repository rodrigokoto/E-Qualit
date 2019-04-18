using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Web.Integration.Test
{
    public class JsonResultDynamicWrapper : DynamicObject
    {
        private readonly object _resultObject;

        public JsonResultDynamicWrapper(object resultObject)
        {
            if (resultObject == null) throw new ArgumentNullException(nameof(resultObject));
            _resultObject = resultObject;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (string.IsNullOrEmpty(binder.Name))
            {
                result = null;
                return false;
            }

            PropertyInfo property = _resultObject.GetType().GetProperty(binder.Name);

            if (property == null)
            {
                result = null;
                return false;
            }

            result = property.GetValue(_resultObject, null);
            return true;
        }
    }
}
