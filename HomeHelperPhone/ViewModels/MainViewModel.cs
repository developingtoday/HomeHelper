using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using HomeHelper.Common;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Utils;
using Microsoft.Phone.Controls;
using Sparrow.Chart;

namespace HomeHelperPhone.ViewModels
{
    public class MainViewModel:BindableBase
    {
        private ObservableCollection<Utilitati> _listUtilitati;
        private ObservableCollection<ConsumUtilitate> _consum; 
        private RelayCommand _commandAdd,_filterGraphDataCommand;
        private IRepository<Utilitati> _repositoryUtilitati;
        private IRepository<AlertaUtilitate> _repositoryAlerte; 
        private ObservableCollection<AlertaUtilitate> _alerteUtilitati;
        private SeriesCollection _seriesCollection;
        private DateTime _from, _to;

        public MainViewModel()
        {
            _repositoryUtilitati = FactoryRepository.GetInstanceRepositoryUtilitati();
            _repositoryAlerte = FactoryRepository.GetInstanceAlertaUtilitate();
            _to = DateTime.Now;
            _from = _to.AddMonths(-1);
            _seriesCollection = new SeriesCollection();
            Consums = new ObservableCollection<ConsumUtilitate>();

        }


        public ObservableCollection<Utilitati> ListaUtilitati
        {
            get { return _listUtilitati; }
            set { SetProperty(ref _listUtilitati, value, "ListaUtilitati"); }
        }

        public ObservableCollection<ConsumUtilitate> Consums
        {
            get { return _consum; }
            set { SetProperty(ref _consum, value, "Consums"); }
        }

        public ObservableCollection<AlertaUtilitate> AlerteUtilitati
        {
            get { return _alerteUtilitati; }
            set { SetProperty(ref _alerteUtilitati, value, "AlerteUtilitati"); }
        } 
        public RelayCommand AddCommand
        {
            get
            {
                if (_commandAdd == null)
                {
                    _commandAdd=new RelayCommand(o => CurrentFrame.NavigationService.Navigate(new Uri("/Views/EditViewUtilitate.xaml",UriKind.Relative)));
                }
                return _commandAdd;
            }
        }

        public RelayCommand FilterCommand
        {
            get
            {
                if (_filterGraphDataCommand == null)
                {
                    _filterGraphDataCommand = new RelayCommand(o =>
                                                                 {
                                                                     Consums =
                                                                         new ObservableCollection<ConsumUtilitate>(
                                                                             _repositoryUtilitati.GetAll()
                                                                                                 .FirstOrDefault()
                                                                                                 .Consums);
                                                                 });
                }
                return _filterGraphDataCommand;
            }
        }



        public PhoneApplicationPage CurrentFrame { get; set; }

        public void RefreshUtilitati()
        {
            ListaUtilitati = _repositoryUtilitati.GetAll();
            AlerteUtilitati = _repositoryAlerte.GetAll();
        }

        public DateTime From
        {
            get { return _from; }
            set { SetProperty(ref _from, value, "From"); }
        }

        public DateTime To
        {
            get { return _to; }
            set { SetProperty(ref _to, value, "To"); }
        }



    }
}
