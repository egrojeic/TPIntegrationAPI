using CommonsWeb.DAL;
using IntegrationAPIs.Models;
using IntegrationAPIs.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace IntegrationAPIs.Controllers
{

    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        SqlServerHelper SQLConection = new SqlServerHelper();

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            string strSQL = "EXEC ValidaUsuarioAPI " + login.Username + ", " + login.Password;

            int tmpUsuarioValido = 0;
            tmpUsuarioValido = Convert.ToInt32(SQLConection.ExecuteScalar(strSQL));
            
            if (tmpUsuarioValido > 0)
            {
                var rolename = "Farms";
                var token = TokenGenerator.GenerateTokenJwt(login.Username, rolename);
                return Ok(token);
            }

            return Unauthorized();
        }
    }
}
