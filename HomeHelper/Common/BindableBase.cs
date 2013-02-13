using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace HomeHelper.Common
{
    /// <summary>
    /// Implementation of <see cref="INotifyPropertyChanged"/> to simplify models.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public class RelayCommand:ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> exec ):this(exec,null)
        {
        }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute  )
        {
            if(execute==null) throw new ArgumentNullException("The delegate for the execution is null");
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            return (_canExecute == null) || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter); 
        }

        public void RaiseCanExecuteChanged()
        {
            var copy = CanExecuteChanged;
            if(copy!=null) copy(this,new EventArgs());
        }
        public event EventHandler CanExecuteChanged;

    }
    public class ListViewClickCommand
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command",
                                                                                                        typeof (ICommand
                                                                                                            ),
                                                                                                        typeof (
                                                                                                            ListViewClickCommand
                                                                                                            ),
                                                                                                        new PropertyMetadata
                                                                                                            (null,
                                                                                                             CommadPropertyChanged));

        public static void SetCommand(DependencyObject aO, ICommand val)
        {
            aO.SetValue(CommandProperty,val);
        }
        public static ICommand GetCommand(DependencyObject aO)
        {
           return (ICommand) aO.GetValue(CommandProperty);
        }
        private static void CommadPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ListView).ItemClick += (s, e1) =>
                                             {
                                                 var cast = s as ListView;
                                                 var cmd = GetCommand(cast);
                                                 cmd.Execute(e1.ClickedItem);
                                             };
        } 
    }
}
