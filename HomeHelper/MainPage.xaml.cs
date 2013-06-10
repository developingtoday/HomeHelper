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

        private MainViewModel _mainViewModel;
        public MainPage()
        {
            _mainViewModel = new MainViewModel();
            
            this.InitializeComponent();
            this.DataContext = _mainViewModel;
            this.Loaded += (s, e) => _mainViewModel.CurrentFrame = Frame;
            _mainViewModel.RefreshGraph=LineChart.UpdateLayout;
            

        }


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

    }


}
