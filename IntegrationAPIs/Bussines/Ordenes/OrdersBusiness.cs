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

        public OrdersResponse GetOrders(string prmFarm)
        {
            OrdersResponse ResponseOrders = new OrdersResponse();

            try
            {
                string strSQL = "";
                DataSet dsOrdenes;
                List<Orders> LstOrders = new List<Orders>();
                List<OrderDetails> LstOrdersDetails = new List<OrderDetails>();

                strSQL = "EXEC ConsultaOrdenesAPI " + prmFarm;
                dsOrdenes = SQLConection.ExecuteProcedureToDataSet(strSQL);

                if (dsOrdenes != null && dsOrdenes.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dataRowOrdenes in dsOrdenes.Tables[0].Rows)
                    {
                        Orders Ordenes;
                        OrderDetails OrdenesDetalles;

                        OrdenesDetalles = LstOrdersDetails.Find(x => x.ID == Convert.ToInt32(dataRowOrdenes["IDDetalles"]));
                        Ordenes = LstOrders.Find(x => x.ID == Convert.ToInt32(dataRowOrdenes["ID"]));
                       
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
                            }
                            LstOrders.Add(Ordenes);
                        }

                    }
                }
                else
                {
                    ResponseOrders.Response.StatusCode = "200";
                    ResponseOrders.Response.Message = "No Existen Ordenes de Produccion";
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
    }
}