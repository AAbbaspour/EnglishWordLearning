using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Data.SQLite;
using System.Data;

namespace DataAccess
{
   public class Dictionery
    {
        public readonly static Dictionery instance = new Dictionery();

        private  SQLiteConnection Conn = new SQLiteConnection(@"Data Source="  + @"C:\Users\afshin_abbaspour\source\repos\EnglishWordLearning\EnglishWordLearning\bin\Debug\" + "Dic.db");


        public List<Common.DTO.DicEntity> GetAll()
        {
            SQLiteDataAdapter SDA = new SQLiteDataAdapter("Select * from Dictionary", Conn);
            if (Conn.State != ConnectionState.Open) Conn.Open();
            DicDataset Ds = new DicDataset();
            SDA.Fill(Ds.Dictionery);
            Conn.Close();
            return Ds.Dictionery.map();
        }


        public  void AddWord(Common.DTO.DicEntity dic)
        {
            SQLiteCommand Comm = new SQLiteCommand("INSERT INTO Dictionary(\"Word\",\"Meaning\") VALUES ('"+ dic.Word +"','"+ dic.Meaning +"');", Conn);
            if (Conn.State != ConnectionState.Open) Conn.Open();
            Comm.ExecuteNonQuery();
            Conn.Close();
        }

        public  Common.DTO.DicEntity getByWord(String Word)
        {
            SQLiteDataAdapter SDA = new SQLiteDataAdapter("Select * from Dictionary where Word = \"" + Word + "\"", Conn);
            if (Conn.State != ConnectionState.Open) Conn.Open();
            DicDataset Ds = new DicDataset();
            SDA.Fill(Ds.Dictionery);
            Conn.Close();
            return Ds.Dictionery.map().FirstOrDefault();
        }

    }
}
