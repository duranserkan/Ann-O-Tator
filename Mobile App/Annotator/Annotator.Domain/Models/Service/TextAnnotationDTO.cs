using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json.Linq;

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

        public string ToJsonLd(string annotatorHomePage = "")
        {
            var annotation = new JObject();
            annotation.Add("@context", "http://www.w3.org/ns/anno.jsonld");
            annotation.Add("type", "Annotation");

            if (!string.IsNullOrWhiteSpace(id))
            {
                annotation.Add("id", id);
            }

            annotation.Add("created", created);
            annotation.Add("modified", updated);

            var generator = new JObject();
            generator.Add("id", annotatorHomePage);
            generator.Add("type", "software");
            generator.Add("name", "annotator: v1.0");
            generator.Add("homepage", annotatorHomePage);
            generator.Add("generator", generator);

            var body = new JObject();
            body.Add("bodyValue", text);
            annotation.Add("body", body);

            var target = new JObject();
            target.Add("source", uri.ToString());

            var range = ranges[0];
            var rangeSelector = new JObject();
            rangeSelector.Add("type", "RangeSelector");

            var xPathSelectorStart = new JObject();
            xPathSelectorStart.Add("type", "XPathSelector");
            xPathSelectorStart.Add("value", range.start);
            rangeSelector.Add("startSelector", xPathSelectorStart);

            var xPathSelectorEnd = new JObject();
            xPathSelectorEnd.Add("type", "XPathSelector");
            xPathSelectorEnd.Add("value", range.end);
            rangeSelector.Add("endSelector", xPathSelectorEnd);

            var textPositionSelector = new JObject();
            textPositionSelector.Add("type", "TextPositionSelector");
            textPositionSelector.Add("start", range.startOffset);
            textPositionSelector.Add("end", range.endOffset);

            var textQuoteSelector = new JObject();
            textQuoteSelector.Add("type", "TextQuoteSelector");
            textQuoteSelector.Add("exact", quote);
            textQuoteSelector.Add("prefix", "");
            textQuoteSelector.Add("suffix", "");

            textPositionSelector.Add("refinedBy", textQuoteSelector);
            rangeSelector.Add("refinedBy", textPositionSelector);
            target.Add("selector", rangeSelector);
            annotation.Add("target", target);

            return annotation.ToString();
        }

        public TextAnnotationDTO MapFromJsonLd(string jsonLd)
        {
            var annotation = JObject.Parse(jsonLd);
            id = annotation["id"].ToString();
            created = DateTime.Parse(annotation["created"].ToString());
            updated = DateTime.Parse(annotation["modified"].ToString());
            annotator_schema_version = annotation["generator"]["name"].ToString().Replace("annotator: ", "");
            text = annotation["bodyValue"].ToString();
            uri = new Uri(annotation["source"].ToString());

            var rangeSelector = annotation["selector"];
            var startSelector = rangeSelector["startSelector"];
            var endSelector = rangeSelector["endSelector"];
            var textPositionSelector = rangeSelector["refinedBy"];
            var textQuoteSelector = textPositionSelector["refinedBy"];

            var range = new Range();
            range.start = startSelector["value"].ToString();
            range.end = endSelector["value"].ToString();
            range.startOffset = Convert.ToInt32(textPositionSelector["start"].ToString());
            range.endOffset = Convert.ToInt32(textPositionSelector["end"].ToString());
            ranges = new[] { range };

            quote = textQuoteSelector["exact"].ToString();

            return this;
        }
    }
}
