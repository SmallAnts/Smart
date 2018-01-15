extern alias sld;
using Newtonsoft.Json;
using Smart.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using sld::System.Linq.Dynamic;
using System.Reflection;
using System.Web.UI;

namespace Smart.Web.Mvc.UI
{
    /// <summary>
    /// zTree
    /// </summary>
    public class zTree
    {
        private zTreeNodeCollection _nodes;
        /// <summary>
        /// zTree 树节点 集合
        /// </summary>
        public zTreeNodeCollection Nodes { get { return _nodes ?? (_nodes = new zTreeNodeCollection(null)); } }

        /// <summary>
        /// 数据源
        /// </summary>
        public object DataSource { get; set; }

        /// <summary>
        /// Id字段名
        /// </summary>
        public string IdField { get; set; }

        /// <summary>
        /// 名称字段名
        /// </summary>
        public string NameField { get; set; }

        /// <summary>
        /// 上级Id字段名
        /// </summary>
        public string ParentIdField { get; set; }

        /// <summary>
        /// 根节点的Id值
        /// </summary>
        public string RootId { get; set; }

        /// <summary>
        /// zTree 节点创建完成时触发，可以在该事件设置节点的其它信息
        /// </summary>
        public event zTreeNodeDataBoundEventHandler NodeDataBound;
        private void OnNodeDataBound(zTreeNodeDataBoundEventArgs e)
        {
            if (this.NodeDataBound != null)
            {
                this.NodeDataBound(this, e);
            }
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        public void DataBind()
        {
            if (DataSource == null)
            {
                throw new ArgumentNullException("DataSource");
            }
            if (DataSource is DataTable)
            {
                CreateDataTableNodes(DataSource as DataTable, null);
            }
            else if (DataSource is IEnumerable)
            {
                CreatezTreeNodes(DataSource as IEnumerable, null);
            }
            else
            {
                throw new ArgumentException("不支持该数据源类型");
            }
        }

        private void CreateDataTableNodes(DataTable dataSource, zTreeNode parentNode)
        {
            if (dataSource.Rows.Count == 0) return;
            var hasChild = !this.ParentIdField.IsEmpty();
            DataRowCollection rows;
            var parentId = parentNode == null ? this.RootId : parentNode.Id;
            if (hasChild)
            {
                var dv = (dataSource as DataTable).DefaultView;
                dv.RowFilter = this.ParentIdField + "='" + parentId + "'";
                rows = dv.ToTable().Rows;
            }
            else
            {
                rows = dataSource.Rows;
            }
            var nodes = parentNode == null ? this.Nodes : parentNode.Nodes;
            var args = new zTreeNodeDataBoundEventArgs();
            foreach (DataRow row in rows)
            {
                var node = new zTreeNode(row[this.IdField].AsString(), row[this.NameField].AsString());
                node.ParentNode = parentNode;
                nodes.Add(node);
                args.Node = node;
                args.DataItem = row;
                OnNodeDataBound(args);
                CreateDataTableNodes(dataSource, node);
            }
        }

        private void CreatezTreeNodes(IEnumerable dataSource, zTreeNode parentNode)
        {
            var query = dataSource.AsQueryable();
            var dataType = query.ElementType;
            var idPI = GetPropertyInfo(dataType, this.IdField);
            var namePI = GetPropertyInfo(dataType, this.NameField);
            var hasChild = !this.ParentIdField.IsEmpty();
            var parentId = parentNode == null ? this.RootId : parentNode.Id;
            if (hasChild)
            {
                query = query.Where(this.ParentIdField + "==@0", parentId);
            }
            var nodes = parentNode == null ? this.Nodes : parentNode.Nodes;
            var args = new zTreeNodeDataBoundEventArgs();
            foreach (var row in query)
            {
                if (parentNode != null && !parentNode.IsParent) parentNode.IsParent = true;
                var node = new zTreeNode(idPI.GetValue(row, null).AsString(), namePI.GetValue(row, null).AsString());
                node.ParentNode = parentNode;
                node.IsParent = this.ParentIdField.IsEmpty();
                nodes.Add(node);
                args.Node = node;
                args.DataItem = row;
                OnNodeDataBound(args);
                if (hasChild) CreatezTreeNodes(dataSource, node);
            }
        }

        private PropertyInfo GetPropertyInfo(Type type, string name)
        {
            var propertyinfo = type.GetProperty(name);
            if (propertyinfo == null) throw new ArgumentNullException("未从数据源对象中找到" + name + "属性");
            return propertyinfo;
        }
    }

    #region zTree 树节点

    /// <summary>
    /// zTree 树节点集合
    /// </summary>
    public class zTreeNodeCollection : Collection<zTreeNode>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentNode"></param>
        public zTreeNodeCollection(zTreeNode parentNode)
        {
            this.ParentNode = parentNode;
        }
        /// <summary>
        /// 通过Node唯一标识索引
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public zTreeNode this[string Id]
        {
            get { return this.Items.FirstOrDefault(n => n.Id == Id); }
        }
        /// <summary>
        /// 父节点
        /// </summary>
        [JsonIgnore]
        public zTreeNode ParentNode { get; set; }
        /// <summary>
        /// 将对象添加到 树节点集合 的结尾处。
        /// </summary>
        /// <param name="node"></param>
        public new void Add(zTreeNode node)
        {
            node.ParentNode = this.ParentNode;
            if (node.ParentNode != null && !node.ParentNode.IsParent) node.ParentNode.IsParent = true;
            this.Items.Add(node);
        }
        /// <summary>
        /// 将对象添加到 树节点集合 的结尾处。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void Add(string id, string name)
        {
            var node = new zTreeNode(id, name);
            this.Add(node);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Items == null ? "[]" : this.Items.ToJson();
        }
    }
    /// <summary>
    /// zTree 树节点
    /// </summary>
    [JsonObject]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class zTreeNode
    {
        /// <summary>
        /// 
        /// </summary>
        public zTreeNode() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="name"></param>
        public zTreeNode(string tid, string name)
        {
            this.Id = tid;
            this.Name = name;
        }

        /// <summary>
        /// <para>节点的 checkBox / radio 的 勾选状态。[check.enable = true 并且 treeNode.nocheck = false 时有效]</para>
        /// <para>1、如果不使用 checked 属性设置勾选状态，请修改 data.key.checked</para>
        /// <para>2、建立 treeNode 数据时设置 treeNode.checked = true 可以让节点的输入框默认为勾选状态</para>
        /// <para>3、修改节点勾选状态，可以使用 treeObj.checkNode / checkAllNodes / updateNode 方法，具体使用哪种请根据自己的需求而定</para>
        /// <para>4、为了解决部分朋友生成 json 数据出现的兼容问题, 支持 "false","true" 字符串格式的数据</para>
        /// <para>默认值：false</para>
        /// </summary>
        [JsonProperty(PropertyName = "checked")]
        [DefaultValue(false)]
        public bool Checked { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        [JsonIgnore]
        public zTreeNode ParentNode { get; set; }

        private zTreeNodeCollection _nodes;
        /// <summary>
        /// <para>节点的子节点数据集合。</para>
        /// <para>1、如果不使用 Nodes 属性保存子节点数据，请修改 data.key.children</para>
        /// <para>2、异步加载时，对于设置了 isParent = true 的节点，在展开时将进行异步加载</para>
        /// <para>默认值：无</para>
        /// </summary>
        [NotifyParentProperty(true)]
        [TypeConverter(typeof(CollectionConverter))]
        [PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [JsonProperty(PropertyName = "children")]
        public zTreeNodeCollection Nodes { get { return _nodes ?? (_nodes = new zTreeNodeCollection(this)); } }

        /// <summary>
        /// <para>1、设置节点的 checkbox / radio 是否禁用 [check.enable = true 时有效]</para>
        /// <para>2、为了解决部分朋友生成 json 数据出现的兼容问题, 支持 "false","true" 字符串格式的数据</para>
        /// <para>3、请勿对已加载的节点修改此属性，禁止 或 取消禁止 请使用 setChkDisabled() 方法</para>
        /// <para>4、初始化时，如果需要子孙节点继承父节点的 chkDisabled 属性，请设置 check.chkDisabledInherit 属性</para>
        /// <para>默认值：false</para>
        /// </summary>
        [JsonProperty(PropertyName = "chkDisabled")]
        [DefaultValue(false)]
        public bool ChkDisabled { get; set; }

        /// <summary>
        ///  <para>强制节点的 checkBox / radio 的 半勾选状态。[check.enable = true 并且 treeNode.nocheck = false 时有效]</para>
        ///  <para>1、强制为半勾选状态后，不再进行自动计算半勾选状态</para>
        ///  <para>2、设置 treeNode.halfCheck = false 或 null 才能恢复自动计算半勾选状态</para>
        ///  <para>3、为了解决部分朋友生成 json 数据出现的兼容问题, 支持 "false","true" 字符串格式的数据</para>
        ///  <para>默认值：false</para>
        /// </summary>
        [JsonProperty(PropertyName = "halfCheck")]
        [DefaultValue(false)]
        public bool HalfCheck { get; set; }

        /// <summary>
        /// <para>节点自定义图标的 URL 路径。</para>
        /// <para>1、父节点如果只设置 icon ，会导致展开、折叠时都使用同一个图标</para>
        /// <para>2、父节点展开、折叠使用不同的个性化图标需要同时设置 treeNode.iconOpen / treeNode.iconClose 两个属性</para>
        /// <para>3、如果想利用 className 设置个性化图标，需要设置 treeNode.iconSkin 属性</para>
        /// <para>默认值：无</para>
        /// </summary>
        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        /// <summary>
        /// <para>父节点自定义折叠时图标的 URL 路径。</para>
        /// <para>1、此属性只针对父节点有效</para>
        /// <para>2、此属性必须与 iconOpen 同时使用</para>
        /// <para>3、如果想利用 className 设置个性化图标，需要设置 treeNode.iconSkin 属性</para>
        /// <para>默认值：无</para>
        /// </summary>
        [JsonProperty(PropertyName = "iconClose")]
        public string IconClose { get; set; }

        /// <summary>
        /// <para>父节点自定义展开时图标的 URL 路径。</para>
        /// <para>1、此属性只针对父节点有效</para>
        /// <para>2、此属性必须与 iconClose 同时使用</para>
        /// <para>3、如果想利用 className 设置个性化图标，需要设置 treeNode.iconSkin 属性</para>
        /// <para>默认值：无</para>
        /// </summary>
        [JsonProperty(PropertyName = "iconOpen")]
        public string IconOpen { get; set; }

        /// <summary>
        /// <para>节点自定义图标的 className</para>
        /// <para>1、需要修改 css，增加相应 className 的设置</para>
        /// <para>2、css 方式简单、方便，并且同时支持父节点展开、折叠状态切换图片</para>
        /// <para>3、css 建议采用图片分割渲染的方式以减少反复加载图片，并且避免图片闪动</para>
        /// <para>4、zTree v3.x 的 iconSkin 同样支持 IE6</para>
        /// <para>5、如果想直接使用 图片的Url路径 设置节点的个性化图标，需要设置 treeNode.icon / treeNode.iconOpen / reeNode.iconClose 属性</para>
        /// <para>默认值：无</para>
        /// </summary>
        [JsonProperty(PropertyName = "iconSkin")]
        public string IconSkin { get; set; }

        /// <summary>
        /// <para>判断 treeNode 节点是否被隐藏。</para>
        /// <para>1、初始化 zTree 时，如果节点设置 isHidden = true，会被自动隐藏</para>
        /// <para>2、请勿对已加载的节点修改此属性，隐藏 / 显示 请使用 hideNode() / hideNodes() / showNode() / showNodes() 方法</para>
        /// </summary>
        [JsonProperty(PropertyName = "isHidden")]
        [DefaultValue(false)]
        public bool IsHidden { get; set; }

        /// <summary>
        /// 记录 treeNode 节点是否为父节点。
        /// 1、初始化节点数据时，根据 treeNode.children 属性判断，有子节点则设置为 true，否则为 false
        /// 2、初始化节点数据时，如果设定 treeNode.isParent = true，即使无子节点数据，也会设置为父节点
        /// 3、为了解决部分朋友生成 json 数据出现的兼容问题, 支持 "false","true" 字符串格式的数据
        /// </summary>
        [JsonProperty(PropertyName = "isParent")]
        [DefaultValue(false)]
        public bool IsParent { get; set; }
        /// <summary>
        /// <para>节点名称。</para>
        /// <para>如果不使用 name 属性保存节点名称，请修改 data.key.name</para>
        /// <para>默认值：无</para>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// treeNode 节点的唯一标识 tId。
        /// </summary>
        [Description("treeNode 节点的id。")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// <para>1、设置节点是否隐藏 checkbox / radio [check.enable = true 时有效]</para>
        /// <para>2、为了解决部分朋友生成 json 数据出现的兼容问题, 支持 "false","true" 字符串格式的数据</para>
        /// <para>默认值：false</para>
        /// </summary>
        [JsonProperty(PropertyName = "nocheck")]
        [DefaultValue(false)]
        public bool Nocheck { get; set; }

        /// <summary>
        /// <para>记录 treeNode 节点的 展开 / 折叠 状态。</para>
        /// <para>1、初始化节点数据时，如果设定 treeNode.open = true，则会直接展开此节点</para>
        /// <para>2、叶子节点 treeNode.open = false</para>
        /// <para>3、为了解决部分朋友生成 json 数据出现的兼容问题, 支持 "false","true" 字符串格式的数据</para>
        /// <para>4、非异步加载模式下，无子节点的父节点设置 open=true 后，可显示为展开状态，但异步加载模式下不会生效。（v3.5.15+）</para>
        /// <para>默认值：false</para>
        /// </summary>
        [JsonProperty(PropertyName = "open")]
        [DefaultValue(false)]
        public bool Open { get; set; }

        /// <summary>
        /// 设置点击节点后在何处打开 url。[treeNode.url 存在时有效]
        /// 默认值：无
        /// </summary>
        [JsonProperty(PropertyName = "target")]
        public string Target { get; set; }

        /// <summary>
        /// 节点链接的目标 URL
        /// 1、编辑模式 (edit.enable = true) 下此属性功能失效，如果必须使用类似功能，请利用 onClick 事件回调函数自行控制。
        /// 2、如果需要在 onClick 事件回调函数中进行跳转控制，那么请将 URL 地址保存在其他自定义的属性内，请勿使用 url
        /// 默认值：无
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        ///// <summary>
        ///// 字体样式 如：{'color':'red'}
        ///// </summary>
        //[JsonProperty(PropertyName = "font")]
        //public Font Font { get; set; }

        private Dictionary<string, object> _attributes;
        /// <summary>
        /// 用于保存节点的其他自定义数据信息，不要与 zTree 使用的属性相同即可，用户可随意设定。
        /// </summary>
        [JsonProperty(PropertyName = "attributes")]
        public Dictionary<string, object> Attributes { get { return _attributes ?? (_attributes = new Dictionary<string, object>()); } }
    }

    #endregion

    /// <summary>
    /// zTree绑定每个树节点时触发
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void zTreeNodeDataBoundEventHandler(object sender, zTreeNodeDataBoundEventArgs e);

    /// <summary>
    /// zTree树节点绑定事件参数
    /// </summary>
    public class zTreeNodeDataBoundEventArgs : EventArgs
    {
        public zTreeNodeDataBoundEventArgs() : base() { }
        public zTreeNodeDataBoundEventArgs(object dataItem, zTreeNode node)
            : base()
        {
            this.DataItem = dataItem;
            this.Node = node;
        }
        /// <summary>
        /// 树节点
        /// </summary>
        public zTreeNode Node { get; internal set; }

        /// <summary>
        /// 当前节点数据绑定项
        /// </summary>
        public object DataItem { get; internal set; }
    }
}
