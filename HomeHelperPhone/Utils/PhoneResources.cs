using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HomeHelper.Model.Abstract;
using HomeHelperPhone.Resources;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
namespace HomeHelperPhone.Utils
{
    public class PhoneResources:IResources 
    {

        public string GetString(string resource)
        {
            return AppResources.ResourceManager.GetString(resource);
        }
    }

    public static class Extension
    {
        public static void FocusUpdateLastElement()
        {
            var focused = FocusManager.GetFocusedElement();
            if (focused is TextBox)
            {
                var binding = (focused as TextBox).GetBindingExpression(TextBox.TextProperty);
                if (binding != null) binding.UpdateSource();
            }
            if (focused is DatePicker)
            {
                var binding = (focused as DatePicker).GetBindingExpression(DatePicker.ValueProperty);
                if (binding != null) binding.UpdateSource();
            }

        }
    }

}
