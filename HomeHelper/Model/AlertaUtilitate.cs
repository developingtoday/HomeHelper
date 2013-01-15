using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHelper.Model
{
    public class AlertaUtilitate
    {
        [SQLite.PrimaryKey,SQLite.AutoIncrement]
        public int IdAlertaUilitate { get; set; }
        [SQLite.Indexed]
        public int IdUitlitate { get; set; }
        public DateTime DataAlerta { get; set; }

        public override string ToString()
        {
            return string.Format("IdAlertaUilitate: {0}, IdUitlitate: {1}, DataAlerta: {2}", IdAlertaUilitate, IdUitlitate, DataAlerta);
        }
    }
}
