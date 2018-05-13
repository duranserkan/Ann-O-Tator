using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Annotator.API.ActionResults
{
    public class SeeOtherResult : ActionResult
    {
        private readonly string _url;

        public SeeOtherResult(string url)
        {
            _url = url;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = 303;
            context.HttpContext.Response.Headers[HeaderNames.Location] = _url;

            return base.ExecuteResultAsync(context);
        }
    }
}