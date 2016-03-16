using Newtonsoft.Json;
using Smart.Core.Extensions;

namespace Smart.Web.Mvc.UI.JqGrid
{
    public class EditFormOptions
    {
        public string ElmPrefix { get; set; }
        public string ElmSuffix { get; set; }
        public string Label { get; set; }
        public int? RowPos { get; set; }
        public int? ColPos { get; set; }

        public override string ToString()
        {
            var settings = new JsonSerializerSettings();
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            settings.NullValueHandling = NullValueHandling.Ignore;
            return this.ToJson(settings: settings);
        }
    }
}
