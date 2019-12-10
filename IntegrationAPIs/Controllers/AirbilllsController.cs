using IntegrationAPIs.Bussines;
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
    [RoutePrefix("api/Airbills")]
    public class AirbilllsController : ApiController
    {
        [HttpPost]
        [Route("Upload")]
        public MsgResponse CargarAirbills(AirbillRequest AirbillRequest)
        {
            string tmpFarm = "";

            MsgResponse Message = new MsgResponse();

            try
            {
                tmpFarm = HttpContext.Current.User.Identity.Name.ToString();

                if (tmpFarm != "")
                {
                    Common.CreateTrace.WriteLogJson(JsonConvert.SerializeObject(AirbillRequest), tmpFarm + "UploadAirbill",0);

                    if (ModelState.IsValid)
                    {
                        AirbillBusiness AirbillBnss = new AirbillBusiness();
                        Message = AirbillBnss.CargarAirbills(tmpFarm, AirbillRequest);
                    }
                    else
                    {
                        Message.StatusCode = "500";
                        Message.Message = "Error en Request Structure Not Valid";
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Airbills/Upload " + tmpFarm, "Error en Request: " + ModelState);
                    }
                }
                else
                {
                    Message.StatusCode = "401";
                    Message.Message = "Error de Autenticacion";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Airbills/Upload " + tmpFarm, "Error de Autenticacion");
                }
            }
            catch (Exception ex)
            {
                Message.Message = "Error:" + ex.Message;
                Message.StatusCode = "500";
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Airbills/Upload " + tmpFarm, ex.Message);
            }

            Common.CreateTrace.WriteLogJson(JsonConvert.SerializeObject(Message), tmpFarm + "UploadAirbill", 1);
            return Message;
        }

        [HttpPost]
        [Route("Status")]
        public MsgResponse ActualizaEstadoAirbill(AirbillStatusRequest AirbillStatusRequest)
        {
            string tmpFarm = "";

            MsgResponse Message = new MsgResponse();

            try
            {
                tmpFarm = HttpContext.Current.User.Identity.Name.ToString();

                if (tmpFarm != "")
                {
                    Common.CreateTrace.WriteLogJson(JsonConvert.SerializeObject(AirbillStatusRequest), tmpFarm + "UpdateStatusAirbill",0);

                    if (ModelState.IsValid)
                    {
                        AirbillBusiness AirbillBnss = new AirbillBusiness();
                        Message = AirbillBnss.ActualizaEstadoAirbill(tmpFarm, AirbillStatusRequest);
                    }
                    else
                    {
                        Message.StatusCode = "500";
                        Message.Message = "Error en Request Format Structure Not Valid";
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Airbills/Status " + tmpFarm, "Error en Request: " + ModelState);
                    }
                }
                else
                {
                    Message.StatusCode = "401";
                    Message.Message = "Error de Autenticacion";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Airbills/Status " + tmpFarm, "Error de Autenticacion");
                }
            }
            catch (Exception ex)
            {
                Message.Message = "Error:" + ex.Message;
                Message.StatusCode = "500";
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Airbills/Status " + tmpFarm, ex.Message);
            }

            Common.CreateTrace.WriteLogJson(JsonConvert.SerializeObject(Message), tmpFarm + "UpdateStatusAirbill", 1);
            return Message;

        }
    }
}