using System;

namespace Smart.Web.Mvc.UI.JqGrid
{
    public enum Align
    {
        Center,
        Left,
        Right
    }

    public enum DataType
    {
        Json,
        JsonString,
        Xml,
        XmlString,
        Local,
        Javascript,
        Function,
        ClientSide
    }

    public enum Direction
    {
        Vertical,
        Horizontal
    }

    public enum EditType
    {
        Text,
        TextArea,
        Select,
        Checkbox,
        Password,
        Button,
        Image,
        File,
        Custom
    }

    public enum Formatters
    {
        Integer,
        Number,
        Currency,
        Date,
        Email,
        Link,
        Showlink,
        Checkbox,
        Select,
        Custom
    }

    public enum GridState
    {
        Visible,
        Hidden
    }

    public enum LoadUi
    {
        Enable,
        Disable,
        Block
    }

    public enum MultiKey
    {
        None,
        AltKey,
        CtrlKey,
        ShiftKey
    }

    public enum PagerPos
    {
        Center,
        Left,
        Right
    }

    public enum RecordPos
    {
        Center,
        Left,
        Right
    }

    public enum RequestType
    {
        Get,
        Post
    }

    [Flags]
    public enum SearchOptions
    {
        None = 0,
        Equal = 1,
        NotEqual = 2,
        Less = 4,
        LessOrEqual = 8,
        Greater = 16,
        GreaterOrEqual = 32,
        BeginsWith = 64,
        DoesNotBeginWith = 128,
        IsIn = 256,
        IsNotIn = 1024,
        EndsWith = 2048,
        DoesNotEndWith = 4096,
        Contains = 8192,
        DoesNotContain = 16384
    }

    public enum Searchtype
    {
        Text,
        Select,
        Datepicker
    }

    public enum SortOrder
    {
        Asc,
        Desc
    }

    public enum SortType
    {
        Integer,
        Float,
        Currency,
        Number,
        Date,
        Text
    }

    public enum SubmitType
    {
        Remote,
        ClientArray
    }

    public enum ToolbarPosition
    {
        Top,
        Bottom,
        Both
    }

    public enum TreeGridModel
    {
        /// <summary>
        /// 嵌套模式
        /// </summary>
        Nested,
        /// <summary>
        /// 相邻模式
        /// </summary>
        Adjacency
    }

}
