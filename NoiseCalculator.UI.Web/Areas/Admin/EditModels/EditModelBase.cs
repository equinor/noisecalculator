using System;
using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.EditModels
{
    public abstract class EditModelBase
    {
        protected IList<string> _validationErrors;

        protected bool IsValid(Func<IList<string>> validateMethod)
        {
            if (_validationErrors == null)
            {
                _validationErrors = validateMethod.Invoke();
            }

            return _validationErrors.Count <= 0;
        }

        protected IList<string> GetValidationErrors(Func<IList<string>> validateMethod)
        {
            if (_validationErrors == null)
            {
                _validationErrors = validateMethod.Invoke();
            }

            return _validationErrors;
        }
    }
}