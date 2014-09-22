using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HomeHelper.Common;
using HomeHelper.Model;
using HomeHelper.Repository.Concret;
using HomeHelper.Utils;
using HomeHelper.ViewModel;

namespace HomeHelperPhone.Views
{
    public partial class EditViewUtilitate : EditViewUtilitateGeneric
    {
        public EditViewUtilitate()
        {
            InitializeComponent();           
        }


        
    }
    public class EditViewUtilitateGeneric:PhoneViewBaseForViewModel<Utilitati>
    {
        protected EditViewUtilitateGeneric():this(() => new UtilitateInputViewModel(FactoryRepository.GetInstanceRepositoryUtilitati()))
        {
            
        }
        private EditViewUtilitateGeneric(Func<InputViewModelBase<Utilitati>> viewModelCtor) : base(viewModelCtor)
        {

        }
    }
}