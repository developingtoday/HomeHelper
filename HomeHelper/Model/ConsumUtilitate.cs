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
                var list = cons.GetAll().Where(a=>a.IdUtilitate==IdUtilitate && a.IdConsumUtilitate!=IdConsumUtilitate).ToArray();
                if (list.Any(a => a.IdUtilitate == IdUtilitate && a.DataConsum == DataConsum))
                {
                    Errors.Add(new StringKeyValue()
                                   {
                                       Key = "DataConsum",
                                       Value = "Pe aceasta data mai exista adaugat un consum"
                                   });
                }
                if (list.Any())
                {
                    var last = list.OrderBy(a => a.DataConsum).LastOrDefault(a => a.DataConsum <= DataConsum);
                    var first = list.OrderBy(a => a.DataConsum).LastOrDefault(a => a.DataConsum >= DataConsum);
                    if (last != null && first != null)
                    {
                        var prevConsum = last.IndexUtilitate;
                        var nextConsum = first.IndexUtilitate;
                        if (!(prevConsum <= IndexUtilitate && IndexUtilitate <= nextConsum))
                        {
                            Errors.Add(new StringKeyValue()
                                           {
                                               Key = "IndexUtilitate",
                                               Value =
                                                   string.Format("Valoarea nu se afla intervalul {0}:{1}", prevConsum,
                                                                 nextConsum)
                                           });
                        }
                    }else if (last != null && first == null)
                    {
                        if (last.IndexUtilitate < IndexUtilitate)
                        {
                            Errors.Add(new StringKeyValue()
                                           {
                                               Key = "IndexUtilitate",
                                               Value = string.Format("Valoarea este mai mare decat {0}", IndexUtilitate)
                                           });
                        }
                    }
                    else if (last == null && first != null)
                    {
                        if (first.IndexUtilitate > IndexUtilitate)
                        {
                            Errors.Add(new StringKeyValue()
                                           {
                                               Key = "Index utilitate",
                                               Value =
                                                   string.Format("Valoarea este mai mica decat {0}",
                                                                 IndexUtilitate)
                                           });
                        }
                    }
                }
            }

        }

        public List<StringKeyValue> Errors { get; private set; }
    }
}
