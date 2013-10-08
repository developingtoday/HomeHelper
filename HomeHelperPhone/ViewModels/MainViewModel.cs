using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using HomeHelper.Common;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Utils;
using Microsoft.Phone.Controls;

namespace HomeHelperPhone.ViewModels
{
    public class MainViewModel:BindableBase
    {
        private ObservableCollection<Utilitati> _listUtilitati;
        private RelayCommand _commandAdd;
        private IRepository<Utilitati> _repositoryUtilitati;  
        public MainViewModel()
        {
            _repositoryUtilitati = FactoryRepository.GetInstanceRepositoryUtilitati();
            
        }

        public ObservableCollection<Utilitati> ListaUtilitati
        {
            get { return _listUtilitati; }
            set { SetProperty(ref _listUtilitati, value, "ListaUtilitati"); }
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

        public PhoneApplicationPage CurrentFrame { get; set; }

        public void RefreshUtilitati()
        {
            ListaUtilitati = _repositoryUtilitati.GetAll();
        }
    }
}
