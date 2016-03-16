using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smart.Core.Extensions;

namespace Smart.Web.Mvc.UI.JqGrid
{
    public class Column
    {
        #region 私有字段

        private readonly string _columnName;
        private Align _align;
        private string _cellattr;
        private string _classes;

        private string _customFormatter;
        private Formatters? _formatter;
        private string _formatOptions;

        private bool? _fixedWidth;
        private bool? _hidden;
        private string _index;
        private bool? _key;
        private string _label;
        private bool? _resizeable;
        private bool? _search;
        private bool? _clearSearch;
        private string _searchDateFormat;
        private IDictionary<string, string> _searchTerms;
        private Searchtype? _searchType;
        private bool? _sortable;
        private SortType? _sortType;
        private SortOrder? _firstSortOrder;
        private bool? _title;
        private int? _width;
        private string _defaultSearchValue;
        private bool? _expandableInTree;
        private List<string> _searchOptions = new List<string>();

        private bool? _editable;
        private EditType? _editType;
        private EditOptions _editOptions;
        private EditRules _editRules;
        private EditFormOptions _editFormOptions;

        #endregion

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name = "columnName">列名，不能是空的或设置为“subgrid”、“cb”、“rn”</param>
        /// <param name="label"></param>
        /// <param name="width"></param>
        /// <param name="searchable"></param>
        /// <param name="editable"></param>
        public Column(string columnName, string label = null, int width = 0, bool searchable = false, bool editable = false)
        {
            if (columnName.IsEmpty())
            {
                throw new ArgumentException("没有指定的列名");
            }

            var reservedNames = new[] { "subgrid", "cb", "rn" };
            if (reservedNames.Contains(columnName))
            {
                throw new ArgumentException("列名 '" + columnName + "' 不可用");
            }

            _columnName = columnName;
            if (!label.IsEmpty()) SetLabel(label);
            if (width > 0) SetWidth(width);
            if (searchable) SetSearch();
            if (editable) SetEditable();
        }

        /// <summary>
        ///  定义单元格中内容对齐方式，可用值： left, center, right.
        /// </summary>
        /// <param name = "align"></param>
        public Column SetAlign(Align align)
        {
            _align = align;
            return this;
        }

        /// <summary>
        /// function(rowId,val,cm,rdata){}
        /// </summary>
        /// <param name="cellAttrfunc"></param>
        /// <returns></returns>
        public Column SetCellAttr(string cellAttrfunc)
        {
            _cellattr = cellAttrfunc;
            return this;
        }

        /// <summary>
        /// 设置列的css。多个class之间用空格分隔，如：'class1 class2' 。表格默认的css属性是ui-ellipsis
        /// </summary>
        /// <param name = "classes"></param>
        public Column SetClasses(string classes)
        {
            _classes = classes;
            return this;
        }

        /// <summary>
        ///    当配置了查询模块后，是否允许将此列作为查询条件。
        /// </summary>
        /// <param name = "searchOption"></param>
        /// <param name = "clearSearch"></param>
        /// <param name = "searchDateFormat">日期格式化字符串 (默认: dd-mm-yy)</param>
        public Column SetSearch(SearchOptions searchOption = SearchOptions.None, string defaultSearchValue = null,
            bool clearSearch = true, string searchDateFormat = null, string[] searchTerms = null)
        {
            _search = true;
            _clearSearch = clearSearch;
            _defaultSearchValue = defaultSearchValue;
            SetSearchOption(searchOption);
            if (!_searchDateFormat.IsEmpty()) _searchDateFormat = searchDateFormat;
            if (searchTerms != null) _searchTerms = searchTerms.ToDictionary(t => t);
            return this;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name = "searchTerms"></param>
        public Column SetSearchTerms(IDictionary<string, string> searchTerms)
        {
            _searchTerms = searchTerms;
            return this;
        }

        internal Column SetSearchOption(SearchOptions searchOption)
        {
            var searchOptionValues = Enum.GetValues(typeof(SearchOptions));
            foreach (SearchOptions value in searchOptionValues)
            {
                if ((searchOption & value) == value)
                {
                    switch (value)
                    {
                        case SearchOptions.Equal:
                            _searchOptions.Add("eq");
                            break;
                        case SearchOptions.NotEqual:
                            _searchOptions.Add("ne");
                            break;
                        case SearchOptions.Less:
                            _searchOptions.Add("lt");
                            break;
                        case SearchOptions.LessOrEqual:
                            _searchOptions.Add("le");
                            break;
                        case SearchOptions.Greater:
                            _searchOptions.Add("gt");
                            break;
                        case SearchOptions.GreaterOrEqual:
                            _searchOptions.Add("ge");
                            break;
                        case SearchOptions.BeginsWith:
                            _searchOptions.Add("bw");
                            break;
                        case SearchOptions.DoesNotBeginWith:
                            _searchOptions.Add("bn");
                            break;
                        case SearchOptions.IsIn:
                            _searchOptions.Add("in");
                            break;
                        case SearchOptions.IsNotIn:
                            _searchOptions.Add("ni");
                            break;
                        case SearchOptions.EndsWith:
                            _searchOptions.Add("ew");
                            break;
                        case SearchOptions.DoesNotEndWith:
                            _searchOptions.Add("en");
                            break;
                        case SearchOptions.Contains:
                            _searchOptions.Add("cn");
                            break;
                        case SearchOptions.DoesNotContain:
                            _searchOptions.Add("nc");
                            break;
                    }
                }
            }


            return this;
        }

        internal string SearchOption
        {
            get
            {
                return _searchOptions.Any()
                    ? _searchOptions.First()
                    : "bw";
            }
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name = "formatter"></param>
        /// <param name = "customFormatter"></param>
        /// <param name = "formatOptions"></param>
        public Column SetFormatter(Formatters formatter = Formatters.Custom, string customFormatter = null, string formatOptions = "")
        {
            _customFormatter = customFormatter;
            _formatter = formatter;
            _formatOptions = formatOptions;
            return this;
        }

        /// <summary>
        ///     初始化表格时是否要隐藏此列. (default: false)
        /// </summary>
        /// <param name = "hidden"></param>
        public Column SetHidden(bool hidden)
        {
            _hidden = hidden;
            return this;
        }

        /// <summary>
        ///     当排序时定义排序字段名称的索引，参数名为sidx (default: Same as columnname)
        /// </summary>
        /// <param name = "index">Name of index</param>
        public Column SetIndex(string index)
        {
            _index = index;
            return this;
        }

        internal string Index
        {
            get { return _index; }
        }

        /// <summary>
        ///     如果从服务器获取的数据部包含id，可以通过此配置指定唯一id列。
        ///     只有一列能指定此属性，如果多列配置了这个属性，第一个配置的生效，后续会被忽略。
        /// </summary>
        /// <param name = "key"></param>
        public Column SetKey(bool key)
        {
            _key = key;
            return this;
        }

        /// <summary>
        ///     如果jqGrid的colNames数组为空，这个将作为此列的列头。
        ///     如果colNames和这个配置为空，那name配置将作为此列的列头。
        /// </summary>
        /// <param name = "label"></param>
        public Column SetLabel(string label)
        {
            _label = label;
            return this;
        }

        /// <summary>
        ///     定义此列是否允许调整宽度 (default: true)
        /// </summary>
        /// <param name = "resizeable"></param>
        public Column SetResizeable(bool resizeable)
        {
            _resizeable = resizeable;
            return this;
        }

        /// <summary>
        ///     定义查询对象的类型。
        /// </summary>
        /// <param name = "searchType">Search type</param>
        public Column SetSearchType(Searchtype searchType)
        {
            _searchType = searchType;
            return this;
        }

        /// <summary>
        ///     定义是否允许点击列表头进行排序
        /// </summary>
        /// <param name = "sortable"></param>
        /// <param name = "sortType">当datatype为local时有效。定义适当的排序类型。</param>
        public Column SetSortable(bool sortable, SortType sortType = SortType.Text, SortOrder firstSortOrder = SortOrder.Asc)
        {
            _sortable = sortable;
            _sortType = sortType;
            _firstSortOrder = firstSortOrder;
            return this;
        }

        /// <summary>
        ///     如果设置为false，当鼠标移动到单元格上时不显示title提示信息。 (default: true)
        /// </summary>
        /// <param name = "title"></param>
        public Column SetTitle(bool title)
        {
            _title = title;
            return this;
        }

        /// <summary>
        ///     列宽度是否要固定不可变 (default: false)
        /// </summary>
        /// <param name = "fixedWidth"></param>
        public Column SetFixedWidth(bool fixedWidth)
        {
            _fixedWidth = fixedWidth;
            return this;
        }

        /// <summary>
        ///     默认列的宽度，只能是象素值，不能是百分比 (default: 150)
        /// </summary>
        /// <param name = "width">Width in pixels</param>
        public Column SetWidth(int width)
        {
            _width = width;
            return this;
        }

        /// <summary>
        /// 设置TreeGrid模式时自动展开列
        /// </summary>
        /// <returns></returns>
        public Column SetAsExpandable()
        {
            _expandableInTree = true;
            return this;
        }

        internal bool HasDefaultSearchValue
        {
            get { return !_defaultSearchValue.IsEmpty(); }
        }
        internal string DefaultSearchValue { get { return _defaultSearchValue; } }

        public bool IsKey
        {
            get { return _key ?? false; }
        }
        public bool IsExpandable
        {
            get { return _expandableInTree ?? false; }
        }

        public string Name
        {
            get { return _columnName; }
        }

        /// <summary>
        /// 定义字段是否可以编辑。用于单元格，行编辑，和表单编辑。
        /// </summary>
        /// <param name="editType">编辑的类型</param>
        /// <param name="editOptions">根据edittype提供允许的值列表。</param>
        /// <param name="editOptions">对于可编辑单元格的一些额外属性设置。</param>
        /// <returns></returns>
        public Column SetEditable(EditType editType = EditType.Text,
            EditOptions editOptions = null, EditRules editRules = null, EditFormOptions editFormOptions = null)
        {
            _editable = true;
            _editType = editType;
            _editOptions = editOptions;
            _editRules = editRules;
            _editFormOptions = editFormOptions;
            return this;
        }

        /// <summary>
        ///     创建JavaScript字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var script = new StringBuilder();

            script.Append("{");

            if (_align != Align.Left) script.AppendFormat("align:'{0}',", _align.ToString().ToLower());

            if (!_classes.IsEmpty())
                script.AppendFormat("classes:'{0}',", _classes);

            script.AppendFormat("name:'{0}',", _columnName);

            if (_firstSortOrder.HasValue) script.AppendFormat("firstsortorder:'{0}',", _firstSortOrder.ToString().ToLower());

            if (_fixedWidth.HasValue)
                script.AppendFormat("fixed:{0},", _fixedWidth.Value.ToString().ToLower());

            if (_formatter.HasValue && _formatter == Formatters.Custom && !_customFormatter.IsEmpty())
                script.AppendFormat("formatter:{0},", _customFormatter);

            if (_formatter.HasValue && _formatter != Formatters.Custom)
                script.AppendFormat("formatter:'{0}',", _formatter.ToString().ToLower());

            if (_formatter.HasValue && !_formatOptions.IsEmpty())
                script.Append("formatter:'" + _formatter.ToString().ToLower() + "', formatoptions: {" + _formatOptions + "},");

            if (_hidden.HasValue) script.AppendFormat("hidden:{0},", _hidden.Value.ToString().ToLower());

            if (_key.HasValue) script.AppendFormat("key:{0},", _key.Value.ToString().ToLower());

            if (!_label.IsEmpty()) script.AppendFormat("label:'{0}',", _label);

            if (_resizeable.HasValue)
                script.AppendFormat("resizable:{0},", _resizeable.Value.ToString().ToLower());

            if (_search.HasValue) script.AppendFormat("search:{0},", _search.Value.ToString().ToLower());

            if (_sortable.HasValue)
                script.AppendFormat("sortable:{0},", _sortable.Value.ToString().ToLower());

            if (_title.HasValue) script.AppendFormat("title:{0},", _title.Value.ToString().ToLower());

            if (_width.HasValue) script.AppendFormat("width:{0},", _width.Value);

            if (_editable.HasValue)
                script.AppendFormat("editable:{0},", _editable.Value.ToString().ToLower());

            var searchOptions = new Dictionary<string, string>();

            if (_searchType.HasValue)
            {
                if (_searchType.Value == Searchtype.Text)
                {
                    script.Append("stype:'text',");
                }

                if (_searchType.Value == Searchtype.Select)
                {
                    script.Append("stype:'select',");
                }

                if (_searchOptions.Any())
                {
                    searchOptions.Add("sopt", string.Format("['{0}']", _searchOptions.Aggregate((current, next) => current + "',  '" + next)));
                }
                else
                {
                    searchOptions.Add("sopt", "['bw']");
                }
            }

            if (_searchType == Searchtype.Select || _searchType == Searchtype.Datepicker)
            {
                if (_searchType == Searchtype.Select)
                {
                    if (_searchTerms != null)
                    {
                        var emtpyOption = (_searchTerms.Any()) ? ":;" : ":";
                        searchOptions.Add("value", "\"" + string.Format("{0}{1}", emtpyOption, string.Join(";", _searchTerms.Select(s => s.Key + ":" + s.Value).ToArray())) + "\"");
                    }
                    else
                    {
                        searchOptions.Add("value", "':'");
                    }
                }

                if (_searchType == Searchtype.Datepicker)
                {
                    if (_searchDateFormat.IsEmpty())
                    {
                        searchOptions.Add("dataInit", "function(el){$(el).datepicker({changeYear:true, onSelect: function() {var sgrid = $('###gridid##')[0]; sgrid.triggerToolbar();},dateFormat:'dd-mm-yy'});}");
                    }
                    else
                    {
                        searchOptions.Add("dataInit", "function(el){$(el).datepicker({changeYear:true, onSelect: function() {var sgrid = $('###gridid##')[0]; sgrid.triggerToolbar();},dateFormat:'" + _searchDateFormat + "'});}");
                    }
                }
            }

            if (_searchType.HasValue && !_defaultSearchValue.IsEmpty())
            {
                searchOptions.Add("defaultValue", "'" + _defaultSearchValue + "'");
            }

            if ((!_searchType.HasValue && !_defaultSearchValue.IsEmpty()))
                searchOptions.Add("defaultValue", "'" + _defaultSearchValue + "'");

            if (_clearSearch.HasValue)
                searchOptions.Add("clearSearch", _clearSearch.Value.ToString().ToLower());

            if (_searchOptions.Any() && !_searchType.HasValue) // When searchtype is set, searchoptions is already added
                searchOptions.Add("sopt", "['" + _searchOptions.Aggregate((current, next) => current + "', '" + next) + "']");

            if (searchOptions.Any())
                script.Append("searchoptions: { " + string.Join(", ", searchOptions.Select(x => x.Key + ":" + x.Value)) + " },");

            if (_editType.HasValue)
                script.AppendFormat("edittype:'{0}',", _editType.Value.ToString().ToLower());

            if (_editOptions != null)
                script.AppendFormat("editoptions:{0},", _editOptions.ToString());

            if (_editRules != null)
                script.AppendFormat("editrules:{0},", _editRules.ToString());

            if (_editFormOptions != null)
                script.AppendFormat("formoptions:{0},", _editFormOptions.ToString());

            if (_sortType.HasValue)
                script.AppendFormat("sorttype:'{0}',", _sortType.ToString().ToLower());

            script.AppendFormat("index:'{0}'", _index);

            script.Append("}");

            return script.ToString();
        }
    }
}
