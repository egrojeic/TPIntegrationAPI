using IntegrationAPIs.Bussines.Ordenes;
using IntegrationAPIs.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace IntegrationAPIs.Controllers
{
    [Authorize]
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        [HttpPost]
        [Route("Get")]
        public OrdersResponse ConsultaOrders(OrdersRequest OrderRequest)
        {

            string tmpFarm = "";

            OrdersResponse ResponseOrders = new OrdersResponse();
            MsgResponse Message = new MsgResponse();
            ResponseOrders.Response = Message;

            try
            {
                tmpFarm = HttpContext.Current.User.Identity.Name.ToString();

                if (tmpFarm != "")
                {
                    Common.CreateTrace.WriteLogJson(JsonConvert.SerializeObject(OrderRequest), tmpFarm + "OrderRequest");

                    if (ModelState.IsValid)
                    {
                        OrdersBusiness OrderBnss = new OrdersBusiness();
                        ResponseOrders = OrderBnss.GetOrders(tmpFarm, OrderRequest);
                    }
                    else
                    {
                        ResponseOrders.Response.StatusCode = "500";
                        ResponseOrders.Response.Message = "Error en Request Structure Not Valid";
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Orders/Get " + tmpFarm, "Error en Request: " + ModelState);
                    }
                }
                else
                {
                    ResponseOrders.Response.StatusCode = "401";
                    ResponseOrders.Response.Message = "Error de Autenticacion";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Orders/Get " + tmpFarm, "Error de Autenticacion");
                }
            }
            catch (Exception ex)
            {
                ResponseOrders.Response.Message = "Error:" + ex.Message;
                ResponseOrders.Response.StatusCode = "500";
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Orders/Get " + tmpFarm, ex.Message);
            }


            return ResponseOrders;
        }

        [HttpPost]
        [Route("Confirmation")]
        public MsgResponse ConfirmarOrdenes(OrdersConfirmRequest OrdersConfirm)
        {
            string tmpFarm = "";

            MsgResponse Message = new MsgResponse();

            try
            {
                tmpFarm = HttpContext.Current.User.Identity.Name.ToString();

                if (tmpFarm != "")
                {
                    Common.CreateTrace.WriteLogJson(JsonConvert.SerializeObject(OrdersConfirm), tmpFarm + "OrdersConfirm");

                    if (ModelState.IsValid)
                    {
                        OrdersBusiness OrderBnss = new OrdersBusiness(); 
                        Message = OrderBnss.ConfirmOrders(tmpFarm, OrdersConfirm);
                    }
                    else
                    {
                        Message.StatusCode = "500";
                        Message.Message = "Error en Request Structure Not Valid";
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Orders/Confirmation " + tmpFarm, "Error en Request");
                    }
                }
                else
                {
                    Message.StatusCode = "401";
                    Message.Message = "Error de Autenticacion";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Orders/Confirmation " + tmpFarm, "Error de Autenticacion");
                }
            }
            catch (Exception ex)
            {
                Message.Message = "Error:" + ex.Message;
                Message.StatusCode = "500";
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Orders/Confirmation " + tmpFarm, ex.Message);
            }


            return Message;

        }
    }
}
