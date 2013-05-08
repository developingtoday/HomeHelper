using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Common;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Repository.Concret;

namespace HomeHelper.ViewModel
{
    public class UtilitateInputViewModel:InputViewModelBase<Utilitati>
    {
        public UtilitateInputViewModel(IRepository<Utilitati> repository) : base(repository)
        {
        }
    }

    public class ConsumUtilitateInputViewModel:InputViewModelBase<ConsumUtilitate>
    {
        private readonly IRepository<Utilitati> _repositoryUtilitati=new UtilitatiRepository();  
        public ConsumUtilitateInputViewModel(IRepository<ConsumUtilitate> repository) : base(repository)
        {

        }
        public bool IsEnabledCombobox
        {
            get { return ObiectInBinding.IdUtilitate == 0 || ObiectInBinding.IdConsumUtilitate==0; }
        }
        public ObservableCollection<Utilitati> ListaUtilitati
        {
            get { return _repositoryUtilitati.GetAll(); }
        }  

    }
}
