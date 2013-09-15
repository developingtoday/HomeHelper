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
    public class UtilitatiRepository:IRepositoryEnhancing<Utilitati>
    {
        private readonly IEnhancedRepository<AlertaUtilitate> _repositoryAlerte=new AlertaUtilitateRepository(); 
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
                    sqlConn.BeginTransaction();
                    
                    var id = t.IdUtilitati;
                    foreach (var consum in sqlConn.Table<ConsumUtilitate>().Where(a=>a.IdUtilitate==id).ToList())
                    {
                        sqlConn.Delete(consum);
                    }
                    foreach (var alerta in sqlConn.Table<AlertaUtilitate>().Where(a=>a.IdUitlitate==id).ToList())
                    {
                       AlertaUtilitateRepository.DeleteFromSchedule(alerta);
                        sqlConn.Delete(alerta);
                    }
                    sqlConn.Delete(t);
                    sqlConn.Commit();
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


        public bool HasReferences(int id)
        {
            using (var sqlConn=new SQLiteConnection(DbUtils.DbPath))
            {
                return sqlConn.Table<ConsumUtilitate>().Any(a => a.IdUtilitate == id) ||
                       sqlConn.Table<AlertaUtilitate>().Any(a => a.IdUitlitate == id);
            }
        }
    }
}
