using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HomeHelper.Model;
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
        }

        public Utilitati GetViewModel()
        {
            var context = DataContext as Utilitati;
            context.DenumireUtilitate = txtUtilitate.Text;
            context.UnitateMasura = txtUnitate.Text;
            context.ValoareInitiala = int.Parse(txtValoareInitiala.Text);
            return context;
        }

    }
}
