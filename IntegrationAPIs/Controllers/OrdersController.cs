using IntegrationAPIs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IntegrationAPIs.Controllers
{
    [Authorize]
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {

        


        [HttpPost]
        [Route("Confirmation")]
        public Orders ConfirmarOrdenes(OrdersRequest OrdersConfirm)
        {
            Orders ResponseOrders = new Orders();



            return ResponseOrders;
        }
    }
}
