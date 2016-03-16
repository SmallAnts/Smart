namespace Smart.Web.Mvc.UI.JqGrid.DataReaders
{
    public class JsonReader
    {
        public JsonReader()
        {
            RepeatItems = false;
        }

        public bool RepeatItems { get; set; }
        public string Id { get; set; }

        // root: "rows", 
        // page: "page", 
        // total: "total", 
        // records: "records", 
        // repeatitems: true, 
        // cell: "cell", 
        // id: "id",
        // userdata: "userdata",
        // subgrid: { 
        //    root:"rows", 
        //    repeatitems: true, 
        //    cell:"cell" 
        // }
    }
}
