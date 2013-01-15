using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Model;
using SQLite;

namespace HomeHelper.Utils
{
    public static class DbUtils
    {

        public static string DbPath{get
        {
            return Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "homehelper.sqlite");
        }}
        public static string InitializeDb()
        {
           
            using (var sqlConn=new SQLiteConnection(DbPath))
            {
                try
                {
                   sqlConn.CreateTable<Utilitati>();
                    sqlConn.CreateTable<ConsumUtilitate>();
                    sqlConn.CreateTable<AlertaUtilitate>();
                    return "Succes";

                }
                catch (Exception ex)
                {
                    sqlConn.Rollback();
                    return ex.Message;
                }
            }

        }
    }
}
