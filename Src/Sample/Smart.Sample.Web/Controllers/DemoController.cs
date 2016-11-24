using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Smart.Sample.Web.Controllers
{
    public class DemoController : ControllerBase
    {
        #region zTree

        public ActionResult zTree()
        {
            return View();
        }
        [HttpPost]
        public JsonResult getZTreeNodes()
        {
            var ztree = new Smart.Web.Mvc.UI.zTree();
            //传统做法：根据自己的递归算法创建 Nodes
            return Json(ztree.Nodes);
        }

        // 异步一次加载所有节点
        [HttpPost]
        public JsonResult getZTree2Nodes()
        {
            var service = this.GetService<Core.IServices.IUserService>();
            var ztree = new Smart.Web.Mvc.UI.zTree();
            ztree.DataSource = service.GetUserFuncs(Operator.SysUserId);
            ztree.IdField = "SysFuncId";
            ztree.NameField = "Name";
            ztree.ParentIdField = "ParentId";
            ztree.RootId = "";
            ztree.NodeDataBound += (s, e) =>
            {
                e.Node.Open = true;
            };
            ztree.DataBind();
            return Json(ztree.Nodes);
        }

        // 异步加载节点数据
        [HttpPost]
        public JsonResult getZTree3Nodes()
        {
            var parentId = Request["id"] ?? "";
            var ztree = new Smart.Web.Mvc.UI.zTree();
            ztree.DataSource = GetData(parentId);
            ztree.IdField = "SysFuncId";
            ztree.NameField = "Name";
            ztree.NodeDataBound += (s, e) =>
            {
                if (e.Node.Id == "000000" || Request["lv"] == "0") e.Node.IsParent = false;
            };
            ztree.DataBind();
            return Json(ztree.Nodes);
        }
        private List<Core.Entites.SysFunc> GetData(string parentId)
        {
            var service = this.GetService<Core.IServices.IUserService>();
            var data = service.GetUserFuncs(Operator.SysUserId);
            return data.Where(d => d.ParentId == parentId).ToList();
        }

        #endregion

        #region 下拉控件
        public ActionResult Combo()
        {
            return View();
        }
        #endregion
    }
}