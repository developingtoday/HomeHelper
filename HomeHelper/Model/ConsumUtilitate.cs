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
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            _errors = new List<StringKeyValue>();
            if (IdUtilitate == 0)
            {
                GetErrors().Add(new StringKeyValue()
                               {
                                   Key = "IdUtilitate",
                                   Value = loader.GetString(resource: "IdUtilitateErrorValid")
                               });
            }
            if (IndexUtilitate < 0)
            {
                _errors.Add(new StringKeyValue()
                               {
                                   Key = "IndexUtilitate",
                                   Value = loader.GetString(resource: "IndexInvalid")
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
                    _errors.Add(new StringKeyValue()
                                   {
                                       Key = "DataConsum",
                                       Value = loader.GetString(resource: "DataConsumExistent")
                                   });
                }
                if (DataConsum < util.DataIndexInitial)
                {
                    _errors.Add(new StringKeyValue()
                                    {
                                        Key = "DataConsum",
                                        Value = loader.GetString(resource: "DataConsumMicaDecatIndexInitial")
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
                            _errors.Add(new StringKeyValue()
                                           {
                                               Key = "IndexUtilitate",
                                               Value =
                                                   string.Format("{2} {0}-{1}",
                                                                 first.IndexUtilitate, last.IndexUtilitate, loader.GetString(resource: "IndexUtilitateInvalidInterval"))
                                           });
                        }
                    }
                    else if (DataConsum <= minData)
                    {
                        var first = list.FirstOrDefault(a => a.DataConsum.Date >= DataConsum.Date);
                        if (!(util.IndexInitial <= IndexUtilitate && IndexUtilitate <= first.IndexUtilitate))
                        {
                            _errors.Add(new StringKeyValue()
                                           {
                                               Key = "IndexUtilitate",
                                               Value =
                                                   string.Format("{2} {0}-{1}",
                                                                 util.IndexInitial, first.IndexUtilitate, loader.GetString(resource: "IndexUtilitateInvalidInterval"))
                                           });
                        }
                    }
                    else if (DataConsum >= maxData)
                    {
                        var last = list.LastOrDefault(a => a.DataConsum.Date <= DataConsum.Date);
                        if (IndexUtilitate < last.IndexUtilitate)
                        {
                            _errors.Add(new StringKeyValue()
                                           {
                                               Key = "IndexUtilitate",
                                               Value =
                                                   string.Format("{1} {0}", last.IndexUtilitate, loader.GetString(resource: "IndexMaiMicDecat"))
                                           });
                        }
                    }
                }
                else
                {
                    if (IndexUtilitate < util.IndexInitial)
                    {
                        _errors.Add(new StringKeyValue()
                                       {
                                           Key = "IndexUtilitate",
                                           Value = string.Format("{1} {0}", util.IndexInitial, loader.GetString(resource: "IndexMaiMicDecat"))
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
