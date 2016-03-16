using Newtonsoft.Json;

namespace Smart.Web.Mvc.UI
{
    public class Shortcut
    {
        public Shortcut() { }
        public Shortcut(string buttonClass, string iconClass)
        {
            this.ButtonClass = buttonClass;
            this.IconClass = iconClass;
        }

        [JsonProperty("iconClass")]
        public string IconClass { get; set; }

        [JsonProperty("buttonClass")]
        public string ButtonClass { get; set; }
    }
}
