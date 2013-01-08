using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Repository.Concret;
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
        private IRepository<Utilitati> rep = new UtilitatiRepository(); 


        void EditConsumUtilitateUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbUtilitate.ItemsSource = rep.GetAll();
            cmbUtilitate.DisplayMemberPath = "DenumireUtilitate";
            cmbUtilitate.SelectedValuePath = "IdUtilitati";

        }
        public EditConsumUtilitateUserControl()
        {
            this.InitializeComponent();
            Loaded += EditConsumUtilitateUserControl_Loaded;
        }
        public DateTime DataConsum
        {
            get { return dtpLocal.Date; }
        }
        public float ValoareConsum
        {
            get { return float.Parse(txtConsum.Text); }
        }


        
    }
}
