using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using HomeHelper.Common;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Utils;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace HomeHelperPhone.Views
{
    public partial class ViewConsumuri : PhoneApplicationPage
    {
        private Utilitati _utilitati;
        private ObservableCollection<ConsumUtilitate> _consumUtilitates;
        private readonly IRepository<Utilitati> _repository = FactoryRepository.GetInstanceRepositoryUtilitati();
        private readonly IRepository<ConsumUtilitate> _repositoryConsum = FactoryRepository.GetInstanceRepositoryConsum(); 

        public ViewConsumuri()
        {
            InitializeComponent();
            fltGrafic.FromDate = DateTime.Now.AddMonths(-1).Date;
            fltGrafic.ToDate = DateTime.Now.Date;
            fltGrafic.FilterCommand = new RelayCommand(FiltertGraph);
        }

        private void FiltertGraph(object o)
        {
            if (!_utilitati.Consums.Any()) return;
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var param = string.Empty;
            if (NavigationContext.QueryString.TryGetValue("Id", out param))
            {
                _utilitati = _repository.GetAll().FirstOrDefault(a => a.IdUtilitati == int.Parse(param));
            }
            else
            {
                _utilitati = new Utilitati();
            }
            DataContext = _utilitati;
            //var prevDat = fltGrafic.FromDate;
            //fltGrafic.FromDate = prevDat;
            //var prevTo = fltGrafic.ToDate;
            //var prevCmd = fltGrafic.FilterCommand;
            //fltGrafic.FilterCommand = prevCmd;
        }



        private void AddConsumptionButton(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/EditViewConsumUtilitate.xaml", UriKind.Relative));
        }

        private void UIElement_OnTap(object sender, GestureEventArgs e)
        {
            if (e == null) return;
            var item = (e.OriginalSource as FrameworkElement).DataContext;
            if (item == null) return;
            var cast = item as ConsumUtilitate;
            if (cast == null) return;
            NavigationService.Navigate(new Uri(string.Format("/Views/EditViewConsumUtilitate.xaml?Id={0}",cast.IdConsumUtilitate), UriKind.Relative));
        }
    }
}