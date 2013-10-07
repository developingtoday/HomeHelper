using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Model.Abstract;
using HomeHelperPhone.Resources;

namespace HomeHelperPhone.Utils
{
    public class PhoneResources:IResources 
    {

        public string GetString(string resource)
        {
            return AppResources.ResourceManager.GetString(resource);
        }
    }
}
