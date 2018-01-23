using System;
using System.Collections.Generic;
using System.Data;

namespace Smart.Data
{
    /// <summary>
    /// 分页查询数据对象
    /// </summary>
    /// <typeparam name="T">实体对象类型</typeparam>
    [Serializable]
    public class Page<T> : Page where T : class
    {
        /// <summary>
        /// 结果列表
        /// </summary>
        public new List<T> Items { get; set; }
    }
    /// <summary>
    /// 分页数据对象
    /// </summary>
    [Serializable]
    public class Page
    {
        /// <summary>
        /// 获取或设置当前页码，从1开始
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 获取或设置分页大小，每页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 获取总页数
        /// </summary>
        public int TotalPages
        {
            get
            {
                return PageSize > 0 ? TotalItems / PageSize + (TotalItems % PageSize > 0 ? 1 : 0) : 1;
            }
        }

        /// <summary>
        /// 获取或设置总记录数
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        public DataTable Items { get; set; }
    }
}
