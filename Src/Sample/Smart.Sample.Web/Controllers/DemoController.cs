using System.Collections;
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

        public ActionResult Chosen()
        {
            var sexs = new List<dynamic>();
            sexs.Add(new { Id = 1, Name = "男" });
            sexs.Add(new { Id = 2, Name = "女" });
            sexs.Add(new { Id = 9, Name = "未知" });
            ViewBag.Sexs = sexs;
            return View();
        }
        public ActionResult Select2() { return View(); }
        public ActionResult MultiSelect() { return View(); }

        //下拉树控件
        public ActionResult ComboTree() { return View(); }

        public ActionResult ComboGrid() { return View(); }


        public ActionResult ImageInput() { return View(); }
    }
}