using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Model.Abstract;
using HomeHelper.Repository.Concret;

namespace HomeHelper.Model
{
    public class ConsumUtilitate:IValidation
    {
        public ConsumUtilitate()
        {
            DataConsum = DateTime.Now;
        }
        [SQLite.PrimaryKey,SQLite.AutoIncrement]
        public int IdConsumUtilitate { get; set; }
        [SQLite.Indexed]
        public int IdUtilitate { get; set; }
        public DateTime DataConsum { get; set; }
        public float IndexUtilitate { get; set; }
        public float Consum { get; set; }
        public string DataConsumGrafic
        {
            get { return DataConsum.ToString("d"); }
        }
        public override string ToString()
        {
            return string.Format("IdConsumUtilitate: {0}, IdUtilitate: {1}, DataConsum: {2}, IndexUtilitate: {3}", IdConsumUtilitate, IdUtilitate, DataConsum, IndexUtilitate);
        }

        public void DoValidation()
        {
            Errors = new List<StringKeyValue>( );
            if (IdUtilitate == 0)
            {
                Errors.Add(new StringKeyValue()
                               {
                                   Key = "IdUtilitate",
                                   Value = "Utilitatea n-a fost selectata"
                               });
            }
            if (IndexUtilitate < 0)
            {
                Errors.Add(new StringKeyValue()
                               {
                                   Key = "IndexUtilitate",
                                   Value = "Valoare invalida la index"
                               });
            }
            if (IdUtilitate != 0)
            {
                var cons = new ConsumUtilitateRepository();
                var rep = new UtilitatiRepository();
                var util = rep.GetById(IdUtilitate);
                var list = cons.GetAll().Where(a=>a.IdUtilitate==IdUtilitate && a.IdConsumUtilitate!=IdConsumUtilitate).OrderBy(a=>a.DataConsum).ToArray();
                if (list.Any(a => a.IdUtilitate == IdUtilitate && a.DataConsum.Date == DataConsum.Date))
                {
                    Errors.Add(new StringKeyValue()
                                   {
                                       Key = "DataConsum",
                                       Value = "Pe aceasta data mai exista adaugat un consum"
                                   });
                }
                if (list.Any())
                {
                    var maxData = list.Max(a => a.DataConsum.Date);
                    var minData = list.Min(a => a.DataConsum.Date);
                    if (minData <= DataConsum && DataConsum <= maxData)
                    {
                        var first =
                            list.OrderByDescending(a => a.DataConsum)
                                .FirstOrDefault(a => a.DataConsum.Date <= DataConsum.Date);
                        var last = list.FirstOrDefault(a => a.DataConsum.Date >= DataConsum.Date);
                        if (!(first.IndexUtilitate <= IndexUtilitate && IndexUtilitate <= last.IndexUtilitate))
                        {
                            Errors.Add(new StringKeyValue()
                                           {
                                               Key = "IndexUtilitate",
                                               Value =
                                                   string.Format("Indexul nu se afla in intervalul {0}-{1}",
                                                                 first.IndexUtilitate, last.IndexUtilitate)
                                           });
                        }
                    }
                    else if (DataConsum <= minData)
                    {
                        var first = list.FirstOrDefault(a => a.DataConsum.Date >= DataConsum.Date);
                        if (!(util.IndexInitial <= IndexUtilitate && IndexUtilitate <= first.IndexUtilitate))
                        {
                            Errors.Add(new StringKeyValue()
                                           {
                                               Key = "IndexUtilitate",
                                               Value =
                                                   string.Format("Indexul nu se afla in intervalul {0}-{1}",
                                                                 util.IndexInitial, first.IndexUtilitate)
                                           });
                        }
                    }
                    else if (DataConsum >= maxData)
                    {
                        var last = list.LastOrDefault(a => a.DataConsum.Date <= DataConsum.Date);
                        if (IndexUtilitate < last.IndexUtilitate)
                        {
                            Errors.Add(new StringKeyValue()
                                           {
                                               Key = "IndexUtilitate",
                                               Value =
                                                   string.Format("Indexul este mai mic decat {0}", last.IndexUtilitate)
                                           });
                        }
                    }
                }
                else
                {
                    if (IndexUtilitate < util.IndexInitial)
                    {
                        Errors.Add(new StringKeyValue()
                                       {
                                           Key = "IndexUtilitate",
                                           Value = string.Format("Indexul este mai mic decat {0}",util.IndexInitial)
                                       });
                    }
                }
            }

        }

        public List<StringKeyValue> Errors { get; private set; }
    }
}
