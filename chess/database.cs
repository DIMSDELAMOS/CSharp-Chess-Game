using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace chess
{
    class Database
    {
        public SQLiteConnection Connect;
        public string ConnectionString { get; set; }
        string connection;
        public void getconnection()
        {
            connection = @"Data Source=Database2.db; Version=3;";
            ConnectionString = connection;
        }
        public Database()
        {
            if (!File.Exists("./Database.db"))
            {
                SQLiteConnection.CreateFile("Database.db");
                getconnection();
                using (SQLiteConnection con = new SQLiteConnection(connection))
                {
                    SQLiteCommand command = new SQLiteCommand();
                    con.Open();
                    string query = @"
                                    CREATE TABLE IF NOT EXISTS CHESS (
                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name TEXT,
                                    
                                    )";
                    command.CommandText = query;
                    command.Connection = con;
                    command.ExecuteNonQuery();
                    con.Close();
                }



            }
            
           
        }
    }
    
    
}
