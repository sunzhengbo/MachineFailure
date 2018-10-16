using BLL;
using System;
using System.Web.Mvc;
using Tools;

namespace UI.Controllers
{
    public class MachinesController : Controller
    {
        private MachinesBLL machinesBLL = new MachinesBLL();
        
        /// <summary>
        /// 分页展示机组页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="searchText"></param>
        /// <param name="siteName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowMachines(string id,string searchText,string siteName)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(id, out result);
            int totalPage;
            var macs = machinesBLL.LimitPageGetMachiesByDAL(result, searchText,1 , out totalPage);
            ViewBag.totalPage = totalPage == 0 ? 1 : totalPage;
            ViewBag.currentPage = 1;
            ViewBag.searchText = searchText;
            ViewBag.id = id;
            ViewBag.siteName = siteName;
            return View(macs);
        }

        /// <summary>
        /// 分页显示机组
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public string MachinesLimitPage(string id,string searchText, int currentPage)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(id, out result);
            int totalPage;
            var macs = machinesBLL.LimitPageGetMachiesByDAL(result, searchText, currentPage, out totalPage);
            ViewBag.totalPage = totalPage == 0 ? currentPage : totalPage;
            ViewBag.currentPage = currentPage;
            ViewBag.searchText = searchText;
            return Utils.ConvertListToJson(macs);
        }


        /// <summary>
        /// 加载机组页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="searchText"></param>
        /// <param name="siteName"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetMachines(string id, string searchText)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(id, out result);
            var macs = machinesBLL.GetMachinesByDAL(result, searchText);
            return Utils.ConvertListToJson(macs);
        }

        [HttpPost]
        public string AjaxLoadMachines(string ID)
        {
            Guid id = Guid.Empty;
            Guid.TryParse(ID, out id);
            return Utils.ConvertListToJson(machinesBLL.AjaxGetMachinesByDAL(id));
        }
    }
}