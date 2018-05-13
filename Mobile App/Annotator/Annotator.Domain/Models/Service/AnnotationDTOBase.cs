using System;
using System.Collections.Generic;
using System.Text;

namespace Annotator.Domain.Models.Service
{
    public class AnnotationDTOBase
    {
        public string id { get; set; }

        public string annotator_schema_version { get; set; }

        public DateTime created { get; set; }

        public DateTime updated { get; set; }

        public Uri uri { get; set; }

        public string user { get; set; } = "";

        public string consumer { get; set; } = "";

        public string[] tags { get; set; } = new string[0];

        public Permission permissions { get; set; }

        public class Permission
        {
            public string[] read { get; set; } = new string[0];
            public string[] admin { get; set; } = new string[0];
            public string[] update { get; set; } = new string[0];
            public string[] delete { get; set; } = new string[0];
        }
    }
}
