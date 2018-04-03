using SqlReflect.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlReflectTest.Model
{
    [Table("Shippers")]
    public struct Shippers
    {
        [PK(IsIdentity = true)]
        public int ShipperID { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
    }
}
