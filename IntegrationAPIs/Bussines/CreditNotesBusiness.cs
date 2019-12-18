using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntegrationAPIs.Models;
using CommonsWeb.DAL;
using System.Data;

namespace IntegrationAPIs.Bussines
{
    public class CreditNotesBusiness
    {
        SqlServerHelper SQLConection = new SqlServerHelper();
        string fechaInicial = "1900/01/01";
        string fechaFinal = "1900/01/01";
        int codigo = 0;
        string strError = "";
        string strConfirmadas = "";
        string tmpMsg = "";

        public CreditNotesResponse GetNotasCredito(string prmFarm, CreditNotesRequest prmcreditNRequest)
        {
            CreditNotesResponse ResponseCredit = new CreditNotesResponse();
            ResponseCredit.Response = new MsgResponse();

            try
            {
                ObtenerDatos(prmcreditNRequest);

                string strSQL = "";
                DataSet dsNotasCredito;
                List<CreditNotes> LstCreditNotes = new List<CreditNotes>();
                List<CreditNotesDetails> LstCreditNotesDetails = new List<CreditNotesDetails>();

                strSQL = "EXEC ConsultaNotasCreditoAPI '" + prmFarm + "', '" + fechaInicial + "' , '" + fechaFinal + "', " + codigo;
                dsNotasCredito = SQLConection.ExecuteProcedureToDataSet(strSQL);

                if (dsNotasCredito != null && dsNotasCredito.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dataRowCreditos in dsNotasCredito.Tables[0].Rows)
                    {
                        CreditNotes Creditos;
                        CreditNotesDetails CreditosDetalles;
                        
                        CreditosDetalles = LstCreditNotesDetails.Find(x => x.ID == Convert.ToInt32(dataRowCreditos["IDDetalles"]));
                        Creditos = LstCreditNotes.Find(x => x.ID == Convert.ToInt32(dataRowCreditos["ID"]));

                        if (CreditosDetalles == null)
                        {
                            CreditosDetalles = new CreditNotesDetails();
                            CreditosDetalles.ID = Convert.ToInt32(dataRowCreditos["IDDetalles"]);
                            CreditosDetalles.CodeBoxProduct = Convert.ToString(dataRowCreditos["CodFlores"]);
                            CreditosDetalles.BoxProduct = Convert.ToString(dataRowCreditos["Flores"]);
                            CreditosDetalles.Pack = Convert.ToInt32(dataRowCreditos["Pack"]);
                            CreditosDetalles.Qty = Convert.ToInt32(dataRowCreditos["Cantidad"]);
                            CreditosDetalles.UnitPrice = Convert.ToDecimal(dataRowCreditos["PrecioUnit"]);
                            CreditosDetalles.AdditionalCreditValue = Convert.ToDecimal(dataRowCreditos["ValorAdicionalCredito"]);
                            CreditosDetalles.Invoice = Convert.ToInt32(dataRowCreditos["NumFactura"]);
                            CreditosDetalles.CompanyCode = Convert.ToInt32(dataRowCreditos["CompanyCode"]);
                            CreditosDetalles.SeasonCode = Convert.ToInt32(dataRowCreditos["IDTemporadasDetallesPO"]);
                            CreditosDetalles.CustomerCode = Convert.ToInt32(dataRowCreditos["IDClientes_Codigo"]);
                            CreditosDetalles.CodeBoxProduct = Convert.ToString(dataRowCreditos["CodProdComercial"]);
                            CreditosDetalles.ModelCode = Convert.ToInt32(dataRowCreditos["IDFloresRecetaModelos"]);
                            CreditosDetalles.TrackBoxCode = Convert.ToInt32(dataRowCreditos["CodCaja"]);
                        }
                        
                        if (Creditos == null)
                        {
                            Creditos = new CreditNotes();
                            Creditos.ID = Convert.ToInt32(dataRowCreditos["ID"]);
                            Creditos.Code = Convert.ToInt32(dataRowCreditos["Codigo"]);
                            Creditos.Date = Convert.ToDateTime(dataRowCreditos["Fecha"]).ToString("MM-dd-yyyy");
                            Creditos.Status = Convert.ToString(dataRowCreditos["Estado"]);
                            Creditos.Reason = Convert.ToString(dataRowCreditos["BAS_MotivosCreditos"]);
                            Creditos.Airbill = Convert.ToString(dataRowCreditos["CodAirBill"]);
                            Creditos.TotalValue = Convert.ToDecimal(dataRowCreditos["ValorTotal"]);
                            Creditos.Details = new List<CreditNotesDetails>();

                            if (CreditosDetalles != null)
                            {
                                Creditos.Details.Add(CreditosDetalles);
                                LstCreditNotesDetails.Add(CreditosDetalles);
                            }

                            LstCreditNotes.Add(Creditos);
                        }
                        else
                        {
                            if (!Creditos.Details.Contains(CreditosDetalles))
                                Creditos.Details.Add(CreditosDetalles);
                            LstCreditNotesDetails.Add(CreditosDetalles);
                        }
                    }

                    ResponseCredit.CreditNotes = LstCreditNotes;
                    ResponseCredit.Response.StatusCode = "200";
                    ResponseCredit.Response.Message = "Notas Credito Listadas Correctamente";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Information, "CAPA DE NEGOCIO CreditNotesBusiness:GetCredits", "Notas Credito Listadas Correctamente - " + strSQL);
                }
                else
                {
                    ResponseCredit.Response.StatusCode = "200";
                    ResponseCredit.Response.Message = "No Existen Notas Credito";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Information, "CAPA DE NEGOCIO CreditNotesBusiness:GetCredits", "No Existen Notas Credito - " + strSQL);
                }
            }
            catch (Exception ex)
            {
                Exception lex;

                lex = ex.InnerException != null ? ex.InnerException : ex;
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN CAPA DE NEGOCIO CreditNotesBusiness:GetCredits", lex.Message);
                throw new Exception(lex.Message, lex);
            }

            return ResponseCredit;
        }

        public MsgResponse ActualizaEstadoNotasCredito(string prmFarm, CreditNotesStatusRequest prmCreditNStatusRequest)
        {
            string strSQL = "";
            MsgResponse msgResponse = new MsgResponse();
            int tmpRsta = 0;
            int tmpCodigo;
            string tmpStatus;

            try
            {
                for (int i = 0; i < prmCreditNStatusRequest.Credits.Count; i++)
                {
                    tmpCodigo = prmCreditNStatusRequest.Credits[i].Code;
                    tmpStatus = prmCreditNStatusRequest.Credits[i].Status;

                    strSQL = "EXEC ActualizaStatusCreditNoteAPI '" + tmpCodigo + "', '" + tmpStatus + "'";
                    tmpRsta = Convert.ToInt32(SQLConection.ExecuteScalar(strSQL));

                    if (tmpRsta == 0)
                    {
                        strError = strError + tmpCodigo + " , ";
                    }
                    else
                    {
                        strConfirmadas = strConfirmadas + tmpCodigo + " , ";
                    }
                }

                msgResponse.StatusCode = "200";
                if (strError.Length > 0)
                {
                    tmpMsg = "Notas Credito con Errores " + strError;
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO CreditNotesBusiness:UpdateStatusCredits", "Credits con Errores - " + strError);
                }
                if (strConfirmadas.Length > 0)
                {
                    tmpMsg = tmpMsg + " Notas Credito Actualizadas " + strConfirmadas;
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO CreditNotesBusiness:UpdateStatusCredits", "Credits Actualizadas - " + strConfirmadas);
                }

                msgResponse.Message = tmpMsg;

            }
            catch (Exception ex)
            {
                Exception lex;

                lex = ex.InnerException != null ? ex.InnerException : ex;

                msgResponse.StatusCode = "500";
                msgResponse.Message = "UpdateStatusCredits" + lex.Message.ToString();

                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN CAPA DE NEGOCIO CreditNotesBusiness:UpdateStatusCredits", lex.Message);
                throw new Exception(lex.Message, lex);
            }

            return msgResponse;
        }

        private void ObtenerDatos(CreditNotesRequest prmOrderRequest)
        {
            try
            {
                if (prmOrderRequest.InitialDate != null && prmOrderRequest.InitialDate != new DateTime())
                {
                    fechaInicial = prmOrderRequest.InitialDate.ToString("MM/dd/yyyy");
                }
                if (prmOrderRequest.FinalDate != null && prmOrderRequest.FinalDate != new DateTime())
                {
                    fechaFinal = prmOrderRequest.FinalDate.ToString("MM/dd/yyyy");
                }
                if (prmOrderRequest.Code != 0)
                {
                    codigo = prmOrderRequest.Code;
                }

            }
            catch (Exception ex)
            {
                Exception lex;


                lex = ex.InnerException != null ? ex.InnerException : ex;
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN CAPA DE NEGOCIO OrdersBusiness:ObtenerDatos", lex.Message);
                throw new Exception(lex.Message, lex);
            }

        }
    }

   
}