using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using CommonsWeb.DAL;

namespace Common
{
    /// <summary>
    /// Clase que contiene funcionalidad estándar para logs.
    /// </summary>
    public class CreateTrace
    {
        const string FormatoFecha = "yyyy-MM-dd";
        const string FormatoHora = "HH:mm:ss.FFF";
        public static string Bloqueo;

        public enum LogLevel
        {
            Debug,
            Information,
            Warning,
            Error,
            Fatal
        }

        /// <summary>
        /// Escribe en el DataBase log a traves de un string
        /// </summary>
        /// <param name="logText">Texto a ser guardado en el log</param>
        public static void WriteLogToDB(LogLevel loglevel, string logTitle, string logText)
        {

            SqlServerHelper SQLConection = new SqlServerHelper();
            string strSQLog = "";

            strSQLog = "INSERT INTO LogEventosAPI SELECT Nivel = '" + loglevel + "', Fecha = '" + DateTime.Now.ToString(FormatoFecha) + "', Hora = '" + DateTime.Now.ToString(FormatoHora) + "', Titulo = '" + logTitle + "', Mensaje = '" + logText.Replace("'", "\"") + "'";
            SQLConection.ExecuteLogDB(strSQLog);
        }

        public static void WriteLogJson(string logText, string logName)
        {

            CreateTrace Log = new CreateTrace();
            Bloqueo = "1";
            escribir(logText, logName + " "+ DateTime.Now.ToString(FormatoFecha) + " [" + DateTime.Now.ToString(FormatoHora) + "]");

        }


        private static void escribir(string mensaje, string fileName)
        {
            string RutaArchivo = "C:/LogIntegrationAPI/";
            string fullPathName = @"" + RutaArchivo + fileName.Replace("-","_").Replace(":",".");

            if (!System.IO.Directory.Exists(@"" + RutaArchivo))
                System.IO.Directory.CreateDirectory(@"" + RutaArchivo);

            try
            {
                lock (Bloqueo)
                {
                    using (StreamWriter w = File.AppendText(fullPathName))
                    {
                        w.WriteLine(mensaje);
                    }
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception ex)
            {
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN UTILS CrateJsonFile ", ex.Message);
            }
        }

    }
}