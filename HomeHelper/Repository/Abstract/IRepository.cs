using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHelper.Repository.Abstract
{
    public interface IRepository<T>
    {
         Tuple<string, bool> CreateOrUpdate(T t);
         Tuple<string, bool> Delete(T t);
        ObservableCollection<T> GetAll();
        T GetById(int id);

    }
    public interface IEnhancedRepository<T>:IRepository<T>
    {
        Tuple<string, bool, int> CreateOrUpdateEnhanced(T t);
    }
}
