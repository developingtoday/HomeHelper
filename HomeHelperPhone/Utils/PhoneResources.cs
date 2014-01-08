using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Input;
using HomeHelper.Model;
using HomeHelper.Model.Abstract;
using HomeHelper.Repository.Abstract;
using HomeHelper.Utils;
using HomeHelperPhone.Resources;
using Microsoft.Phone.Scheduler;
using DatePicker = Microsoft.Phone.Controls.DatePicker;

namespace HomeHelperPhone.Utils
{
    public class PhoneResources:IResources 
    {

        public string GetString(string resource)
        {
            return AppResources.ResourceManager.GetString(resource);
        }
    }

    public static class Extension
    {
        public static void FocusUpdateLastElement()
        {
            var focused = FocusManager.GetFocusedElement();
            if (focused is TextBox)
            {
                var binding = (focused as TextBox).GetBindingExpression(TextBox.TextProperty);
                if (binding != null) binding.UpdateSource();
            }
            if (focused is DatePicker)
            {
                var binding = (focused as DatePicker).GetBindingExpression(DatePicker.ValueProperty);
                if (binding != null) binding.UpdateSource();
            }

        }

        public static readonly DependencyProperty MultiSeriesProperty =
            DependencyProperty.RegisterAttached("MultiSeries", typeof (ObservableCollection<ISeries>), typeof (Extension), new PropertyMetadata(default(ObservableCollection<ISeries>),DataChanged));

        private static void DataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLineIf(d==null,"The object is null");
            var cast = d as Chart;
            if (cast == null) return;
            var obj = GetMultiSeries(d);
            if (obj == null) return;
            cast.Series.Clear();
            foreach (var s in obj)
            {
                cast.Series.Add(s);
            }
        }

        public static void SetMultiSeries(DependencyObject dependencyObject, ObservableCollection<ISeries> val)
        {
            dependencyObject.SetValue(MultiSeriesProperty,val);
        }

        public static ObservableCollection<ISeries> GetMultiSeries(DependencyObject d)
        {
            return (ObservableCollection<ISeries>) d.GetValue(MultiSeriesProperty);
        }

     
    }

    public class WindowsPhoneSchedulerAlert:IScheduleRepository
    {
        
 
        public void AddAlertToSchedule(AlertaUtilitate t)
        {
            if (t.IdAlertaUilitate != 0)
            {
                DeleteFromSchedule(t);
            }
            var recurence = RecurrenceInterval.None;
            switch (t.FrecventaAlerta)
            {
                case (int)RepetareAlerta.Anual:
                    recurence=RecurrenceInterval.Yearly;
                    break;
                case (int)RepetareAlerta.Lunar:
                    recurence=RecurrenceInterval.Monthly;
                    break;
                case (int)RepetareAlerta.Saptamanal:
                    recurence=RecurrenceInterval.Weekly;
                    break;
                case (int)RepetareAlerta.Zilnic:
                    recurence=RecurrenceInterval.Daily;
                    break;
            }
            var alarm = new Reminder(t.IdAlertaUilitate.ToString());
            alarm.Content = string.Format(DbUtils.Loader.GetString("AlertaIndexConsum"), t.NumeUtilitate);
            alarm.BeginTime = t.DataAlerta;
            alarm.RecurrenceType = recurence;
            alarm.NavigationUri = new Uri("/Views/EditViewConsumUtilitate.xaml", UriKind.Relative);
            
            ScheduledActionService.Add(alarm);
        }

        public void DeleteFromSchedule(AlertaUtilitate t)
        {
            var found = ScheduledActionService.Find(t.IdAlertaUilitate.ToString());
            if (found==null) return;
            ScheduledActionService.Remove(t.IdAlertaUilitate.ToString());
        }
    }
}
