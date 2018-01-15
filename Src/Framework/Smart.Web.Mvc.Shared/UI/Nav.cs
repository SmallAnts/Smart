using Newtonsoft.Json;

namespace Smart.Web.Mvc.UI
{
    public class Nav
    {
        public Nav() { }
        public Nav(string text, string url)
        {
            this.Text = text; this.Url = url;
        }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
