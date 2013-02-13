using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Common;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Repository.Concret;
using HomeHelper.Views;
using Windows.UI.Xaml.Controls;

namespace HomeHelper.ViewModel
{
    public class MainViewModel:BindableBase
    {
        private Utilitati _utilitateSelectata;
        private ObservableCollection<Utilitati> _listaUtilitati;
        private AlertaUtilitate _alertaSelectata;
        private ObservableCollection<AlertaUtilitate> _alerteUtilitati; 
        private ObservableCollection<ConsumUtilitate> _consumUtilitate;
        private IRepository<Utilitati> _repositoryUtilitati;
        private IRepository<ConsumUtilitate> _repositoryConsum;
        private IRepository<AlertaUtilitate> _repositoryAlerta;
        private RelayCommand _adaugaUtilitatCommand, _adaugaConsumCommand, _adaugaAlertaCommand;
        private RelayCommand _editeazaUtilitateCommand, _editeazaConsumCommand, _editeazaAlertaCommand;
        private RelayCommand _stergeUtilitateCommand, _stergeAlertaCommand,_cmdListView;

        public MainViewModel()
        {
            _repositoryConsum=new ConsumUtilitateRepository();
            _repositoryAlerta = new AlertaUtilitateRepository();
            _repositoryUtilitati = new UtilitatiRepository();
        }

        public Utilitati UtilitateSelectata
        {
            get { return _utilitateSelectata; }
            set { SetProperty(ref _utilitateSelectata, value, "UtilitateSelectata"); }
        }
        public ObservableCollection<Utilitati> ListaUtilitati
        {
           get { return _repositoryUtilitati.GetAll(); }
        }
        public AlertaUtilitate AlertaSelectata
        {
            get { return _alertaSelectata; }
            set { SetProperty(ref _alertaSelectata, value, "AlertaSelectata"); }
        }

        public ObservableCollection<ConsumUtilitate> ConsumUtilitate
        {
            get { return _consumUtilitate; }
            set { SetProperty(ref _consumUtilitate, value, "ConsumUtilitate"); }
        }  
        public ObservableCollection<AlertaUtilitate> AlerteUtilitati
        {
            get { return _repositoryAlerta.GetAll(); }
        } 

        public RelayCommand AddUtilitateCommand
        {
            get
            {
                if (_adaugaUtilitatCommand == null)
                {
                    _adaugaUtilitatCommand=new RelayCommand(o => CurrentFrame.Navigate(typeof(EditView),0),o => true);
                   
                }
                return _adaugaUtilitatCommand;
            }
        }

        public RelayCommand AddConsumCommand
        {
            get
            {
                if (_adaugaConsumCommand == null)
                {
                    _adaugaConsumCommand=new RelayCommand(o=>CurrentFrame.Navigate(typeof(EditViewConsum),new Tuple<int,int>(0,0)),x=>true);
                }
                return _adaugaConsumCommand;
            }
        }
        public Frame CurrentFrame { get; set; }

        public RelayCommand CmdListView
        {
            get
            {
                if (_cmdListView == null)
                {
                    _cmdListView = new RelayCommand(o =>
                                                        {
                                                            var cast = o as Utilitati;
                                                            if (cast == null) return;
                                                            UtilitateSelectata = cast;
                                                            ConsumUtilitate = new ObservableCollection<ConsumUtilitate>(cast.Consums);
                                                        },o=>ListaUtilitati.Any());
                }
                return _cmdListView;
            }
        }
    }
}
