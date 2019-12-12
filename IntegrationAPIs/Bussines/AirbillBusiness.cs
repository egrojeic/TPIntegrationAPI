using CommonsWeb.DAL;
using IntegrationAPIs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Bussines
{
    public class AirbillBusiness
    {
        SqlServerHelper SQLConection = new SqlServerHelper();
        string strError = "";
        string strConfirmadas = "";
        string tmpMsg = "";

        public MsgResponse CargarAirbills(string prmFarm, AirbillRequest prmAirbillRequest)
        {
            MsgResponse msgResponse = new MsgResponse();
            try
            {
                string strSQL = "";
                string tmpAWB;
                string tmpDate;
                string tmpAirline;
                string tmpFarmInvoice;
                string tmpHAWB;
                string tmpChargeAgency;
                int tmpIDAirbill = 0;

                int tmpOrderCode;
                int tmpCustomerCode;
                string tmpCustomer;
                string tmpCodeBoxProduct;
                string tmpBoxedProduct;
                int tmpBoxCode;
                int tmpPack;
                int tmpQty;
                string tmpCost;

                string strRsta = "";
                strRsta = Validaciones(prmFarm, prmAirbillRequest);

                if (strRsta.Length == 0)
                {
                    for (int i = 0; i < prmAirbillRequest.Airbills.Count; i++)
                    {
                        tmpAWB = prmAirbillRequest.Airbills[i].AWB;
                        tmpDate = prmAirbillRequest.Airbills[i].Date.ToString("MM/dd/yyyy");
                        tmpFarmInvoice = prmAirbillRequest.Airbills[i].FarmInvoice;
                        tmpHAWB = prmAirbillRequest.Airbills[i].HAWB;
                        tmpChargeAgency = prmAirbillRequest.Airbills[i].ChargeAgencyCode;
                        tmpAirline = prmAirbillRequest.Airbills[i].AirlineCode;

                        strSQL = "EXEC CrearAirbillFromAPI '" + prmFarm + "' , '" + tmpAWB + "' , '" + tmpDate + "' , '" + tmpAirline + "' , '" + tmpFarmInvoice + "' , '" + tmpHAWB + "' , '" + tmpChargeAgency + "'";
                        tmpIDAirbill = Convert.ToInt32(SQLConection.ExecuteScalar(strSQL));

                        if (tmpIDAirbill > 0)
                        {
                            strConfirmadas = strConfirmadas + tmpAWB + " , ";

                            for (int j = 0; j < prmAirbillRequest.Airbills[i].Details.Count; j++)
                            {
                                tmpOrderCode = prmAirbillRequest.Airbills[i].Details[j].OrderCode;
                                tmpCustomerCode = prmAirbillRequest.Airbills[i].Details[j].CustomerCode;
                                tmpCustomer = prmAirbillRequest.Airbills[i].Details[j].Customer;
                                tmpCodeBoxProduct = prmAirbillRequest.Airbills[i].Details[j].CodeBoxProduct;
                                tmpBoxedProduct = prmAirbillRequest.Airbills[i].Details[j].BoxedProduct;
                                tmpBoxCode = prmAirbillRequest.Airbills[i].Details[j].BoxCode;
                                tmpPack = prmAirbillRequest.Airbills[i].Details[j].Pack;
                                tmpQty = prmAirbillRequest.Airbills[i].Details[j].Qty;
                                tmpCost = prmAirbillRequest.Airbills[i].Details[j].Cost.ToString().Replace(',', '.');

                                strSQL = "INSERT INTO tmpAirbillDetailsAPI VALUES(" + tmpIDAirbill + ", " + tmpOrderCode + ", " + tmpCustomerCode + ", '" + tmpCustomer + "', '" + tmpCodeBoxProduct + "', '" + tmpBoxedProduct + "', '" + tmpBoxCode + "', " + tmpPack + ", " + tmpQty + ", " + tmpCost + ")";
                                SQLConection.ExecuteCRUD(strSQL);
                            }

                            strSQL = "EXEC InterpretaDetallesAirbillAPI " + tmpIDAirbill;
                            SQLConection.ExecuteScalar(strSQL);
                        }
                        else
                        {
                            switch (tmpIDAirbill)
                            {
                                case 0:
                                    strError = strError + tmpAWB + " Invalid Farm , ";
                                    break;
                                case -1:
                                    strError = strError + tmpAWB + " Invalid Airline , ";
                                    break;
                                case -2:
                                    strError = strError + tmpAWB + " Invalid Charge Agency , ";
                                    break;

                            }
                        }
                    }

                    msgResponse.StatusCode = "200";
                    if (strError.Length > 0)
                    {
                        tmpMsg = "Airbills con Errores: " + strError;
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO AirbillBusiness:UploadAirbill", "Airbills con Errores - " + strError);
                    }
                    if (strConfirmadas.Length > 0)
                    {
                        tmpMsg = tmpMsg + " Airbills Creadas Correctamente: " + strConfirmadas;
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO AirbillBusiness:UploadAirbill", "Airbills Creadas - " + strConfirmadas);
                    }
                }
                else
                {
                    tmpMsg = "Airbills con Errores: " + strRsta;
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO AirbillBusiness:UploadAirbill", "Airbills con Errores - " + strError);
                }
                
                msgResponse.Message = tmpMsg;
            }
            catch (Exception ex)
            {
                Exception lex;

                lex = ex.InnerException != null ? ex.InnerException : ex;

                msgResponse.StatusCode = "500";
                msgResponse.Message = "UploadAirbill" + lex.Message.ToString();

                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN CAPA DE NEGOCIO AirbillBusiness:UploadAirbill", lex.Message);
                throw new Exception(lex.Message, lex);
            }

            return msgResponse;
        }

        private string Validaciones(string prmFarm, AirbillRequest prmAirbillRequest)
        {
            string strSQL = "";
            string strRsta = "";
            string tmpAWB;
            string tmpDate;
            int tmpCountAirbill = 0;
            int tmpOrderCode;
            int tmpCountFarmOrder;

            try
            {
                for (int i = 0; i < prmAirbillRequest.Airbills.Count; i++)
                {
                    tmpAWB = prmAirbillRequest.Airbills[i].AWB;
                    if (prmAirbillRequest.Airbills[i].Date != null && prmAirbillRequest.Airbills[i].Date != new DateTime())
                    {
                        tmpDate = prmAirbillRequest.Airbills[i].Date.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        strRsta = strRsta + "AWB " + tmpAWB + " Format Date Invalid";
                    }
                    if (prmAirbillRequest.Airbills[i].Date != null && prmAirbillRequest.Airbills[i].Date != new DateTime() && prmAirbillRequest.Airbills[i].Date < DateTime.Now.Date)
                    {
                        strRsta = strRsta + " AWB " + tmpAWB + " Wrong Date, It can't be less than today";
                    }

                    strSQL = "EXEC VerificaExistenciaAirbill '" + prmFarm + "' , '" + tmpAWB + "'";
                    tmpCountAirbill = Convert.ToInt32(SQLConection.ExecuteScalar(strSQL));

                    if (tmpCountAirbill != 0)
                    {
                        strRsta = strRsta + " AWB " + tmpAWB + " Already Exists";
                    }

                    for (int j = 0; j < prmAirbillRequest.Airbills[i].Details.Count; j++)
                    {
                        tmpOrderCode = prmAirbillRequest.Airbills[i].Details[j].OrderCode;
                        strSQL = "EXEC VerificaOrdenFinca '" + prmFarm + "' , " + tmpOrderCode;
                        tmpCountFarmOrder = Convert.ToInt32(SQLConection.ExecuteScalar(strSQL));

                        if (tmpCountFarmOrder == 0)
                        {
                            strRsta = strRsta + " Farm Code " + tmpOrderCode + " NOT Confirmed";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception lex;
                strRsta = "Error Validaciones";

                lex = ex.InnerException != null ? ex.InnerException : ex;

                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN CAPA DE NEGOCIO AirbillBusiness:UploadAirbill.Validaciones", lex.Message);
                throw new Exception(lex.Message, lex);
            }

            return strRsta;
    }

        public MsgResponse ActualizaEstadoAirbill(string prmFarm, AirbillStatusRequest prmairbillStatusRequest)
        {
            string strSQL = "";
            MsgResponse msgResponse = new MsgResponse();
            int tmpRsta = 0;
            string tmpAirbill;
            string tmpStatus;
            
            try
            {
                for (int i = 0; i < prmairbillStatusRequest.Airbills.Count; i++)
                {
                    tmpAirbill = prmairbillStatusRequest.Airbills[i].AWB;
                    tmpStatus = prmairbillStatusRequest.Airbills[i].Status;

                    strSQL = "EXEC ActualizaStatusAirbillAPI '" + tmpAirbill + "', '" + tmpStatus + "'";
                    tmpRsta = Convert.ToInt32(SQLConection.ExecuteScalar(strSQL));

                    if (tmpRsta == 0)
                    {
                        strError = strError + tmpAirbill + " , ";
                    }
                    else
                    {
                        strConfirmadas = strConfirmadas + tmpAirbill + " , ";
                    }
                }

                msgResponse.StatusCode = "200";
                if (strError.Length > 0)
                {
                    tmpMsg = "Airbills con Errores " + strError;
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO AirbillBusiness:UpdateStatusAirbill", "Airbills con Errores - " + strError);
                }
                if (strConfirmadas.Length > 0)
                {
                    tmpMsg = tmpMsg + " Airbills confirmadas " + strConfirmadas;
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO AirbillBusiness:UpdateStatusAirbill", "Airbills Confirmadas - " + strConfirmadas);
                }

                msgResponse.Message = tmpMsg;

            }
            catch (Exception ex)
            {
                Exception lex;

                lex = ex.InnerException != null ? ex.InnerException : ex;

                msgResponse.StatusCode = "500";
                msgResponse.Message = "UpdateStatusAirbill" + lex.Message.ToString();

                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN CAPA DE NEGOCIO AirbillBusiness:UpdateStatusAirbill", lex.Message);
                throw new Exception(lex.Message, lex);
            }

            return msgResponse;
        }
    }
}