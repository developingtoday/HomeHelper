using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using HomeHelper.Model;
using HomeHelperPhone.Utils;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace HomeHelperPhone.Views
{
    public partial class ViewPhoto : PhoneApplicationPage
    {
        private ConsumUtilitate _utilitate;
        public ViewPhoto()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var param = NavigationService.GetLastNavigationData();
            if (param == null) return;
            var cast = param as ConsumUtilitate;
            _utilitate = cast;
            if (cast == null) return;
            if (string.IsNullOrEmpty(cast.ImagePath)) return;
            img.Source = new BitmapImage(new Uri(cast.ImagePath,UriKind.Absolute));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (!(img.Source is BitmapImage)) return;
            img.Source = null;
            GC.Collect();
        }

        private void DeleteImageClick(object sender, EventArgs e)
        {
            _utilitate.ImagePath = string.Empty;
            NavigationService.GoBack();
        }

        
    }
}