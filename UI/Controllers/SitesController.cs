using BLL;
using System;
using System.Web.Mvc;
using Tools;

namespace UI.Controllers
{
    public class SitesController : Controller
    {
        SitesBLL sitesBLL = new SitesBLL();
        /// <summary>
        /// 分页显示站点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ShowSites(string searchText)
        {
            int totalPage;
            ViewBag.searchText = searchText;
            var sites = sitesBLL.LimitPageGetSiteByDAL(searchText, 1, out totalPage);
            ViewBag.totalPage = totalPage == 0 ? 1 : totalPage;
            ViewBag.currentPage = 1;
            return View(sites);
        }

        /// <summary>
        /// 分页显示站点
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        [HttpPost]
        public string SitesLimitPage(string searchText,int currentPage)
        {
            int totalPage;
            var sites = sitesBLL.LimitPageGetSiteByDAL(searchText, currentPage, out totalPage);
            ViewBag.totalPage = totalPage == 0 ? currentPage : totalPage;
            ViewBag.currentPage = currentPage;
            return Utils.ConvertListToJson(sites);
        }

        /// <summary>
        /// 根据故障类型名称查询站点信息
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetSitesByBLL(string searchText)
        {
            return Utils.ConvertListToJson(sitesBLL.GetSiteByDAL(searchText));
        }

        [HttpPost]
        public string AjaxLoadSites(string ID)
        {
            Guid id = Guid.Empty;
            Guid.TryParse(ID, out id);
            return Utils.ConvertListToJson(sitesBLL.AjaxGetSiteByDAL(id));
        }
    }
}