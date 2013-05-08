using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HomeHelper.Common;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Repository.Concret;
using HomeHelper.Utils;
using HomeHelper.ViewModel;
using HomeHelper.Views;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
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
//        private readonly IRepository<Utilitati> _repository = FactoryRepository.GetInstanceRepositoryUtilitati();
//        private readonly IRepository<AlertaUtilitate> _repositoryAlerta = new AlertaUtilitateRepository(); 
//        private readonly IRepository<ConsumUtilitate> _repositoryConsum =
//            FactoryRepository.GetInstanceRepositoryConsum();
//
//        private int _lastUtilitateSelect = 0;
//        private LineSeries line;
        private MainViewModel _mainViewModel;
        public MainPage()
        {
            _mainViewModel = new MainViewModel();
            
            this.InitializeComponent();
            this.DataContext = _mainViewModel;
            this.Loaded += (s, e) => _mainViewModel.CurrentFrame = Frame;
            _mainViewModel.RefreshGraph=LineChart.UpdateLayout;
            
//            line = ((LineSeries)LineChart.Series[0]);
//            CreateLineSeries();
//            itemListUtilitati.ItemClick += itemListUtilitati_ItemClick;
//            itemListUtilitati.IsItemClickEnabled = true;
//            itemListAlerte.ItemClick += itemListAlerte_ItemClick;
//            itemListAlerte.IsItemClickEnabled = true;
//            itemListUtilitati.SelectionChanged +=ItemListUtilitatiOnSelectionChanged;
//            itemListAlerte.SelectionChanged += ItemListUtilitatiOnSelectionChanged;
        }

//        private void ItemListUtilitatiOnSelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            btnDelete.Visibility = (e.AddedItems.Any())
//                                                                                     ? Visibility.Visible
//                                                                                     : Visibility.Collapsed;
//            btnEdit.Visibility = (e.AddedItems.Any())
//                                     ? Visibility.Visible
//                                     : Visibility.Collapsed;
//            botomAppbar.IsOpen = e.AddedItems.Any();
//        }
//
//        void itemListAlerte_ItemClick(object sender, ItemClickEventArgs e)
//        {
//
//            var obj = e.ClickedItem as AlertaUtilitate;
//            if (obj == null) return;
//            Frame.Navigate(typeof (EditViewAlerta), obj.IdAlertaUilitate);
//
//        }
//
//        void itemListUtilitati_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            if (itemListUtilitati.SelectedItems.Any()) itemListUtilitati.SelectedItem=null;
//            var obj = e.ClickedItem as Utilitati;
//            LineChart.Title =
//                string.Format("Evolutia consumului utilitatii : {0}",
//                              obj.DenumireUtilitate);
//            line.Title = obj.DenumireUtilitate;
//            line.ItemsSource = obj.Consums;
//            _lastUtilitateSelect = obj.IdUtilitati;
//        }
//
//        private void CreateLineSeries()
//        {
//          
//           if(!LineChart.Series.Any()) LineChart.Series.Add(line);
//            line.SelectionChanged += line_SelectionChanged;
//        }
//
//
//        void line_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            var obj = line.SelectedItem as ConsumUtilitate;
//            if (obj == null) return;
//            Frame.Navigate(typeof(EditViewConsum), new Tuple<int, int>(obj.IdConsumUtilitate, obj.IdUtilitate));
//        }
//
//  
//        private void MockUpListItem()
//        {
//          
//            itemListUtilitati.ItemsSource = _repository.GetAll();
//            itemListAlerte.ItemsSource = _repositoryAlerta.GetAll();
//        }
//        /// <summary>
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
//
//        private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
//        {
//            try
//            {
//                var obj = itemListUtilitati.SelectedItem as Utilitati;
//                var objAlerta = itemListAlerte.SelectedItem as AlertaUtilitate;
//                if(obj!=null) _repository.Delete(obj);
//                if (objAlerta != null) _repositoryAlerta.Delete(objAlerta);
//                MockUpListItem();
//            }
//            catch (Exception ex)
//            {
//                var msg = new MessageDialog(ex.Message,ResurseMesaje.NumeFereastraEroare);
//                 msg.ShowAsync();
//
//            }
//
//        }
//
//        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
//        {
//            Frame.Navigate(typeof (EditView), 0);
//        }
//
//        private void BtnEdit_OnClick(object sender, RoutedEventArgs e)
//        {
//            var obj = itemListUtilitati.SelectedItem as Utilitati;
//            if (obj == null) return;
//            Frame.Navigate(typeof (EditView), obj.IdUtilitati);
//        }
//
//        private void BtnAddConsum_OnClick(object sender, RoutedEventArgs e)
//        {       
//            Frame.Navigate(typeof (EditViewConsum), new Tuple<int,int>(0,_lastUtilitateSelect));
//        }
//
//
//        private void BtnAddAlerta_OnClick(object sender, RoutedEventArgs e)
//        {
//            Frame.Navigate(typeof (EditViewAlerta), 0);
//        }
    }


}
