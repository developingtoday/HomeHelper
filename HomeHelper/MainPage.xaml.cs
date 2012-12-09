using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using HomeHelper.Common;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Utils;
using HomeHelper.Views;
using ModernUI.Toolkit.Data.Charting.Charts.Series;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace HomeHelper
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : HomeHelper.Common.LayoutAwarePage
    {
        private readonly IRepository<Utilitati> _repository = FactoryRepository.GetInstanceRepositoryUtilitati();

        private readonly IRepository<ConsumUtilitate> _repositoryConsum =
            FactoryRepository.GetInstanceRepositoryConsum(); 
        public MainPage()
        {
            this.InitializeComponent();
            itemListUtilitati.ItemClick += (s, e) =>
                                               {
                                                   var obj = e.ClickedItem as Utilitati;
                                                   //itemListConsum.ItemsSource = obj.Consums;
                                                   //LineChart.DataContext = obj.Consums;
                                                   var line = ((LineSeries)LineChart.Series[0]);
                                                   line.DataContext = obj.Consums;
                                                   line.ItemsSource = obj.Consums;
                                                   
                                               };
            itemListUtilitati.IsItemClickEnabled = true;

        }


        private void MockUpListItem()
        {
          
            itemListUtilitati.ItemsSource = _repository.GetAll();
        }
        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            MockUpListItem();
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = itemListUtilitati.SelectedItem as Utilitati;
                _repository.Delete(obj);
                MockUpListItem();
            }
            catch (Exception ex)
            {
                var msg = new MessageDialog(ex.Message,ResurseMesaje.NumeFereastraEroare);
                 msg.ShowAsync();

            }

        }

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (EditView), 0);
        }

        private void BtnEdit_OnClick(object sender, RoutedEventArgs e)
        {
            var obj = itemListUtilitati.SelectedItem as Utilitati;
            if (obj == null) return;
            Frame.Navigate(typeof (EditView), obj.IdUtilitati);
        }

        private void BtnAddConsum_OnClick(object sender, RoutedEventArgs e)
        {
            var obj = itemListUtilitati.SelectedItem as Utilitati;
            if (obj == null) return;
            Frame.Navigate(typeof (EditViewConsum), new Tuple<int,int>(0,obj.IdUtilitati));
        }

        private void BtnDeleteConsum_OnClick(object sender, RoutedEventArgs e)
        {
            //var obj = itemListConsum.SelectedItem as ConsumUtilitate;
            //if (obj == null) return;
            //_repositoryConsum.Delete(obj);
            //MockUpListItem();
        }

        private void BtnEditConsum_OnClick(object sender, RoutedEventArgs e)
        {
            //var obj = itemListConsum.SelectedItem as ConsumUtilitate;
            //if (obj == null) return;
            //Frame.Navigate(typeof(EditViewConsum), new Tuple<int, int>(obj.IdConsumUtilitate, obj.IdUtilitate));
           
        }
    }
}
