using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlReflect;
using SqlReflectTest.DataMappers;

namespace SqlReflectTest
{
    [TestClass]
    public class ShippersDataMapperTest : AbstractShippersDataMapperTest
    {
        
        public ShippersDataMapperTest() : base(new ShippersDataMapper(NORTHWIND))
        {
        }

        [TestMethod]
        public new void TestShippersGetAll()
        {
            base.TestShippersGetAll();
        }


        [TestMethod]
        public new void TestShippersGetById()
        {
            base.TestShippersGetById();
        }


        [TestMethod]
        public new void TestShippersInsertAndDelete()
        {
            base.TestShippersInsertAndDelete();
        }

        [TestMethod]
        public new void TestShippersUpdate()
        {
            base.TestShippersUpdate();
        }
    }
}
