using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using HomeHelperPhone.Utils;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace HomeHelperPhone.Views
{
    public partial class ViewPhoto : PhoneApplicationPage
    {
        public ViewPhoto()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var param = NavigationService.GetLastNavigationData();
            if (param == null) return;
            var cast = param as Uri;
            if (cast == null) return;
            img.Source = new BitmapImage(cast);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (!(img.Source is BitmapImage)) return;
            img.Source = null;
            GC.Collect();
        }
    }
}