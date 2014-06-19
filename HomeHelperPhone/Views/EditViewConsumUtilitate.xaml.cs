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
using HomeHelperPhone.Utils;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

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
            Loaded += (s, e) => LoadImage();
            if (ViewModelConsum.ObiectInBinding != null && ViewModelConsum.ObiectInBinding.IdConsumUtilitate != 0)
            {
               LoadImage();
            }
        }

        public ConsumUtilitateInputViewModel ViewModelConsum
        {
            get { return DataContext as ConsumUtilitateInputViewModel; }
        }

        async void CameraCaptureTaskCompleted(object sender, PhotoResult e)
        {
            if (e.TaskResult != TaskResult.OK) return;
            var fileName = await IoUtils.SaveImage(e);
            (DataContext as ConsumUtilitateInputViewModel).ObiectInBinding.ImagePath = fileName;
            LoadImage();
        }

        public void LoadImage()
        {
            if (string.IsNullOrEmpty(ViewModelConsum.ObiectInBinding.ImagePath))
            {
                rtEmpty.Visibility = Visibility.Visible;
                img.Visibility = Visibility.Collapsed;
            }
            else
            {
                img.Visibility = Visibility.Visible;
                rtEmpty.Visibility = Visibility.Collapsed;
                ImagePath = new Uri(ViewModelConsum.ObiectInBinding.ImagePath, UriKind.Absolute);
            }
            img.Source = new BitmapImage(ImagePath) { DecodePixelWidth = 150, DecodePixelHeight = 150 };
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
            LoadImage();
        }


        private void CaptureClick(object sender, RoutedEventArgs e)
        {
            _task.Show();
        }

        private void Img_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate("/Views/ViewPhoto.xaml", ViewModelConsum.ObiectInBinding);
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