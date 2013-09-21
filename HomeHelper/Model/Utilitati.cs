using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Common;
using HomeHelper.Model.Abstract;
using HomeHelper.Utils;
using SQLite;
using Windows.ApplicationModel.Resources;

namespace HomeHelper.Model
{
    public class Utilitati:BindableBase,IValidation 
    {
       
        private readonly ResourceLoader loader=new ResourceLoader();

        [PrimaryKey,AutoIncrement]
        public int IdUtilitati { get; set; }
        

        public string DenumireUtilitate { get; set; }

 
        public string UnitateMasura { get; set; }

  
        public float IndexInitial { get; set; }

        public DateTime DataIndexInitial { get; set; }

        public string InformatieLunaCurenta
        {
            get { return string.Format("{2}: {0} {1}", ConsumActual, UnitateMasura, loader.GetString(resource: "LunaCurenta")); }
        }
        public string InformatieLunaAnterioara
        {
            get { return string.Format("{2}: {0} {1}", ConsumLunaAnterioara, UnitateMasura, loader.GetString(resource: "LunaAnterioara")); }
        }

        

        public bool IsCrestere
        {
            get { return ConsumActual > ConsumLunaAnterioara; }
        }
        public bool IsScadere
        {
            get { return ConsumActual < ConsumLunaAnterioara; }
        }
        public bool IsStagnare
        {
            get { return ConsumActual == ConsumLunaAnterioara; }
        }


        public IEnumerable<ConsumUtilitate> Consums
        {
            get
            {
              var list=
                    FactoryRepository.GetInstanceRepositoryConsum()
                                     .GetAll()
                                     .Where(x => x.IdUtilitate == IdUtilitati).OrderBy(x => x.DataConsum)
                                     ;
                ConsumUtilitateRefacereColectie(ref list);
                return list;
            }
        }

        public Utilitati()
        {
            DataIndexInitial = DateTime.Now;
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
            if (!Consums.Any()) return 0;
            var list =
                Consums.Where(a => a.DataConsum.Month == monthSearch.Month)
                       .OrderByDescending(a => a.DataConsum)
                       .FirstOrDefault();
            if (list == null)
            {
                firstDay = DataIndexInitial;
            }
            else
            {

                firstDay = list.DataConsum;   
            }
            return GetConsumUtilitateIntreDate(lastDay, firstDay);
        }

        public float GetConsumUtilitatePeLunaCurenta(DateTime time)
        {
            var lastDay = new DateTime(time.Year, time.Month, 1).AddMonths(1).AddDays(-1);
            var firstDay = new DateTime(time.Year, time.Month, 1);
            var monthSearch = firstDay.AddDays(-1);
            if (!Consums.Any()) return 0;
            var aux = Consums.Where(a => a.DataConsum.Month == monthSearch.Month)
                             .OrderByDescending(a => a.DataConsum);
            var list =
                Consums.Where(a => a.DataConsum.Month == monthSearch.Month)
                       .OrderByDescending(a => a.DataConsum)
                       .FirstOrDefault();
            if (list == null)
            {
                firstDay = DataIndexInitial;
            }
            else
            {

                firstDay = list.DataConsum;
            }
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
            var source = list.Where(a => dataI.Date <=a.DataConsum.Date && a.DataConsum.Date <= dataF.Date).OrderBy(a=>a.DataConsum);
            if (!source.Any()) return 0;
            var indexOld=source.FirstOrDefault().IndexUtilitate;
            if (dataI == DataIndexInitial)
            {
                indexOld = IndexInitial;
            }
            return Math.Abs(source.LastOrDefault().IndexUtilitate- indexOld);
        }

        private void ConsumUtilitateRefacereColectie(ref IOrderedEnumerable<ConsumUtilitate> source)
        {
            //TODO este duplicat cu o metoda mai jos
            if (!source.Any()) return;
            if (source.Count() == 1)
            {
                source.FirstOrDefault().Consum = source.FirstOrDefault().IndexUtilitate - IndexInitial;
                return;
            }
            ConsumUtilitate prev = source.FirstOrDefault();
            prev.Consum = prev.IndexUtilitate - IndexInitial;
            for (int i = 1; i < source.Count(); i++)
            {
                source.ElementAt(i).Consum = source.ElementAt(i).IndexUtilitate - source.ElementAt(i - 1).IndexUtilitate;
            }
            
        }

        private float ConsumUtilitateLaData(IOrderedEnumerable<ConsumUtilitate> source)
        {
            if (source.Count() == 1)
            {
                return source.FirstOrDefault().IndexUtilitate - IndexInitial;
            }
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
            return sum;
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


        public void DoValidation()
        {
            
            _errors = new List<StringKeyValue>();
            if (string.IsNullOrEmpty(DenumireUtilitate))
            {
                _errors.Add(new StringKeyValue()
                               {
                                   Key = "DenumireUtilitate",
                                   Value = loader.GetString(resource: "DenumireUtilitateCampGol")
                               });
            }
            if (string.IsNullOrEmpty(UnitateMasura))
            {
                _errors.Add(new StringKeyValue()
                               {
                                   Key = "UnitateMasura",
                                   Value = loader.GetString(resource: "UnitateMasuraCampGol")
                               });
            }
            if (IndexInitial < 0)
            {
                _errors.Add(new StringKeyValue()
                               {
                                   Key = "IndexInitial",
                                   Value = loader.GetString(resource: "IndexInitialValoareInvalida")
                               });
            }
            if (IndexInitial > 0)
            {
                var listConsum =
                    FactoryRepository.GetInstanceRepositoryConsum()
                                     .GetAll()
                                     .Where(a => a.IdUtilitate == IdUtilitati)
                                     .OrderBy(a => a.DataConsum)
                                     .FirstOrDefault(a => DataIndexInitial.Date <= a.DataConsum.Date);
                if (listConsum != null)
                {
                    if(IndexInitial>listConsum.IndexUtilitate)
                    _errors.Add(new StringKeyValue()
                                    {
                                        
                                        Key = "IndexInitial",
                                        Value = string.Format("{0} {1}",loader.GetString(resource:"IndexMaiMareDecat"),IndexInitial)
                                    });
                }
            }
            if (DataIndexInitial != DateTime.MinValue)
            {
                var list =
                    FactoryRepository.GetInstanceRepositoryConsum()
                                     .GetAll()
                                     .Where(a => a.IdUtilitate == IdUtilitati).OrderBy(a=>a.DataConsum)
                                     .ToList();
                if (list.Any())
                {
                    var minDate = list.Min(a => a.DataConsum).Date;
                    var maxDate = list.Max(a => a.DataConsum).Date;
                    var condInterval = DataIndexInitial.Date > minDate || DataIndexInitial.Date > maxDate;
                    if (condInterval)
                    {
                        _errors.Add(new StringKeyValue()
                                        {
                                            Key = "DataIndexInitial",
                                            Value = loader.GetString(resource: "DataIndexInitialRespectareConsumInitial")
                                                                                                    
                                        });
                    }
                }
            }

        }

        private List<StringKeyValue> _errors;
        

        public List<StringKeyValue> GetErrors()
        {
            return _errors;
        }
    }
}
