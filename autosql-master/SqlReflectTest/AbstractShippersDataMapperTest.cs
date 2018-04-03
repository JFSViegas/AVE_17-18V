using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlReflect;
using SqlReflectTest.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlReflectTest
{
    public abstract class AbstractShippersDataMapperTest  
    {
        protected static readonly string NORTHWIND = @"
                    Server=(LocalDB)\MSSQLLocalDB;
                    Integrated Security=true;
                    AttachDbFileName=" +
                        Environment.CurrentDirectory +
                        "\\data\\NORTHWND.MDF";

        readonly IDataMapper shippers;


        public AbstractShippersDataMapperTest(IDataMapper shippers)
        {
            this.shippers = shippers;
        }


        public void TestShippersGetAll()
        {
            IEnumerable res = shippers.GetAll();
            int count = 0;
            foreach (object p in res)
            {
                Console.WriteLine(p);
                count++;
            }
            Assert.AreEqual(3, count);
        }

        public void TestShippersGetById()
        {
            Shippers shipper = (Shippers)shippers.GetById(3);
            Assert.AreEqual("Federal Shipping", shipper.CompanyName);
            Assert.AreEqual("(503) 555-9931", shipper.Phone);
        }

        public void TestShippersInsertAndDelete()
        {
            //
            // Create and Insert new Category
            // 
            Shippers shipper = new Shippers()
            {
                CompanyName = "Fish",
                Phone = "1904"
            };
            object id = shippers.Insert(shipper);
            //
            // Get the new category object from database
            //
            Shippers actual = (Shippers)shippers.GetById(id);
            Assert.AreEqual(shipper.CompanyName, actual.CompanyName);
            Assert.AreEqual(shipper.Phone, actual.Phone);
            //
            // Delete the created category from database
            //
            shippers.Delete(actual);
            object res = shippers.GetById(id);
            actual = res != null ? (Shippers)res : default(Shippers);
            Assert.IsNull(actual.CompanyName);
            Assert.IsNull(actual.Phone);
        }

        public void TestShippersUpdate()
        {
            Shippers original = (Shippers)shippers.GetById(3);
            Shippers modified = new Shippers()
            {
                ShipperID = original.ShipperID,
                CompanyName = "Coco",
                Phone = "1904"
            };
            shippers.Update(modified);
            Shippers actual = (Shippers)shippers.GetById(3);
            Assert.AreEqual(modified.CompanyName, actual.CompanyName);
            Assert.AreEqual(modified.Phone, actual.Phone);
            shippers.Update(original);
            actual = (Shippers)shippers.GetById(3);
            Assert.AreEqual("Federal Shipping", actual.CompanyName);
            Assert.AreEqual("(503) 555-9931", actual.Phone);
        }
    }
}
