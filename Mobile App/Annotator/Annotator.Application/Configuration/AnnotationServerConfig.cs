using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annotator.Common;
using static System.String;

namespace Annotator.Application.Configuration
{
    public class AnnotationServerConfig : VerifiableBase
    {
        public string ServerUrl { get; set; }
        public string ContainerId { get; set; }

        public override void Verify()
        {
            IsNullOrEmpty(nameof(ServerUrl));
            IsNullOrEmpty(nameof(ContainerId));
        }
    }
}
