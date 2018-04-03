using System;
using SqlReflect;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SqlReflectTest.Model;

namespace SqlReflectTest.DataMappers
{
    class ShippersDataMapper : AbstractDataMapper
    {
        const string COLUMNS = "CompanyName, Phone";
        const string SQL_GET_ALL = @"SELECT ShipperID, " + COLUMNS + " FROM Shippers";
        const string SQL_GET_BY_ID = SQL_GET_ALL + " WHERE ShipperID=";
        const string SQL_INSERT = "INSERT INTO Shippers (" + COLUMNS + ") OUTPUT INSERTED.ShipperID VALUES ";
        const string SQL_DELETE = "DELETE FROM Shippers WHERE ShipperID = ";
        const string SQL_UPDATE = "UPDATE Shippers SET CompanyName={1}, Phone={2} WHERE ShipperID = {0}";
        public ShippersDataMapper(string connStr) : base(connStr)
        {
        }

        protected override object Load(SqlDataReader dr)
        {
            Shippers ship = new Shippers();
            ship.ShipperID = (int)dr["ShipperID"];
            ship.CompanyName = (string)dr["CompanyName"];
            ship.Phone = (string)dr["Phone"];
            return ship;
        }

        
        protected override string SqlGetAll()
        {
            return SQL_GET_ALL;
        }

        protected override string SqlGetById(object id)
        {
            return SQL_GET_BY_ID + id;
        }

        protected override string SqlInsert(object target)
        {
            Shippers shippers = (Shippers)target;
            string values = "'" + shippers.CompanyName + "' , '" + shippers.Phone + "'";
            return SQL_INSERT + "(" + values + ")";
        }

        protected override string SqlUpdate(object target)
        {
            Shippers shippers = (Shippers)target;
            return String.Format(SQL_UPDATE,
                shippers.ShipperID,
                "'" + shippers.CompanyName+ "'",
                "'" + shippers.Phone + "'");
        }

        protected override string SqlDelete(object target)
        {
            Shippers ship = (Shippers)target;
            return SQL_DELETE + ship.ShipperID;
        }

    }
}
