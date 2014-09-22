using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using HomeHelper.Common;
using HomeHelper.Model;
using HomeHelper.Model.Abstract;
using HomeHelper.Utils;
using HomeHelper.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace HomeHelperPhone.Views
{
    public partial class EditViewAlertaUtilitate : EditViewAlertaUtilitateGeneric
    {
        public EditViewAlertaUtilitate()
        {
            InitializeComponent();
            Loaded += (s, e) =>
                          {
                              if (lstUtils.SelectedItem == null && lstUtils.Items.Any())
                              {
                                  lstUtils.SelectedIndex = 0;
                              }
                          };
        }

        private void LstUtilitati_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cast = sender as ListPicker;
            if (cast == null) return;
            var select = cast.SelectedItem as Utilitati;
            if (select == null) return;
            ViewModelBase.ObiectInBinding.IdUitlitate = select.IdUtilitati;
        }


        private void FrecventeTap(object sender, SelectionChangedEventArgs e)
        {
            var cast = sender as ListPicker;
            if (cast == null) return;
            var select = cast.SelectedItem as StringIntKeyValue;
            if (select == null) return;
            ViewModelBase.ObiectInBinding.FrecventaAlerta = select.Key;
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