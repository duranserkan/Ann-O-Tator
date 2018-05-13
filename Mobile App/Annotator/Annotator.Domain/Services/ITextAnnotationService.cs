using Annotator.Domain.Models.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Annotator.Domain.Services
{
    public interface ITextAnnotationService
    {
        Task<HttpRequestResult<List<TextAnnotationDTO>>> GetAnnotationsAsync(string referer);
        Task<HttpRequestResult<TextAnnotationDTO>> GetAsync(string id);
        Task<HttpRequestResult<string>> CreateAsync(TextAnnotationDTO annotation);
        Task<HttpRequestResult<string>> UpdateAsync(TextAnnotationDTO annotation);
        Task<HttpRequestResult<string>> DeleteAsync(string id);
    }
}
