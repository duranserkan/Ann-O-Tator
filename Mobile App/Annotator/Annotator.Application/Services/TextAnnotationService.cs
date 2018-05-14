using Annotator.Application.Configuration;
using Annotator.Domain.Models.Service;
using Annotator.Domain.Services;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Annotator.Application.Services
{
    public class TextAnnotationService : ITextAnnotationService
    {
        private readonly AnnotationServerConfig _annotationServerConfig;
        private readonly AnnotationApiConfig _annotationApiConfig;

        public TextAnnotationService(AnnotationServerConfig annotationServerConfig, AnnotationApiConfig annotationApiConfig)
        {
            _annotationServerConfig = annotationServerConfig;
            _annotationApiConfig = annotationApiConfig;
        }

        public async Task<HttpRequestResult<List<TextAnnotationDTO>>> GetAnnotationsAsync(string referer)
        {
            var searchUrl = GetSearchUrl(referer);
            var requestResult = new HttpRequestResult<List<TextAnnotationDTO>>();

            try
            {
                var response = await searchUrl.GetAsync();
                requestResult.ErrorMessage = "";
                requestResult.HasError = false;
                requestResult.HttpStatusCode = response.StatusCode;
                if (requestResult.HttpStatusCode != HttpStatusCode.OK)
                {
                    requestResult.Data = null;
                }
                else
                {
                    var annotations = new List<TextAnnotationDTO>();
                    var jsonLd = await response.Content.ReadAsStringAsync();

                    var jsonDocument = JObject.Parse(jsonLd);
                    var annotationTokens = jsonDocument["first"]["items"]?.Children() ?? new JEnumerable<JToken>();
                    foreach (var annotationToken in annotationTokens)
                    {
                        annotations.Add(new TextAnnotationDTO().MapFromJsonLd(annotationToken.ToString()));
                    }

                    requestResult.Data = annotations;
                }
            }
            catch (Exception e)
            {
                requestResult.Data = null;
                requestResult.ErrorMessage = e.Message;
                requestResult.HasError = true;
            }

            return requestResult;

        }

        public async Task<HttpRequestResult<TextAnnotationDTO>> GetAsync(string id)
        {
            var request = GetAnnotationUrl(id)
                .WithHeader("Accept", "application/ld+json; profile=\"http://www.w3.org/ns/anno.jsonld\"");
            var requestResult = new HttpRequestResult<TextAnnotationDTO>();

            try
            {
                var response = await request.GetAsync();

                requestResult.ErrorMessage = "";
                requestResult.HasError = false;
                requestResult.HttpStatusCode = response.StatusCode;

                if (requestResult.HttpStatusCode != HttpStatusCode.OK)
                {
                    requestResult.Data = null;
                }
                else
                {
                    var jsonLd = await response.Content.ReadAsStringAsync();
                    requestResult.Data = new TextAnnotationDTO().MapFromJsonLd(jsonLd);
                }
            }
            catch (Exception e)
            {
                requestResult.Data = null;
                requestResult.ErrorMessage = e.Message;
                requestResult.HasError = true;
            }

            return requestResult;
        }

        public async Task<HttpRequestResult<string>> CreateAsync(TextAnnotationDTO annotation)
        {
            var request = GetAnnotationUrl(annotation.id)
                .WithHeader("Accept", "application/ld+json; profile=\"http://www.w3.org/ns/anno.jsonld\"");
            request.WithHeader("Content-Type", "application/ld+json; profile=\"http://www.w3.org/ns/anno.jsonld\"");

            var requestResult = new HttpRequestResult<string>();

            try
            {
                annotation.created = DateTime.Now;
                annotation.updated = annotation.created;
                var response = await request.PostStringAsync(annotation.ToJsonLd(_annotationApiConfig.url));
                var responseContent = await response.Content.ReadAsStringAsync();
                var annotationId = JObject.Parse(responseContent)["id"].ToString().Split('/').Last();

                requestResult.ErrorMessage = "";
                requestResult.HasError = false;
                requestResult.HttpStatusCode = response.StatusCode;
                requestResult.Data = requestResult.HttpStatusCode != HttpStatusCode.Created ? null : GetEndpointUrl(annotationId);
            }
            catch (Exception e)
            {
                requestResult.Data = "";
                requestResult.ErrorMessage = e.Message;
                requestResult.HasError = true;
            }

            return requestResult;
        }

        public async Task<HttpRequestResult<string>> UpdateAsync(TextAnnotationDTO annotation)
        {
            var request = GetAnnotationUrl(annotation.id)
                .WithHeader("Accept", "application/ld+json; profile=\"http://www.w3.org/ns/anno.jsonld\"");
            request.WithHeader("Content-Type", "application/ld+json; profile=\"http://www.w3.org/ns/anno.jsonld\"");
            request.WithHeader("If-Match", "797c2ee5253966de8882f496c25dd823");
            var requestResult = new HttpRequestResult<string>();

            try
            {
                var annotationTobeUpdated = await GetAsync(annotation.id);
                annotationTobeUpdated.Data.text = annotation.text;
                annotationTobeUpdated.Data.updated = DateTime.Now;
                annotationTobeUpdated.Data.id = GetAnnotationUrl(annotationTobeUpdated.Data.id);

                var response = await request.PutStringAsync(annotationTobeUpdated.Data.ToJsonLd());

                requestResult.ErrorMessage = "";
                requestResult.HasError = false;
                requestResult.HttpStatusCode = response.StatusCode;

                requestResult.Data = requestResult.HttpStatusCode != HttpStatusCode.OK ? null : GetEndpointUrl(annotation.id);
            }
            catch (Exception e)
            {
                requestResult.Data = "";
                requestResult.ErrorMessage = e.Message;
                requestResult.HasError = true;
            }

            return requestResult;
        }

        public async Task<HttpRequestResult<string>> DeleteAsync(string id)
        {
            var request = GetAnnotationUrl(id)
                .WithHeader("If-Match", "24d535a13f2c16e2701bf46b11407cea");
            var requestResult = new HttpRequestResult<string>();

            try
            {
                var response = await request.DeleteAsync();
                requestResult.Data = "";
                requestResult.ErrorMessage = "";
                requestResult.HasError = false;
                requestResult.HttpStatusCode = response.StatusCode;
            }
            catch (Exception e)
            {
                requestResult.Data = "";
                requestResult.ErrorMessage = e.Message;
                requestResult.HasError = true;
            }

            return requestResult;
        }

        private Url GetEndpointUrl(string id)
        {
            var request = _annotationApiConfig.url
                .AppendPathSegment("/api/")
                .AppendPathSegment("Annotations/")
                .AppendPathSegment(id);

            return request;
        }

        private Url GetAnnotationUrl(string id)
        {
            var request = _annotationServerConfig.ServerUrl
                .AppendPathSegment("/w3c/")
                .AppendPathSegment(_annotationServerConfig.ContainerId + "/");

            if (!string.IsNullOrWhiteSpace(id))
            {
                request.AppendPathSegment(id);
            }

            return request;
        }

        private Url GetSearchUrl(string referer)
        {
            var request = _annotationServerConfig.ServerUrl
                .AppendPathSegment("/w3c/")
                .AppendPathSegment("services")
                .AppendPathSegment("search")
                .AppendPathSegment("target")
                .SetQueryParams(new { fields = "source", value = referer, strict = "false" });

            return request;
        }
    }
}
