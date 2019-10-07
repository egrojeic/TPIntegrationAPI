using IntegrationAPIs.Bussines.Ordenes;
using IntegrationAPIs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace IntegrationAPIs.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        [HttpPost]
        [Route("Get")]
        public OrdersResponse ConsultaOrders()
        {
            string tmpFarm = "";

            OrdersResponse ResponseOrders = new OrdersResponse();
            MsgResponse Message = new MsgResponse();
            ResponseOrders.Response = Message;

            try
            {
                tmpFarm = HttpContext.Current.User.ToString();

                if (tmpFarm != "")
                {
                    OrdersBusiness OrderBnss = new OrdersBusiness();
                    ResponseOrders = OrderBnss.GetOrders(tmpFarm);
                }
                else
                {
                    ResponseOrders.Response.StatusCode = "401";
                    ResponseOrders.Response.Message = "Error de Autenticacion";
                }
            }
            catch (Exception ex)
            {
                ResponseOrders.Response.Message = "Error:" + ex.Message;
                ResponseOrders.Response.StatusCode = "500";
                Common.CreateTrace.WriteLog(Common.CreateTrace.LogLevel.Error, "ERROR EN API Orders/Get " + tmpFarm + " " + ex.Message);
            }


            return ResponseOrders;
        }
    }

    //[HttpPost]
    //[Route("Confirmation")]
    //public OrdersResponse ConfirmarOrdenes(OrdersRequest OrdersConfirm)
    //{

    //}
}
