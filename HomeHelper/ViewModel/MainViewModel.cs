using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Common;
using HomeHelper.Controls;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Repository.Concret;
using HomeHelper.Views;
using Windows.UI.Xaml.Controls;

namespace HomeHelper.ViewModel
{
    public class MainViewModel:BindableBase
    {

        private bool _showEdit;
        private Utilitati _utilitateSelectata;
        private ObservableCollection<Utilitati> _listaUtilitati;
        private AlertaUtilitate _alertaSelectata;
        private ConsumUtilitate _consumSelect;
        private ObservableCollection<AlertaUtilitate> _alerteUtilitati; 
        private ObservableCollection<ConsumUtilitate> _consumUtilitates;
        private IRepository<Utilitati> _repositoryUtilitati;
        private IRepository<ConsumUtilitate> _repositoryConsum;
        private IRepository<AlertaUtilitate> _repositoryAlerta;
        private RelayCommand _adaugaUtilitatCommand, _adaugaConsumCommand, _adaugaAlertaCommand;
        private RelayCommand _editeazaUtilitateCommand, _editeazaConsumCommand, _editeazaAlertaCommand;
        private RelayCommand _cmdListView,_stergeCommand;

        public MainViewModel()
        {
            _repositoryConsum=new ConsumUtilitateRepository();
            _repositoryAlerta = new AlertaUtilitateRepository();
            _repositoryUtilitati = new UtilitatiRepository();
            _listaUtilitati = _repositoryUtilitati.GetAll();
            _alerteUtilitati = _repositoryAlerta.GetAll();

        }

        public Action RefreshGraph { get; set; }
        public Utilitati UtilitateSelectata
        {
            get { return _utilitateSelectata; }
            set
            {
                if (value == null && _alertaSelectata != null) ShowEdit = true;
                if(value!=null && _alertaSelectata==null) ShowEdit = true;
                if (value == null && _alertaSelectata == null) ShowEdit = false;
                SetProperty(ref _utilitateSelectata, value, "UtilitateSelectata");
            }
        }
        public ObservableCollection<Utilitati> ListaUtilitati
        {
           get { return _listaUtilitati; }
            set { SetProperty(ref _listaUtilitati, value, "ListaUtilitati"); }
        }

        public ConsumUtilitate ConsumSelect
        {
            get { return _consumSelect; }
            set
            {
                SetProperty(ref _consumSelect, value, "ConsumSelect");
                CurrentFrame.Navigate(typeof (EditViewConsum),
                                      new Tuple<int, int>(value.IdConsumUtilitate, value.IdUtilitate));
            }
        }
        public AlertaUtilitate AlertaSelectata
        {
            get { return _alertaSelectata; }
            set
            {
                SetProperty(ref _alertaSelectata, value, "AlertaSelectata");
                if (RefreshGraph != null) RefreshGraph();
            }
        }

        public ObservableCollection<ConsumUtilitate> ConsumUtilitates
        {
            get { return _consumUtilitates; }
            set { SetProperty(ref _consumUtilitates, (value.Any()) ? value : new ObservableCollection<ConsumUtilitate>() { new ConsumUtilitate() { DataConsum = DateTime.Now, ValoareConsum = 0, IdUtilitate =0 }, new ConsumUtilitate() { DataConsum = DateTime.Now, ValoareConsum = 0, IdUtilitate = 0 } }, "ConsumUtilitates"); }
        }  
        public ObservableCollection<AlertaUtilitate> AlerteUtilitati
        {
            get {return  _alerteUtilitati; }
            set { SetProperty(ref _alerteUtilitati, value, "AlerteUtilitati"); }
        } 

        public bool ShowEdit
        {
            get { return _showEdit; }
            set
            {
                SetProperty(ref _showEdit, value, "ShowEdit");
            }
        }

        public RelayCommand AddUtilitateCommand
        {
            get
            {
                if (_adaugaUtilitatCommand == null)
                {
                    _adaugaUtilitatCommand=new RelayCommand(o => ShowInputUtilitate(InputViewOperatiune.Adaugare, new Utilitati()),o => true);
                   
                }
                return _adaugaUtilitatCommand;
            }
        }
        public RelayCommand AdaugaAlertaCommand
        {
            get
            {
                if (_adaugaAlertaCommand == null)
                {
                    _adaugaAlertaCommand=new RelayCommand(x=>CurrentFrame.Navigate(typeof(EditViewAlerta),0),x=>true);
                }
                return _adaugaAlertaCommand;
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
                                                            ConsumUtilitates = new ObservableCollection<ConsumUtilitate>(cast.Consums);
                                                        },o=>ListaUtilitati.Any());
                }
                return _cmdListView;
            }
        }

        public RelayCommand EditeazaCommand
        {
            get
            {
                if (UtilitateSelectata != null)
                {


                    if (_editeazaUtilitateCommand == null)
                    {
                        _editeazaUtilitateCommand =
                            new RelayCommand(
                                x => ShowInputUtilitate(InputViewOperatiune.Modificare ,UtilitateSelectata));
                    }
                    return _editeazaUtilitateCommand;
                }
           
                return null;
            }
        }

   

        public RelayCommand EditeazaConsumCommand
        {
            get
            {
                if (_consumSelect != null)
                {
                    if (_editeazaConsumCommand == null)
                    {
                        _editeazaConsumCommand=new RelayCommand(o =>
                                                                    {
                                                                        var cast = o as ConsumUtilitate;
                                                                        if (cast == null) return;
                                                                        CurrentFrame.Navigate(typeof (EditViewConsum),
                                                                                              cast.IdConsumUtilitate);
                                                                    });
                    }
                    return _editeazaConsumCommand;
                }
                return null;
            }
        }
        public RelayCommand EditeazaAlertaCommand
        {
            get
            {
              
                if (_editeazaAlertaCommand == null)
                {
                    _editeazaAlertaCommand = new RelayCommand(o =>
                                                                  {
                                                                      var cast = o as AlertaUtilitate;
                                                                      if (cast == null) return;
                                                                      CurrentFrame.Navigate(typeof (EditViewAlerta),
                                                                                            cast.IdAlertaUilitate);
                                                                  });
                }
                return _editeazaAlertaCommand;
               
            }
        }
        public RelayCommand StergeCommand
        {
            get {
               
                  
                    if (_stergeCommand == null)
                    {
                        _stergeCommand=new RelayCommand(o =>
                                                            {
                                                                if (UtilitateSelectata != null)
                                                                {
                                                               
                                                                        _repositoryUtilitati.Delete(
                                                                            UtilitateSelectata);

                                                                    ListaUtilitati =
                                                                        _repositoryUtilitati.GetAll();
                                                                }
                                                                if (AlertaSelectata == null) return;
                                                                
                                                                    _repositoryAlerta.Delete(AlertaSelectata);
                                                                AlerteUtilitati = _repositoryAlerta.GetAll();

                                                            });
                    }
               
                return _stergeCommand;
            }
        }

        private void ShowInputUtilitate(InputViewOperatiune op, Utilitati u)
        {
            var uc = new EditUtilitateUserControl(op,
                                                  u);
            var popup = UiHelper.ShowPopup(CurrentFrame, uc);
            var dataContext = uc.DataContext as UtilitateInputViewModel;
            if (dataContext != null)
            {
                dataContext.IsClosedChanged += (s, e) => popup.IsOpen = !dataContext.IsClosed;
            }
            popup.Closed += (s, e) =>
                                {

                                    if (dataContext !=null && !dataContext.RefreshDataSource) return;
                                    ListaUtilitati = _repositoryUtilitati.GetAll();
                                };
        }
    }
 


}
