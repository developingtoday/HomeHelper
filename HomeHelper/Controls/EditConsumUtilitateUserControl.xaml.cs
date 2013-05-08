using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HomeHelper.Common;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
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
    public sealed partial class EditConsumUtilitateUserControl : UserControl
    {

        public EditConsumUtilitateUserControl()
        {
            this.InitializeComponent();
        }
        public EditConsumUtilitateUserControl(InputViewOperatiune op, ConsumUtilitate c)
        {
            this.InitializeComponent();
            var viewModel = new ConsumUtilitateInputViewModel(new ConsumUtilitateRepository())
                                {
                                    Operatiune = op,
                                    ObiectInBinding = c
                                };
            DataContext = viewModel;
        }
    }
}
