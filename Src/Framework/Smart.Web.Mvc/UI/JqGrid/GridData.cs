using Newtonsoft.Json;
using System.Collections.Generic;

namespace Smart.Web.Mvc.UI.JqGrid
{
    /// <summary>
    /// 
    /// </summary>
    public class GridData
    {
        /// <summary>
        /// 获取或设置总页数
        /// </summary>
        [JsonProperty("total")]
        public int Total { get; set; }

        /// <summary>
        /// 获取或设置当前页码
        /// </summary>
        [JsonProperty("page")]
        public int Page { get; set; }

        /// <summary>
        /// 获取或设置总记录数
        /// </summary>
        [JsonProperty("records")]
        public int Records { get; set; }

        /// <summary>
        /// 获取或设置绑定的数据集
        /// </summary>
        [JsonProperty("rows")]
        public IEnumerable<object> Rows { get; set; }
    }
}
