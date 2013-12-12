using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using HomeHelper.Common;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Utils;
using HomeHelper.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace HomeHelperPhone.Views
{
    public partial class EditViewConsumUtilitate : EditViewConsumUtilitateGeneric
    {
        private readonly IRepository<Utilitati> _repository = FactoryRepository.GetInstanceRepositoryUtilitati();
        private Utilitati utilPicker;
        
        public EditViewConsumUtilitate()
        {
            InitializeComponent();
            

        }

        private void ListPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var cast = sender as ListPicker;
            
            //if (cast == null) return;
            //var select = cast.SelectedItem as Utilitati;
            //if (select == null) return;
            //ViewModelBase.ObiectInBinding.IdUtilitate = select.IdUtilitati;
    

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var data = dtpIndex.Value;
            var prevSel = lstUtils.SelectedItem as Utilitati;
            var prev = ViewModelBase.ObiectInBinding.IndexUtilitate;
            base.OnNavigatedTo(e);
           // lstUtils.SelectedItem = _repository.GetById(ViewModelBase.ObiectInBinding.IdUtilitate);
            //dtpIndex.Value = data;
            //ViewModelBase.ObiectInBinding.IndexUtilitate = prev;
            //lstUtils.SelectedItem = prevSel;
            //ViewModelBase.ObiectInBinding.IdUtilitate = prevSel.IdUtilitati;
            //txtIndexUtil.Text = prev.ToString();
        }



       

    
    }

    public class EditViewConsumUtilitateGeneric:PhoneViewBaseForViewModel<ConsumUtilitate>
    {
        protected EditViewConsumUtilitateGeneric()
            : this(() => new ConsumUtilitateInputViewModel(FactoryRepository.GetInstanceRepositoryConsum()))
        {
            
        }

        protected EditViewConsumUtilitateGeneric(Func<InputViewModelBase<ConsumUtilitate>> viewModelCtor)
            : base(viewModelCtor)
        {

        }
    }
}