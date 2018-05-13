using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Annotator.Domain.Models.Service;

namespace Annotator.Domain.Services
{
    public interface ITextAnnotationService
    {
        Task<List<TextAnnotationDTO>> GetAsync(string referer);
        Task<TextAnnotationDTO> GetAsync(string id, string referer);
        Task<string> CreateAsync(TextAnnotationDTO annotation, string referer);
        Task<string> UpdateAsync(TextAnnotationDTO annotation, string id, string referer);
        Task DeleteAsync(string id, string referer);
    }
}
