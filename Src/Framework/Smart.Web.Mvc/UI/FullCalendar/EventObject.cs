using Newtonsoft.Json;
using System;

namespace Smart.Web.Mvc.UI.FullCalendar
{
    public class EventObject
    {
        /// <summary>
        ///  可选，事件唯一标识，重复的事件具有相同的id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 必须，事件在日历上显示的title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 可选，true or false，是否是全天事件。
        /// </summary>
        [JsonProperty("allDay")]
        public bool AllDay { get; set; }

        /// <summary>
        /// 必须，事件的开始时间。 
        /// </summary>
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        /// <summary>
        /// 可选，结束时间。 
        /// </summary>
        [JsonProperty("end")]
        public DateTime End { get; set; }

        /// <summary>
        /// 可选，当指定后，事件被点击将打开对应url。 
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// 指定事件的样式。 
        /// </summary>
        [JsonProperty("className")]
        public string ClassName { get; set; }

        /// <summary>
        /// 事件是否可编辑，可编辑是指可以移动, 改变大小等。
        /// </summary>
        [JsonProperty("editable")]
        public bool Editable { get; set; }

        ///// <summary>
        /////   指向次event的eventsource对象。 
        ///// </summary>
        //public string source { get; set; }

        /// <summary>
        /// 背景和边框颜色。 
        /// </summary>
        [JsonProperty("color")]
        public string Color { get; set; }

        ///// <summary>
        ///// 背景颜色。 
        ///// </summary>
        //[JsonProperty("backgroundColor")]
        //public string BackgroundColor { get; set; }

        ///// <summary>
        ///// 边框颜色。
        ///// </summary>
        //[JsonProperty("borderColor")]
        //public string BorderColor { get; set; }

        ///// <summary>
        ///// 文本颜色。 
        ///// </summary>
        //[JsonProperty("textColor")]
        //public string TextColor { get; set; }

    }

    public class EventObjectColor
    {
        public string Red = "#f56954";
        public string Yellow = "#f39c12";
        public string Blue = "#0073b7";
        public string Aqua = "#00c0ef";
        public string Green = "#00a65a";
        public string Light_Blue = "#3c8dbc";
    }
}
