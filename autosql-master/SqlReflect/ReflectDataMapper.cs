using SqlReflect.Attributes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SqlReflect
{
    public class ReflectDataMapper : AbstractDataMapper
    {
        Type Klass;
        TableAttribute tableName;
        
        //Dictionaries
        static Dictionary<Type, Queries> query = new Dictionary<Type, Queries>();
        static Dictionary<Type, PropertyInfo[]> propertiesDic = new Dictionary<Type, PropertyInfo[]>();
        static Dictionary<string, ReflectDataMapper> dataMapperDic = new Dictionary<string, ReflectDataMapper>();
        string PKNAME;
        PropertyInfo[] props;
        Queries queries;
       
  
        public ReflectDataMapper(Type klass, string connStr) : base(connStr)
        {
            this.Klass = klass;
            tableName = Klass.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;

            if (!propertiesDic.TryGetValue(Klass, out props))
            {
                props = Klass.GetProperties();
                propertiesDic.Add(Klass, props);
            }

            if (!query.TryGetValue(Klass, out queries))
            {
                queries = new Queries(props, tableName.Name);
                query.Add(Klass, queries);
            }
            ReflectDataMapper reflect;
            if(!dataMapperDic.TryGetValue(Klass.Name, out  reflect))
            dataMapperDic.Add(Klass.Name, this); 
        }

        protected override object Load(SqlDataReader dr)
        {
           object dtm = Activator.CreateInstance(Klass);

            foreach (PropertyInfo p in props)
            {
                object val = null;
                ReflectDataMapper rf;
                Type pt = p.PropertyType;
                if (dataMapperDic.TryGetValue(p.Name, out rf))
                {
                    val = rf.GetById(dr[rf.queries.GetID()]);

                }
                else val = dr[p.Name];
                if (val is DBNull) val = null;
                p.SetValue(dtm, val);
            }
            return dtm;
        }

        protected override string SqlGetAll()
        {

            return queries.GetAll();
        }

        protected override string SqlGetById(object id)
        {         
            return queries.GetAllById() + "'"+id.ToString()+"'";
        }

        protected override string SqlInsert(object target)
        {
            string values = "" ;
           foreach(PropertyInfo p in props)
            {
                if (p.IsDefined(typeof(TableAttribute)))
                {
                    ReflectDataMapper rf;
                    dataMapperDic.TryGetValue(p.Name, out rf);

                    foreach (PropertyInfo pp in rf.props)
                    {
                        if(pp.IsDefined(typeof(PKAttribute))) { values += "" + p.GetValue(pp.GetValue(target)) + ","; }
                        break;
                    }
                   
                }
                if (p.IsDefined(typeof(PKAttribute)) &&((p.GetCustomAttribute(typeof(Attribute)) as PKAttribute).IsIdentity)) continue ; //Should I verify this in here
 
                else if (p.PropertyType.IsPrimitive) values += p.GetValue(target) + ", ";

                else values += "'" + p.GetValue(target) + "',";
            }

            return queries.Insert() + "(" + values.Substring(0, values.Length - 1) + ")";
        }

        protected override string SqlDelete(object target)
        {
            string id="";
            foreach (PropertyInfo p in props)
            {
                if (p.Name.Equals(queries.GetID())) id = p.GetValue(target).ToString();
            }

            return queries.Delete() + id;
        }

        protected override string SqlUpdate(object target)
        {
            string toUpdate = "{0} = '{1}'";
            string update = queries.Update();
            string condition = " where " + queries.GetID() + "= ";
            int lastP = 0;
            foreach(PropertyInfo p in props)
            {
                if (p.IsDefined(typeof(PKAttribute)))
                {
                    condition += p.GetValue(target);
                    lastP++;
                    continue;
                }
                if (lastP == props.Length - 1) update+=  String.Format(toUpdate, p.Name, p.GetValue(target));
                else
                {
                    update += String.Format(toUpdate, p.Name,  p.GetValue(target)) + ", ";
                }
                lastP++;
            }

            update += condition;
            return update;
        }
    }
}
