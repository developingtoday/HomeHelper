﻿using System;
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
using Windows.UI.Popups;
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

        private ObservableCollection<ConsumUtilitate> _consumUtilitatesGrafica =
            new ObservableCollection<ConsumUtilitate>(); 
        private IRepositoryEnhancing<Utilitati> _repositoryUtilitati;
        private IRepository<ConsumUtilitate> _repositoryConsum;
        private IEnhancedRepository<AlertaUtilitate> _repositoryAlerta;
        private RelayCommand _adaugaUtilitatCommand, _adaugaConsumCommand, _adaugaAlertaCommand;
        private RelayCommand _editeazaUtilitateCommand, _editeazaConsumCommand, _editeazaAlertaCommand;
        private RelayCommand _cmdListView,_stergeCommand;
        private RelayCommand _filterCommand, _cancelFilterCommand;
        private DateTime _dateFrom, _dateTo;
        private string _mesaj;
        private ThreadPoolTimer _timer;
        private ThreadPoolTimer _timeTile;
        private LiveTileCreator _tileCreator;
        private bool _isFilter;
        private bool _showNoFilterResults;
        private readonly Windows.ApplicationModel.Resources.ResourceLoader _loader;
        public MainViewModel()
        {
            _isFilter = false;
            _loader = new ResourceLoader();
            MesajFaraInregistrariConsum = SeteazaMesajInregistrari(null);
            _repositoryConsum=new ConsumUtilitateRepository();
            _repositoryAlerta = new AlertaUtilitateRepository();
            _repositoryUtilitati = new UtilitatiRepository();
            _listaUtilitati = _repositoryUtilitati.GetAll();
            _alerteUtilitati = _repositoryAlerta.GetAll();
            _dateFrom = DateTime.Now;
            _dateTo = DateTime.Now;
            _tileCreator = new LiveTileCreator();
            _tileCreator.LiveTileTtl = 10;
#if !DEBUG
            _timeTile = ThreadPoolTimer.CreatePeriodicTimer(poolTimer => _tileCreator.SetupTileNotificationWide(),
                                                            TimeSpan.FromSeconds(20));
            _timer = ThreadPoolTimer.CreatePeriodicTimer(poolTimer => RefreshAlerte(), TimeSpan.FromSeconds(30));
#endif   
        }
        #region Filter
        public DateTime DateFromFilter
        {
            get { return _dateFrom; }
            set { SetProperty(ref _dateFrom, value, "DateFromFilter"); }
        }

        public DateTime DateToFilter
        {
            get { return _dateTo; }
            set { SetProperty(ref _dateTo, value, "DateToFilter"); }
        }

       

        public RelayCommand FilterCommand
        {
            get
            {
                if (_filterCommand == null)
                {
                    _filterCommand=new RelayCommand(async (o) =>
                                                        {

                                                            if (!ConsumUtilitates.Any()) return;
                                                            if (
                                                                ConsumUtilitates.All(
                                                                    a =>
                                                                    !(_dateFrom.Date <= a.DataConsum.Date &&
                                                                      a.DataConsum.Date <= _dateTo.Date)))
                                                            {
                                                                //
                                                                await Mbox(_loader.GetString("filtruDataInvalid"), _loader.GetString("filtruInvalid"));
                                                                return;
                                                            }
                                                            ApplyFilter();
                                                            _isFilter = true;
                                                        });
                }
                return _filterCommand;
            }
        }

        private void ApplyFilter()
        {
            foreach (
                var consumUtilitate in
                    ConsumUtilitates.Where(a => !(_dateFrom.Date <= a.DataConsum.Date && a.DataConsum.Date <= _dateTo.Date)))
            {
                ListaGraficConsum.Remove(consumUtilitate);
            }
            
        }

        public RelayCommand CancelFilterCommand
        {
            get
            {
                if (_cancelFilterCommand == null)
                {
                    _cancelFilterCommand=new RelayCommand(o =>
                                                              {
                                                                  ListaGraficConsum =
                                                                      new ObservableCollection<ConsumUtilitate>(
                                                                          ConsumUtilitates);
                                                                  _isFilter = false;
                                                                  DateFromFilter = DateTime.Now;
                                                                  DateToFilter = DateFromFilter;
                                                              });
                }
                return _cancelFilterCommand;
            }
        }

        #endregion

        public ObservableCollection<ConsumUtilitate> ListaGraficConsum
        {
            get { return _consumUtilitatesGrafica; }
            set { SetProperty(ref _consumUtilitatesGrafica, value, "ListaGraficConsum"); }
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
                ShowEdit = value != null;
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
                ListaGraficConsum = new ObservableCollection<ConsumUtilitate>(value);
                if(_isFilter) ApplyFilter();

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
                                                var cast = uc.DataContext as ConsumUtilitateInputViewModel;
                                                if (cast != null)
                                                {
                                                    if (cast.ObiectInBinding != null)
                                                    {
                                                        var finder = _repositoryUtilitati.GetById(cast.ObiectInBinding.IdUtilitate);
                                                      
                                                        LegendaUtilitateGrafic = finder.DenumireUtilitate;
                                                        var consums = finder.Consums;
                                                       ConsumUtilitates=new ObservableCollection<ConsumUtilitate>(consums);
                                                      //ConsumUtilitates.Add(finder.Consums.FirstOrDefault(a=>a.IdConsumUtilitate==cast.ObiectInBinding.IdConsumUtilitate));
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

                    if (_editeazaUtilitateCommand == null)
                    {
                        _editeazaUtilitateCommand =
                            new RelayCommand(
                                x =>
                                    {
                                        if (UtilitateSelectata == null)
                                        {
                                            return;
                                        }
                                        ShowInput<Utilitati>(
                                            new EditUtilitateUserControl(InputViewOperatiune.Modificare,
                                                                         UtilitateSelectata),
                                            () =>
                                                {
                                                    ConsumUtilitates =
                                                        new ObservableCollection<ConsumUtilitate>(
                                                            UtilitateSelectata.Consums);
                                                    ListaUtilitati = _repositoryUtilitati.GetAll();
                                                }

                                            );
                                    });
                    }
                    return _editeazaUtilitateCommand;
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
                        _stergeCommand=new RelayCommand(async(o)=>
                                                            {
                                                                if (UtilitateSelectata != null)
                                                                {
                                                                     //are referinte     

                                                                    var willDelete = true;
                                                                    var hasRefs = false;
                                                                    if (
                                                                        _repositoryUtilitati.HasReferences(
                                                                            UtilitateSelectata.IdUtilitati))
                                                                    {
                                                                        hasRefs = true;
                                                                        willDelete = await DeleteMbox();
                                                                    }

                                                                    if (willDelete)
                                                                    {
                                                                        _repositoryUtilitati.Delete(
                                                                            UtilitateSelectata);
                                                                        _utilitateClicked = new Utilitati();
                                                                        ListaUtilitati =
                                                                            _repositoryUtilitati.GetAll();
                                                                        if (hasRefs)
                                                                        {
                                                                            ConsumUtilitates =
                                                                                new ObservableCollection
                                                                                    <ConsumUtilitate>();
                                                                            AlerteUtilitati = _repositoryAlerta.GetAll();
                                                                        }
                                                                        MesajFaraInregistrariConsum =
                                                                            SeteazaMesajInregistrari(null);
                                                                    }
                                                                }
                                                                if (ConsumSelectList!=null)
                                                                {
                                                                    var prevId = ConsumSelectList.IdUtilitate;
                                                                    _repositoryConsum.Delete(ConsumSelectList);
                                                                    ConsumUtilitates.Remove(ConsumSelectList);
                                                                    ListaUtilitati = _repositoryUtilitati.GetAll();
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


        private async Task<bool> DeleteMbox()
        {
            //TODO in future release this should be refactor
            var msg = new MessageDialog(_loader.GetString("cntDeleteMbox"), _loader.GetString("cntDeleteTitle"));
            var result = false;
            msg.Commands.Add(new UICommand(_loader.GetString("cntYes"),command =>
                result=true));
            msg.Commands.Add(new UICommand(_loader.GetString("cntNo"), command => result = false));
           await msg.ShowAsync();
            return result;
        }

        private async Task Mbox(string msg,string title)
        {
            var msgd = new MessageDialog(msg, title);
            await msgd.ShowAsync();
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
