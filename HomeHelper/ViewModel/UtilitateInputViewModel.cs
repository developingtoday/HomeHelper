using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Common;
using HomeHelper.Model;
using HomeHelper.Model.Abstract;
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


#if !NETFX_CORE
        private Utilitati _utilitateSelect;
        public Utilitati UtilitateSelectata
        {
            get { return _utilitateSelect; }
            set
            {
                if (value == null) return;
                ObiectInBinding.IdUtilitate = value.IdUtilitati;
                SetProperty(ref _utilitateSelect, value, "UtilitateSelectata");
            }
        }

        public override ConsumUtilitate ObiectInBinding
        {
            get
            {
                return base.ObiectInBinding;
            }
            set
            {
                if (value == null) return;
                UtilitateSelectata = ListaUtilitati.FirstOrDefault(a => a.IdUtilitati == value.IdUtilitate);
                base.ObiectInBinding = value;
            }
        }
#endif  
        

    }

    public class AlertaUtilitateViewModel:InputViewModelBase<AlertaUtilitate>
    {
        private readonly IRepository<Utilitati> _repositoryUtilitati=new UtilitatiRepository(); //TODO facut cumva collection view?? 
        //private readonly List<StringIntKeyValue> _list = Util.FrecventeAlerte(); 
        public AlertaUtilitateViewModel(IRepository<AlertaUtilitate> repository ):base(repository)
        {
            
        }
        public List<StringIntKeyValue> Frecvente
        {
            get { return Util.FrecventeAlerte(); }
        }
        public ObservableCollection<Utilitati> ListaUtilitati
        {
            get { return _repositoryUtilitati.GetAll(); }
        }

#if !NETFX_CORE
        private Utilitati _utilitateSelect;
        public Utilitati UtilitateSelectata
        {
            get { return _utilitateSelect; }
            set
            {
                if (value == null) return;
                ObiectInBinding.IdUitlitate = value.IdUtilitati;
                SetProperty(ref _utilitateSelect, value, "UtilitateSelectata");
            }
        }

        public override AlertaUtilitate ObiectInBinding
        {
            get
            {
                return base.ObiectInBinding;
            }
            set
            {
                if (value == null) return;
                UtilitateSelectata = ListaUtilitati.FirstOrDefault(a => a.IdUtilitati == value.IdUitlitate);
                base.ObiectInBinding = value;
            }
        }
#endif  
    }
}
