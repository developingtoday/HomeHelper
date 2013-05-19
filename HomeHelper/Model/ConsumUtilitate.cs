using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHelper.Model
{
    public class ConsumUtilitate
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
        public string DataConsumGrafic
        {
            get { return DataConsum.ToString("d"); }
        }
        public override string ToString()
        {
            return string.Format("IdConsumUtilitate: {0}, IdUtilitate: {1}, DataConsum: {2}, IndexUtilitate: {3}", IdConsumUtilitate, IdUtilitate, DataConsum, IndexUtilitate);
        }
    }
}
