using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNETCore22_Seguridad.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace AspNETCore22_Seguridad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("PermitirApiRequest")]

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class ValuesController : ControllerBase
    {
        private readonly IDataProtector _protector;
        private readonly HashService _hashService;

        public ValuesController(IDataProtectionProvider protectionProvider, HashService hashService)
        {
            _protector = protectionProvider.CreateProtector("valor_unico_y_quizas_secreto");
            _hashService = hashService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("hash")]
        public ActionResult GetHash()
        {
            string textoPlano = "Duvan Gonzalez";
            var hashResult1 = _hashService.Hash(textoPlano).Hash;
            var hashResult2 = _hashService.Hash(textoPlano).Hash;
            return Ok(new { textoPlano, hashResult1, hashResult2 });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [EnableCors("PermitirApiRequest")]
        public ActionResult<string> Get(int id)
        {
            string textoPlano = "Duvan Gonzalez";

            //Opcion 1
            /*string textoCifrado = _protector.Protect(textoPlano);
            string textoDesencriptado = _protector.Unprotect(textoCifrado);*/

            //Opcion 2 - Limitado por tiempo
            var protectorLimitadoPorTiempo = _protector.ToTimeLimitedDataProtector();
            string textoCifrado = protectorLimitadoPorTiempo.Protect(textoPlano, TimeSpan.FromSeconds(5));
            Thread.Sleep(6000);
            string textoDesencriptado = protectorLimitadoPorTiempo.Unprotect(textoCifrado);

            return Ok(new { textoPlano, textoCifrado, textoDesencriptado});
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
