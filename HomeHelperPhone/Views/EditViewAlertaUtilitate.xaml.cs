using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using HomeHelper.Common;
using HomeHelper.Model;
using HomeHelper.Utils;
using HomeHelper.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace HomeHelperPhone.Views
{
    public partial class EditViewAlertaUtilitate : EditViewAlertaUtilitateGeneric
    {
        public EditViewAlertaUtilitate()
        {
            InitializeComponent();
        }

        private void LstUtilitati_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }

    public class EditViewAlertaUtilitateGeneric:PhoneViewBaseForViewModel<AlertaUtilitate>
    {
        protected EditViewAlertaUtilitateGeneric()
            : base(() => new AlertaUtilitateViewModel(FactoryRepository.GetInstanceAlertaUtilitate()))
        {
            
        }
        protected EditViewAlertaUtilitateGeneric(Func<InputViewModelBase<AlertaUtilitate>> viewModelCtor) : base(viewModelCtor)
        {

        }
    }
}