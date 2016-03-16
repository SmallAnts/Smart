using Newtonsoft.Json;
using Smart.Core.Extensions;

namespace Smart.Web.Mvc.UI.JqGrid
{
    public class EditRules
    {
        public bool? EditHidden { get; set; }

        public bool? Required { get; set; }

        public bool? Number { get; set; }

        public bool? Integer { get; set; }

        public int? MinValue { get; set; }

        public int? MaxValue { get; set; }

        public bool? Email { get; set; }

        public bool? Url { get; set; }

        public bool? Date { get; set; }

        public bool? Time { get; set; }

        public bool? Custom { get; set; }

        public string CustomFunc { get; set; }

        public override string ToString()
        {
            var settings = new JsonSerializerSettings();
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            settings.NullValueHandling = NullValueHandling.Ignore;
            return this.ToJson(settings: settings);
        }
    }
}
