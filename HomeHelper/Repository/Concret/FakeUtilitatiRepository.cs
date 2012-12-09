using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Utils;

namespace HomeHelper.Repository.Concret
{
    public class FakeUtilitatiRepository:IRepository<Utilitati>
    {
        private readonly ObservableCollection<Utilitati> col;

        public FakeUtilitatiRepository()
        {
             col = new ObservableCollection<Utilitati>
                             {
                                 new Utilitati()
                                     {
                                         DenumireUtilitate = "Apa Calda",
                                         IdUtilitati = 1,
                                         UnitateMasura = "metru cub"
                                     },
                                 new Utilitati()
                                     {
                                         DenumireUtilitate = "Apa Rece",
                                         IdUtilitati = 2,
                                         UnitateMasura = "metru cub"
                                     },
                                 new Utilitati()
                                     {
                                         DenumireUtilitate = "Gaze",
                                         IdUtilitati = 3,
                                         UnitateMasura = "metru cub"
                                     },
                                 new Utilitati()
                                     {
                                         DenumireUtilitate = "Curent",
                                         IdUtilitati = 4,
                                         UnitateMasura = "kilowatt"
                                     },
                                 new Utilitati()
                                     {
                                         DenumireUtilitate = "Caldura",
                                         IdUtilitati = 5,
                                         UnitateMasura = "metru cub"
                                     }
                             };

        }
        public Tuple<string, bool> CreateOrUpdate(Utilitati t)
        {
            if (t.IdUtilitati != 0)
            {
                var first = col.FirstOrDefault(x => x.IdUtilitati == t.IdUtilitati);
                if(first==null) return new Tuple<string, bool>(ResurseMesaje.InregistrareNegasita,false);
                var index = GetIndex(t.IdUtilitati);
                col[index] = t;
                
            }
            else
            {
                col.Add(t);
            }
            return new Tuple<string, bool>(ResurseMesaje.CrudSucces,true);
        }

        public Tuple<string, bool> Delete(Utilitati t)
        {
            var found = col.FirstOrDefault(x => x.IdUtilitati == t.IdUtilitati);
            if(found==null) return new Tuple<string, bool>(ResurseMesaje.InregistrareNegasita,false);
            col.Remove(found);
            return new Tuple<string, bool>(ResurseMesaje.CrudSucces,true);
        }

        public ObservableCollection<Utilitati> GetAll()
        {
            return col;
        }

        public Utilitati GetById(int id)
        {
            return col.FirstOrDefault(x => x.IdUtilitati == id);
        }

        private int GetIndex(int id)
        {
            for (var i = 0; i < col.Count; i++)
            {
                if (col[i].IdUtilitati == id) return i;
            }
            return 0;
        }
    }
}
