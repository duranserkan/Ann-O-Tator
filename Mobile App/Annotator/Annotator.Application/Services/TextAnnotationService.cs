using Annotator.Application.Configuration;
using Annotator.Domain.Models.Service;
using Annotator.Domain.Services;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Annotator.Common.ExtensionMethods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Annotator.Application.Services
{
    public class TextAnnotationService : ITextAnnotationService
    {
        private AnnotationServerConfig _annotationServerConfig;
        private AnnotationApiConfig _annotationApiConfig;

        public TextAnnotationService(AnnotationServerConfig annotationServerConfig, AnnotationApiConfig annotationApiConfig)
        {
            _annotationServerConfig = annotationServerConfig;
            _annotationApiConfig = annotationApiConfig;
        }

        public async Task<HttpRequestResult<List<TextAnnotationDTO>>> GetAnnotationsAsync(string referer)
        {
            //200
            var searchUrl = getSearchUrl(referer);
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
                    string jsonLd = await response.Content.ReadAsStringAsync();

                    JObject jsonDocument = JObject.Parse(jsonLd);
                    var annotationTokens = jsonDocument["first"]["items"].Children();
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
            //200
            var request = getAnnotationUrl(id)
                .WithHeader("Accept", "application/ld+json; profile=\"http://www.w3.org/ns/anno.jsonld\"");
            var repsonse = await request.GetAsync();
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
                    string jsonLd = await response.Content.ReadAsStringAsync();
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
            //201

            var request = getAnnotationUrl(annotation.id)
                .WithHeader("Accept", "application/ld+json; profile=\"http://www.w3.org/ns/anno.jsonld\"");
            request.WithHeader("Content-Type", "application/ld+json; profile=\"http://www.w3.org/ns/anno.jsonld\"");
            request.WithHeader("If-Match", "797c2ee5253966de8882f496c25dd823");
            var requestResult = new HttpRequestResult<string>();

            try
            {
                var response = await request.PostStringAsync(annotation.ToJsonLd());

                requestResult.ErrorMessage = "";
                requestResult.HasError = false;
                requestResult.HttpStatusCode = response.StatusCode;

                if (requestResult.HttpStatusCode != HttpStatusCode.Created)
                {
                    requestResult.Data = null;
                }
                else
                {
                    //id is empty
                    requestResult.Data = getEndpointUrl(annotation.id);
                }
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

            var request = getAnnotationUrl(annotation.id)
                .WithHeader("Accept", "application/ld+json; profile=\"http://www.w3.org/ns/anno.jsonld\"");
            request.WithHeader("Content-Type", "application/ld+json; profile=\"http://www.w3.org/ns/anno.jsonld\"");
            request.WithHeader("If-Match", "797c2ee5253966de8882f496c25dd823");
            var requestResult = new HttpRequestResult<string>();

            try
            {
                var response = await request.PutStringAsync(annotation.ToJsonLd());

                requestResult.ErrorMessage = "";
                requestResult.HasError = false;
                requestResult.HttpStatusCode = response.StatusCode;

                if (requestResult.HttpStatusCode != HttpStatusCode.OK)
                {
                    requestResult.Data = null;
                }
                else
                {
                    requestResult.Data = getEndpointUrl(annotation.id);
                }
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
            var request = getAnnotationUrl(id)
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

        private Url getEndpointUrl(string id)
        {
            var request = _annotationApiConfig.url
                .AppendPathSegment("/api/")
                .AppendPathSegment("Annotations")
                .AppendPathSegment(id);

            return request;
        }

        private Url getAnnotationUrl(string id)
        {
            var request = _annotationServerConfig.ServerUrl
                .AppendPathSegment("/w3c/")
                .AppendPathSegment(_annotationServerConfig.ContainerId)
                .AppendPathSegment(id);

            return request;
        }

        private Url getSearchUrl(string referer)
        {
            var request = _annotationServerConfig.ServerUrl
                .AppendPathSegment("/w3c/")
                .AppendPathSegment("services")
                .AppendPathSegment("search")
                .AppendPathSegment("target")
                .SetQueryParams(new { fields = "id", value = referer, strict = "false" });

            return request;
        }
    }
}
