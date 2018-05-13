using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Annotator.API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("LetThemIn")]
    public class AnnotationsController : Controller
    {
        [HttpGet("/api")]
        public string Root()
        {
            string referer = getReferer();
            string ipString = HttpContext.Connection.RemoteIpAddress.ToString();

            return "hello world";
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            string referer = getReferer();

            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public async Task<string> Get(string id)
        {
            string referer = getReferer();

            return "value";
        }

        [HttpPost]
        public async void Post([FromBody]string value)
        {
            string referer = getReferer();
        }

        [HttpPut("{id}")]
        public async void Put(string id, [FromBody]string value)
        {
            string referer = getReferer();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            string referer = getReferer();
        }


        private string getReferer()
        {
            return Request.Headers["Referer"].ToString();
        }
    }
}
