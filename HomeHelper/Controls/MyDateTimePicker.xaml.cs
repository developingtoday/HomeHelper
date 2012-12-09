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
    public sealed partial class MyDateTimePicker : UserControl
    {
        private bool initializing = true;
        public MyDateTimePicker()
        {
            InitializeComponent();

            UpdateValues(0, 0);
            Day.SelectedIndex = 0;
            Month.SelectedIndex = 0;
            Year.SelectedIndex = 0;
            initializing = false; 
        }

        public static readonly DependencyProperty AllowNullProperty =
            DependencyProperty.Register("AllowNull", typeof(bool), typeof(MyDateTimePicker), new PropertyMetadata(true));

        public bool AllowNull
        {
            get { return (bool)GetValue(AllowNullProperty); }
            set { SetValue(AllowNullProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(MyDateTimePicker), new PropertyMetadata(null, OnSelectedItemChanged));

        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public event RoutedEventHandler SelectedItemChanged; 

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (MyDateTimePicker)d;
            if (ctrl.initializing)
                return;

            ctrl.initializing = true;
            ctrl.UpdateDate();
            ctrl.initializing = false; 

            if (ctrl.SelectedItemChanged != null)
                ctrl.SelectedItemChanged(ctrl, new RoutedEventArgs());
        }

        public void UpdateDate()
        {
            if (SelectedDate.HasValue)
            {
                UpdateValues(SelectedDate.Value.Year, SelectedDate.Value.Month);

                if (AllowNull)
                {
                    Day.SelectedIndex = SelectedDate.Value.Day;
                    Month.SelectedIndex = SelectedDate.Value.Month;
                    //Year.SelectedIndex = SelectedDate.Value.Year - 2000 + 1;
                    Year.SelectedIndex = SelectedDate.Value.Year;
                }
                else
                {
                    Day.SelectedIndex = SelectedDate.Value.Day - 1;
                    Month.SelectedIndex = SelectedDate.Value.Month - 1;
                    Year.SelectedIndex = SelectedDate.Value.Year - 2000;
                }
            }
            else
            {
                UpdateValues(0, 0);
                Day.SelectedIndex = 0;
                Month.SelectedIndex = 0;
                Year.SelectedIndex = 0;
            }
        }

        public void UpdateValues(int year, int month)
        {
            var days = new List<string>();
            if (AllowNull)
                days.Add(" ");
            for (var i = 1; i <= 31; i++)//(year != 0 && month != 0 ? DateTime.DaysInMonth(year, month) : 31); i++)
                days.Add(i.ToString());

            var months = new List<string>();
            if (AllowNull)
                months.Add(" ");
            for (var i = 1; i <= 12; i++)
                months.Add(i.ToString());

            var years = new List<string>();
            if (AllowNull)
                years.Add(" ");
            for (var i = 0; i <= 5000; i++)
                years.Add(i.ToString());

            if (Month.SelectedIndex > months.Count - 1)
                Month.SelectedIndex = months.Count - 1; 

            Day.ItemsSource = days;
            Month.ItemsSource = months;
            Year.ItemsSource = years; 
        }

        private void Day_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initializing)
                return;

            var hour = SelectedDate != null ? SelectedDate.Value.Hour : 0;
            var minute = SelectedDate != null ? SelectedDate.Value.Minute : 0;
            var second = SelectedDate != null ? SelectedDate.Value.Second : 0;

            initializing = true;
            if (AllowNull && (Day.SelectedIndex == 0 || Month.SelectedIndex == 0 || Year.SelectedIndex == 0))
                SelectedDate = null;
            else
            {
                if (AllowNull)
                    SelectedDate = new DateTime(Year.SelectedIndex + 2000 - 1, Month.SelectedIndex, Day.SelectedIndex, hour, minute, second);
                else
                    SelectedDate = new DateTime(Year.SelectedIndex + 2000, Month.SelectedIndex + 1, Day.SelectedIndex + 1, hour, minute, second);
            }

            //if (SelectedItem.HasValue)
            //  UpdateValues(SelectedItem.Value.Year, SelectedItem.Value.Month);
            //else
            //  UpdateValues(0, 0);

            initializing = false; 
        }
    }
}
