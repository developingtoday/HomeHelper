using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Common;
using HomeHelper.Controls;
using HomeHelper.Model;
using HomeHelper.Model.Abstract;
using HomeHelper.Repository.Abstract;
using HomeHelper.Repository.Concret;
using HomeHelper.Utils;
using Windows.ApplicationModel.Resources;
using Windows.System.Threading;
using Windows.UI.Xaml;
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
        private ConsumUtilitate _consumSelect,_consumSelectList;
        private ObservableCollection<AlertaUtilitate> _alerteUtilitati=new ObservableCollection<AlertaUtilitate>(); 
        private ObservableCollection<ConsumUtilitate> _consumUtilitates=new ObservableCollection<ConsumUtilitate>();
        private IRepository<Utilitati> _repositoryUtilitati;
        private IRepository<ConsumUtilitate> _repositoryConsum;
        private IEnhancedRepository<AlertaUtilitate> _repositoryAlerta;
        private RelayCommand _adaugaUtilitatCommand, _adaugaConsumCommand, _adaugaAlertaCommand;
        private RelayCommand _editeazaUtilitateCommand, _editeazaConsumCommand, _editeazaAlertaCommand;
        private RelayCommand _cmdListView,_stergeCommand;
        private string _mesaj;
        private ThreadPoolTimer _timer;
        private ThreadPoolTimer _timeTile;
        private LiveTileCreator _tileCreator;
        private readonly Windows.ApplicationModel.Resources.ResourceLoader _loader;
        public MainViewModel()
        {
            _loader = new ResourceLoader();
            MesajFaraInregistrariConsum = SeteazaMesajInregistrari(null);
            _repositoryConsum=new ConsumUtilitateRepository();
            _repositoryAlerta = new AlertaUtilitateRepository();
            _repositoryUtilitati = new UtilitatiRepository();
            _listaUtilitati = _repositoryUtilitati.GetAll();
            _alerteUtilitati = _repositoryAlerta.GetAll();
            _tileCreator = new LiveTileCreator();
            _tileCreator.LiveTileTtl = 10;
#if !DEBUG
            _timeTile = ThreadPoolTimer.CreatePeriodicTimer(poolTimer => _tileCreator.SetupTileNotificationWide(),
                                                            TimeSpan.FromSeconds(20));
            _timer = ThreadPoolTimer.CreatePeriodicTimer(poolTimer => RefreshAlerte(), TimeSpan.FromSeconds(30));
#endif   
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
                ShowEditConsumUtilitate(value);
            }
        }

        private void ShowEditConsumUtilitate(ConsumUtilitate value)
        {
            if (value == null) return;
            ShowInput<ConsumUtilitate>(new EditConsumUtilitateUserControl(InputViewOperatiune.Modificare, value),
                                       () =>
                                           {
                                               ConsumUtilitates =
                                                   new ObservableCollection<ConsumUtilitate>(
                                                       _repositoryUtilitati.GetById(value.IdUtilitate).Consums);
                                               ListaUtilitati = _repositoryUtilitati.GetAll();
                                           });
        }

        public ConsumUtilitate ConsumSelectList
        {
            get { return _consumSelectList; }
            set { SetProperty(ref _consumSelectList, value, "ConsumSelectList"); }
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
                if(value.Any()) value=new ObservableCollection<ConsumUtilitate>(value.OrderBy(a=>a.DataConsum));
                SetProperty(ref _consumUtilitates,
                            value,
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

        public string MesajFaraInregistrariConsum
        {
            get { return _mesaj; }
            set { SetProperty(ref _mesaj, value, "MesajFaraInregistrariConsum"); }
        }

        private string SeteazaMesajInregistrari(Utilitati u)
        {
            if (u == null)
            {
                return _loader.GetString("NuSuntInregistrariAdaugate");
            }
            return string.Format("{1} {0}",
                                 u.DenumireUtilitate, _loader.GetString("NuSuntInregistrariAdaugateUtilitatea"));
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
                                {
                                    var uc = new EditConsumUtilitateUserControl(InputViewOperatiune.Adaugare,
                                                                                new ConsumUtilitate()
                                                                                    {
                                                                                        IdUtilitate =
                                                                                            (_utilitateClicked == null)
                                                                                                ? 0
                                                                                                : _utilitateClicked
                                                                                                      .IdUtilitati
                                                                                    });
                                    uc.Unloaded += (s, e) =>
                                                       {
                                                           var c = uc.DataContext as ConsumUtilitate;
                                                           if (c == null) return;
                                                           _utilitateClicked = new Utilitati()
                                                                                   {
                                                                                       IdUtilitati = c.IdUtilitate
                                                                                   };
                                                       };
                                    ShowInput<ConsumUtilitate>(
                                        uc,
                                        () =>
                                            {

                                                var id = _utilitateClicked.IdUtilitati;
                                                if (id == 0) return;
                                                var cast = uc.DataContext as ConsumUtilitateInputViewModel;
                                                if (cast != null)
                                                {
                                                    if (cast.ObiectInBinding != null)
                                                    {
                                                        var finder = _repositoryUtilitati.GetById(cast.ObiectInBinding.IdUtilitate);
                                                        var consums = finder.Consums;
                                                      ConsumUtilitates.Add(finder.Consums.FirstOrDefault(a=>a.IdConsumUtilitate==cast.ObiectInBinding.IdConsumUtilitate));
                                                    }
                                                }
                                                ListaUtilitati = _repositoryUtilitati.GetAll();
                                            }
                                        );
                                },
                            x => true);
                }
                return _adaugaConsumCommand;
            }
        }

        public RelayCommand EditeazaConsumCommandList
        {
            get
            {
                if (_editeazaConsumCommand == null)
                {
                    _editeazaConsumCommand=new RelayCommand(o =>ShowEditConsumUtilitate(o as ConsumUtilitate) );
                }
                return _editeazaConsumCommand;
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
                                                            MesajFaraInregistrariConsum=SeteazaMesajInregistrari(cast);
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
                                x => ShowInput<Utilitati>(new EditUtilitateUserControl(InputViewOperatiune.Modificare, UtilitateSelectata),
                                    () =>
                                        {
                                            ConsumUtilitates = new ObservableCollection<ConsumUtilitate>(UtilitateSelectata.Consums);
                                            ListaUtilitati = _repositoryUtilitati.GetAll();
                                        }
                                            
                                            ));
                    }
                    return _editeazaUtilitateCommand;
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
                                                                if (ConsumSelectList!=null)
                                                                {
                                                                    var prevId = ConsumSelectList.IdUtilitate;
                                                                    _repositoryConsum.Delete(ConsumSelectList);
                                                                    ConsumUtilitates.Remove(ConsumSelectList);
                                                                        
                                                                }
                                                                if (AlertaSelectata == null) return;
                                                                
                                                                    _repositoryAlerta.Delete(AlertaSelectata);
                                                                AlerteUtilitati = _repositoryAlerta.GetAll();

                                                            });
                    }
               
                return _stergeCommand;
            }
        }

        private void RefreshAlerte()
        {
            foreach (var alertaUtilitate in _repositoryAlerta.GetAll())
            {
                if (DateTime.Now > alertaUtilitate.DataAlerta)
                {
                   
                    if (alertaUtilitate.FrecventaAlerta == (int)RepetareAlerta.Anual)
                    {
                        alertaUtilitate.DataAlerta = alertaUtilitate.DataAlerta.AddYears(1);
                    }
                    if (alertaUtilitate.FrecventaAlerta == (int)RepetareAlerta.Lunar)
                    {
                        alertaUtilitate.DataAlerta = alertaUtilitate.DataAlerta.AddMonths(1);
                    }
                    if (alertaUtilitate.FrecventaAlerta == (int)RepetareAlerta.Saptamanal)
                    {
                        alertaUtilitate.DataAlerta = alertaUtilitate.DataAlerta.AddDays(7);
                    }
                    if (alertaUtilitate.FrecventaAlerta == (int)RepetareAlerta.Zilnic)
                    {
                        alertaUtilitate.DataAlerta = alertaUtilitate.DataAlerta.AddDays(1);
                    }

                    if (alertaUtilitate.FrecventaAlerta != (int) RepetareAlerta.FaraRepetare)
                    {
                        _repositoryAlerta.CreateOrUpdateEnhanced(alertaUtilitate);
                    }
                    else
                    {
                        _repositoryAlerta.Delete(alertaUtilitate);
                    }


                }
            }

            CurrentFrame.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => AlerteUtilitati = _repositoryAlerta.GetAll());
        }

        private void ShowInput<T>(UserControl control, Action refresData) where T : class, IValidation, new()
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

                                    if (dataContext != null && !dataContext.RefreshDataSource) return;
                                    if (refresData != null) refresData();
                                };
        }
    }
 


}
