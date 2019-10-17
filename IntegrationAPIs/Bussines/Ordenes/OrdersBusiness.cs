using CommonsWeb.DAL;
using IntegrationAPIs.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IntegrationAPIs.Bussines.Ordenes
{
    public class OrdersBusiness
    {
        SqlServerHelper SQLConection = new SqlServerHelper();
        string fechaInicial = "1900/01/01";
        string fechaFinal = "1900/01/01";
        int codigoOrden = 0;

        public OrdersResponse GetOrders(string prmFarm, OrdersRequest prmOrderRequest)
        {
            OrdersResponse ResponseOrders = new OrdersResponse();
            ResponseOrders.Response = new MsgResponse();

            try
            {
                ObtenerDatos(prmOrderRequest);

                string strSQL = "";
                DataSet dsOrdenes;
                List<Orders> LstOrders = new List<Orders>();
                List<OrderDetails> LstDetalles = new List<OrderDetails>();
                List<OrderBunchDetails> LstRamos = new List<OrderBunchDetails>();
                List<OrderMaterialDetails> LstMateriales = new List<OrderMaterialDetails>();
                List<OrderFlowerDetails> LstTallos = new List<OrderFlowerDetails>();

                strSQL = "EXEC ConsultaOrdenesAPI '" + prmFarm + "', '" + fechaInicial + "' , '" + fechaFinal + "', " + codigoOrden;
                dsOrdenes = SQLConection.ExecuteProcedureToDataSet(strSQL);

                if (dsOrdenes != null && dsOrdenes.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dataRowOrdenes in dsOrdenes.Tables[0].Rows)
                    {
                        Orders Ordenes;
                        OrderDetails OrdenesDetalles;
                        OrderBunchDetails Ramos;
                        OrderFlowerDetails Tallos;
                        OrderMaterialDetails Materiales;

                        Materiales = LstMateriales.Find(x => x.ID == Convert.ToInt32(dataRowOrdenes["IDRecetaMaterial"]));
                        Tallos = LstTallos.Find(x => x.ID == Convert.ToInt32(dataRowOrdenes["IDRecetaTallos"]));
                        Ramos = LstRamos.Find(x => x.ID == Convert.ToInt32(dataRowOrdenes["IDRecetaRamo"]));
                        OrdenesDetalles = LstDetalles.Find(x => x.ID == Convert.ToInt32(dataRowOrdenes["IDDetalles"]));
                        Ordenes = LstOrders.Find(x => x.ID == Convert.ToInt32(dataRowOrdenes["ID"]));

                        if (Materiales == null)
                        {
                            Materiales = new OrderMaterialDetails();
                            Materiales.ID = Convert.ToInt32(dataRowOrdenes["IDRecetaMaterial"]);
                            Materiales.Type = Convert.ToString(dataRowOrdenes["MaterialType"]);
                            Materiales.Material = Convert.ToString(dataRowOrdenes["Material"]);
                            Materiales.Qty = Convert.ToInt32(dataRowOrdenes["QtyMaterial"]);
                        }

                        if (Tallos == null)
                        {
                            Tallos = new OrderFlowerDetails();
                            Tallos.ID = Convert.ToInt32(dataRowOrdenes["IDRecetaTallos"]);
                            Tallos.Type = Convert.ToString(dataRowOrdenes["FlowerType"]);
                            Tallos.Category = Convert.ToString(dataRowOrdenes["CategoriasFlor"]);
                            Tallos.Color = Convert.ToString(dataRowOrdenes["Colores"]);
                            Tallos.Flower = Convert.ToString(dataRowOrdenes["Tallo"]);
                            Tallos.Qty = Convert.ToInt32(dataRowOrdenes["QtyTallos"]);
                            Tallos.Grade = Convert.ToString(dataRowOrdenes["GradosFlor"]);
                            Tallos.Quality = Convert.ToString(dataRowOrdenes["Quality"]);
                            Tallos.Treatment = Convert.ToString(dataRowOrdenes["TratamientosFlor"]);
                            Tallos.BloomCount = Convert.ToString(dataRowOrdenes["BloomCount"]);
                            Tallos.TreatmentTechnique = Convert.ToString(dataRowOrdenes["TecnicaTratamiento"]);
                            Tallos.TinctureTones = Convert.ToString(dataRowOrdenes["TonosTintura"]);
                            Tallos.TinctureBase = Convert.ToString(dataRowOrdenes["FloresBaseTintura"]);
                            Tallos.GlitterType = Convert.ToString(dataRowOrdenes["GlitterType"]);
                        }

                        if (Ramos == null)
                        {
                            Ramos = new OrderBunchDetails();
                            Ramos.ID = Convert.ToInt32(dataRowOrdenes["IDRecetaRamo"]);
                            Ramos.Bunch = Convert.ToString(dataRowOrdenes["Ramo"]);
                            Ramos.Qty = Convert.ToInt32(dataRowOrdenes["QtyRamos"]);
                            Ramos.Flowers = new List<OrderFlowerDetails>();
                            Ramos.Materials = new List<OrderMaterialDetails>();

                            if (Tallos != null)
                            {
                                Ramos.Flowers.Add(Tallos);
                                LstTallos.Add(Tallos);
                            }

                            if (Materiales != null)
                            {
                                Ramos.Materials.Add(Materiales);
                                LstMateriales.Add(Materiales);
                            }
                        }
                        else
                        {
                            if (!Ramos.Flowers.Contains(Tallos))
                                Ramos.Flowers.Add(Tallos);
                            if (!Ramos.Materials.Contains(Materiales))
                                Ramos.Materials.Add(Materiales);

                            LstTallos.Add(Tallos);
                            LstMateriales.Add(Materiales);
                        }

                        if (OrdenesDetalles == null)
                        {
                            OrdenesDetalles = new OrderDetails();
                            OrdenesDetalles.ID = Convert.ToInt32(dataRowOrdenes["IDDetalles"]);
                            OrdenesDetalles.CodeBoxProduct = Convert.ToString(dataRowOrdenes["CodProdComercial"]);
                            OrdenesDetalles.BoxProduct = Convert.ToString(dataRowOrdenes["ProdComercial"]);
                            OrdenesDetalles.Pack = Convert.ToInt32(dataRowOrdenes["Pack"]);
                            OrdenesDetalles.Qty = Convert.ToInt32(dataRowOrdenes["Qty"]);
                            OrdenesDetalles.Stems = Convert.ToInt32(dataRowOrdenes["Stems"]);
                            OrdenesDetalles.Box = Convert.ToString(dataRowOrdenes["IDBox_Nombre"]);
                            OrdenesDetalles.UnitCost = Convert.ToDecimal(dataRowOrdenes["UnitCost"]);
                            OrdenesDetalles.TotalCost = Convert.ToDecimal(dataRowOrdenes["TotalCost"]);
                            OrdenesDetalles.PullDateWithFormat = Convert.ToString(dataRowOrdenes["PullDateWithFormat"]);
                            OrdenesDetalles.UPC = Convert.ToString(dataRowOrdenes["UPC"]);
                            OrdenesDetalles.UPCRetailPrice = Convert.ToDecimal(dataRowOrdenes["UPCRetailPrice"]);
                            OrdenesDetalles.Bunches = new List<OrderBunchDetails>();

                            if (Ramos != null)
                            {
                                OrdenesDetalles.Bunches.Add(Ramos);
                                LstRamos.Add(Ramos);
                            }
                        }
                        else
                        {
                            if (!OrdenesDetalles.Bunches.Contains(Ramos))
                                OrdenesDetalles.Bunches.Add(Ramos);
                            LstRamos.Add(Ramos);
                        }

                        if (Ordenes == null)
                        {
                            Ordenes = new Orders();
                            Ordenes.ID = Convert.ToInt32(dataRowOrdenes["ID"]);
                            Ordenes.PO = Convert.ToString(dataRowOrdenes["PO"]);
                            Ordenes.OrderCode = Convert.ToInt32(dataRowOrdenes["Codigo"]);
                            Ordenes.FarmShipDate = Convert.ToDateTime(dataRowOrdenes["FarmShipDate"]).ToString("MM-dd-yyyy");
                            Ordenes.MiamiShipDate = Convert.ToDateTime(dataRowOrdenes["MiamiShipDate"]).ToString("MM-dd-yyyy");
                            Ordenes.DeliveryDate = Convert.ToDateTime(dataRowOrdenes["DeliveryDate"]).ToString("MM-dd-yyyy");
                            Ordenes.Status = Convert.ToString(dataRowOrdenes["EstadoFinca"]);
                            Ordenes.PullDate = Convert.ToDateTime(dataRowOrdenes["PullDate"]).ToString("MM-dd-yyyy");
                            Ordenes.CustomerCode = Convert.ToInt32(dataRowOrdenes["IDClientes_Codigo"]);
                            Ordenes.Customer = Convert.ToString(dataRowOrdenes["IDClientes_Nombre"]);
                            Ordenes.Farm = Convert.ToString(dataRowOrdenes["Farm"]);
                            Ordenes.Details = new List<OrderDetails>();

                            if (OrdenesDetalles != null)
                            {
                                Ordenes.Details.Add(OrdenesDetalles);
                                LstDetalles.Add(OrdenesDetalles);
                            }

                            LstOrders.Add(Ordenes);
                        }
                        else
                        {
                            if (!Ordenes.Details.Contains(OrdenesDetalles))
                                Ordenes.Details.Add(OrdenesDetalles);
                            LstDetalles.Add(OrdenesDetalles);
                        }
                    }

                    ResponseOrders.Orders = LstOrders;
                    ResponseOrders.Response.StatusCode = "200";
                    ResponseOrders.Response.Message = "Ordenes de Produccion Listadas Correctamente";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO OrdersBusiness:GetOrders", "Ordenes de Produccion Listadas Correctamente - " + strSQL);
                }
                else
                {
                    ResponseOrders.Response.StatusCode = "200";
                    ResponseOrders.Response.Message = "No Existen Ordenes de Produccion";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO OrdersBusiness:GetOrders", "No Existen Ordenes de Produccion - " + strSQL);
                }
            }
            catch (Exception ex)
            {
                Exception lex;

                lex = ex.InnerException != null ? ex.InnerException : ex;
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN CAPA DE NEGOCIO OrdersBusiness:GetOrders", lex.Message);
                throw new Exception(lex.Message, lex);
            }

            return ResponseOrders;
        }

        public MsgResponse ConfirmOrders(string prmFarm, OrdersConfirmRequest prmOrderRequest)
        {
            MsgResponse msgResponse = new MsgResponse();
            string strSQL = "";
            string strError = "";
            string strConfirmadas = "";
            int tmpRsta = 0;
            try
            {
                for (int i = 0; i < prmOrderRequest.OrderCode.Length; i++)
                {
                    int orderElement = prmOrderRequest.OrderCode[i];
                    strSQL = "EXEC ConfirmaOrdenesAPI '" + prmFarm + "', " + orderElement;
                    tmpRsta = Convert.ToInt32(SQLConection.ExecuteScalar(strSQL));

                    if (tmpRsta == 1)
                    {
                        strError = strError + orderElement + " - ";
                    }
                    else
                    {
                        strConfirmadas = strConfirmadas + orderElement + " - ";
                    }
                }

                msgResponse.StatusCode = "200";
                if (strError.Length > 0)
                {
                    msgResponse.Message = "Ordenes con Errores " + strError;
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO OrdersBusiness:ConfirmOrders", "Ordenes de Produccion Errores - " + strError);
                }
                else
                {
                    msgResponse.Message = "Ordenes confirmadas " + strConfirmadas;
                }

                if (strConfirmadas.Length > 0)
                {
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "CAPA DE NEGOCIO OrdersBusiness:ConfirmOrders", "Ordenes de Produccion Confirmadas - " + strConfirmadas);
                }
            }
            catch (Exception ex)
            {
                Exception lex;

                lex = ex.InnerException != null ? ex.InnerException : ex;
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN CAPA DE NEGOCIO OrdersBusiness:ConfirmOrders", lex.Message);

                msgResponse.StatusCode = "500";
                msgResponse.Message = lex.Message;

                throw new Exception(lex.Message, lex);
            }

            return msgResponse;
        }

        private void ObtenerDatos(OrdersRequest prmOrderRequest)
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
                if (prmOrderRequest.OrderCode != 0)
                {
                    codigoOrden = prmOrderRequest.OrderCode;
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