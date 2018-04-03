using System;

namespace SqlReflect.Attributes
{
    public class PKAttribute : Attribute
    {
        public bool IsIdentity { set; get; }
        
    }


}
