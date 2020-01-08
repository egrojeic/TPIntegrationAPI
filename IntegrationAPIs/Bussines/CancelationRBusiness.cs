using CommonsWeb.DAL;
using IntegrationAPIs.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Bussines
{
    public class CancelationRBusiness
    {
        SqlServerHelper SQLConection = new SqlServerHelper();

        public CancelationResponse GetCancelations(string prmFarm)
        {
            CancelationResponse ResponseCancelations = new CancelationResponse();
            ResponseCancelations.Response = new MsgResponse();

            try
            {
                string strSQL = "";
                DataSet dsCancelaciones;
                List<Cancelations> LstCancelations = new List<Cancelations>();

                strSQL = "EXEC ConsultaSolicitudesCancelacionAPI '" + prmFarm + "'";
                dsCancelaciones = SQLConection.ExecuteProcedureToDataSet(strSQL);

                if (dsCancelaciones != null && dsCancelaciones.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dataRowCancelaciones in dsCancelaciones.Tables[0].Rows)
                    {
                        Cancelations Cancelaciones;

                        Cancelaciones = new Cancelations();
                        Cancelaciones.RegNumber = Convert.ToInt32(dataRowCancelaciones["IDDetalles"]);
                        Cancelaciones.SeasonCode = Convert.ToInt32(dataRowCancelaciones["IDTemporadasDetallesPO"]);
                        Cancelaciones.Season = (string)(dataRowCancelaciones["IDTemporadasDetallesPO_Nombre"]);
                        Cancelaciones.ModelCode = Convert.ToInt32(dataRowCancelaciones["IDFloresRecetaModelos"]);
                        Cancelaciones.CodeBoxProduct = (string)(dataRowCancelaciones["CodProdComercial"]);
                        Cancelaciones.BoxProduct = (string)(dataRowCancelaciones["ProdComercial"]);
                        Cancelaciones.Pack = Convert.ToInt32(dataRowCancelaciones["Pack"]);
                        Cancelaciones.QtyToCancel = Convert.ToInt32(dataRowCancelaciones["Qty"]);
                        Cancelaciones.BoxCode = Convert.ToInt32(dataRowCancelaciones["IDBox"]);
                        Cancelaciones.Box = (string)(dataRowCancelaciones["IDBox_Nombre"]);
                        Cancelaciones.PO = (string)(dataRowCancelaciones["PO"]);
                        Cancelaciones.OrderCode = Convert.ToInt32(dataRowCancelaciones["Codigo"]);
                        Cancelaciones.FarmShipDate = Convert.ToDateTime(dataRowCancelaciones["FarmShipDate"]).ToString("MM-dd-yyyy");
                        Cancelaciones.MiamiShipDate = Convert.ToDateTime(dataRowCancelaciones["MiamiShipDate"]).ToString("MM-dd-yyyy");
                        Cancelaciones.CustomerCode = Convert.ToInt32(dataRowCancelaciones["IDClientes_Codigo"]);
                        Cancelaciones.Customer = (string)(dataRowCancelaciones["IDClientes_Nombre"]);
                        Cancelaciones.CustomerOrderCode = Convert.ToInt32(dataRowCancelaciones["CustomerOrderCode"]);
                        LstCancelations.Add(Cancelaciones);
                    }

                    ResponseCancelations.CancelationsRequest = LstCancelations;
                    ResponseCancelations.Response.StatusCode = "200";
                    ResponseCancelations.Response.Message = "Solicitudes de Cancelacion Listadas Correctamente";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Information, "CAPA DE NEGOCIO CancelationRBusiness:GetCancelations", "Solicitudes de Cancelacion Listadas Correctamente - " + strSQL);
                }
                else
                {
                    ResponseCancelations.Response.StatusCode = "200";
                    ResponseCancelations.Response.Message = "No Existen Cancelaciones de Produccion";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Information, "CAPA DE NEGOCIO CancelationRBusiness:GetCancelations", "No Existen Solicitud de Cancelacion - " + strSQL);
                }
            }
            catch (Exception ex)
            {
                Exception lex;

                lex = ex.InnerException != null ? ex.InnerException : ex;
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN CAPA DE NEGOCIO CancelationRBusiness:GetCancelations", lex.Message);
                throw new Exception(lex.Message, lex);
            }

            return ResponseCancelations;
        }

        public MsgResponse ConfirmCancelations(string prmFarm, CancelationsConfirmRequest prmCancelationConfirm)
        {
            MsgResponse msgResponse = new MsgResponse();
            string strSQL = "";
            string strError = "";
            string strConfirmadas = "";
            string strAdvConfirmada = "";
            string strMensaje = "";

            int tmpRsta = 0;

            int tmpRegNumber;
            string tmpCancel;
            int tmpCancelar;

            try
            {
                for (int i = 0; i < prmCancelationConfirm.Confirmations.Count; i++)
                {
                    tmpRegNumber = prmCancelationConfirm.Confirmations[i].RegNumber;
                    if (tmpRegNumber == 0)
                    {
                        throw new System.Exception("RegNumber Invalid");
                    }

                    tmpCancel = prmCancelationConfirm.Confirmations[i].Cancel;

                    if (tmpCancel.ToUpper().ToString() == "SI" | tmpCancel.ToUpper().ToString() == "NO")
                    {
                        tmpCancelar = tmpCancel.ToUpper().ToString() == "SI" ? 1 : 0; 
                    }
                    else
                    {
                        throw new System.Exception("Field Cancel Invalid");
                    }

                    strSQL = "EXEC ConfirmaSolicitudCancelacionAPI '" + prmFarm + "', " + tmpRegNumber + ", " + tmpCancelar;
                    tmpRsta = Convert.ToInt32(SQLConection.ExecuteScalar(strSQL));

                    switch (tmpRsta)
                    {
                        case 0:
                            strError = strError + tmpRegNumber + " , ";
                            break;
                        case 1:
                            strConfirmadas = strConfirmadas + tmpRegNumber + " , ";
                            break;
                        case 2:
                            strAdvConfirmada = strAdvConfirmada + tmpRegNumber + " , ";
                            break;
                    }
                }

                msgResponse.StatusCode = "200";
                if (strError.Length > 0)
                {
                    strMensaje = "RegNumber con Errores " + strError;
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO CancelationRBusiness:ConfirmCancelations", "IDOrdenesCompraFincasDetalles Errores - " + strError);
                }

                if (strConfirmadas.Length > 0)
                {
                    strMensaje = strMensaje + " RegNumber Confirmados " + strConfirmadas;
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO CancelationRBusiness:ConfirmCancelations", "IDOrdenesCompraFincasDetalles Confirmadas - " + strConfirmadas);
                }

                if (strAdvConfirmada.Length > 0)
                {
                    strMensaje = strMensaje + " RegNumber Anteriormente Confirmados " + strAdvConfirmada;
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO CancelationRBusiness:ConfirmCancelations", "IDOrdenesCompraFincasDetalles Anteriormente Confirmados - " + strAdvConfirmada);
                }

                msgResponse.Message = strMensaje;
            }
            catch (Exception ex)
            {
                Exception lex;

                lex = ex.InnerException != null ? ex.InnerException : ex;
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN CAPA DE NEGOCIO CancelationRBusiness:ConfirmCancelations", lex.Message);

                msgResponse.StatusCode = "500";
                msgResponse.Message = lex.Message;

                throw new Exception(lex.Message, lex);
            }

            return msgResponse;
        }
    }
}