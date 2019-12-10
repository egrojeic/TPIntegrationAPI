using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;

namespace CommonsWeb.DAL
{
    /// <summary>
    /// Clase que implementa las operaciones de la conexión a la base de datos
    /// </summary>
    public class SqlServerHelper
    {
        private static readonly string strConexion;
        private static readonly string strConexionLogDB;
        private static object syncRoot;
        private static SqlServerHelper instance;

        static SqlServerHelper()
        {
            syncRoot = new Object();
            strConexion = System.Configuration.ConfigurationManager.ConnectionStrings["APIsConnectionString"].ConnectionString;
            strConexionLogDB = System.Configuration.ConfigurationManager.ConnectionStrings["LogDBConnectionString"].ConnectionString;
            instance = null;
        }

        public static SqlServerHelper GetInstace()
        {
            lock (syncRoot)
            {
                if (instance == null)
                {
                    instance = new SqlServerHelper();
                }
            }
            return instance;
        }

        /// <summary>
        /// stringBuilder Nativo de SqlClient
        /// </summary>
        /// <param name="Server">DataSource IpServer</param>
        /// <param name="DataSource">Nombre de base de datos</param>
        /// <param name="UserName">User Id</param>
        /// <param name="Password">Password</param>
        /// <returns>string</returns>
        //private static string GetConectionString(string dataSource, string dataBaseName, string userId, string password, int timeOut)
        //{
        //    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        //    builder.DataSource = dataSource.Trim();
        //    builder.InitialCatalog = dataBaseName.Trim();
        //    builder.UserID = userId.Trim();
        //    builder.Password = password.Trim();
        //    builder.AsynchronousProcessing = true;
        //    builder.ConnectTimeout = timeOut;
        //    return builder.ConnectionString;
        //}


        /// <summary>
        /// Ejecuta el script SQL que recibe como parametro y 
        /// retorna un DataSet con los valores de la consulta.
        /// </summary>
        /// <param name="sentenceSql"></param>
        /// <returns>DataSet</returns>
        /// <exception cref="DAL.Helper.DALException"/>
        public int ExecuteCRUD(string sentenceSql)
        {
            SqlCommand command = null;
            try
            {
                int result = 0;

                using (SqlConnection connection = new SqlConnection(strConexion))
                {
                    command = PrepareCommand(sentenceSql, connection);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    result = command.ExecuteNonQuery();

                    return result;
                }
            }
            catch (Exception ex)
            {
                CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Debug, "ERROR EN CAPA DAL:ExecuteCRUD", ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        /// <summary>
        /// Ejecuta el script SQL que recibe como parametro y 
        /// retorna un DataSet con los valores de la consulta.
        /// </summary>
        /// <param name="sentenceSql"></param>
        /// <returns>DataSet</returns>
        /// <exception cref="DAL.Helper.DALException"/>
        public int ExecuteLogDB(string sentenceSql)
        {
            SqlCommand command = null;
            try
            {
                int result = 0;

                using (SqlConnection connection = new SqlConnection(strConexionLogDB))
                {
                    command = PrepareCommand(sentenceSql, connection);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    result = command.ExecuteNonQuery();

                    return result;
                }
            }
            catch (Exception ex)
            {
                CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Debug, "ERROR EN CAPA DAL:ExecuteCRUDLogDB", ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }
        /// <summary>
        /// Ejecuta el script SQL que recibe como parametro. 
        /// Retorna un único valor de tipo string
        /// La lista "IList<SqlParameter>" es usada como parametros de entra de la consulta.
        /// </summary>
        /// <param name="sentenceSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="DAL.Helper.DALException"/>
        public string ExecuteScalar(string sentenceSql)
        {
            SqlCommand command = null;
            try
            {
                string result = string.Empty;

                using (SqlConnection connection = new SqlConnection(strConexion))
                {
                    command = PrepareCommand(sentenceSql, connection);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    Object res = command.ExecuteScalar();
                    if (res != null)
                    {
                        result = res.ToString();
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Debug, "ERROR EN CAPA DAL:ExecuteScalar", ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        /// <summary>
        /// Ejecuta el procedimiento almacenado cuyo nombre recibe como parametro. 
        /// La lista "IList<SqlParameter>" es usada como parametros de entra para el procedimiento.
        /// Retorna un DataSet con los valores de la consulta
        /// </summary>
        /// <param name="prmstrSQL"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="DAL.Helper.DALException"/>
        public DataSet ExecuteProcedureToDataSet(string prmstrSQL)
        {
            //ImprimirParametros(procedureName, parameters);

            SqlCommand command = null;
            SqlDataAdapter dbAdapter = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(strConexion))
                {
                    DataSet ResultsDataSet = new DataSet();
                    ResultsDataSet.Locale = CultureInfo.InvariantCulture;
                    command = PrepareCommandProcedure(prmstrSQL, connection);
                    dbAdapter = new SqlDataAdapter(command);
                    dbAdapter.Fill(ResultsDataSet);
                    return ResultsDataSet;
                }
            }
            catch (Exception ex)
            {
                CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Debug, "ERROR EN CAPA DAL:ExecuteProcedureToDataSet", ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                //Common.CreateTrace.WriteLog(Common.CreateTrace.LogLevel.Debug, "----------------------------------------- FIN");
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }

                if (dbAdapter != null)
                {
                    dbAdapter.Dispose();
                    dbAdapter = null;
                }
            }
        }

        /// <summary>
        /// Ejecuta la sentencia SQL que recibe como parametro. 
        /// La lista "IList<SqlParameter>" es usada como parametros de entra para el procedimiento.
        /// Retorna un DataSet con los valores de la consulta
        /// </summary>
        /// <param name="sentenceSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="DAL.Helper.DALException"/>
        public DataSet ExecuteSqlToDataSet(string sentenceSql)
        {
            //ImprimirParametros(sentenceSql, parameters);

            SqlCommand command = null;
            SqlDataAdapter dbAdapter = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(strConexion))
                {

                    DataSet ResultsDataSet = new DataSet();
                    ResultsDataSet.Locale = CultureInfo.InvariantCulture;
                    command = PrepareCommand(sentenceSql, connection);
                    dbAdapter = new SqlDataAdapter(command);
                    dbAdapter.Fill(ResultsDataSet);
                    return ResultsDataSet;
                }
            }
            catch (Exception ex)
            {
                CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Debug, "ERROR EN CAPA DAL:ExecuteSqlToDataSet", ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                //Common.CreateTrace.WriteLog(Common.CreateTrace.LogLevel.Debug, "----------------------------------------- FIN");
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }

                if (dbAdapter != null)
                {
                    dbAdapter.Dispose();
                    dbAdapter = null;
                }
            }
        }

       
        private SqlCommand PrepareCommand(string sentenceSql, SqlConnection connection)
        {

            using (SqlCommand command = new SqlCommand(sentenceSql, connection))
            {
                command.CommandTimeout = 600;
                return command;
            }
        }

        private SqlCommand PrepareCommandProcedure(string procedureName, SqlConnection connection)
        {

            using (SqlCommand command = new SqlCommand())
            {
                command.CommandText = procedureName;
                command.CommandType = CommandType.Text;
                command.Connection = connection;
                command.CommandTimeout = 600;

                return command;
            }
        }



        //private void ImprimirParametros(string procedureName, IList<SqlParameter> parameters)
        //{
        //    string cadena = "[" + procedureName + "] ";
        //    if (parameters != null)
        //    {
        //        for (int i = 0; i < parameters.Count; i++)
        //            cadena = cadena + "[" + parameters[i].ParameterName + "|" + parameters[i].Value + "]";

        //        Common.CreateTrace.WriteLog(Common.CreateTrace.LogLevel.Debug, cadena);
        //    }
        //}

    }
}