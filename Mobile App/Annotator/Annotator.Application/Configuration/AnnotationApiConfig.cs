using Annotator.Common;

namespace Annotator.Application.Configuration
{
    public class AnnotationApiConfig : VerifiableBase
    {
        public string url { get; set; }

        public override void Verify()
        {
            IsURL(nameof(url));
        }
    }
}
