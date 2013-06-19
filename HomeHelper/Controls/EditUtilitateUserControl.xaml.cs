using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HomeHelper.Common;
using HomeHelper.Model;
using HomeHelper.Repository.Concret;
using HomeHelper.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace HomeHelper.Controls
{
    public sealed partial class EditUtilitateUserControl : UserControl
    {
        public EditUtilitateUserControl()
        {
            this.InitializeComponent();
            var _viewModel = new UtilitateInputViewModel(new UtilitatiRepository());
            DataContext = _viewModel;
         
        }
        public EditUtilitateUserControl(InputViewOperatiune op, Utilitati t)
        {
            this.InitializeComponent();
            var _viewModel = new UtilitateInputViewModel(new UtilitatiRepository());
            _viewModel.ObiectInBinding = t;
            _viewModel.Operatiune = op;
            DataContext = _viewModel;
        }

      

    }
}
