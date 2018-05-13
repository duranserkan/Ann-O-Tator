using Annotator.API.ActionResults;
using Annotator.API.Configuration;
using Annotator.Domain.Models.Service;
using Annotator.Domain.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Annotator.API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("LetThemIn")]
    public class AnnotationsController : Controller
    {
        private readonly ITextAnnotationService _textAnnotationService;

        public AnnotationsController(ITextAnnotationService textAnnotationService)
        {
            _textAnnotationService = textAnnotationService;
        }

        [HttpGet("/api")]
        public JsonResult Root([FromServices] AnnotationApiVersion annotationApiVersion)
        {
            return Json(annotationApiVersion);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _textAnnotationService.GetAnnotationsAsync(getReferer());

            if (result.HttpStatusCode == HttpStatusCode.OK)
            {
                return Json(result.Data);
            }

            if (result.HasError)
            {
                return new StatusCodeResult(500);
            }

            return new StatusCodeResult((int) result.HttpStatusCode);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _textAnnotationService.GetAsync(id);

            if (result.HttpStatusCode == HttpStatusCode.OK)
            {
                return Json(result.Data);
            }

            if (result.HasError)
            {
                return new StatusCodeResult(500);
            }

            return new StatusCodeResult((int) result.HttpStatusCode);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TextAnnotationDTO annotation)
        {
            var result = await _textAnnotationService.CreateAsync(annotation);

            if (result.HttpStatusCode == HttpStatusCode.Created)
            {
                return new SeeOtherResult(result.Data);
            }

            if (result.HasError)
            {
                return new StatusCodeResult(500);
            }

            return new StatusCodeResult((int) result.HttpStatusCode);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]TextAnnotationDTO annotation)
        {
            var result = await _textAnnotationService.UpdateAsync(annotation);
            if (result.HttpStatusCode == HttpStatusCode.OK)
            {
                return new SeeOtherResult(result.Data);
            }

            if (result.HasError)
            {
                return new StatusCodeResult(500);
            }

            return new StatusCodeResult((int) result.HttpStatusCode);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _textAnnotationService.DeleteAsync(id);

            if (result.HttpStatusCode == HttpStatusCode.NoContent)
            {
                return NoContent();
            }

            if (result.HasError)
            {
                return new StatusCodeResult(500);
            }

            return new StatusCodeResult((int) result.HttpStatusCode);
        }

        private string getReferer()
        {
            return Request.Headers["Referer"].ToString();
        }

    }
}
