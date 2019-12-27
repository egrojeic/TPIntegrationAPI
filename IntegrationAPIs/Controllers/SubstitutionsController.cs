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
    [RoutePrefix("api/Substitutions")]
    public class SubstitutionsController : ApiController
    {
        [HttpGet]
        [Route("Get")]
        public SubstitutionsResponse ConsultaSustitucionesParaFinca()
        {

            string tmpFarm = "";

            SubstitutionsResponse ResponseSustParaFinca = new SubstitutionsResponse();
            MsgResponse Message = new MsgResponse();
            ResponseSustParaFinca.Response = Message;

            try
            {
                tmpFarm = HttpContext.Current.User.Identity.Name.ToString();

                if (tmpFarm != "")
                {
                    if (ModelState.IsValid)
                    {
                        SubstitutionBusiness SubstitutionsBnss = new SubstitutionBusiness();
                        ResponseSustParaFinca = SubstitutionsBnss.ObtieneSustituciones(tmpFarm);
                    }
                    else
                    {
                        ResponseSustParaFinca.Response.StatusCode = "500";
                        ResponseSustParaFinca.Response.Message = "Error en Request Structure Not Valid";
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Substitutions/Get " + tmpFarm, "Error en Request: " + ModelState);
                    }
                }
                else
                {
                    ResponseSustParaFinca.Response.StatusCode = "401";
                    ResponseSustParaFinca.Response.Message = "Error de Autenticacion";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Substitutions/Get " + tmpFarm, "Error de Autenticacion");
                }
            }
            catch (Exception ex)
            {
                ResponseSustParaFinca.Response.Message = "Error:" + ex.Message;
                ResponseSustParaFinca.Response.StatusCode = "500";
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Substitutions/Get " + tmpFarm, ex.Message);
            }

            Common.CreateTrace.WriteLogJson(JsonConvert.SerializeObject(ResponseSustParaFinca), tmpFarm + "ResponseSubstitutionsGet", 1);
            return ResponseSustParaFinca;
        }

        [HttpGet]
        [Route("GetConfirmation")]
        public SubsConfirmResponse ObtieneConfirmacionSustitucion()
        {

            string tmpFarm = "";

            SubsConfirmResponse ResponseSustConfirma = new SubsConfirmResponse();
            MsgResponse Message = new MsgResponse();
            ResponseSustConfirma.Response = Message;

            try
            {
                tmpFarm = HttpContext.Current.User.Identity.Name.ToString();

                if (tmpFarm != "")
                {
                    if (ModelState.IsValid)
                    {
                        SubstitutionBusiness SubsConfirmBnss = new SubstitutionBusiness();
                        ResponseSustConfirma = SubsConfirmBnss.ObtieneConfirmacionSust(tmpFarm);
                    }
                    else
                    {
                        ResponseSustConfirma.Response.StatusCode = "500";
                        ResponseSustConfirma.Response.Message = "Error en Request Structure Not Valid";
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Substitutions/GetConfirmation " + tmpFarm, "Error en Request: " + ModelState);
                    }
                }
                else
                {
                    ResponseSustConfirma.Response.StatusCode = "401";
                    ResponseSustConfirma.Response.Message = "Error de Autenticacion";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Substitutions/GetConfirmation " + tmpFarm, "Error de Autenticacion");
                }
            }
            catch (Exception ex)
            {
                ResponseSustConfirma.Response.Message = "Error:" + ex.Message;
                ResponseSustConfirma.Response.StatusCode = "500";
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Substitutions/GetConfirmation " + tmpFarm, ex.Message);
            }

            Common.CreateTrace.WriteLogJson(JsonConvert.SerializeObject(ResponseSustConfirma), tmpFarm + "ResponseGetConfirmation", 1);
            return ResponseSustConfirma;
        }

        [HttpPost]
        [Route("Upload")]
        public MsgResponse EnviaSustitucionesFinca(SubstitutionsRequest sustitucionesFinca)
        {

            string tmpFarm = "";

            MsgResponse Message = new MsgResponse();

            try
            {
                tmpFarm = HttpContext.Current.User.Identity.Name.ToString();

                if (tmpFarm != "")
                {
                    Common.CreateTrace.WriteLogJson(JsonConvert.SerializeObject(sustitucionesFinca), tmpFarm + "UploadSubstitution", 0);

                    if (ModelState.IsValid)
                    {
                        SubstitutionBusiness SubsConfirmBnss = new SubstitutionBusiness();
                        Message = SubsConfirmBnss.CargaSustitucion(tmpFarm, sustitucionesFinca);
                    }
                    else
                    {
                        Message.StatusCode = "500";
                        Message.Message = "Error en Request Structure Not Valid";
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Substitutions/Upload " + tmpFarm, "Error en Request: " + ModelState);
                    }
                }
                else
                {
                    Message.StatusCode = "401";
                    Message.Message = "Error de Autenticacion";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Substitutions/Upload " + tmpFarm, "Error de Autenticacion");
                }
            }
            catch (Exception ex)
            {
                Message.Message = "Error:" + ex.Message;
                Message.StatusCode = "500";
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Substitutions/Upload " + tmpFarm, ex.Message);
            }

            Common.CreateTrace.WriteLogJson(JsonConvert.SerializeObject(Message), tmpFarm + "ResponseUploadSubstitutions", 1);
            return Message;
        }

        [HttpPost]
        [Route("UploadConfirmation")]
        public MsgResponse ConfirmaSustitucionParaFinca(SubsConfirmRequest confirmaSustitucion)
        {

            string tmpFarm = "";

            MsgResponse Message = new MsgResponse();

            try
            {
                tmpFarm = HttpContext.Current.User.Identity.Name.ToString();

                if (tmpFarm != "")
                {
                    Common.CreateTrace.WriteLogJson(JsonConvert.SerializeObject(confirmaSustitucion), tmpFarm + "UploadConfirmation", 0);

                    if (ModelState.IsValid)
                    {
                        SubstitutionBusiness SubsConfirmBnss = new SubstitutionBusiness();
                        Message = SubsConfirmBnss.CargaConfirmacionSust(tmpFarm, confirmaSustitucion);
                    }
                    else
                    {
                        Message.StatusCode = "500";
                        Message.Message = "Error en Request Structure Not Valid";
                        Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Substitutions/UploadConfirmation " + tmpFarm, "Error en Request: " + ModelState);
                    }
                }
                else
                {
                    Message.StatusCode = "401";
                    Message.Message = "Error de Autenticacion";
                    Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Substitutions/UploadConfirmation " + tmpFarm, "Error de Autenticacion");
                }
            }
            catch (Exception ex)
            {
                Message.Message = "Error:" + ex.Message;
                Message.StatusCode = "500";
                Common.CreateTrace.WriteLogToDB(Common.CreateTrace.LogLevel.Error, "ERROR EN API Substitutions/UploadConfirmation " + tmpFarm, ex.Message);
            }

            Common.CreateTrace.WriteLogJson(JsonConvert.SerializeObject(Message), tmpFarm + "ResponseUploadConfirmation", 1);
            return Message;
        }
    }
}
