using DataLayer.Interfaces;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace DataLayer
{
    public class XMLProcess : BDConnection
    {
        public XMLProcess()
        {

        }
        public XMLProcess(string connectionString) : base(connectionString)
        {
        }

        public List<XMLTemplate> ProcessTXT(XMLTemplate rel, string filename)
        {
            List<DbParameter> parameterList = new List<DbParameter>();
            List<XMLTemplate> Tests = new List<XMLTemplate>();


            parameterList.Add(new SqlParameter()
            {
                ParameterName = $"@idMapClient",
                SqlDbType = SqlDbType.Int,
                Value = 1
            });

            using (DbDataReader dataReader = base.GetDataReader("Test_GetTemplate", parameterList, CommandType.StoredProcedure))
            {
                if (dataReader != null && dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        XMLTemplate TestItem = new XMLTemplate();
                        TestItem.IdTemplate = (int)dataReader["IdTemplate"];
                        TestItem.Attribute = (string)dataReader["Attribute"];
                        TestItem.Element = (string)dataReader["Element"];
                        TestItem.Column = Convert.IsDBNull(dataReader["Column"]) ? null : (string)dataReader["Column"];
                        TestItem.IdMapClient = (int)dataReader["IdMapClient"];
                        TestItem.IdType = Convert.IsDBNull(dataReader["IdType"]) ? null : (int?)dataReader["IdType"];
                        TestItem.IsRequeried = (bool)dataReader["IsRequeried"];
                        TestItem.ParentElement = (string)dataReader["ParentElement"];
                        TestItem.Row = Convert.IsDBNull(dataReader["Row"]) ? null : (string)dataReader["Row"];
                        TestItem.Section = (string)dataReader["Section"];

                        Tests.Add(TestItem);
                    }

                }
                return Tests;
            }
        }

        public List<XMLTemplate> GetTemplate(int client)
        {
            List<DbParameter> parameterList = new List<DbParameter>();
            List<XMLTemplate> Tests = new List<XMLTemplate>();


            parameterList.Add(new SqlParameter()
            {
                ParameterName = $"@idMapClient",
                SqlDbType = SqlDbType.Int,
                Value = client
            });

            using (DbDataReader dataReader = base.GetDataReader("GetAllTemplate", parameterList, CommandType.StoredProcedure))
            {
                if (dataReader != null && dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        XMLTemplate TestItem = new XMLTemplate();
                        TestItem.IdTemplate = (int)dataReader["IdTemplate"];
                        TestItem.Attribute = (string)dataReader["Attribute"];
                        TestItem.Element = (string)dataReader["Element"];
                        TestItem.Column = Convert.IsDBNull(dataReader["Column"]) ? null : (string)dataReader["Column"];
                        TestItem.IdMapClient = (int)dataReader["IdMapClient"];
                        TestItem.IdType = Convert.IsDBNull(dataReader["IdType"]) ? null : (int?)dataReader["IdType"];
                        TestItem.IsRequeried = (bool)dataReader["IsRequeried"];
                        TestItem.ParentElement = (string)dataReader["ParentElement"];
                        TestItem.Row = Convert.IsDBNull(dataReader["Row"]) ? null : (string)dataReader["Row"];
                        TestItem.Section = (string)dataReader["Section"];

                        Tests.Add(TestItem);
                    }

                }
                return Tests;
            }
        }

        public List<XMLTemplate> GetElementsBySection(string section)
        {
            List<DbParameter> parameterList = new List<DbParameter>();
            List<XMLTemplate> Tests = new List<XMLTemplate>();


            parameterList.Add(new SqlParameter()
            {
                ParameterName = $"@Section",
                SqlDbType = SqlDbType.VarChar,
                Value = section
            });

            using (DbDataReader dataReader = base.GetDataReader("GetElementsBySection", parameterList, CommandType.StoredProcedure))
            {
                if (dataReader != null && dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        XMLTemplate TestItem = new XMLTemplate();
                        TestItem.IdTemplate = (int)dataReader["IdTemplate"];
                        TestItem.Attribute = (string)dataReader["Attribute"];
                        TestItem.Element = (string)dataReader["Element"];
                        TestItem.Column = Convert.IsDBNull(dataReader["Column"]) ? null : (string)dataReader["Column"];
                        TestItem.IdMapClient = (int)dataReader["IdMapClient"];
                        TestItem.IdType = Convert.IsDBNull(dataReader["IdType"]) ? null : (int?)dataReader["IdType"];
                        TestItem.IsRequeried = (bool)dataReader["IsRequeried"];
                        TestItem.ParentElement = (string)dataReader["ParentElement"];
                        TestItem.Row = Convert.IsDBNull(dataReader["Row"]) ? null : (string)dataReader["Row"];
                        TestItem.Section = (string)dataReader["Section"];

                        Tests.Add(TestItem);
                    }

                }
                return Tests;
            }
        }

        public Dictionary<string, string> GetXMLAttributes(int client)
        {
            List<DbParameter> parameterList = new List<DbParameter>();
            Dictionary<string,string> param = new Dictionary<string,string>();


            parameterList.Add(new SqlParameter()
            {
                ParameterName = $"@IdClient",
                SqlDbType = SqlDbType.Int,
                Value = client
            });

            using (DbDataReader dataReader = base.GetDataReader("GetXMLAttributes", parameterList, CommandType.StoredProcedure))
            {
                if (dataReader != null && dataReader.HasRows)
                {
                    while (dataReader.Read())
                    { 
                        param.Add((string)dataReader["AttributeName"], (string)dataReader["AttributeValue"]);
                    }
                   
                }

            }
            return param;
        }
    }
}
