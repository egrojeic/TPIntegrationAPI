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

                for (int i = 0; i < prmAirbillRequest.Airbills.Count; i++)
                {
                    tmpAWB = prmAirbillRequest.Airbills[i].AWB;
                    if (prmAirbillRequest.Airbills[i].Date != null && prmAirbillRequest.Airbills[i].Date != new DateTime())
                    {
                        tmpDate = prmAirbillRequest.Airbills[i].Date.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        throw new System.Exception("Format Date Invalid");
                    }

                    tmpAirline = prmAirbillRequest.Airbills[i].Airline;
                    tmpFarmInvoice = prmAirbillRequest.Airbills[i].FarmInvoice;
                    tmpHAWB = prmAirbillRequest.Airbills[i].HAWB;
                    tmpChargeAgency = prmAirbillRequest.Airbills[i].ChargeAgency == null ? "" : prmAirbillRequest.Airbills[i].ChargeAgency;

                    strSQL = "EXEC CrearAirbillFromAPI '" + prmFarm + "' , '" + tmpAWB + "' , '" + tmpDate + "' , '" + tmpAirline + "' , '" + tmpFarmInvoice + "' , '" + tmpHAWB + "' , '" + tmpChargeAgency + "'";
                    tmpIDAirbill = Convert.ToInt32(SQLConection.ExecuteScalar(strSQL));

                    if (tmpIDAirbill != 0)
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
                            tmpCost = prmAirbillRequest.Airbills[i].Details[j].Cost.ToString().Replace(',','.');

                            strSQL = "INSERT INTO tmpAirbillDetailsAPI VALUES(" + tmpIDAirbill + ", " + tmpOrderCode + ", " + tmpCustomerCode + ", '" + tmpCustomer + "', '" + tmpCodeBoxProduct + "', '" + tmpBoxedProduct + "', '" + tmpBoxCode + "', " + tmpPack + ", " + tmpQty + ", " + tmpCost + ")";
                            SQLConection.ExecuteCRUD(strSQL);
                        }

                        strSQL = "EXEC InterpretaDetallesAirbillAPI " + tmpIDAirbill;
                        SQLConection.ExecuteScalar(strSQL);
                    }
                    else
                    {
                        strError = strError + tmpAWB + " , ";
                    }
                }

                msgResponse.StatusCode = "200";
                if (strError.Length > 0)
                {
                    tmpMsg = "Airbills con Errores " + strError;
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO AirbillBusiness:UploadAirbill", "Airbills con Errores - " + strError);
                }
                if (strConfirmadas.Length > 0)
                {
                    tmpMsg = tmpMsg + " Airbills Creadas " + strConfirmadas;
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO AirbillBusiness:UploadAirbill", "Airbills Creadas - " + strConfirmadas);
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