using CommonsWeb.DAL;
using IntegrationAPIs.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Bussines
{
    public class SubstitutionBusiness
    {
        SqlServerHelper SQLConection = new SqlServerHelper();
        string strError = "";
        string strConfirmadas = "";
        string tmpMsg = "";

        public SubstitutionsResponse ObtieneSustituciones(string prmFarm)
        {
            SubstitutionsResponse ResponseSustituciones = new SubstitutionsResponse();
            ResponseSustituciones.Response = new MsgResponse();

            try
            {
                string strSQL = "";
                DataSet dsSustituciones;
                List<Substitutions> LstSustituciones = new List<Substitutions>();
                List<SubstitutionsDetails> LstDetalles = new List<SubstitutionsDetails>();

                strSQL = "EXEC ConsultaSustitucionesParaFincaAPI '" + prmFarm + "'";
                dsSustituciones = SQLConection.ExecuteProcedureToDataSet(strSQL);

                if (dsSustituciones != null && dsSustituciones.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dataRowSustituciones in dsSustituciones.Tables[0].Rows)
                    {
                        Substitutions Sustituciones;
                        SubstitutionsDetails SustitucionesDetalles;

                        SustitucionesDetalles = LstDetalles.Find(x => x.RegNumber == Convert.ToInt32(dataRowSustituciones["IDDetalles"]));
                        Sustituciones = LstSustituciones.Find(x => x.ID == Convert.ToInt32(dataRowSustituciones["ID"]));

                        if (SustitucionesDetalles == null)
                        {
                            SustitucionesDetalles = new SubstitutionsDetails();
                            SustitucionesDetalles.RegNumber = Convert.ToInt32(dataRowSustituciones["IDDetalles"]);
                            SustitucionesDetalles.SeasonCode = Convert.ToInt32(dataRowSustituciones["IDTemporadasDetallesPO"]);
                            SustitucionesDetalles.Season = (string)(dataRowSustituciones["IDTemporadasDetallesPO_Nombre"]);
                            SustitucionesDetalles.ModelCode = Convert.ToInt32(dataRowSustituciones["IDFloresRecetaModelos"]);
                            SustitucionesDetalles.CodeBoxProduct = (string)(dataRowSustituciones["CodProdComercial"]);
                            SustitucionesDetalles.BoxProduct = (string)(dataRowSustituciones["ProdComercial"]);
                            SustitucionesDetalles.BoxProductType = (string)(dataRowSustituciones["TipoProductoComercial"]);
                            SustitucionesDetalles.Pack = Convert.ToInt32(dataRowSustituciones["Pack"]);
                            SustitucionesDetalles.Qty = Convert.ToInt32(dataRowSustituciones["Qty"]);
                            SustitucionesDetalles.QtyConfirmed = Convert.ToInt32(dataRowSustituciones["QtyConfirmed"]);
                            SustitucionesDetalles.Stems = Convert.ToInt32(dataRowSustituciones["Stems"]);
                            SustitucionesDetalles.BoxCode = Convert.ToInt32(dataRowSustituciones["IDBox"]);
                            SustitucionesDetalles.Box = (string)(dataRowSustituciones["IDBox_Nombre"]);
                            SustitucionesDetalles.UnitCost = Convert.ToDecimal(dataRowSustituciones["UnitCost"]);
                            SustitucionesDetalles.TotalCost = Convert.ToDecimal(dataRowSustituciones["TotalCost"]);
                            //SustitucionesDetalles.PullDateWithFormat = (string)(dataRowSustituciones["PullDateWithFormat"]);
                            //SustitucionesDetalles.UPC = (string)(dataRowSustituciones["UPC"]);
                            //SustitucionesDetalles.UPCRetailPrice = Convert.ToDecimal(dataRowSustituciones["UPCRetailPrice"]);
                            //SustitucionesDetalles.Wet = (string)(dataRowSustituciones["Wet"]);
                            SustitucionesDetalles.Maritime = (string)(dataRowSustituciones["Container"]);
                            SustitucionesDetalles.SemaphoreSubs = Convert.ToInt32(dataRowSustituciones["NivelSustitucion"]);
                            SustitucionesDetalles.Notes = (string)(dataRowSustituciones["Notas"]);
                        }

                        if (Sustituciones == null)
                        {
                            Sustituciones = new Substitutions();
                            Sustituciones.ID = Convert.ToInt32(dataRowSustituciones["ID"]);
                            Sustituciones.PO = (string)(dataRowSustituciones["PO"]);
                            Sustituciones.OrderCode = Convert.ToInt32(dataRowSustituciones["Codigo"]);
                            Sustituciones.Type = (string)(dataRowSustituciones["TipoDocumento"]);
                            Sustituciones.FarmShipDate = Convert.ToDateTime(dataRowSustituciones["FarmShipDate"]).ToString("MM-dd-yyyy");
                            Sustituciones.MiamiShipDate = Convert.ToDateTime(dataRowSustituciones["MiamiShipDate"]).ToString("MM-dd-yyyy");
                            Sustituciones.DeliveryDate = Convert.ToDateTime(dataRowSustituciones["DeliveryDate"]).ToString("MM-dd-yyyy");
                            //Sustituciones.Status = (string)(dataRowSustituciones["EstadoFinca"]);
                            //Sustituciones.PullDate = Convert.ToDateTime(dataRowSustituciones["PullDate"]).ToString("MM-dd-yyyy");
                            Sustituciones.CustomerCode = Convert.ToInt32(dataRowSustituciones["IDClientes_Codigo"]);
                            Sustituciones.Customer = (string)(dataRowSustituciones["IDClientes_Nombre"]);
                            Sustituciones.Farm = (string)(dataRowSustituciones["Farm"]);
                            Sustituciones.Details = new List<SubstitutionsDetails>();

                            if (SustitucionesDetalles != null)
                            {
                                Sustituciones.Details.Add(SustitucionesDetalles);
                                LstDetalles.Add(SustitucionesDetalles);
                            }

                            LstSustituciones.Add(Sustituciones);
                        }
                        else
                        {
                            if (!Sustituciones.Details.Contains(SustitucionesDetalles))
                                Sustituciones.Details.Add(SustitucionesDetalles);
                            LstDetalles.Add(SustitucionesDetalles);
                        }
                    }

                    ResponseSustituciones.Substitutions = LstSustituciones;
                    ResponseSustituciones.Response.StatusCode = "200";
                    ResponseSustituciones.Response.Message = "Sustituciones Para Finca Listadas Correctamente";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Information, "CAPA DE NEGOCIO SubstitutionsBusiness:ObtieneSustituciones", "Sustituciones Para Finca Listadas Correctamente - " + strSQL);
                }
                else
                {
                    ResponseSustituciones.Response.StatusCode = "200";
                    ResponseSustituciones.Response.Message = "No Existen Sustituciones Para Finca";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Information, "CAPA DE NEGOCIO SubstitutionsBusiness:ObtieneSustituciones", "No Existen Sustituciones Para Finca - " + strSQL);
                }
            }
            catch (Exception ex)
            {
                Exception lex;

                lex = ex.InnerException != null ? ex.InnerException : ex;
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN CAPA DE NEGOCIO SubstitutionsBusiness:ObtieneSustituciones", lex.Message);
                throw new Exception(lex.Message, lex);
            }

            return ResponseSustituciones;
        }

        public MsgResponse CargaConfirmacionSust(string prmFarm, SubsConfirmRequest prmconfirmaSustitucion)
        {
            MsgResponse msgResponse = new MsgResponse();
            try
            {
                string strSQL = "";
                int tmpRegNumber = 0;
                int tmpRsta;
                int tmpAprobado;
                int tmpRevisado;

                string strRsta = "";
                strRsta = ValidacionesConfirm(prmFarm, prmconfirmaSustitucion);

                if (strRsta.Length == 0)
                {
                    for (int i = 0; i < prmconfirmaSustitucion.SubstitutionsConfirm.Count; i++)
                    {
                        tmpRegNumber = prmconfirmaSustitucion.SubstitutionsConfirm[i].RegNumber;
                        tmpAprobado = prmconfirmaSustitucion.SubstitutionsConfirm[i].Approved.ToUpper().ToString() == "SI" ? 1 : 0;
                        tmpRevisado = prmconfirmaSustitucion.SubstitutionsConfirm[i].Revised.ToUpper().ToString() == "SI" ? 1 : 0;

                        strSQL = "EXEC ConfirmaSustitucionFincaFromAPI '" + prmFarm + "' , " + tmpRegNumber + " , " + tmpAprobado + ", " + tmpRevisado;
                        tmpRsta = Convert.ToInt32(SQLConection.ExecuteScalar(strSQL));

                        switch (tmpRsta)
                        {
                            case 0:
                                strError = strError + " Invalid RegNumber: " + tmpRegNumber + ", ";
                                break;
                            case 1:
                                strConfirmadas = strConfirmadas + "RegNumber: " + tmpRegNumber + ", ";
                                break;
                        }
                    }
                    msgResponse.StatusCode = "200";
                    if (strError.Length > 0)
                    {
                        tmpMsg = "Sustituciones con Errores: " + strError;
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO SubstitutionBusiness:CargaConfirmacionSust", "Sustituciones con Errores - " + strError);
                    }
                    if (strConfirmadas.Length > 0)
                    {
                        tmpMsg = tmpMsg + " Sustituciones Confirmadas Correctamente: " + strConfirmadas;
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO SubstitutionBusiness:CargaConfirmacionSust", "Sustituciones Confirmadas - " + strConfirmadas);
                    }
                }
                else
                {
                    tmpMsg = "Sustituciones con Errores: " + strRsta;
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO SubstitutionBusiness:CargaConfirmacionSust", "Sustituciones con Errores - " + strError);
                }

                msgResponse.Message = tmpMsg;
            }
            catch (Exception ex)
            {
                Exception lex;

                lex = ex.InnerException != null ? ex.InnerException : ex;

                msgResponse.StatusCode = "500";
                msgResponse.Message = "UploadConfirmSubstitutions" + lex.Message.ToString();

                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN CAPA DE NEGOCIO SubstitutionBusiness:CargaConfirmacionSust", lex.Message);
                throw new Exception(lex.Message, lex);
            }

            return msgResponse;
        }

        public SubsConfirmResponse ObtieneConfirmacionSust(string prmFarm)
        {
            SubsConfirmResponse ResponseSubsConfirm = new SubsConfirmResponse();
            ResponseSubsConfirm.Response = new MsgResponse();

            try
            {
                string strSQL = "";
                DataSet dsConfirmSustituciones;
                List<SubsConfirm> LstSustConfirm = new List<SubsConfirm>();

                strSQL = "EXEC ConsultaConfirmacionesSustitucionesAPI '" + prmFarm + "'";
                dsConfirmSustituciones = SQLConection.ExecuteProcedureToDataSet(strSQL);

                if (dsConfirmSustituciones != null && dsConfirmSustituciones.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dataRowSustituciones in dsConfirmSustituciones.Tables[0].Rows)
                    {
                        SubsConfirm Sustituciones;

                        Sustituciones = new SubsConfirm();
                        Sustituciones.PO = (string)(dataRowSustituciones["PO"]);
                        Sustituciones.OrderCode = Convert.ToInt32(dataRowSustituciones["Codigo"]);
                        Sustituciones.FarmShipDate = Convert.ToDateTime(dataRowSustituciones["FarmShipDate"]).ToString("MM-dd-yyyy");
                        Sustituciones.MiamiShipDate = Convert.ToDateTime(dataRowSustituciones["MiamiShipDate"]).ToString("MM-dd-yyyy");
                        Sustituciones.CustomerCode = Convert.ToInt32(dataRowSustituciones["IDClientes_Codigo"]);
                        Sustituciones.Customer = (string)(dataRowSustituciones["IDClientes_Nombre"]);
                        Sustituciones.RegNumber = Convert.ToInt32(dataRowSustituciones["IDDetalles"]);
                        Sustituciones.SeasonCode = Convert.ToInt32(dataRowSustituciones["IDTemporadasDetallesPO"]);
                        Sustituciones.Season = (string)(dataRowSustituciones["IDTemporadasDetallesPO_Nombre"]);
                        Sustituciones.ModelCode = Convert.ToInt32(dataRowSustituciones["IDFloresRecetaModelos"]);
                        Sustituciones.CodeBoxProduct = (string)(dataRowSustituciones["CodProdComercial"]);
                        Sustituciones.BoxProduct = (string)(dataRowSustituciones["ProdComercial"]);
                        Sustituciones.Pack = Convert.ToInt32(dataRowSustituciones["Pack"]);
                        Sustituciones.Qty = Convert.ToInt32(dataRowSustituciones["Qty"]);
                        Sustituciones.QtyConfirmed = Convert.ToInt32(dataRowSustituciones["QtyConfirmed"]);
                        Sustituciones.BoxCode = Convert.ToInt32(dataRowSustituciones["IDBox"]);
                        Sustituciones.Box = (string)(dataRowSustituciones["IDBox_Nombre"]);
                        Sustituciones.Approved = (string)(dataRowSustituciones["Aprobado"]);
                        Sustituciones.Revised = (string)(dataRowSustituciones["Revisado"]);
                        LstSustConfirm.Add(Sustituciones);
                    }

                    ResponseSubsConfirm.SubstitutionsConfirm = LstSustConfirm;
                    ResponseSubsConfirm.Response.StatusCode = "200";
                    ResponseSubsConfirm.Response.Message = "Confirmacion de Sustituciones de Finca Listadas Correctamente";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Information, "CAPA DE NEGOCIO SubstitutionsBusiness:ObtieneConfirmacionSust", "Confirmacion de Sustituciones de Finca Listadas Correctamente - " + strSQL);
                }
                else
                {
                    ResponseSubsConfirm.Response.StatusCode = "200";
                    ResponseSubsConfirm.Response.Message = "No Existen Confirmacion de Sustituciones de Finca";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Information, "CAPA DE NEGOCIO SubstitutionsBusiness:ObtieneConfirmacionSust", "Confirmacion de Sustituciones de Finca - " + strSQL);
                }
            }
            catch (Exception ex)
            {
                Exception lex;

                lex = ex.InnerException != null ? ex.InnerException : ex;
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN CAPA DE NEGOCIO SubstitutionsBusiness:ObtieneConfirmacionSust", lex.Message);
                throw new Exception(lex.Message, lex);
            }

            return ResponseSubsConfirm;
        }

        public MsgResponse CargaSustitucion(string prmFarm, SubstitutionsRequest prmSustitucionesFinca)
        {
            MsgResponse msgResponse = new MsgResponse();
            try
            {
                string strSQL = "";
                int tmpRegNumber = 0;
                string tmpNotas;
                int tmpRsta;

                string strRsta = "";
                strRsta = Validaciones(prmFarm, prmSustitucionesFinca);

                if (strRsta.Length == 0)
                {
                    for (int i = 0; i < prmSustitucionesFinca.Substitutions.Count; i++)
                    {
                        for (int j = 0; j < prmSustitucionesFinca.Substitutions[i].Details.Count; j++)
                        {
                            tmpRegNumber = prmSustitucionesFinca.Substitutions[i].Details[j].RegNumber;
                            tmpNotas = prmSustitucionesFinca.Substitutions[i].Details[j].Notes;

                            strSQL = "EXEC CrearSustitucionFromAPI '" + prmFarm + "' , " + tmpRegNumber + " , '" + tmpNotas + "'";
                            tmpRsta = Convert.ToInt32(SQLConection.ExecuteScalar(strSQL));

                            switch (tmpRsta)
                            {
                                case 0:
                                    strError = strError + " Invalid RegNumber: " + tmpRegNumber + ", ";
                                    break;
                                case 1:
                                    strConfirmadas = strConfirmadas + "RegNumber: " + tmpRegNumber + ", ";
                                    break;
                            }
                        }
                    }

                    msgResponse.StatusCode = "200";
                    if (strError.Length > 0)
                    {
                        tmpMsg = "Sustituciones con Errores: " + strError;
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO SubstitutionBusiness:CargaSustitucion", "Sustituciones con Errores - " + strError);
                    }
                    if (strConfirmadas.Length > 0)
                    {
                        tmpMsg = tmpMsg + " Sustituciones Creadas Correctamente: " + strConfirmadas;
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO SubstitutionBusiness:CargaSustitucion", "Sustituciones Creadas - " + strConfirmadas);
                    }
                }
                else
                {
                    tmpMsg = "Sustituciones con Errores: " + strRsta;
                    msgResponse.StatusCode = "500";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO SubstitutionBusiness:CargaSustitucion", "Sustituciones con Errores - " + strError);
                }

                msgResponse.Message = tmpMsg;
            }
            catch (Exception ex)
            {
                Exception lex;

                lex = ex.InnerException != null ? ex.InnerException : ex;

                msgResponse.StatusCode = "500";
                msgResponse.Message = "UploadSubstitutions" + lex.Message.ToString();

                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN CAPA DE NEGOCIO SubstitutionBusiness:CargaSustitucion", lex.Message);
                throw new Exception(lex.Message, lex);
            }

            return msgResponse;
        }

        private string Validaciones(string prmFarm, SubstitutionsRequest prmSustitucionesFinca)
        {
            string strSQL = "";
            string strRsta = "";
            int tmpRegNumber;
            string tmpNotes;
            int tmpOrderCode;
            int tmpCountFarmOrder;
            int tmpCountSustitucion;

            try
            {
                for (int i = 0; i < prmSustitucionesFinca.Substitutions.Count; i++)
                {
                    tmpOrderCode = prmSustitucionesFinca.Substitutions[i].OrderCode;
                    strSQL = "EXEC VerificaOrdenFinca '" + prmFarm + "' , " + tmpOrderCode;
                    tmpCountFarmOrder = Convert.ToInt32(SQLConection.ExecuteScalar(strSQL));

                    if (tmpCountFarmOrder == 0)
                    {
                        strRsta = strRsta + " Farm Code " + tmpOrderCode + " NOT Confirmed";
                    }

                    for (int j = 0; j < prmSustitucionesFinca.Substitutions[i].Details.Count; j++)
                    {
                        tmpNotes = prmSustitucionesFinca.Substitutions[i].Details[j].Notes;
                        if (tmpNotes.Trim().Length  == 0)
                        {
                            strRsta = strRsta + " Notes Invalid. Order Code " + tmpOrderCode;
                        }

                        tmpRegNumber = prmSustitucionesFinca.Substitutions[i].Details[j].RegNumber;

                        if (tmpRegNumber == 0)
                        {
                            strRsta = strRsta + " RegNumber Invalid. Order Code " + tmpOrderCode;
                        }
                        else
                        {
                            strSQL = "EXEC VerificaExistenciaSustitucionAPI '" + prmFarm + "' , '" + tmpRegNumber + "', 1";
                            tmpCountSustitucion = Convert.ToInt32(SQLConection.ExecuteScalar(strSQL));

                            if (tmpCountSustitucion != 0)
                            {
                                strRsta = strRsta + " Substitution " + tmpRegNumber + " Already Exists";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception lex;
                strRsta = "Error Validaciones";

                lex = ex.InnerException != null ? ex.InnerException : ex;

                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN CAPA DE NEGOCIO SubstitutionsBusiness:CargaSustitucion.Validaciones", lex.Message);
                throw new Exception(lex.Message, lex);
            }

            return strRsta;
        }

        private string ValidacionesConfirm(string prmFarm, SubsConfirmRequest prmsubsConfirmRequest)
        {
            string strSQL = "";
            string strRsta = "";
            int tmpRegNumber;
            string tmpAprobado;
            string tmpRevisado;
            int tmpOrderCode;
            int tmpCountSustitucion;
            try
            {
                for (int i = 0; i < prmsubsConfirmRequest.SubstitutionsConfirm.Count; i++)
                {
                    tmpOrderCode = prmsubsConfirmRequest.SubstitutionsConfirm[i].OrderCode;
                    tmpRegNumber = prmsubsConfirmRequest.SubstitutionsConfirm[i].RegNumber;

                    if (tmpRegNumber == 0)
                    {
                        strRsta = strRsta + " RegNumber Invalid. Order Code " + tmpOrderCode;
                    }
                    else
                    {
                        strSQL = "EXEC VerificaExistenciaSustitucionAPI '" + prmFarm + "' , '" + tmpRegNumber + "', 0";
                        tmpCountSustitucion = Convert.ToInt32(SQLConection.ExecuteScalar(strSQL));

                        if (tmpCountSustitucion == 0)
                        {
                            strRsta = strRsta + " Substitution " + tmpRegNumber + " Not Exists";
                        }
                    }

                    tmpAprobado = prmsubsConfirmRequest.SubstitutionsConfirm[i].Approved;

                    if (tmpAprobado.ToUpper().ToString() != "SI" & tmpAprobado.ToUpper().ToString() != "NO")
                    {
                        strRsta = strRsta + " RegNumber: " + tmpRegNumber + " Field Approved Invalid";
                    }

                    tmpRevisado = prmsubsConfirmRequest.SubstitutionsConfirm[i].Revised;

                    if (tmpRevisado.ToUpper().ToString() != "SI" & tmpRevisado.ToUpper().ToString() != "NO")
                    {
                        strRsta = strRsta + " RegNumber: " + tmpRegNumber + " Field Revised Invalid";
                    }
                }
            }
            catch (Exception ex)
            {
                Exception lex;
                strRsta = "Error Validaciones";

                lex = ex.InnerException != null ? ex.InnerException : ex;

                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN CAPA DE NEGOCIO SubstitutionsBusiness:CargaConfirmacionSust.ValidacionesConfirm", lex.Message);
                throw new Exception(lex.Message, lex);
            }

            return strRsta;
        }
    }
}