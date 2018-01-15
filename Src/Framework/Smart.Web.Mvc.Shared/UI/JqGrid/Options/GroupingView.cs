using System.ComponentModel;
using System.Text;
using Smart.Core.Extensions;
using System;
using System.Text.RegularExpressions;

namespace Smart.Web.Mvc.UI.JqGrid
{
    /// <summary>
    /// groupingView选项配置 
    /// </summary>
    public class GroupingView
    {
        /// <summary>
        /// 分组列名（列名来自colModel）第一个值为第一层，第二个值为第二层，依次类推
        /// </summary>
        public string[] GroupField { get; set; }

        /// <summary>
        /// 分组层级排序，可用值：asc或者desc，默认值为asc
        /// </summary>
        public string[] GroupOrder { get; set; }

        /// <summary>
        /// 定义每个分组层级的页头内容。
        /// 默认为“{0}”，显示分组汇总值。还有另外一个值“{1}”，表示此分组的总统计。
        /// 可以设置任何内容，包括有效的html代码内容
        /// </summary>
        public string[] GroupText { get; set; }

        /// <summary>
        /// 分组时显示/隐藏哪些列。数组的值为true/false对应分组层级。默认值为true
        /// </summary>
        public bool[] GroupColumnShow { get; set; }

        /// <summary>
        /// 是否对当前分组层级启用汇总（页脚）行，默认值为false
        /// </summary>
        public bool[] GroupSummary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public bool HideFirstGroupCol { get; set; }

        /// <summary>
        /// 收缩分组时是否同时隐藏汇总（页脚）行
        /// </summary>
        [DefaultValue(false)]
        public bool ShowSummaryOnHide { get; set; }

        /// <summary>
        /// 定义初始化时隐藏或者显示分组的详细信息数据行。
        /// </summary>
        [DefaultValue(false)]
        public bool GroupCollapse { get; set; }

        /// <summary>
        /// 设置展开分组的图标
        /// </summary>
        public string Plusicon { get; set; }

        /// <summary>
        /// 设置收缩分组的图标
        /// </summary>
        public string Minusicon { get; set; }

        /// <summary>
        /// 里面的元素和分组数量数量一致。
        /// 数组的元素为方法（需要返回true/false）。
        /// 返回false，元素被添加到分组。
        /// 方法参数：previous value, current value, group index, group object
        /// </summary>
        public string[] IsInTheSameGroup { get; set; }

        /// <summary>
        /// 里面的元素和分组数量数量一致。
        /// 数组的元素为方法，需要返回一个值用于显示在分组中。
        /// 方法参数：current value, source value, colModel option, group index and group object.
        /// </summary>
        public string[] FormatDisplayField { get; set; }

        public override string ToString()
        {
            var json = new StringBuilder();
            json.Append("groupingView : {");
            if (GroupField.IsEmpty())
            {
                throw new ArgumentException("必须为分组设置至少一个列名！");
            }
            json.AppendFormat("groupField:{0},", GroupField.ToJson());
            if (!GroupOrder.IsEmpty()) json.AppendFormat("groupOrder:{0},", GroupOrder.ToJson());
            if (!GroupText.IsEmpty()) json.AppendFormat("groupText:{0},", GroupText.ToJson());
            if (!GroupColumnShow.IsEmpty()) json.AppendFormat("groupColumnShow:{0},", GroupColumnShow.ToJson());
            if (!GroupSummary.IsEmpty()) json.AppendFormat("groupSummary:{0},", GroupSummary.ToJson());
            if (HideFirstGroupCol) json.Append("groupSummary:true,");
            if (ShowSummaryOnHide) json.Append("showSummaryOnHide:true,");
            if (!Plusicon.IsEmpty()) json.AppendFormat("plusicon:'{0}',", Plusicon);
            if (!Minusicon.IsEmpty()) json.AppendFormat("minusicon:'{0}',", Minusicon);
            if (!IsInTheSameGroup.IsEmpty()) json.AppendFormat("isInTheSameGroup:{0},", Regex.Replace(IsInTheSameGroup.ToJson(), "'\"", ""));
            if (!FormatDisplayField.IsEmpty()) json.AppendFormat("formatDisplayField:{0},", Regex.Replace(FormatDisplayField.ToJson(), "'\"", ""));

            json.Append("}");
            return json.ToString();
        }
    }
}
