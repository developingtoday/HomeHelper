using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Navigation;
using HomeHelper.Model;
using HomeHelper.Repository.Concret;
using HomeHelperPhone.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HomeHelperPhone.Resources;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace HomeHelperPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        private MainViewModel _mainViewModel;
        public MainPage()
        {
            InitializeComponent();
            _mainViewModel = new MainViewModel();
            DataContext = _mainViewModel;
            Loaded += (s, e) => _mainViewModel.CurrentFrame = this;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            (DataContext as MainViewModel).RefreshUtilitati();
        }

        private void ApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {

            var name = pvt.SelectedItem;
            if (name == null) return;
            var cast = name as PivotItem;
            if (cast == null) return;
            if (cast.Name.Equals("pvtItmAlerte"))
            {
                NavigationService.Navigate(new Uri("/Views/EditViewAlertaUtilitate.xaml", UriKind.Relative));
                return;
            }
            var dc = DataContext as MainViewModel;
            dc.AddCommand.Execute(null);
        }



        private void EditItemClick(object sender, RoutedEventArgs e)
        {
            var item = lstUtilitati.SelectedItem as Utilitati;
            if (item == null) return;
            if (e == null) return;
            NavigationService.Navigate(new Uri(string.Format("/Views/EditViewUtilitate.xaml?Id={0}",item.IdUtilitati), UriKind.Relative));
        }

        private void LstUtilitati_OnTap(object sender, GestureEventArgs e)
        {
            var item = (e.OriginalSource as FrameworkElement).DataContext as Utilitati;
            if (item == null) return;
            if (e == null) return;
            NavigationService.Navigate(new Uri(string.Format("/Views/ViewConsumuri.xaml?Id={0}", item.IdUtilitati), UriKind.Relative));

        }

        private void LstAerte_OnTap(object sender, GestureEventArgs e)
        {
            var item = (e.OriginalSource as FrameworkElement).DataContext as AlertaUtilitate;
            if (item == null) return;
            if (e == null) return;
            NavigationService.Navigate(new Uri(string.Format("/Views/EditViewAlertaUtilitate.xaml?Id={0}", item.IdAlertaUilitate), UriKind.Relative));
        }


      
    }
}