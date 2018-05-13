using Annotator.Common;

namespace Annotator.API.Configuration
{
    public class AnnotationApiVersion : VerifiableBase
    {
        public string name { get; set; }
        public string version { get; set; }

        public override void Verify()
        {
            IsNullOrEmpty(nameof(name));
            IsNullOrEmpty(nameof(version));
        }
    }
}
