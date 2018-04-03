using SqlReflect.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlReflect
{
    public class Queries
    {


        private  string TABLE; 
        private  string ID; // KEY / PK / FK
        private  string COLUMNS = "";
        // private string SQL_GET_ALL = @"SELECT * FROM ";
        //verificar se PK  é identity, se sim encontra.se nas colunas, se nao, nao entra
        public string SQL_INSERT = "INSERT INTO {0} ( {1} ) OUTPUT INSERTED.{2} VALUES ";  

        private string SQL_DELETE = "DELETE FROM {0} WHERE {1} = ";
        string SQL_UPDATE = "UPDATE {0} SET ";
       

        public Queries(PropertyInfo[]  pi,  string tableName) {
            TABLE = tableName;
            FillColumns(pi);
        }

        public Queries() { }

        private void FillColumns(PropertyInfo [] pi)
        {
            foreach(PropertyInfo p in pi)
            {
                //TODO verificar se a property é um TableAttribute e em seguida ir buscar o PKname Dela
               /* if (p.IsDefined(typeof(TableAttribute)))
                {

                    {
                        if (p.IsDefined(typeof(PKAttribute))) { COLUMNS += "'" + p.Name + "',"; }

                    }
                }*/

                if (p.GetCustomAttribute(typeof(PKAttribute)) != null)
                {
                PKAttribute pk = p.GetCustomAttribute(typeof(PKAttribute)) as PKAttribute;
                ID = p.Name;
                    if (!pk.IsIdentity) COLUMNS += ID;
                    continue;
                }
                string columnName = p.Name;
                
                COLUMNS += columnName+", ";
            }
            int index = COLUMNS.LastIndexOf(',');
       
            StringBuilder sb = new StringBuilder(COLUMNS);
            sb[index] = ' ';
            COLUMNS = sb.ToString();
        }

        public string GetAll() { return @"SELECT * FROM " + TABLE; }

        public string GetAllById() { return GetAll() + " WHERE " + ID + "= "; }

        public string Insert() { return String.Format(SQL_INSERT, TABLE, COLUMNS, ID); }

        public string Delete() { return String.Format(SQL_DELETE, TABLE, ID); }

        public string GetID() { return ID; }
        public string Update() { return String.Format(SQL_UPDATE, TABLE); }
    } 
}
