using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web.Mvc;
using System.Text;
using System.IO;
using Smart.Core.Extensions;

namespace Smart.Web.Mvc.UI.JqGrid
{
    [ModelBinder(typeof(GridModelBinder))]
    public class BindArgs
    {
        public bool IsSearch { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public Filter Where { get; set; }
    }

    [DataContract]
    public class Filter
    {
        [DataMember]
        public string groupOp { get; set; }
        [DataMember]
        public Rule[] rules { get; set; }

        public static Filter Create(string jsonData)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(Filter));
                //var ms = new System.IO.MemoryStream(Encoding.Default.GetBytes(jsonData));
                var ms = new MemoryStream(
                    Encoding.Convert(
                        Encoding.Default,
                        Encoding.UTF8,
                        Encoding.Default.GetBytes(jsonData)));
                return serializer.ReadObject(ms) as Filter;
            }
            catch
            {
                return null;
            }
        }
    }

    [DataContract]
    public class Rule
    {
        [DataMember]
        public string field { get; set; }
        [DataMember]
        public string op { get; set; }
        [DataMember]
        public string data { get; set; }
    }

    public class GridModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                var request = controllerContext.HttpContext.Request;
                return new BindArgs
                {
                    IsSearch = request["_search"].AsBool(),
                    PageIndex = request["page"].AsInt(1).Value,
                    PageSize = request["rows"].AsInt(10).Value,
                    SortColumn = request["sidx"] ?? "",
                    SortOrder = request["sord"] ?? "asc",
                    Where = Filter.Create(request["filters"] ?? "")
                };
            }
            catch
            {
                return null;
            }
        }
    }
}