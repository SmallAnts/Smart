using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Smart.Web.Mvc.UI.JqGrid.DataReaders;
using Smart.Core.Extensions;

namespace Smart.Web.Mvc.UI.JqGrid
{
    /// <summary>
    ///     
    /// </summary>
    public class Grid : IHtmlString
    {
        #region 私有字段
        private readonly List<Column> _columns = new List<Column>();
        private readonly string _id;
        private bool _asyncLoad;
        private string _altClass;
        private bool _altRows = false;
        private bool _autoEncode = false;
        private bool _autoWidth = false;
        private string _caption;
        private int _cellLayout = 5;

        private bool _cellEdit = false;
        private SubmitType _cellsubmit = SubmitType.Remote;
        private string _cellurl;

        //private object _cmTemplate;
        //private object[] _data;
        //private string _datastr;
        private bool _stringResult = true;
        private DataType _dataType = DataType.Json;
        private bool _deepempty = false;
        private bool _deselectAfterSort = true;
        //private string _direction;
        private Direction? _sortIconDirection;
        private string _editurl;
        private string _emptyRecords;   //see lang file
        private bool _footerRow = false;
        private bool _forceFit = false;
        private GridState _gridstate = GridState.Visible;
        private bool _gridView = false;

        private bool _grouping = false;
        private GroupingView _groupingView;

        private bool _headerTitles = false;
        private int _height = 150;
        private bool _hiddenGrid = false;
        private bool _hideGrid = true;
        private bool _hoverRows = true;
        //private string _idPrefix;
        private bool _ignoreCase = false;
        //private object _inlineData;
        private JsonReader _jsonReader;
        //private int _lastpage = 0;
        private bool _loadonce = false;
        private string _loadtext;
        private LoadUi _loadui = LoadUi.Enable;
        private RequestType? _mtype = RequestType.Get;

        private MultiKey _multiKey = MultiKey.None;
        private bool _multiboxOnly = false;
        private bool _multiSelect = false;
        private int _multiSelectWidth = 20;
        private bool _multiSort = false;

        private int _page = 0;
        private bool _pager = false;
        private PagerPos _pagerPos = PagerPos.Center;
        private bool _pgButtons = true;
        private bool _pgInput = true;
        private string _pgText;
        //private object _postData;
        private RecordPos _recordPos = RecordPos.Right;
        private string _recordText;
        private bool _topPager = false;
        private bool _viewRecords = false;
        //private bool _viewsortcols = false;//[false,'vertical',true]

        private string _resizeClass;

        private int[] _rowList;
        private bool _rowNumbers = false;
        private int _rowNum = 20;
        //private int? _rowTotal;
        private int _rowNumWidth = 25;

        private bool _scroll = false;
        private int _scrollOffset = 18;
        private int _scrollTimeout = 200;
        private bool _scrollRows = false;
        private int? _scrollInt;

        private bool _shrinkToFit = true;
        private int _shrinkToFitInt;

        private bool _sortable = false;
        private string _sortName;
        private SortOrder _sortOrder = SortOrder.Asc;

        //private bool _subGrid = false;
        //private object _subGridOptions;
        //private object[] _subGridModel;
        //private string _subGridType;
        //private string _subGridUrl;
        //private int _subGridWidth = 20;

        private bool _toolbar = false;
        private ToolbarPosition _toolbarPosition = ToolbarPosition.Top;

        private bool _treeGrid = false;
        private TreeGridModel _treeGridModel = TreeGridModel.Nested;
        //private TreeIcons _treeIcons;
        //private object _treeReader;
        private int? _treeRootLevel;
        //private bool _ExpandColClick = true;
        //private string ExpandColumn;

        private string _url;

        //private object _userData;
        private bool _userDataOnFooter;


        private int? _width;

        private bool? _searchClearButton;
        private bool? _searchOnEnter;
        private bool? _searchToolbar;
        private bool? _showAllSortIcons;
        private bool? _sortOnHeaderClick;
        private bool? _searchToggleButton;
        private string _rowAttr;

        private string _onAfterInsertRow;
        private string _onBeforeRequest;
        private string _onBeforeSelectRow;
        private string _onCellSelect;
        private string _onDblClickRow;
        private string _onGridComplete;
        private string _onHeaderClick;
        private string _onLoadBeforeSend;
        private string _onLoadComplete;
        private string _onLoadError;
        private string _onPaging;
        private string _onResizeStart;
        private string _onResizeStop;
        private string _onRightClickRow;
        private string _onSelectAll;
        private string _onSelectRow;
        private string _onSerializeGridData;
        private string _onSortCol;

        #endregion

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name = "id">Id of grid</param>
        /// <param name = "caption"></param>
        /// <param name = "url"></param>
        /// <param name = "asyncLoad"></param>
        public Grid(string id, string caption = null, string url = null, bool asyncLoad = false)
        {
            if (id.IsEmpty())
            {
                throw new ArgumentException("Id参数不能设置为空！必需为有效的HTML节点ID");
            }
            _id = id;
            _caption = caption;
            _url = url;
            _asyncLoad = asyncLoad;

            _autoWidth = true;
            _pager = true;
            _mtype = RequestType.Post;
        }

        #region 属性

        /// <summary>
        ///  新增列
        /// </summary>
        /// <param name = "column">Colomn object</param>
        public Grid AddColumn(Column column)
        {
            _columns.Add(column);
            return this;
        }

        /// <summary>
        ///  新增多个列
        /// </summary>
        /// <param name = "columns">IEnumerable of Colomn objects to add to the grid</param>
        public Grid AddColumns(IEnumerable<Column> columns)
        {
            _columns.AddRange(columns);
            return this;
        }

        /// <summary>
        ///  设置交替行样式，
        ///  此选项仅当altrows选项设置为True是有效的，（默认值：ui-priority-secondary）
        /// </summary>
        /// <param name = "altClass">Classname for alternate rows</param>
        public Grid SetAltClass(string altClass)
        {
            _altClass = altClass;
            return this;
        }

        /// <summary>
        /// 启用或禁用交替行样式
        /// </summary>
        /// <param name = "altRows"></param>
        public Grid SetAltRows(bool altRows = true)
        {
            _altRows = altRows;
            return this;
        }

        /// <summary>
        /// 启用或禁用对url进行编码，默认false
        /// </summary>
        /// <param name = "autoEncode">当为 ture 时对url进行编码</param>
        public Grid SetAutoEncode(bool autoEncode = true)
        {
            _autoEncode = autoEncode;
            return this;
        }

        /// <summary>
        ///     启用或禁用自动计算宽度 （默认值：false）
        /// </summary>
        /// <param name = "autoWidth"> 
        ///     为ture时，则当表格在首次被创建时会根据父元素比例重新调整表格宽度。
        ///     如果父元素宽度改变，为了使表格宽度能够自动调整则需要实现函数：setGridWidth
        /// </param>
        public Grid SetAutoWidth(bool autoWidth = true)
        {
            _autoWidth = autoWidth;
            return this;
        }

        /// <summary>
        ///     设置表格标题内容
        /// </summary>
        /// <param name = "caption">标题内容</param>
        public Grid SetCaption(string caption)
        {
            _caption = caption;
            return this;
        }

        /// <summary>
        /// 定义了单元格padding + border 宽度。通常不必修改此值。初始值为5，paddingLef?2+paddingRight?2+borderLeft?1=5
        /// </summary>
        /// <param name="cellLayout"></param>
        /// <returns></returns>
        public Grid SetCellLayout(int cellLayout)
        {
            _cellLayout = cellLayout;
            return this;
        }

        /// <summary>
        /// 启用或者禁用单元格编辑功能
        /// </summary>
        /// <param name="cellSubmit"></param>
        /// <param name="cellUrl"></param>
        /// <returns></returns>
        public Grid SetCellEdit(SubmitType cellSubmit = SubmitType.Remote, string cellUrl = null)
        {
            _cellEdit = true;
            _cellsubmit = cellSubmit;
            _cellurl = cellUrl;
            return this;
        }

        /// <summary>
        ///  设置从服务器端返回的数据类型，默认json。
        /// </summary>
        /// <param name = "dataType"></param>
        public Grid SetDataType(DataType dataType)
        {
            _dataType = dataType;
            return this;
        }

        /// <summary>
        ///     设置为true。此配置使用 jQuery empty作用于行和行的子元素。
        ///     会有速度开销，但是可以防止内存泄露。
        ///     可排序的行或者列激活时也需要设置为true
        /// </summary>
        /// <param name="deepempty"></param>
        /// <returns></returns>
        public Grid SetDeepEmpty(bool deepempty = true)
        {
            _deepempty = deepempty;
            return this;
        }

        /// <summary>
        ///  当排序时不选择当前行，只有当datatype为local时起作用。
        /// </summary>
        /// <param name="deselectAfterSort"></param>
        /// <returns></returns>
        public Grid SetDeselectAfterSort(bool deselectAfterSort = true)
        {
            _deselectAfterSort = deselectAfterSort;
            return this;
        }

        /// <summary>
        ///     定义行编辑或者表单编辑时完成编辑后保存数据的动态页地址。
        ///     可以设置为客户端数组 clientArray 然后手动提交数据到服务器。
        /// </summary>
        /// <param name="editurl"></param>
        /// <returns></returns>
        public Grid SetEditUrl(string editurl)
        {
            _editurl = editurl;
            return this;
        }

        /// <summary>
        /// 设置当返回的数据行数为0时显示的信息。只有当属性 viewrecords 设置为ture时起作用
        /// </summary>
        /// <param name = "emptyRecords"></param>
        public Grid SetEmptyRecords(string emptyRecords)
        {
            _emptyRecords = emptyRecords;
            return this;
        }

        /// <summary>
        ///    启用或禁用显示表格页脚行
        /// </summary>
        /// <param name = "footerRow"></param>
        public Grid SetFooterRow(bool footerRow = true)
        {
            _footerRow = footerRow;
            return this;
        }

        /// <summary>
        /// 启用或禁调整列宽度不会改变表格的宽度。当shrinkToFit 为false时，此属性会被忽略
        /// </summary>
        /// <param name = "forceFit"></param>
        public Grid SetForceFit(bool forceFit = true)
        {
            _forceFit = forceFit;
            return this;
        }

        /// <summary>
        /// 定义当前表格的状态：'visible'（默认） or 'hidden'
        /// </summary>
        /// <param name="gridState"></param>
        /// <returns></returns>
        public Grid SetGridState(GridState gridState)
        {
            _gridstate = gridState;
            return this;
        }

        /// <summary>
        ///     构造一行数据后添加到grid中，
        ///     如果设为true则是将整个表格的数据都构造完成后再添加到grid中，
        ///     但treeGrid, subGrid, or afterInsertRow 不能用
        /// </summary>
        /// <param name = "gridView"></param>
        public Grid SetGridView(bool gridView = true)
        {
            _gridView = gridView;
            return this;
        }

        /// <summary>
        /// 启用分组功能
        /// </summary>
        /// <param name="groupingView"></param>
        /// <returns></returns>
        public Grid SetGrouping(GroupingView groupingView = null)
        {
            _grouping = true;
            _groupingView = groupingView;

            // 当启用分组时，下面的选项的值设置成这样
            _scroll = false;
            _rowNumbers = false;
            _treeGrid = false;
            _gridView = true; //(afterInsertRow不会触发)
            return this;
        }

        /// <summary>
        ///   启用或禁用title
        /// </summary>
        /// <param name = "headerTitles">设置为true将会添加到表头的title属性</param>
        public Grid SetHeaderTitles(bool headerTitles = true)
        {
            _headerTitles = headerTitles;
            return this;
        }

        /// <summary>
        /// 设置表格高度，可以是数字，像素值或者百分比
        /// </summary>
        /// <param name = "height"></param>
        public Grid SetHeight(int height)
        {
            _height = height;
            return this;
        }

        /// <summary>
        ///     当为ture时，表格不会被显示，只显示表格的标题。
        ///     只有当点击显示表格的那个按钮时才会去初始化表格数据。默认false
        /// </summary>
        /// <param name = "hiddenGrid"></param>
        public Grid SetHiddenGrid(bool hiddenGrid = true)
        {
            _hiddenGrid = hiddenGrid;
            return this;
        }

        /// <summary>
        ///  启用或禁用显示/隐藏“网格”按钮，该按钮出现在标题层右侧。
        ///  仅当标题属性不是空字符串时才生效。（默认值：true）
        /// </summary>
        /// <param name = "hideGrid">Boolean indicating if show/hide button is enabled</param>
        public Grid SetHideGrid(bool hideGrid = true)
        {
            _hideGrid = hideGrid;
            return this;
        }

        /// <summary>
        /// 启用或禁用行的鼠标悬停效果，默认值：true
        /// </summary>
        /// <param name = "hoverRows"></param>
        public Grid SetHoverRows(bool hoverRows = true)
        {
            _hoverRows = hoverRows;
            return this;
        }

        /// <summary>
        /// 设置查询和排序是否区分大小写。
        /// </summary>
        /// <param name="ignoreCase">设置为true区分大小写</param>
        /// <returns></returns>
        public Grid SetIgnoreCase(bool ignoreCase = true)
        {
            _ignoreCase = ignoreCase;
            return this;
        }

        /// <summary>
        ///     描述json 数据格式的配置
        /// </summary>
        /// <param name="jsonReader"></param>
        /// <returns></returns>
        public Grid SetJsonReader(JsonReader jsonReader)
        {
            _jsonReader = jsonReader;
            return this;
        }

        /// <summary>
        /// 如果为ture则数据只从服务器端抓取一次，之后所有操作都是在客户端执行，翻页功能会被禁用，默认为false
        /// </summary>
        /// <param name = "loadOnce"></param>
        public Grid SetLoadOnce(bool loadOnce = true)
        {
            _loadonce = loadOnce;
            return this;
        }

        /// <summary>
        /// 设置当请求或者排序时所显示的文字内容
        /// </summary>
        /// <param name = "loadText">Loadtext</param>
        public Grid SetLoadText(string loadText)
        {
            _loadtext = loadText;
            return this;
        }

        /// <summary>
        ///     当执行ajax请求时要干什么。
        ///     disable禁用ajax执行提示；
        ///     enable默认，当执行ajax请求时的提示； 
        ///     block启用Loading提示，但是阻止其他操作
        /// </summary>
        /// <param name = "loadUi">Load ui mode</param>
        public Grid SetLoadUi(LoadUi loadUi)
        {
            _loadui = loadUi;
            return this;
        }

        /// <summary>
        ///  设置ajax提交方式。POST或者GET，默认GET
        /// </summary>
        /// <param name = "requestType"></param>
        public Grid SetMType(RequestType requestType)
        {
            _mtype = requestType;
            return this;
        }

        /// <summary>
        ///     启用或禁用多选，默认值：false
        /// </summary>
        /// <param name = "multiSelect">为 true 时显示多选列</param>
        /// <param name="multiBoxOnly">为 ture 时只有选择checkbox才会起作用</param>
        /// <param name="multiKey">定义使用那个key来做多选</param>
        /// <param name="multiSelectWidth">multiselect列宽度，默认值：20</param>
        /// <returns></returns>
        public Grid SetMultiSelect(bool multiSelect = true, bool multiBoxOnly = false, MultiKey multiKey = MultiKey.None, int multiSelectWidth = 20)
        {
            _multiSelect = multiSelect;
            _multiboxOnly = multiBoxOnly;
            _multiKey = multiKey;
            _multiSelectWidth = multiSelectWidth;
            return this;
        }

        /// <summary>
        ///     是否允许多重排序，仅当datatype为local时有效。
        ///     sidx包含发送给服务器端排序的内容。
        ///     排序的字段 用逗号分隔，如“field1 asc, field2 desc …, fieldN”。
        ///     注意最后的字段不包含asc 或者desc。
        ///     设置为ture时通过sord参数获取
        /// </summary>
        /// <param name="multiSort"></param>
        /// <returns></returns>
        public Grid SetMultiSort(bool multiSort = true)
        {
            _multiSort = multiSort;
            return this;
        }

        /// <summary>
        /// 设置翻页用的导航栏
        /// </summary>
        /// <param name="page">设置初始页码，默认值：1</param>
        /// <param name="rowNum">设置在grid上显示记录条数，这个参数是要被传递到后台，默认20</param>
        /// <param name="rowList">设置分页行数下拉选择框数据源，用来改变显示记录数，当选择时会覆盖rowNum参数传递到后台</param>
        /// <param name="pager">为 true 启用底部导航栏</param>
        /// <param name="topPager">为 true 启用顶部导航栏</param>
        /// <param name="pgButtons">为 true 显示翻页按钮</param>
        /// <param name="pgInput">为 true 显示翻页输入框</param>
        /// <param name="viewrecords">为 true 显示记录数信息</param>
        /// <param name="pagerPos">设置导航内容所在位置</param>
        /// <param name="recordPos">设置记录信息的位置</param>
        /// <param name="pgText">设置当前页信息，如: "Page {0} of {1}"</param>
        /// <param name="recordText">显示记录数信息。{0} 为记录数开始，{1}为记录数结束。viewrecords为ture时才能起效，且总记录数大于0时才会显示此信息</param>
        /// <returns></returns>
        public Grid SetPager(
            int page = 1, int rowNum = 20, int[] rowList = null,
            bool pager = true, bool topPager = false,
            bool pgButtons = true, bool pgInput = true, bool viewrecords = true,
            PagerPos pagerPos = PagerPos.Center, RecordPos recordPos = RecordPos.Right,
            string pgText = null, string recordText = null)
        {
            _page = page;
            _rowNum = rowNum;
            _rowList = rowList;
            _pager = pager;
            _topPager = topPager;
            _pgButtons = pgButtons;
            _pgInput = pgInput;
            _viewRecords = viewrecords;
            _pagerPos = pagerPos;
            _recordPos = recordPos;
            _pgText = pgText;
            _recordText = recordText;
            return this;
        }

        /// <summary>
        /// 定义一个class到一个列上用来显示列宽度调整时的效果
        /// </summary>
        /// <param name = "resizeClass"></param>
        /// <returns></returns>
        public Grid SetResizeClass(string resizeClass)
        {
            _resizeClass = resizeClass;
            return this;
        }

        /// <summary>
        /// 为ture则会在表格左边新增一列，显示行顺序号，从1开始递增。此列名为'rn'.
        /// </summary>
        /// <param name = "rowNumbers"></param> 
        /// <param name = "rowNumWidth">设置行数列的宽度，如果rownumbers选项设置为true。（默认：25）</param> 
        public Grid SetRowNumbers(bool rowNumbers = true, int rowNumWidth = 25)
        {
            _rowNumbers = rowNumbers;
            _rowNumWidth = rowNumWidth;
            return this;
        }

        /// <summary>
        ///     Determines how to post the data on which we perform searching. 
        ///     When the this option is false the posted data is in key:value pair, if the option is true, the posted data is equal on those as in searchGrid method.
        ///     See here: http://www.trirand.com/jqgridwiki/doku.php?id=wiki:advanced_searching#options
        ///     (default: true)
        /// </summary>
        /// <param name = "stringResult">Boolean indicating if</param>        
        public Grid SetStringResult(bool stringResult = true)
        {
            _stringResult = stringResult;
            return this;
        }

        /// <summary>
        /// 创建一个动态滚动的表格，当为true时，翻页栏被禁用，使用垂直滚动条加载数据，且在首次访问服务器端时将加载所有数据到客户端。
        /// 当此参数为数字时，表格只控制可见的几行，所有数据都在这几行中加载
        /// </summary>
        /// <param name="virtualScroll"></param>
        /// <param name="justHoldVisibleLines">表示网格中的可见线应该在内存中存储（防止内存泄漏）</param>
        /// <param name="scrollOffset">设置垂直滚动条宽度，默认18</param>
        /// <param name="scrollRows">设置为true时，通过setSelection选中某行将会滚动表格使选中的行可见。
        /// 表格有水平滚动条，当使用表格编辑，通过上一条或者下一条导航按钮导向时将会很有用。</param>
        /// <param name="scrollTimeout">控制滚动事件延时时间，当scroll配置为1时有效</param>
        /// <returns></returns>
        public Grid SetScroll(bool justHoldVisibleLines = false, int scrollOffset = 18, bool scrollRows = false, int scrollTimeout = 200)
        {
            _scroll = true;
            if (justHoldVisibleLines)
            {
                _scrollInt = 1;
                _pager = false;
                _topPager = false;
                _scrollTimeout = scrollTimeout;
            }
            else
            {
                _scrollOffset = scrollOffset;
                _scrollRows = scrollRows;
            }
            return this;
        }

        /// <summary>
        /// 此属性用来说明当初始化列宽度时候的计算类型，如果为ture，则按比例初始化列宽度。
        /// 如果为false，则列宽度使用colModel指定的宽度，默认true
        /// </summary>
        /// <param name = "shrinkToFit"></param>
        public Grid SetShrinkToFit(bool shrinkToFit = true, int shrinkToFitInt = 0)
        {
            _shrinkToFit = shrinkToFit;
            if (shrinkToFit) _shrinkToFitInt = shrinkToFitInt;

            return this;
        }

        /// <summary>
        /// 启用或禁用排序功能
        /// </summary>
        /// <param name="sortable">为 true 时启用</param>
        /// <param name="sortName">排序列的名称，此参数会被传到后台</param>
        /// <param name="sortOrder">排序顺序，升序或者降序（asc or desc）</param>
        /// <param name="showAllSortIcons"></param>
        /// <param name="sortIconDirection"></param>
        /// <param name="sortOnHeaderClick"></param>
        /// <returns></returns>
        public Grid SetSort(bool sortable = true, string sortName = null, SortOrder sortOrder = SortOrder.Asc,
            bool showAllSortIcons = true, Direction sortIconDirection = Direction.Vertical, bool sortOnHeaderClick = true)
        {
            _sortable = sortable;
            _sortName = sortName;
            _sortOrder = sortOrder;
            _showAllSortIcons = showAllSortIcons;
            _sortIconDirection = sortIconDirection;
            return this;
        }

        /// <summary>
        /// 搜索设置
        /// </summary>
        /// <param name="searchOnEnter"></param>
        /// <param name="searchToolbar"></param>
        /// <param name="searchClearButton"></param>
        /// <param name="searchToggleButton"></param>
        /// <returns></returns>
        public Grid SetSearch(bool searchOnEnter, bool searchToolbar, bool searchClearButton, bool searchToggleButton)
        {
            _searchOnEnter = searchOnEnter;
            _searchToolbar = searchToolbar;
            _searchClearButton = searchClearButton;
            _searchToggleButton = searchToggleButton;
            return this;
        }

        /// <summary>
        /// 启用或禁用工具栏
        /// </summary>
        /// <param name = "toolbarPosition"></param>
        public Grid SetToolbar(ToolbarPosition toolbarPosition = ToolbarPosition.Top)
        {
            _toolbar = true;
            _toolbarPosition = toolbarPosition;
            return this;
        }

        /// <summary>
        ///  设置获取数据的地址
        /// </summary>
        /// <param name = "url"></param>
        public Grid SetUrl(string url)
        {
            _url = url;
            return this;
        }

        /// <summary>
        /// 如果设置则按此设置为主，如果没有设置则按colModel中定义的宽度计算
        /// </summary>
        /// <param name = "width">Width in pixels</param>
        public Grid SetWidth(int width)
        {
            _width = width;
            return this;
        }

        /// <summary>
        ///  用户数据是否在尾行上，默认值：（false）
        /// </summary>
        /// <param name = "userDataOnFooter"></param>
        public Grid SetUserDataOnFooter(bool userDataOnFooter = true)
        {
            _userDataOnFooter = userDataOnFooter;
            return this;
        }

        /// <summary>
        /// 启用或禁用树型表格
        /// </summary>
        /// <param name="treeGridModel"></param>
        /// <param name="rootLevel"></param>
        /// <returns></returns>
        public Grid SetTreeGrid(TreeGridModel treeGridModel = TreeGridModel.Adjacency, int rootLevel = 0)
        {
            _treeGrid = true;
            _treeRootLevel = 0;
            _treeGridModel = treeGridModel;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowAttr"></param>
        /// <returns></returns>
        public Grid SetRowAttr(string rowAttr)
        {
            _rowAttr = rowAttr;
            return this;
        }

        #endregion

        #region 事件

        /// <summary>
        ///     当插入行时触发。  参数:  'rowid'，'rowdata'，'rowelem' 。
        ///     rowid插入当前行的id；rowdata插入行的数据，格式为name: value，name为colModel中的名字。
        /// </summary>
        /// <param name = "onAfterInsertRow"></param>
        public Grid OnAfterInsertRow(string onAfterInsertRow)
        {
            _onAfterInsertRow = onAfterInsertRow;
            return this;
        }

        /// <summary>
        /// 向服务器端发起请求之前触发此事件但如果datatype是一个function时例外
        /// </summary>
        /// <param name = "onBeforeRequest"></param>
        public Grid OnBeforeRequest(string onBeforeRequest)
        {
            _onBeforeRequest = onBeforeRequest;
            return this;
        }

        /// <summary>
        /// 当用户点击当前行在未选择此行时触发。rowid：此行id；e：事件对象。返回值为ture或者false。
        /// 如果返回true则选择完成，如果返回false则不会选择此行也不会触发其他事件
        /// </summary>
        /// <param name = "onBeforeSelectRow"></param>
        public Grid OnBeforeSelectRow(string onBeforeSelectRow)
        {
            _onBeforeSelectRow = onBeforeSelectRow;
            return this;
        }

        /// <summary>
        /// 当表格所有数据都加载完成而且其他的处理也都完成时触发此事件，排序，翻页同样也会触发此事件，参数无
        /// </summary>
        /// <param name = "onGridComplete"></param>
        public Grid OnGridComplete(string onGridComplete)
        {
            _onGridComplete = onGridComplete;
            return this;
        }

        /// <summary>
        /// 
        ///     'xhr': The XMLHttpRequest
        /// </summary>
        /// <param name = "onLoadBeforeSend"></param>
        public Grid OnLoadBeforeSend(string onLoadBeforeSend)
        {
            _onLoadBeforeSend = onLoadBeforeSend;
            return this;
        }

        /// <summary>
        /// 当从服务器返回响应时执行，xhr：XMLHttpRequest 对象
        ///     'xhr': The XMLHttpRequest
        /// </summary>
        /// <param name = "onLoadComplete"></param>
        public Grid OnLoadComplete(string onLoadComplete)
        {
            _onLoadComplete = onLoadComplete;
            return this;
        }

        /// <summary>
        ///    如果请求服务器失败则调用此方法。xhr：XMLHttpRequest 对象；satus：错误类型，字符串类型；error：exception对象
        ///     'xhr': The XMLHttpRequest
        ///     'status': String describing the type of error
        ///     'error': Optional exception object, if one occurred
        /// </summary>
        /// <param name = "onLoadError"></param>
        public Grid OnLoadError(string onLoadError)
        {
            _onLoadError = onLoadError;
            return this;
        }

        /// <summary>
        ///    当点击单元格时触发。rowid：当前行id；iCol：当前单元格索引；cellContent：当前单元格内容；e：event对象
        ///     Variables available in call:
        ///     'rowid': The id of the row
        ///     'iCol': The index of the cell,
        ///     'cellcontent': The content of the cell,
        ///     'e': The event object element where we click.
        ///     (Be aware that this available when we are not using cell editing module
        ///     and is disabled when using cell editing).
        /// </summary>
        /// <param name = "onCellSelect"></param>
        public Grid OnCellSelect(string onCellSelect)
        {
            _onCellSelect = onCellSelect;
            return this;
        }

        /// <summary>
        ///    双击行时触发。rowid：当前行id；iRow：当前行索引位置；iCol：当前单元格位置索引；e:event对象
        ///     Variables available in call:
        ///     'rowid': The id of the row,
        ///     'iRow': The index of the row (do not mix this with the rowid),
        ///     'iCol': The index of the cell.
        ///     'e': The event object
        /// </summary>
        /// <param name = "onDblClickRow"></param>
        public Grid OnDblClickRow(string onDblClickRow)
        {
            _onDblClickRow = onDblClickRow;
            return this;
        }

        /// <summary>
        ///    当点击显示/隐藏表格的那个按钮时触发；gridstate：表格状态，可选值：visible or hidden
        ///     Variables available in call:
        ///     'gridstate': The state of the grid - can have two values - visible or hidden
        /// </summary>
        /// <param name = "onHeaderClick"></param>
        public Grid OnHeaderClick(string onHeaderClick)
        {
            _onHeaderClick = onHeaderClick;
            return this;
        }

        /// <summary>
        ///     点击翻页按钮填充数据之前触发此事件，同样当输入页码跳转页面时也会触发此事件
        ///     Variables available in call:
        ///     'pgButton': first,last,prev,next in case of button click, records in case when
        ///     a number of requested rows is changed and user when the user change the number of the requested page
        /// </summary>
        /// <param name = "onPaging"></param>
        public Grid OnPaging(string onPaging)
        {
            _onPaging = onPaging;
            return this;
        }

        /// <summary>
        ///    在行上右击鼠标时触发此事件。rowid：当前行id；iRow：当前行位置索引；iCol：当前单元格位置索引；e：event对象
        ///     Variables available in call:
        ///     'rowid': The id of the row,
        ///     'iRow': The index of the row (do not mix this with the rowid),
        ///     'iCol': The index of the cell.
        ///     'e': The event object
        ///     Warning - this event does not work in Opera browsers, since Opera does not support oncontextmenu event
        /// </summary>
        /// <param name = "onRightClickRow"></param>
        public Grid OnRightClickRow(string onRightClickRow)
        {
            _onRightClickRow = onRightClickRow;
            return this;
        }

        /// <summary>
        ///     multiselect为ture，且点击头部的checkbox时才会触发此事件。aRowids：所有选中行的id集合，为一个数组。
        ///     status：boolean变量说明checkbox的选择状态，true选中false不选中。无论checkbox是否选择，aRowids始终有 值
        ///     Variables available in call:
        ///     'aRowids':  Array of the selected rows (rowid's).
        ///     'status': Boolean variable determining the status of the header check box - true if checked, false if not checked.
        ///     Be awareS that the aRowids alway contain the ids when header checkbox is checked or unchecked.
        /// </summary>
        /// <param name = "onSelectAll"></param>
        public Grid OnSelectAll(string onSelectAll)
        {
            _onSelectAll = onSelectAll;
            return this;
        }

        /// <summary>
        ///     当选择行时触发此事件。rowid：当前行id；status：选择状态，当multiselect 为true时此参数才可用
        ///     Variables available in function call:
        ///     'rowid': The id of the row,
        ///     'status': Tthe status of the selection. Can be used when multiselect is set to true.
        ///     true if the row is selected, false if the row is deselected.
        ///     <param name = "onSelectRow"></param>
        /// </summary>
        public Grid OnSelectRow(string onSelectRow)
        {
            _onSelectRow = onSelectRow;
            return this;
        }

        /// <summary>
        ///     当点击排序列但是数据还未进行变化时触发此事件。
        ///     index：name在colModel中位置索引；iCol：当前单元格位置索引；sortorder：排序状态：desc或者asc
        ///     Variables available in call:
        ///     'index': The index name from colModel
        ///     'iCol': The index of column,
        ///     'sortorder': The new sorting order - can be 'asc' or 'desc'.
        ///     If this event returns 'stop' the sort processing is stopped and you can define your own custom sorting
        /// </summary>
        /// <param name = "onSortCol"></param>
        public Grid OnSortCol(string onSortCol)
        {
            _onSortCol = onSortCol;
            return this;
        }

        /// <summary>
        ///     当开始改变一个列宽度时触发此事件。event：event对象；index：当前列在colModel中位置索引
        ///     Variables available in call:
        ///     'event':  The event object
        ///     'index': The index of the column in colModel.
        /// </summary>
        /// <param name = "onResizeStart"></param>
        public Grid OnResizeStart(string onResizeStart)
        {
            _onResizeStart = onResizeStart;
            return this;
        }

        /// <summary>
        ///    当列宽度改变之后触发此事件。newwidth：列改变后的宽度；index：当前列在colModel中的位置索引
        ///     Variables available in call:
        ///     'newwidth': The new width of the column
        ///     'index': The index of the column in colModel.
        /// </summary>
        /// <param name = "onResizeStop"></param>
        public Grid OnResizeStop(string onResizeStop)
        {
            _onResizeStop = onResizeStop;
            return this;
        }

        /// <summary>
        ///    向服务器发起请求时会把数据进行序列化，用户自定义数据也可以被提交到服务器端
        ///     Variables available in call:
        ///     'postData': Posted data
        /// </summary>
        /// <param name = "onSerializeGridData"></param>
        public Grid OnSerializeGridData(string onSerializeGridData)
        {
            _onSerializeGridData = onSerializeGridData;
            return this;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string RenderJavascript()
        {
            var script = new StringBuilder();

            if (_asyncLoad)
                script.Append("jQuery(window).ready(function () {");
            else
                script.Append("jQuery(document).ready(function () {");

            script.AppendFormat("jQuery('#{0}').jqGrid('GridDestroy')", _id);
            if (_pager == true)
            {
                script.AppendFormat(".after('<div id=\"{0}_pager\"></div>')", _id);
            }
            if (_topPager == true)
            {
                script.AppendFormat(".after('<div id=\"{0}_toppager\"></div>')", _id);
            }
            script.Append(".jqGrid({");

            // 确保最多只有一个键
            if (_columns.Count(r => r.IsKey) > 1)
            {
                throw new ArgumentException("添加过多的键列。最大允许为1。");
            }
            if (_grouping && _groupingView == null)
            {
                throw new ArgumentNullException("启用分组必须设置 groupingView！");
            }

            if (!_altClass.IsEmpty()) script.AppendFormat("altclass:'{0}',", _altClass);
            if (_altRows) script.Append("altRows:true,");
            if (_autoEncode) script.Append("autoencode:true,");
            if (_autoWidth) script.Append("autowidth:true,");
            if (!_caption.IsEmpty()) script.AppendFormat("caption:'{0}',", _caption);
            if (_cellLayout != 5) script.AppendFormat("cellLayout:{0},", _cellLayout);

            if (_cellEdit)
            {
                script.Append("cellEdit:true,");
                if (_cellsubmit == SubmitType.Remote)
                {
                    script.AppendFormat("cellurl:'{0}',", _cellurl);
                }
                else
                {
                    script.AppendFormat("cellsubmit:'{0}',", _cellsubmit.ToString().ToLower());
                }
            }

            // Datatype
            script.Append(_treeGrid ? $"treedatatype:'{_dataType.ToString().ToLower()}'," : $"datatype:'{_dataType.ToString().ToLower()}',");

            if (_deepempty) script.Append("deepempty:true,");
            if (_deselectAfterSort == false && _dataType == DataType.Local) script.Append("deselectAfterSort:false,");
            if (!_editurl.IsEmpty()) script.AppendFormat("editurl:'{0}',", _editurl);
            if (!_emptyRecords.IsEmpty()) script.AppendFormat("emptyrecords:'{0}',", _emptyRecords);
            if (_footerRow) script.Append("footerrow:true,");
            if (_forceFit) script.Append("forceFit:true,");
            if (_gridstate != GridState.Visible) script.AppendFormat("footerrow:'{0}',", _gridstate.ToString().ToLower());
            if (_gridView) script.Append("gridView:true,");
            if (_grouping) script.AppendFormat("grouping:true,{0},", _groupingView.ToString());
            if (_headerTitles) script.Append("headertitles:true,");

            if (_height != 150)
            {
                if (_scroll && _scrollInt == 1) script.AppendFormat("height:'{0}',", "100%");
                else script.AppendFormat("height:{0},", _height);
            }
            if (_hiddenGrid) script.Append("hiddengrid:true,");
            if (!_hideGrid) script.Append("hidegrid:false,");
            if (!_hoverRows) script.Append("hoverrows:false,");
            if (_ignoreCase) script.Append("ignoreCase:true,");

            if (_jsonReader != null && _dataType == DataType.Json)
            {
                script.Append("jsonReader:{'repeatitems':" + _jsonReader.RepeatItems.ToString().ToLower() + ", 'id': '" + _jsonReader.Id + "' },");
            }

            if (_loadonce) script.Append("loadonce:true,");
            if (!_loadtext.IsEmpty()) script.AppendFormat("loadtext:'{0}',", _loadtext);
            if (_loadui != LoadUi.Enable) script.AppendFormat("loadui:'{0}',", _loadui.ToString().ToLower());
            if (_mtype != RequestType.Get) script.AppendFormat("mtype:'{0}',", _mtype.ToString().ToUpper());

            if (_multiSelect)
            {
                script.Append("multiselect:true,");
                if (_multiboxOnly) script.Append("multiboxonly:true,");
                if (_multiKey != MultiKey.None) script.AppendFormat("multikey:'{0}',", _multiKey.ToString().ToLower());
                if (_multiSelectWidth != 20) script.AppendFormat("multiselectWidth:{0},", _multiSelectWidth);
            }

            if (_multiSort) script.Append("multiSort:true,");

            if (_pager || _topPager)
            {
                if (_page != 1) script.AppendFormat("page:{0},", _page);
                if (_rowList != null) script.AppendFormat("rowList:[{0}],", _rowList.Join());
                if (_rowNum != 20) script.AppendFormat("rowNum:{0},", _rowNum);
                if (_pager) script.AppendFormat("pager:'#{0}_pager',", _id);
                if (_topPager) script.AppendFormat("topPager:'#{0}_toppager',", _id);
                if (_pagerPos != PagerPos.Center) script.AppendFormat("pagerpos:'{0}',", _pagerPos.ToString().ToLower());
                if (_recordPos != RecordPos.Right) script.AppendFormat("recordpos:'{0}',", _recordPos.ToString().ToLower());
                if (!_pgButtons) script.Append("pgbuttons:false,");
                if (!_pgInput) script.Append("pginput:false,");
                if (!_viewRecords) script.Append("viewRecords:false,");
                if (!_pgText.IsEmpty()) script.AppendFormat("pgtext:'{0}',", _pgText);
                if (!_recordText.IsEmpty() && _viewRecords) script.AppendFormat("recordtext:'{0}',", _recordText);
            }

            if (!_resizeClass.IsEmpty()) script.AppendFormat("resizeclass:'{0}',", _resizeClass);

            if (_rowNumbers)
            {
                script.Append("rownumbers:true,");
                if (_rowNumWidth != 20) script.AppendFormat("rownumWidth:{0},", _rowNumWidth);
            }

            if (_scrollInt == 1)
            {
                script.Append("scroll:1,");
                if (_scrollTimeout != 200) script.AppendFormat("scrollTimeout:{0},", _scrollTimeout);
            }
            if (_scroll)
            {
                script.Append("scroll:true,");
                if (_scrollOffset != 18) script.AppendFormat("scrollOffset:{0},", _scrollOffset);
                if (_scrollRows) script.Append("scrollrows:true,");
            }

            if (_sortable)
            {
                script.Append("sortable:true,");
                if (!_sortName.IsEmpty()) script.AppendFormat("sortname:'{0}',", _sortName);
                if (_sortOrder != SortOrder.Asc) script.AppendFormat("sortorder:'{0}',", _sortOrder.ToString().ToLower());
                if (_showAllSortIcons.HasValue || _sortIconDirection.HasValue || _sortOnHeaderClick.HasValue)
                {
                    if (!_showAllSortIcons.HasValue) _showAllSortIcons = false;
                    if (!_sortIconDirection.HasValue) _sortIconDirection = Direction.Vertical;
                    if (!_sortOnHeaderClick.HasValue) _sortOnHeaderClick = true;

                    script.AppendFormat("viewsortcols:[{0},'{1}',{2}],",
                        _showAllSortIcons.ToString().ToLower(),
                        _sortIconDirection.ToString().ToLower(),
                        _sortOnHeaderClick.ToString().ToLower());
                }
            }

            if (!_shrinkToFit) script.Append("shrinkToFit:false,");
            else if (_shrinkToFitInt > 0) script.AppendFormat("shrinkToFit:{0},", _shrinkToFitInt);

            if (_toolbar) script.AppendFormat("toolbar:[{0},'{1}'],", _toolbar.ToString().ToLower(), _toolbarPosition.ToString().ToLower());
            if (!_url.IsEmpty()) script.AppendFormat("url:'{0}',", _url);
            if (_width.HasValue) script.AppendFormat("width:'{0}',", _width);

            if (_treeGrid)
            {
                if (_columns.Count(r => r.IsExpandable) > 1)
                {
                    throw new ArgumentException("最多只允许一个列设置为 IsExpandable=true");
                }
                var keyColumn = _columns.FirstOrDefault(r => r.IsKey);
                var expandableColumn = _columns.FirstOrDefault(r => r.IsExpandable);
                if (keyColumn == null)
                {
                    throw new ArgumentException("必须设置一个列的 IsKey=true");
                }
                if (expandableColumn == null)
                {
                    throw new ArgumentException("必须设置一个列的 IsExpandable=true");
                }

                script.Append("treeGrid: true,");
                script.AppendFormat("ExpandColumn : '{0}',", expandableColumn.Name);
                script.AppendFormat("treeGridModel : '{0}',", _treeGridModel.ToString().ToLower());
                script.AppendFormat("tree_root_level : {0},", _treeRootLevel);
            }

            if (_userDataOnFooter)
                script.AppendFormat("userDataOnFooter:{0},", _footerRow.ToString().ToLower());
            else if (!_onBeforeRequest.IsEmpty())
                script.AppendFormat("beforeRequest: {0},", _onBeforeRequest);   // function() {{{0}}}

            if (!_onAfterInsertRow.IsEmpty())
                script.AppendFormat("afterInsertRow:{0},", _onAfterInsertRow);  // function(rowid, rowdata, rowelem) {{{0}}}

            if (_columns.Any(x => x.HasDefaultSearchValue))
            {
                #region jqGrid javascript onbefore request hack

                var defaultValueColumns = _columns
                    .Where(x => x.HasDefaultSearchValue)
                    .Select(x => new { field = x.Index, op = x.SearchOption, data = x.DefaultSearchValue });

                var onbeforeRequestHack = @"
                function() {
                        var defaultValueColumns = " + defaultValueColumns.ToJson() + @";
                        var colModel = this.p.colModel;

                        if (defaultValueColumns.length > 0) {
                            var postData = this.p.postData;

                            var filters = {};

                            if (postData.hasOwnProperty('filters')) {
                                filters = JSON.parse(postData.filters);
                            }

                            var rules = [];

                            if (filters.hasOwnProperty('rules')) {
                                rules = filters.rules;
                            }

                            $.each(defaultValueColumns, function (defaultValueColumnIndex, defaultValueColumn) {
                                $.each(rules, function (index, rule) {
                                    if (defaultValueColumn.field == rule.field) {
                                        delete rules[index];
                                        return;
                                    }
                                });

                                rules.push(defaultValueColumn);
                            });

                            filters.groupOp = 'AND';
                            filters.rules = rules;

                            postData._search = true;
                            postData.filters = JSON.stringify(filters);

                            $(this).setGridParam({ search: true, 'postData': postData});
                        }

                        this.p.beforeRequest = function() { " + ((!_onBeforeRequest.IsEmpty()) ? _onBeforeRequest : "") + @" };
                        this.p.beforeRequest.call(this);
                    } ";

                #endregion jqGrid javascript onbefore request hack
                script.AppendFormat("beforeRequest: {0},", onbeforeRequestHack);
            }

            if (!_onBeforeSelectRow.IsEmpty())
                script.AppendFormat("beforeSelectRow: {0},", _onBeforeSelectRow);   //function(rowid, e) {{{0}}}

            if (!_onGridComplete.IsEmpty())
                script.AppendFormat("gridComplete: {0},", _onGridComplete); //function() {{{0}}}

            if (!_onLoadBeforeSend.IsEmpty())
                script.AppendFormat("loadBeforeSend:{0},", _onLoadBeforeSend);  // function(xhr, settings) {{{0}}}

            if (!_onLoadComplete.IsEmpty())
                script.AppendFormat("loadComplete:{0},", _onLoadComplete);  // function(xhr) {{{0}}}

            if (!_onLoadError.IsEmpty())
                script.AppendFormat("loadError:{0},", _onLoadError);    // function(xhr, status, error) {{{0}}}

            if (!_onCellSelect.IsEmpty())
                script.AppendFormat("onCellSelect: {0},", _onCellSelect);   //function(rowid, iCol, cellcontent, e) {{{0}}}

            if (!_onDblClickRow.IsEmpty())
                script.AppendFormat("ondblClickRow:{0},", _onDblClickRow);  // function(rowid, iRow, iCol, e) {{{0}}}

            if (!_onHeaderClick.IsEmpty())
                script.AppendFormat("onHeaderClick: {0},", _onHeaderClick); //function(gridstate) {{{0}}}

            if (!_onPaging.IsEmpty())
                script.AppendFormat("onPaging:{0},", _onPaging);    // function(pgButton) {{{0}}}

            if (!_onRightClickRow.IsEmpty())
                script.AppendFormat("onRightClickRow: {0},", _onRightClickRow); //function(rowid, iRow, iCol, e) {{{0}}}

            if (!_onSelectAll.IsEmpty())
                script.AppendFormat("onSelectAll: {0},", _onSelectAll); //function(aRowids, status) {{{0}}}

            if (!_onSelectRow.IsEmpty())
                script.AppendFormat("onSelectRow: {0},", _onSelectRow); //function(rowid, status) {{{0}}}

            if (!_onSortCol.IsEmpty())
                script.AppendFormat("onSortCol:{0},", _onSortCol);  // function(index, iCol, sortorder) {{{0}}}

            if (!_onResizeStart.IsEmpty())
                script.AppendFormat("resizeStart: {0},", _onResizeStart);   // function(jqGridEvent, index) {{{0}}}

            if (!_onResizeStop.IsEmpty())
                script.AppendFormat("resizeStop: {0},", _onResizeStop); //function(newwidth, index) {{{0}}}

            if (!_onSerializeGridData.IsEmpty())
                script.AppendFormat("serializeGridData: {0},", _onSerializeGridData);   // function(postData) {{{0}}}

            if (!_rowAttr.IsEmpty())
                script.AppendFormat("rowattr:{0},", _rowAttr);  // function(rd) {{{0}}}

            script.Append("colModel: [");
            script.Append(_columns.Join());
            script.Append("]");

            script.Append("});");

            if (_searchToolbar == true && _pager &&
                (_searchClearButton.HasValue && _searchClearButton == true || _searchToggleButton.HasValue && _searchToggleButton == true))
            {
                script.Append("jQuery('#" + _id + "').jqGrid('navGrid',\"#" + _pager + "\",{edit:false,add:false,del:false,search:false,refresh:false}); ");
            }

            if (_searchToolbar == true && _searchClearButton.HasValue && _pager &&
                _searchClearButton == true)
            {
                script.Append("jQuery('#" + _id + "').jqGrid('navButtonAdd',\"#" + _pager +
                                 "\",{caption:\"Clear\",title:\"Clear Search\",buttonicon :'ui-icon-refresh', onClickButton:function(){jQuery('#" + _id + "')[0].clearToolbar(); }}); ");
            }

            if (_searchToolbar == true && _searchToggleButton.HasValue && _pager && _searchToggleButton == true)
            {
                script.Append("jQuery('#" + _id + "').jqGrid('navButtonAdd',\"#" + _pager +
                              "\",{caption:\"Toggle Search\",title:\"Toggle Search\",buttonicon :'ui-icon-refresh', onClickButton:function(){jQuery('#" + _id + "')[0].toggleToolbar(); }}); ");
            }

            if (_searchToolbar == true)
            {
                script.Append("jQuery('#" + _id + "').jqGrid('filterToolbar', {stringResult:" + _stringResult.ToString().ToLower());
                if (_searchOnEnter.HasValue)
                    script.AppendFormat(", searchOnEnter:{0}", _searchOnEnter.ToString().ToLower());
                script.Append("});");
            }

            script.Append("});");

            script.Replace("##gridid##", _id);

            return script.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public string RenderHtmlElements()
        //{
        //    var table = new StringBuilder();
        //    return table.ToString();
        //}
        /// <summary>
        /// 创建 jqGrid 的HTML和SCRIPT
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var script = new StringBuilder();
            //script.Append("<script type=\"text/javascript\">");
            script.Append(RenderJavascript());
            //script.Append("</script>");

            script.Replace("##gridid##", _id);

            return script.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToHtmlString()
        {
            return ToString();
        }
    }
}