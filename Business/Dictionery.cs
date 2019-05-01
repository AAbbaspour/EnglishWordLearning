using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class Dictionery
    {
        public readonly static Dictionery instance = new Dictionery();

        public static List<Common.DTO.DicEntity> GetAll()
        {
            return DataAccess.Dictionery.instance.GetAll();
        }

        public static void AddWord(Common.DTO.DicEntity dic)
        {
            DataAccess.Dictionery.instance.AddWord(dic);
        }

        public static Common.DTO.DicEntity getByWord(String Word)
        {
            return DataAccess.Dictionery.instance.getByWord(Word);
        }
    }
}
