﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public DateTime DataConsum
        {
            get { return dtpLocal.SelectedDate.GetValueOrDefault(); }
        }
        public decimal ValoareConsum
        {
            get { return Convert.ToDecimal(txtConsum.Text); }
        }
    }
}
