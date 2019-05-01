using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public static class ConvertDTO
    {
        public static List<Common.DTO.DicEntity> map(this DicDataset.DictioneryDataTable DT)
        {
            List<Common.DTO.DicEntity> Result = new List<Common.DTO.DicEntity>();
            foreach (var item in DT)
            {
                Result.Add(new Common.DTO.DicEntity()
                {
                    id = item.id,
                    Word = item.Word,
                    Meaning = item.Meaning
                });
            }
            return Result;
        }
    }
}
