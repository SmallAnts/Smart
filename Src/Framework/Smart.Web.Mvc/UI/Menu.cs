using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Smart.Web.Mvc.UI
{
    public class Menu
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("active")]
        public bool Active { get; set; }
        [JsonProperty("attrs")]
        public object HtmlAttributes { get; set; }
        private List<Menu> _menus;
        [JsonProperty("menus")]
        public List<Menu> Menus
        {
            get { return _menus ?? (_menus = new List<Menu>()); }
            set { _menus = value; }
        }
    }
}
