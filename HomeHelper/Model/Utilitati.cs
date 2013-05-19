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
            var source = list.Where(a => a.DataConsum <= data).OrderBy(a=>a.DataConsum);
            return ConsumUtilitateLaData(source);
        }

        /// <summary>
        /// Calculeaza consum pe ultima luna. Daca sunt in luna mai o sa am afisat pe luna aprilie
        /// </summary>
        /// <param name="time">Data de referinta</param>
        public float GetConsumUtilitatePeLunaAnterioara(DateTime time)
        {
            var lastDay = new DateTime(time.Year, time.Month, 1).AddDays(-1);
            var firstDay = new DateTime(time.Year, time.Month, 1).AddMonths(-1);
            var monthSearch = firstDay.AddDays(-1);
            var list =
                Consums.Where(a => a.DataConsum.Month == monthSearch.Month)
                       .OrderByDescending(a => a.DataConsum)
                       .LastOrDefault();
            firstDay = list.DataConsum;
            return GetConsumUtilitateIntreDate(lastDay, firstDay);
        }

        public float GetConsumUtilitatePeLunaCurenta(DateTime time)
        {
            var lastDay = new DateTime(time.Year, time.Month, 1).AddMonths(1).AddDays(-1);
            var firstDay = new DateTime(time.Year, time.Month, 1);
            var monthSearch = firstDay.AddDays(-1);
            var list =
                Consums.Where(a => a.DataConsum.Month == monthSearch.Month)
                       .OrderByDescending(a => a.DataConsum)
                       .LastOrDefault();
            firstDay = list.DataConsum; 
            return GetConsumUtilitateIntreDate(lastDay, firstDay);
        }

        private float GetConsumUtilitateIntreDate(DateTime dataI, DateTime dataF)
        {
            var list = Consums.ToList();
            if (!list.Any()) return 0;
            if (dataI == dataF)
            {
                return GetConsumUtilitateLaData(dataI);
            }
            if (dataI > dataF)
            {
                var aux = dataI;
                dataI = dataF;
                dataF = aux;
            }
            var source = list.Where(a => dataI <=a.DataConsum && a.DataConsum <= dataF).OrderBy(a=>a.DataConsum);
            if (!source.Any()) return 0;
            return ConsumUtilitateLaData(source);
        }

        private float ConsumUtilitateLaData(IOrderedEnumerable<ConsumUtilitate> source)
        {
            var stack = new Stack<ConsumUtilitate>(source);
            var first = stack.Pop().IndexUtilitate;
            float usedDif = 0;
            float sum = 0;
            if (!stack.Any()) return 0;
            while (stack.Any())
            {
                usedDif = stack.Pop().IndexUtilitate;
                first = first - usedDif;
                sum += first;
                first = usedDif;
            }
            return sum - IndexInitial;
        }

        public float ConsumActual
        {
            get { return GetConsumUtilitatePeLunaCurenta(DateTime.Now); }
        }
        public float ConsumLunaAnterioara
        {
            get { return GetConsumUtilitatePeLunaAnterioara(DateTime.Now); }
        }
        public float ConsumPanaLaData
        {
            get
            {
                return
                    GetConsumUtilitateLaData(DateTime.Now);
            }
        }


    }
}
