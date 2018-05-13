using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Annotator.Application.Configuration;
using Annotator.API.ActionResults;
using Annotator.API.Configuration;
using Annotator.Domain.Models.Service;
using Annotator.Domain.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<JsonResult> Get()
        {
            var annotations = await _textAnnotationService.GetAsync(getReferer());

            return Json(annotations);
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> Get(string id)
        {
            var annotation = await _textAnnotationService.GetAsync(id, getReferer());

            return Json(annotation);
        }

        [HttpPost]
        public async Task<SeeOtherResult> Post([FromBody]TextAnnotationDTO annotation)
        {
            var url = await _textAnnotationService.CreateAsync(annotation, getReferer());

            return new SeeOtherResult(url);
        }

        [HttpPut("{id}")]
        public async Task<SeeOtherResult> Put(string id, [FromBody]TextAnnotationDTO annotation)
        {
            var url= await _textAnnotationService.UpdateAsync(annotation, id, getReferer());

            return new SeeOtherResult(url);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<NoContentResult> Delete(string id)
        {
            await _textAnnotationService.DeleteAsync(id, getReferer());

            return NoContent();
        }

        private string getReferer()
        {
            return Request.Headers["Referer"].ToString();
        }

    }
}
