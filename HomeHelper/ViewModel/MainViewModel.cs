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
        private Utilitati _utilitateClicked;
        private string _legendaUtilitate;
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

        public string LegendaUtilitateGrafic
        {
            get { return _legendaUtilitate; }
            set { SetProperty(ref _legendaUtilitate, value, "LegendaUtilitateGrafic"); }
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
                if (value == null) return;
                ShowInput<ConsumUtilitate>(new EditConsumUtilitateUserControl(InputViewOperatiune.Modificare, value),
                                           () =>
                                           ConsumUtilitates =
                                           new ObservableCollection<ConsumUtilitate>(
                                               _repositoryConsum.GetAll().Where(a => a.IdUtilitate == value.IdUtilitate)));
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
            set
            {
                SetProperty(ref _consumUtilitates,
                            (value.Any())
                                ? value
                                : new ObservableCollection<ConsumUtilitate>()
                                      {
                                          new ConsumUtilitate()
                                              {
                                                  DataConsum =
                                                      DateTime
                                                      .Now,
                                                  ValoareConsum
                                                      = 0,
                                                  IdUtilitate
                                                      = 0
                                              },
                                          new ConsumUtilitate()
                                              {
                                                  DataConsum =
                                                      DateTime
                                                      .Now,
                                                  ValoareConsum
                                                      = 0,
                                                  IdUtilitate
                                                      = 0
                                              }
                                      },
                            "ConsumUtilitates");
                LegendaUtilitateGrafic = (value.Any()) ? _repositoryUtilitati.GetById(value.FirstOrDefault().IdUtilitate).DenumireUtilitate : "Utilitate";
            }
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
                    _adaugaUtilitatCommand =
                        new RelayCommand(
                            o =>
                            ShowInput<Utilitati>(new EditUtilitateUserControl(InputViewOperatiune.Adaugare, new Utilitati()),() => ListaUtilitati=_repositoryUtilitati.GetAll()));

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
                    _adaugaAlertaCommand = new RelayCommand(o => ShowInput<AlertaUtilitate>(
                        new AlertaUtilitateUserControl(InputViewOperatiune.Adaugare, new AlertaUtilitate()),
                        () => AlerteUtilitati = _repositoryAlerta.GetAll()
                                                                     ), x => true);
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
                    _adaugaConsumCommand =
                        new RelayCommand(
                            o =>
                            ShowInput<ConsumUtilitate>(
                                new EditConsumUtilitateUserControl(InputViewOperatiune.Adaugare, new ConsumUtilitate()
                                                                                                     {
                                                                                                         IdUtilitate = _utilitateClicked.IdUtilitati
                                                                                                     }),
                                () =>
                                    {
                                        
                                        var id = _utilitateClicked.IdUtilitati;
                                        if (id == 0) return;
                                        ConsumUtilitates =
                                            new ObservableCollection<ConsumUtilitate>(
                                                _repositoryConsum.GetAll()
                                                                 .Where(
                                                                     a =>
                                                                     a.IdUtilitate == id));
                                    }
                                                     
                                                     ),
                            x => true);
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
                                                            if (cast == null)
                                                            {
                                                                _utilitateClicked =new Utilitati();
                                                                return;
                                                            }
                                                            ConsumUtilitates = new ObservableCollection<ConsumUtilitate>(cast.Consums);
                                                            _utilitateClicked = cast;
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
                                x => ShowInput<Utilitati>(new EditUtilitateUserControl(InputViewOperatiune.Modificare, UtilitateSelectata),() => ConsumUtilitates=new ObservableCollection<ConsumUtilitate>(_repositoryConsum.GetAll().Where(a=>a.IdUtilitate==ConsumSelect.IdUtilitate))));
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
                    //if (_editeazaConsumCommand == null)
                    //{
                    //    _editeazaConsumCommand = new RelayCommand(o => ShowInput<ConsumUtilitate>(new EditConsumUtilitateUserControl(InputViewOperatiune.Modificare, _consumSelect))
                    //                                                );
                    //}
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
                                                                      ShowInput<AlertaUtilitate>(new AlertaUtilitateUserControl(InputViewOperatiune.Modificare, cast),()=>AlerteUtilitati=_repositoryAlerta.GetAll() );
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
                                                                    _utilitateClicked = new Utilitati();
                                                                    ListaUtilitati =
                                                                        _repositoryUtilitati.GetAll();
                                                                    ConsumUtilitates=new ObservableCollection<ConsumUtilitate>();
                                                                }
                                                                if (AlertaSelectata == null) return;
                                                                
                                                                    _repositoryAlerta.Delete(AlertaSelectata);
                                                                AlerteUtilitati = _repositoryAlerta.GetAll();

                                                            });
                    }
               
                return _stergeCommand;
            }
        }

        private void ShowInput<T>(UserControl control,Action refresData) where T:class,new()
        {
            var uc = control;
            var popup = UiHelper.ShowPopup(CurrentFrame, uc);
            var dataContext = uc.DataContext as InputViewModelBase<T>;
            if (dataContext != null)
            {
                dataContext.IsClosedChanged += (s, e) => popup.IsOpen = !dataContext.IsClosed;
            }
            popup.Closed += (s, e) =>
                                {

                                    if (dataContext !=null && !dataContext.RefreshDataSource) return;
                                    if (refresData != null) refresData();
                                };
        }
    }
 


}
