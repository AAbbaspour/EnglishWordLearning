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
            try
            {
                if (!IsContainsWord(dic.Word))
                {
                    dic.Word = dic.Word.Replace("'", "''");
                    dic.Meaning = dic.Meaning.Replace("'", "''");
                    DataAccess.Dictionery.instance.AddWord(dic);
                }

            }
            catch (Exception ex)
            {

                // Tof

            }

        }

        public Common.DTO.DicEntity getByWord(String Word)
        {
            Common.DTO.DicEntity dicEntity = DataAccess.Dictionery.instance.getByWord(Word);
            if (dicEntity == null & Word.EndsWith("s"))
                dicEntity = DataAccess.Dictionery.instance.getByWord(Word.Substring(0, Word.Length - 1));

            if (dicEntity == null & Word.EndsWith("ed"))
            {
                dicEntity = DataAccess.Dictionery.instance.getByWord(Word.Substring(0, Word.Length - 1));
                if (dicEntity == null)
                {
                    dicEntity = DataAccess.Dictionery.instance.getByWord(Word.Substring(0, Word.Length - 2));
                }
            }

            if (dicEntity == null & Word.EndsWith("ing"))
                dicEntity = DataAccess.Dictionery.instance.getByWord(Word.Substring(0, Word.Length - 3));

            if (dicEntity == null)
                dicEntity = new Common.DTO.DicEntity() { Word = Word, Meaning = "" };
            return dicEntity;
        }

        public bool IsContainsWord(String Word)
        {
            return getByWord(Word) != null;
        }
    }
}
