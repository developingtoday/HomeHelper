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

  
        public float IndexInitial { get; set; }
        public IEnumerable<ConsumUtilitate> Consums
        {
            get
            {
                return
                    FactoryRepository.GetInstanceRepositoryConsum()
                                     .GetAll()
                                     .Where(x => x.IdUtilitate == IdUtilitati).OrderBy(x => x.DataConsum)
                                     .AsEnumerable();
            }
        }

        public float GetConsumUtilitateLaData(DateTime data)
        {
            var list = Consums.ToList();
            if (!list.Any()) return 0;
            if (list.All(a => a.DataConsum > data)) return 0;
            var source = list.Where(a => a.DataConsum <= data).OrderByDescending(a=>a.DataConsum);
            var sum = source.Sum(a => -a.IndexUtilitate);
            return sum - IndexInitial;
        }

        public float ConsumActual
        {
            get { return GetConsumUtilitateLaData(DateTime.Now); }
        }

    }
}
