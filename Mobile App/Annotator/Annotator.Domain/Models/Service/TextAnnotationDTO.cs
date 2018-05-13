using System;
using System.Collections.Generic;
using System.Text;

namespace Annotator.Domain.Models.Service
{
    public class TextAnnotationDTO : AnnotationDTOBase
    {
        public string text { get; set; }

        public string quote { get; set; }

        public Range[] ranges { get; set; }

        public class Range
        {
            public string start { get; set; }
            public string end { get; set; }
            public int startOffset { get; set; }
            public int endOffset { get; set; }
        }
    }




}
