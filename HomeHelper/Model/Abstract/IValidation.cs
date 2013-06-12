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
        List<StringKeyValue> GetErrors();
    }
    public class StringKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class StringIntKeyValue
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}
