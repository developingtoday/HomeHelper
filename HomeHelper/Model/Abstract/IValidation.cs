using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHelper.Model.Abstract
{
    public interface IValidation
    {
        void DoValidation();
        Dictionary<string, string> Errors { get; }
    }
}
