using IntegrationAPIs.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace IntegrationAPIs.Bussines
{
    [Authorize]
    [RoutePrefix("api/CancelationsRequest")]
    public class CancelationRequestController : ApiController
    {
        [HttpGet]
        [Route("Get")]
        public CancelationResponse ConsultaSolicitudCancelaciones()
        {

            string tmpFarm = "";

            CancelationResponse ResponseSolicitudCancelacion = new CancelationResponse();
            MsgResponse Message = new MsgResponse();
            ResponseSolicitudCancelacion.Response = Message;

            try
            {
                tmpFarm = HttpContext.Current.User.Identity.Name.ToString();

                if (tmpFarm != "")
                {
                    if (ModelState.IsValid)
                    {
                        CancelationRBusiness CancelationBnss = new CancelationRBusiness();
                        ResponseSolicitudCancelacion = CancelationBnss.GetCancelations(tmpFarm);
                    }
                    else
                    {
                        ResponseSolicitudCancelacion.Response.StatusCode = "500";
                        ResponseSolicitudCancelacion.Response.Message = "Error en Request Structure Not Valid";
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API CancelationsRequest/Get " + tmpFarm, "Error en Request: " + ModelState);
                    }
                }
                else
                {
                    ResponseSolicitudCancelacion.Response.StatusCode = "401";
                    ResponseSolicitudCancelacion.Response.Message = "Error de Autenticacion";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API CancelationsRequest/Get " + tmpFarm, "Error de Autenticacion");
                }
            }
            catch (Exception ex)
            {
                ResponseSolicitudCancelacion.Response.Message = "Error:" + ex.Message;
                ResponseSolicitudCancelacion.Response.StatusCode = "500";
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API CancelationsRequest/Get " + tmpFarm, ex.Message);
            }

            Common.CreateTrace.WriteLogJson(JsonConvert.SerializeObject(ResponseSolicitudCancelacion), tmpFarm + "ResponseCancelationsRequest", 1);
            return ResponseSolicitudCancelacion;
        }

        [HttpPost]
        [Route("Confirmation")]
        public MsgResponse ConfirmaSolicitudCancelaciones(CancelationsConfirmRequest CancelationsConfirm)
        {

            string tmpFarm = "";

            MsgResponse Message = new MsgResponse();

            try
            {
                tmpFarm = HttpContext.Current.User.Identity.Name.ToString();

                if (tmpFarm != "")
                {
                    Common.CreateTrace.WriteLogJson(JsonConvert.SerializeObject(CancelationsConfirm), tmpFarm + "CancelationsConfirm", 0);

                    if (ModelState.IsValid)
                    {
                        CancelationRBusiness CancelationsRBnss = new CancelationRBusiness();
                        Message = CancelationsRBnss.ConfirmCancelations(tmpFarm, CancelationsConfirm);
                    }
                    else
                    {
                        Message.StatusCode = "500";
                        Message.Message = "Error en Request Structure Not Valid";
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API CancelationsRequest/Confirmation " + tmpFarm, "Error en Request: " + ModelState);
                    }
                }
                else
                {
                    Message.StatusCode = "401";
                    Message.Message = "Error de Autenticacion";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API CancelationsRequest/Confirmation " + tmpFarm, "Error de Autenticacion");
                }
            }
            catch (Exception ex)
            {
                Message.Message = "Error:" + ex.Message;
                Message.StatusCode = "500";
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API CancelationsRequest/Confirmation " + tmpFarm, ex.Message);
            }

            Common.CreateTrace.WriteLogJson(JsonConvert.SerializeObject(Message), tmpFarm + "ResponseCancelationsConfirm", 1);
            return Message;
        }
    }
}
