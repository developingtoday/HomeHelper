using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DatePicker = Microsoft.Phone.Controls.DatePicker;

namespace HomeHelperPhone
{
    public partial class FilterDataGrafic : UserControl
    {

        public FilterDataGrafic()
        {
            InitializeComponent();
            Loaded += FilterDataGrafic_Loaded;
        }

        void FilterDataGrafic_Loaded(object sender, RoutedEventArgs e)
        {
           if(FromDateBinding!=null) dtpFrom.SetBinding(DatePicker.ValueProperty, FromDateBinding);
           if(ToDateBinding!=null) dtpTo.SetBinding(DatePicker.ValueProperty, ToDateBinding);
           if(FilterCommandBinding!=null) btnFilter.SetBinding(Button.CommandProperty, FilterCommandBinding);
        }

        public Binding FromDateBinding { get; set; }
        public Binding ToDateBinding { get; set; }
        public Binding FilterCommandBinding { get; set; }
        
        public DateTime FromDate
        {
            get { return dtpFrom.Value.GetValueOrDefault(); }
            set { dtpFrom.Value = value; }
        }

        public DateTime ToDate
        {get { return dtpTo.Value.GetValueOrDefault(); } set { dtpTo.Value = value; }}

        public ICommand FilterCommand
        {
            get { return btnFilter.Command; }
            set { btnFilter.Command = value; }
        }


    }
}
