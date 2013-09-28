using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Model.Abstract;
using HomeHelper.Repository.Concret;
using HomeHelper.Utils;

namespace HomeHelper.Model
{
    public enum RepetareAlerta:int
    {   
        FaraRepetare=1,
        Saptamanal=2,
        Lunar=3,
        Anual=4,
        Zilnic=5
    }
    public enum TendintaConsum
    {
        Crestere,Scadere,Stagnare
    }
    public static class Util
    {

        public static List<StringIntKeyValue> FrecventeAlerte()
        {
            var loader = DbUtils.Loader;
            return new List<StringIntKeyValue>()
                       {
                           new StringIntKeyValue()
                               {
                                   Key = 1,
                                   Value = loader.GetString(resource: "enumFrecventeAlerteFaraRepetare")
                               },
                           new StringIntKeyValue()
                               {
                                   Key = 2,
                                   Value = loader.GetString(resource: "enumFrecventeAlerteSaptamanal")
                               },
                           new StringIntKeyValue()
                               {
                                   Key = 3,
                                   Value = loader.GetString(resource: "enumFrecventeAlerteLunar")
                               },
                           new StringIntKeyValue()
                               {
                                   Key = 4,
                                   Value = loader.GetString(resource: "enumFrecventeAlerteAnual")
                               },
                           new StringIntKeyValue()
                               {
                                   Key = 5,
                                   Value = loader.GetString(resource: "enumFrecventeAlerteZilnic")
                               }
                       };
        }
    }
    public class AlertaUtilitate:IValidation 
    {

        

        public AlertaUtilitate()
        {
            DataAlerta = DateTime.Now;
        }
        [SQLite.PrimaryKey,SQLite.AutoIncrement]
        public int IdAlertaUilitate { get; set; }
        [SQLite.Indexed]
        public int IdUitlitate { get; set; }
        public DateTime DataAlerta { get; set; }
        public int FrecventaAlerta { get; set; }
        public string DataAfisareAlerta
        {
            get { return DataAlerta.ToString("d"); }
        }

        public bool AlertaActiva
        {
            get { return DataAlerta >= DateTime.Now; }
        }

        public string NumeUtilitate
        {
            get
            {
                if (IdUitlitate == 0) return string.Empty;
                var rep = new UtilitatiRepository().GetById(IdUitlitate);
                if (rep != null) return rep.DenumireUtilitate;
                return string.Empty;
            }
        }
        public string FrecventaAfisare
        {
            get { return Util.FrecventeAlerte().Find(value => value.Key == FrecventaAlerta).Value ?? string.Empty; }
        }
      

        public override string ToString()
        {
            return string.Format("IdAlertaUilitate: {0}, IdUitlitate: {1}, DataAlerta: {2}, FrecventaAlerta: {3}, DataAfisareAlerta: {4}, AlertaActiva: {5}, NumeUtilitate: {6}", IdAlertaUilitate, IdUitlitate, DataAlerta, FrecventaAlerta, DataAfisareAlerta, AlertaActiva, NumeUtilitate);
        }

        public void DoValidation()
        {
            _errors=new List<StringKeyValue>();
            var loader = DbUtils.Loader;

            if (IdUitlitate == 0)
            {
                _errors.Add(new StringKeyValue()
                                {
                                    Key = "IdUtilitate",
                                    Value = loader.GetString(resource: "IdUtilitateErrorValid")
                                });
            }
            if (!AlertaActiva)
            {
                _errors.Add(new StringKeyValue()
                                {
                                    Key = "DataAlerta",
                                    Value = loader.GetString(resource: "DataAlertaErrorValid")
                                });
            }
        }

        private List<StringKeyValue> _errors;
      
        public List<StringKeyValue> GetErrors()
        {
            return _errors;
        }
    }
}
