using Newtonsoft.Json;
using Smart.Core.Extensions;

namespace Smart.Web.Mvc.UI.JqGrid
{
    public class EditOptions
    {
        public string Value { get; set; }

        public string DataUrl { get; set; }

        public string BuildSelect { get; set; }

        public string DataInit { get; set; }

        public string DataEvents { get; set; }

        public string DefaultValue { get; set; }

        public bool? NullIfEmpty { get; set; }

        public string OtherOptions { get; set; }

        public override string ToString()
        {
            var settings = new JsonSerializerSettings();
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            settings.NullValueHandling = NullValueHandling.Ignore;
            return this.ToJson(settings: settings);
        }
    }
}
