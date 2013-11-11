using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Utils;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

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

        }

    }
}