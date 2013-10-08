using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using HomeHelper.Model;
using HomeHelper.Repository.Concret;
using HomeHelper.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace HomeHelperPhone.Views
{
    public partial class EditViewUtilitate : PhoneApplicationPage
    {
        public EditViewUtilitate()
        {
            InitializeComponent();
            DataContext = new UtilitateInputViewModel(new UtilitatiRepository());
            var uc = DataContext as UtilitateInputViewModel;
            uc.ObiectInBinding = new Utilitati();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var param = string.Empty;
            if (NavigationContext.QueryString.TryGetValue("Id", out param))
            {
                var ctx = DataContext as UtilitateInputViewModel;
                ctx.ObiectInBinding = ctx.Repository.GetById(int.Parse(param));
            }
            else
            {
                var ctx = DataContext as UtilitateInputViewModel;
                ctx.ObiectInBinding = new Utilitati();
            }

        }

        private void SaveUtilitate(object sender, EventArgs e)
        {

            var uc = DataContext as UtilitateInputViewModel;
            uc.SaveCommand.Execute(uc.ObiectInBinding);
            if (uc.Erori.Any()) return;
            NavigationService.GoBack();
        }

        private void DeleteUtilitate(object sender, EventArgs e)
        {
            
            var uc = DataContext as UtilitateInputViewModel;
            uc.DeleteCommand.Execute(uc.ObiectInBinding);
            NavigationService.GoBack();
        }

        private void Cancel(object sender, EventArgs e)
        {
            
            var uc = DataContext as UtilitateInputViewModel;
            uc.CancelCommand.Execute(uc.ObiectInBinding);
            NavigationService.GoBack();
        }

        
    }
}