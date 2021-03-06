﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;
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
        private ObservableCollection<ConsumUtilitate> _consum; 
        private RelayCommand _commandAdd,_filterGraphDataCommand;
        private IRepository<Utilitati> _repositoryUtilitati;
        private IRepository<AlertaUtilitate> _repositoryAlerte; 
        private ObservableCollection<AlertaUtilitate> _alerteUtilitati;
        private Collection<ISeries> _series; 
        private DateTime _from, _to;

        public MainViewModel()
        {
            _repositoryUtilitati = FactoryRepository.GetInstanceRepositoryUtilitati();
            _repositoryAlerte = FactoryRepository.GetInstanceAlertaUtilitate();
            _to = DateTime.Now;
            _from = _to.AddMonths(-1);
            Consums = new ObservableCollection<ConsumUtilitate>();
            Grafice = new Collection<ISeries>();
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
                    _filterGraphDataCommand=new RelayCommand(FilterGraph);
                }
                return _filterGraphDataCommand;
            }
        }

        private void FilterGraph(object o)
        {

            if (From > To) return;
            var g = new ObservableCollection<ISeries>();
            var util = _repositoryUtilitati.GetAll();
            foreach (var utilitati in util)
            {
                var l = new LineSeries();
                l.Title = utilitati.DenumireUtilitate;
                if(!utilitati.Consums.Any()) continue;
                if (
                    utilitati.Consums.All(
                        a =>
                        !(From.Date <= a.DataConsum.Date &&
                          a.DataConsum.Date <= To.Date)))
                {
                    continue;
                }
                l.ItemsSource = utilitati.Consums.Where(a=>From<=a.DataConsum.Date && a.DataConsum.Date<=To).ToList();
                l.DependentValuePath = "Consum";
                l.IndependentValuePath = "DataConsumGrafic";
               g.Add(l);
            }
            Grafice = g;
        }


        public PhoneApplicationPage CurrentFrame { get; set; }

        public void RefreshUtilitati()
        {
            ListaUtilitati = _repositoryUtilitati.GetAll();
            //here check the reminders;
            foreach (var alertaUtilitate in _repositoryAlerte.GetAll())
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

                    if (alertaUtilitate.FrecventaAlerta != (int)RepetareAlerta.FaraRepetare)
                    {
                        _repositoryAlerte.CreateOrUpdate(alertaUtilitate);
                    }
                    else
                    {
                        _repositoryAlerte.Delete(alertaUtilitate);
                    }


                }
            }
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

        public Collection<ISeries> Grafice
        {
            get { return _series; }
            set { SetProperty(ref _series, value, "Grafice"); }
        } 


    }
}
