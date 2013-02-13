using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Repository.Concret;

namespace HomeHelper.Utils
{
    public class FactoryRepository
    {
        public static IRepository<Utilitati> GetInstanceRepositoryUtilitati()
        {
            return new UtilitatiRepository();
        }

        public static IRepository<ConsumUtilitate> GetInstanceRepositoryConsum()
        {
            return new ConsumUtilitateRepository();
        }

        public static IRepository<AlertaUtilitate> GetInstanceAlertaUtilitate()
        {
            return new AlertaUtilitateRepository();
        }
    }
}
