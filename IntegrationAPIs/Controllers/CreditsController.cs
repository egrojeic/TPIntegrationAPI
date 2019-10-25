﻿using IntegrationAPIs.Bussines;
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
    [RoutePrefix("api/Credits")]
    public class CreditsController : ApiController
    {
        [HttpPost]
        [Route("Get")]
        public CreditNotesResponse ConsultaNotasCredito(CreditNotesRequest CreditNRequest)
        {
            string tmpFarm = "";

            CreditNotesResponse CreditNResponse = new CreditNotesResponse();
            MsgResponse Message = new MsgResponse();
            CreditNResponse.Response = Message;

            try
            {
                tmpFarm = HttpContext.Current.User.Identity.Name.ToString();

                if (tmpFarm != "")
                {
                    Common.CreateTrace.WriteLogJson(JsonConvert.SerializeObject(CreditNRequest), tmpFarm + "GetCredits");

                    if (ModelState.IsValid)
                    {
                        CreditNotesBusiness CreditsNBnss = new CreditNotesBusiness();
                        CreditNResponse = CreditsNBnss.GetNotasCredito(tmpFarm, CreditNRequest);
                    }
                    else
                    {
                        Message.StatusCode = "500";
                        Message.Message = "Error en Request Structure Not Valid";
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Credits/Get " + tmpFarm, "Error en Request: " + ModelState);
                    }
                }
                else
                {
                    Message.StatusCode = "401";
                    Message.Message = "Error de Autenticacion";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Credits/Get " + tmpFarm, "Error de Autenticacion");
                }
            }
            catch (Exception ex)
            {
                Message.Message = "Error:" + ex.Message;
                Message.StatusCode = "500";
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Credits/Get " + tmpFarm, ex.Message);
            }


            return CreditNResponse;
        }
    }
}
