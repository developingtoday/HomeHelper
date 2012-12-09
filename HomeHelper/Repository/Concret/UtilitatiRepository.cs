using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Utils;
using SQLite;

namespace HomeHelper.Repository.Concret
{
    public class UtilitatiRepository:IRepository<Utilitati>
    {
        public UtilitatiRepository()
        {
            
        }
        public Tuple<string, bool> CreateOrUpdate(Utilitati t)
        {
            using (var sqlConn=new SQLiteConnection(DbUtils.DbPath))
            {
                try
                {
                    if (t.IdUtilitati != 0)
                    {
                        sqlConn.Update(t);
                    }
                    else sqlConn.Insert(t);
                    return new Tuple<string, bool>(
                        ResurseMesaje.CrudSucces,true);
                }
                catch (Exception ex)
                {
                    sqlConn.Rollback();
                    return new Tuple<string, bool>(ex.Message,false);
                }
            }
        }

        public Tuple<string, bool> Delete(Utilitati t)
        {
            using (var sqlConn=new SQLiteConnection(DbUtils.DbPath))
            {
                try
                {
                    sqlConn.Delete(t);
                    return new Tuple<string, bool>(ResurseMesaje.CrudSucces,true);
                }
                catch (Exception ex)
                {

                    sqlConn.Rollback();
                    return new Tuple<string, bool>(ex.Message,false);
                } 
            }
        }

        public ObservableCollection<Utilitati> GetAll()
        {
            using (var sqlConn=new SQLiteConnection(DbUtils.DbPath))
            {
                try
                {
                    return new ObservableCollection<Utilitati>(sqlConn.Table<Utilitati>());
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
        }

        public Utilitati GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.IdUtilitati == id);
        }
    }
}
