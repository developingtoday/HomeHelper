using System;
using System.Linq;
using System.Windows.Navigation;
using HomeHelper.Common;
using HomeHelper.Model.Abstract;
using HomeHelperPhone.Utils;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace HomeHelperPhone.Views
{
    public class PhoneViewBaseForViewModel<T>:PhoneApplicationPage where T : IValidation, new()
    {
        private readonly InputViewModelBase<T> _viewModelBase;
        private ApplicationBarIconButton _btnSave, _btnCancel, _btnDelete;

        protected PhoneViewBaseForViewModel(Func<InputViewModelBase<T>> viewModelCtor)
        {
            _viewModelBase = viewModelCtor();
            InitStuff();
        }

        protected InputViewModelBase<T> ViewModelBase
        {
            get { return _viewModelBase; }
        } 

        private void InitStuff()
        {
            DataContext = _viewModelBase;
            _viewModelBase.ObiectInBinding = new T();
            AddControls();
        }

        private void AddControls()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            _btnSave = new ApplicationBarIconButton();
            _btnSave.Text = "Save";
            _btnSave.IconUri = new Uri("/Toolkit.Content/save.png",UriKind.Relative);
            _btnDelete = new ApplicationBarIconButton();
            _btnDelete.Text = "Delete";
            _btnDelete.IconUri = new Uri("/Toolkit.Content/ApplicationBar.Delete.png", UriKind.Relative);
            _btnCancel=new ApplicationBarIconButton();
            _btnCancel.IconUri = new Uri("/Toolkit.Content/ApplicationBar.Cancel.png", UriKind.Relative);
            _btnCancel.Text = "Cancel";
            ApplicationBar.Buttons.Add(_btnSave);
            ApplicationBar.Buttons.Add(_btnDelete);
            ApplicationBar.Buttons.Add(_btnCancel);
            _btnSave.Click += BtnSaveClick;
            _btnCancel.Click += BtnCancelClick;
            _btnDelete.Click += BtnDeleteClick;
        }

        private void BtnDeleteClick(object sender, EventArgs e)
        {
            _viewModelBase.DeleteCommand.Execute(_viewModelBase.ObiectInBinding);
            NavigationService.GoBack();
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            _viewModelBase.CancelCommand.Execute(_viewModelBase.ObiectInBinding);
            NavigationService.GoBack();
        }

        private  void BtnSaveClick(object sender, EventArgs e)
        {
            Extension.FocusUpdateLastElement();
            _viewModelBase.SaveCommand.Execute(_viewModelBase.ObiectInBinding);
            if (_viewModelBase.Erori.Any()) return;
            NavigationService.GoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.Back) return;
            var param = string.Empty;
            if (NavigationContext.QueryString.TryGetValue("Id", out param))
            {

                _viewModelBase.ObiectInBinding = _viewModelBase.Repository.GetById(int.Parse(param));
            }
            else
            {
                _viewModelBase.ObiectInBinding = new T();
            }
        }
    }
}