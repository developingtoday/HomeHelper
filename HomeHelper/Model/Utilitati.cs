using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Common;
using HomeHelper.Utils;
using SQLite;

namespace HomeHelper.Model
{
    public class Utilitati:BindableBase
    {
        private int _idUtilitati=0;

        [PrimaryKey,AutoIncrement]
        public int IdUtilitati
        {
            get { return _idUtilitati; }
            set { SetProperty(ref _idUtilitati, value); }
        }

        private string _denumireUtilitate = string.Empty;
        public string DenumireUtilitate
        {
            get { return _denumireUtilitate; }
            set
            {
                SetProperty(ref _denumireUtilitate, value,"DenumireUtilitate");
            }
        }

        private string _unitateMasura = string.Empty;
        public string UnitateMasura
        {
            get { return _unitateMasura; }
            set { SetProperty(ref _unitateMasura, value,"UnitateMasura"); }
        }

        private decimal _valoareInitiala = 0;
        public decimal ValoareInitiala
        {
            get { return _valoareInitiala; }
            set
            {
                SetProperty(ref _valoareInitiala, value,"ValoareInitiala");
            }
        }
        public IEnumerable<ConsumUtilitate> Consums
        {
            get
            {
                return
                    FactoryRepository.GetInstanceRepositoryConsum()
                                     .GetAll()
                                     .Where(x => x.IdUtilitate == _idUtilitati)
                                     .AsEnumerable();
            }
        } 

    }
}
