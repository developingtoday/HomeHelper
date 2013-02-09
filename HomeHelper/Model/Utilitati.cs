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
        public int IdUtilitati { get; set; }
        

        private string _denumireUtilitate = string.Empty;
        public string DenumireUtilitate { get; set; }

        private string _unitateMasura = string.Empty;
        public string UnitateMasura { get; set; }

        private decimal _valoareInitiala = 0;
        public float ValoareInitiala { get; set; }
        public IEnumerable<ConsumUtilitate> Consums
        {
            get
            {
                return
                    FactoryRepository.GetInstanceRepositoryConsum()
                                     .GetAll()
                                     .Where(x => x.IdUtilitate == _idUtilitati).OrderBy(x=>x.DataConsum)
                                     .AsEnumerable();
            }
        } 

    }
}
