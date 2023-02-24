using EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace DataLayer
{
    public class Log : BDConnection
    {
        public Log()
        {

        }
        public Log(string connectionString) : base(connectionString)
        {
        }

        public void WriteLog(System.Exception e, string file)
        {
            Log log = new Log();
            LogError logError = new LogError
            {
                Message = e.ToString(),
                File = file
            };
            log.InsertLog(logError);
        }

        public void InsertLog(LogError logError)
        {
            List<DbParameter> parameterList = new List<DbParameter>();

            parameterList.Add(new SqlParameter()
            {
                ParameterName = $"@Message",
                SqlDbType = SqlDbType.VarChar,
                Value = logError.Message
            });

            parameterList.Add(new SqlParameter()
            {
                ParameterName = $"@File",
                SqlDbType = SqlDbType.VarChar,
                Value = logError.File
            });

            using (DbDataReader dataReader = base.GetDataReader("InsertErrorLog", parameterList, CommandType.StoredProcedure))
            {

            }
        }

    }
}