using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using HomeHelper.Common;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Utils;
using HomeHelper.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace HomeHelperPhone.Views
{
    public partial class EditViewConsumUtilitate : EditViewConsumUtilitateGeneric
    {
        private readonly IRepository<Utilitati> _repository = FactoryRepository.GetInstanceRepositoryUtilitati();
        private Utilitati utilPicker;
        private readonly CameraCaptureTask _task;
       

        public EditViewConsumUtilitate()
        {
            InitializeComponent();
            _task = new CameraCaptureTask();
            _task.Completed += CameraCaptureTaskCompleted;
            if (ViewModelConsum.ObiectInBinding != null && ViewModelConsum.ObiectInBinding.IdConsumUtilitate != 0)
            {
               ImagePath=new Uri(ViewModelConsum.ObiectInBinding.ImagePath);
            }
        }

        public ConsumUtilitateInputViewModel ViewModelConsum
        {
            get { return DataContext as ConsumUtilitateInputViewModel; }
        }

        void CameraCaptureTaskCompleted(object sender, PhotoResult e)
        {
            if (e.TaskResult != TaskResult.OK) return;
            (DataContext as ConsumUtilitateInputViewModel).ObiectInBinding.ImagePath = e.OriginalFileName;
            
            ImagePath=new Uri(ViewModelConsum.ObiectInBinding.ImagePath);
            img.Source=new BitmapImage(ImagePath);
        }

        public Uri ImagePath { get; set; }

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


        private void CaptureClick(object sender, RoutedEventArgs e)
        {
            _task.Show();
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