using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Model.Abstract;
using HomeHelper.Repository.Concret;

namespace HomeHelper.Model
{
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
                return new UtilitatiRepository().GetById(IdUitlitate).DenumireUtilitate;
            }
        }
        public override string ToString()
        {
            return string.Format("IdAlertaUilitate: {0}, IdUitlitate: {1}, DataAlerta: {2}", IdAlertaUilitate, IdUitlitate, DataAlerta);
        }

        public void DoValidation()
        {
            _errors=new List<StringKeyValue>();
            if (IdUitlitate == 0)
            {
                _errors.Add(new StringKeyValue()
                                {
                                    Key = "IdUtilitate",
                                    Value = "Utilitatea nu a fost selectata"
                                });
            }
            if (!AlertaActiva)
            {
                _errors.Add(new StringKeyValue()
                                {
                                    Key = "DataAlerta",
                                    Value = "Alerta nu va fi activa"
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
