using System;
using System.Collections.Generic;
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
}
