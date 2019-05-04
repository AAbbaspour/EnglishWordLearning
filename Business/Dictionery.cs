using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class Dictionery
    {
        public readonly static Dictionery instance = new Dictionery();

        public List<Common.DTO.DicEntity> GetAll()
        {
            return DataAccess.Dictionery.instance.GetAll();
        }

        public void AddWord(Common.DTO.DicEntity dic)
        {
            if (!IsContainsWord(dic.Word))
                DataAccess.Dictionery.instance.AddWord(dic);
        }

        public Common.DTO.DicEntity getByWord(String Word)
        {
            return DataAccess.Dictionery.instance.getByWord(Word);
        }

        public bool IsContainsWord(String Word)
        {
            return getByWord(Word) != null;
        }
    }
}
