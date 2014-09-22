using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Model;

namespace HomeHelper.Repository.Abstract
{
    public interface IRepository<T>
    {
         Tuple<string, bool> CreateOrUpdate(T t);
         Tuple<string, bool> Delete(T t);
        ObservableCollection<T> GetAll();
        T GetById(int id);
    }

    public interface IRepositoryEnhancing<T>:IRepository<T>
    {
        bool HasReferences(int id);
    }

    public interface IEnhancedRepository<T>:IRepository<T>
    {
        Tuple<string, bool, int> CreateOrUpdateEnhanced(T t);
    }

    public interface IScheduleRepository
    {
        void AddAlertToSchedule(AlertaUtilitate t);
        void DeleteFromSchedule(AlertaUtilitate t);
    }
}
