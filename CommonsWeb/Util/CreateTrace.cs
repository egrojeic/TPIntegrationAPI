using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;

namespace Common
{
    /// <summary>
    /// Clase que contiene funcionalidad estándar para logs.
    /// </summary>
    public class CreateTrace
    {
        const string Formato = "yyyy-MM-dd HH:mm:ss.FFF";
        private string fileName;
        private string DatosInvocacion;
        string ruta;
        string Usuario;
        string DireccionIP;
        string DominioIP;
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
        /// pasamos la ruta de un archivo, su utilizará ese para hacer el log
        /// </summary>
        /// <param name="file">Ruta y Nombre del archivo a ser generado</param>
        public CreateTrace(string file)
        {
            fileName = file;
            CargarDatos();
        }

        // caso contrario se utiliza uno por defecto segun parametrizacion
        public CreateTrace()
        {
            string NombreArchivo = "LOG" + DateTime.Now.Date.ToString("yyyy_MM_dd") + ".log";
            string RutaArchivo = "C:/LogIntegrationAPI";
            fileName = @"" + RutaArchivo + NombreArchivo;

            if (!System.IO.Directory.Exists(@"" + RutaArchivo))
                System.IO.Directory.CreateDirectory(@"" + RutaArchivo);


            CargarDatos();

        }

        private void CargarDatos()
        {
            System.Diagnostics.StackTrace sds = new System.Diagnostics.StackTrace();
            char salto = '\n';

            string ruta2 = sds.ToString().Split(salto)[3].Split('(')[0].Remove(0, 6);
            ruta = ruta2;

            if (ruta2.Contains("DAL.Impl.SQLServer.Helper.SqlServerHelper"))
                ruta = sds.ToString().Split(salto)[4].Split('(')[0].Remove(0, 6);

            if (ruta2.Contains("DAL.Impl.SQLServer.Helper.SqlServerHelper.ImprimirParametros"))
                ruta = sds.ToString().Split(salto)[5].Split('(')[0].Remove(0, 6);

            //-- CONSULTAR USUARIO 
            Usuario = "WS";//GetCurrentUser();
            DireccionIP = "WS";//GetCurrentUserIPAddress();
            DominioIP = "WS";//GetCurrentUserDomain();

            DatosInvocacion = "[" + DominioIP + "|" + Usuario + "|" + DireccionIP + "] [" + ruta + "] ";
            Bloqueo = "1";
        }

        /// <summary>
        /// Escribe en el log a traves de un string
        /// </summary>
        /// <param name="logText">Texto a ser guardado en el log</param>
        public static void WriteLog(LogLevel loglevel, string logText)
        {

            CreateTrace Log = new CreateTrace();

            switch (loglevel)
            {
                case LogLevel.Debug:
                    //if (Settings.Default.Debug == true)
                        escribir(DateTime.Now.ToString(Formato) + " [" + loglevel + "]      :" + Log.DatosInvocacion + logText, Log.fileName);
                    break;

                case LogLevel.Information:
                    //if (Settings.Default.Information == true)
                        escribir(DateTime.Now.ToString(Formato) + " [" + loglevel + "]:" + Log.DatosInvocacion + logText, Log.fileName);
                    break;

                case LogLevel.Warning:
                    //if (Settings.Default.Warning == true)
                        escribir(DateTime.Now.ToString(Formato) + " [" + loglevel + "]    :" + Log.DatosInvocacion + logText, Log.fileName);
                    break;

                case LogLevel.Error:
                    //if (Settings.Default.Error == true)
                        escribir(DateTime.Now.ToString(Formato) + " [" + loglevel + "]      :" + Log.DatosInvocacion + logText, Log.fileName);
                    break;

                case LogLevel.Fatal:
                    //if (Settings.Default.Fatal == true)
                        escribir(DateTime.Now.ToString(Formato) + " [" + loglevel + "]      :" + Log.DatosInvocacion + logText, Log.fileName);
                    break;

                default:
                    //if (Settings.Default.Information == true)
                        escribir(DateTime.Now.ToString(Formato) + " - [" + loglevel + "]: " + Log.DatosInvocacion + logText, Log.fileName);
                    break;
            }

        }

        /// <summary>
        /// Escribe en el DataBase log a traves de un string
        /// </summary>
        /// <param name="logText">Texto a ser guardado en el log</param>
        public static void WriteLogToDB(LogLevel loglevel, string logTitle, string logText)
        {

            CreateTrace Log = new CreateTrace();

            switch (loglevel)
            {
                case LogLevel.Debug:
                    //if (Settings.Default.Debug == true)
                    escribir(DateTime.Now.ToString(Formato) + " [" + loglevel + "]      :" + Log.DatosInvocacion + logText, Log.fileName);
                    break;

                case LogLevel.Information:
                    //if (Settings.Default.Information == true)
                    escribir(DateTime.Now.ToString(Formato) + " [" + loglevel + "]:" + Log.DatosInvocacion + logText, Log.fileName);
                    break;

                case LogLevel.Warning:
                    //if (Settings.Default.Warning == true)
                    escribir(DateTime.Now.ToString(Formato) + " [" + loglevel + "]    :" + Log.DatosInvocacion + logText, Log.fileName);
                    break;

                case LogLevel.Error:
                    //if (Settings.Default.Error == true)
                    escribir(DateTime.Now.ToString(Formato) + " [" + loglevel + "]      :" + Log.DatosInvocacion + logText, Log.fileName);
                    break;

                case LogLevel.Fatal:
                    //if (Settings.Default.Fatal == true)
                    escribir(DateTime.Now.ToString(Formato) + " [" + loglevel + "]      :" + Log.DatosInvocacion + logText, Log.fileName);
                    break;

                default:
                    //if (Settings.Default.Information == true)
                    escribir(DateTime.Now.ToString(Formato) + " - [" + loglevel + "]: " + Log.DatosInvocacion + logText, Log.fileName);
                    break;
            }

        }

        /// <summary>
        /// Escribe en el log a traves de una excepcion
        /// </summary>
        /// <param name="ex">Exception</param>
        public static void WriteLog(Exception ex)
        {
            try
            {
                CreateTrace Log = new CreateTrace();
                //if (Settings.Default.Exception == true)
                {
                    //using (StreamWriter w = File.AppendText(Log.fileName))
                    //{
                    string EscribirEXCEPCION = "";
                    EscribirEXCEPCION = "-------------------------------------------------------------------------------- [EXCEPCION]" + System.Environment.NewLine;
                    EscribirEXCEPCION = EscribirEXCEPCION + DateTime.Now.ToString(Formato) + " [EXCEPCION]  :" + Log.DatosInvocacion + System.Environment.NewLine;
                    EscribirEXCEPCION = EscribirEXCEPCION + "Message       : " + ex.Message + System.Environment.NewLine;
                    EscribirEXCEPCION = EscribirEXCEPCION + "Source        : " + ex.Source + System.Environment.NewLine;
                    EscribirEXCEPCION = EscribirEXCEPCION + "TargetSite    : " + ex.TargetSite + System.Environment.NewLine;
                    EscribirEXCEPCION = EscribirEXCEPCION + "StackTrace    :" + ex.StackTrace + System.Environment.NewLine;
                    EscribirEXCEPCION = EscribirEXCEPCION + "InnerException: " + ex.InnerException + System.Environment.NewLine;
                    EscribirEXCEPCION = EscribirEXCEPCION + "--------------------------------------------------------------------------------";

                    escribir(EscribirEXCEPCION, Log.fileName);
                    //escribir("-------------------------------------------------------------------------------- [EXCEPCION]", Log.fileName);
                    //escribir(DateTime.Now.ToString(Formato) + " [EXCEPCION]  :" + Log.DatosInvocacion, Log.fileName);
                    //escribir("Message       : " + ex.Message, Log.fileName);
                    //escribir("Source        : " + ex.Source, Log.fileName);
                    //escribir("TargetSite    : " + ex.TargetSite, Log.fileName);
                    //escribir("StackTrace    :" + ex.StackTrace, Log.fileName);
                    //escribir("InnerException: " + ex.InnerException, Log.fileName);
                    //escribir("--------------------------------------------------------------------------------", Log.fileName);
                    //}
                }
            }
            catch { }
        }

        public static void ImprimirParametro(string procedureName, IList<SqlParameter> parameters)
        {
            CreateTrace Log = new CreateTrace();
            //if (Settings.Default.Information == true)
            {
                string cadena = "[" + procedureName + "] ";
                if (parameters != null)
                {
                    for (int i = 0; i < parameters.Count; i++)
                        cadena = cadena + "[" + parameters[i].ParameterName + "|" + parameters[i].Value + "]";

                    escribir(DateTime.Now.ToString(Formato) + " [" + LogLevel.Information + "]:" + Log.DatosInvocacion + cadena, Log.fileName);
                }
            }

        }


        private static void escribir(string mensaje, string fileName)
        {
            try
            {
                lock (Bloqueo)
                {
                    using (StreamWriter w = File.AppendText(fileName))
                    {
                        w.WriteLine(mensaje);
                    }
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception)
            {
                Exception exep = new Exception();
            }
        }



    }
}