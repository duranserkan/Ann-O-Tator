using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Annotator.Application.Configuration;
using Annotator.Domain.Models.Service;
using Annotator.Domain.Services;

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

        public Task<List<TextAnnotationDTO>> GetAsync(string referer)
        {
            throw new NotImplementedException();
        }

        public Task<TextAnnotationDTO> GetAsync(string id, string referer)
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateAsync(TextAnnotationDTO annotation, string referer)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateAsync(TextAnnotationDTO annotation, string id, string referer)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id, string referer)
        {
            throw new NotImplementedException();
        }

        private string getApiEndpoint(string id = "")
        {
            var builder = new UriBuilder(_annotationApiConfig.url);
            builder.Path = $"/api/annotations/{id}";

            return builder.ToString();
        }
    }
}
