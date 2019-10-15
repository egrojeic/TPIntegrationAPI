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
        const string FormatoFecha = "yyyy-MM-dd HH:mm:ss.FFF";
        const string FormatoHora = "HH:mm:ss.FFF";
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
        /// Escribe en el DataBase log a traves de un string
        /// </summary>
        /// <param name="logText">Texto a ser guardado en el log</param>
        public static void WriteLogToDB(LogLevel loglevel, string logTitle, string logText)
        {

            SqlServerHelper SQLConection = new SqlServerHelper();
            string strSQLog = "";

            strSQLog = "INSERT INTO LogEventosAPI SELECT Nivel = '" + loglevel + "', Fecha = '" + DateTime.Now.ToString(FormatoFecha) + "', Hora = '" + DateTime.Now.ToString(FormatoHora) + "', Titulo = '" + logTitle + "', Mensaje = '" + logText + "'";
            SQLConection.ExecuteCRUD(strSQLog);
        }

    }
}