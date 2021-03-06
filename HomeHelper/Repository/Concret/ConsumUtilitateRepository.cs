﻿using System;
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
    public class ConsumUtilitateRepository:IRepository<ConsumUtilitate>
    {
        public Tuple<string, bool> CreateOrUpdate(ConsumUtilitate t)
        {
            using (var sqlConn=new SQLiteConnection(DbUtils.DbPath))
            {
                try
                {
                    sqlConn.BeginTransaction();
                    if (t.IdConsumUtilitate == 0)
                    {
                        sqlConn.Insert(t);
                    }
                    else sqlConn.Update(t);
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

        public Tuple<string, bool> Delete(ConsumUtilitate t)
        {
            using (var sqlConn=new SQLiteConnection(DbUtils.DbPath))
            {
                try
                {
                    sqlConn.BeginTransaction();
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

        public ObservableCollection<ConsumUtilitate> GetAll()
        {
            using (var sqlConn=new SQLiteConnection(DbUtils.DbPath))
            {
                return new ObservableCollection<ConsumUtilitate>(sqlConn.Table<ConsumUtilitate>());
            }
        }

        public ConsumUtilitate GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.IdConsumUtilitate == id);
        }
    }

    public class AlertaUtilitateRepository : IEnhancedRepository<AlertaUtilitate>
    {

        public static IScheduleRepository ScheduleRepository { get; set; }

        public Tuple<string, bool> CreateOrUpdate(AlertaUtilitate t)
        {
            using (var sqlConn = new SQLiteConnection(DbUtils.DbPath))
            {
                try
                {
                    sqlConn.BeginTransaction();
                    if (t.IdAlertaUilitate == 0)
                    {
                        sqlConn.Insert(t);
                    }
                    else sqlConn.Update(t);
                    ScheduleRepository.AddAlertToSchedule(t);
                    sqlConn.Commit();
                    return new Tuple<string, bool>(ResurseMesaje.CrudSucces, true);
                }
                catch (Exception ex)
                {
                    sqlConn.Rollback();
                    return new Tuple<string, bool>(ex.Message, false);
                }
            }
        }

        public Tuple<string, bool> Delete(AlertaUtilitate t)
        {
            using (var sqlConn = new SQLiteConnection(DbUtils.DbPath))
            {
                try
                {
                    sqlConn.BeginTransaction();
                    sqlConn.Delete(t);
                    ScheduleRepository.DeleteFromSchedule(t);
                    sqlConn.Commit();
                    return new Tuple<string, bool>(ResurseMesaje.CrudSucces, true);
                }
                catch (Exception ex)
                {
                    sqlConn.Rollback();
                    return new Tuple<string, bool>(ex.Message, false);
                }
            }
        }



        public ObservableCollection<AlertaUtilitate> GetAll()
        {
            using (var sqlConn = new SQLiteConnection(DbUtils.DbPath))
            {
                return new ObservableCollection<AlertaUtilitate>(sqlConn.Table<AlertaUtilitate>());
            }
        }

        public AlertaUtilitate GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.IdAlertaUilitate == id);
        }

        public Tuple<string, bool, int> CreateOrUpdateEnhanced(AlertaUtilitate t)
        {
             using (var sqlConn = new SQLiteConnection(DbUtils.DbPath))
             {
                 var loader = DbUtils.Loader;
                try
                {
                    sqlConn.BeginTransaction();
                    if (t.IdAlertaUilitate == 0)
                    {
                        sqlConn.Insert(t);
                    }
                    else sqlConn.Update(t);
                    sqlConn.Commit();
                    ScheduleRepository.AddAlertToSchedule(t);
                    return new Tuple<string, bool,int>(ResurseMesaje.CrudSucces, true,t.IdAlertaUilitate);
                }
                catch (Exception ex)
                {
                    sqlConn.Rollback();
                    return new Tuple<string, bool,int>(ex.Message, false,0);
                }
            }
        }


    }
    }

   

