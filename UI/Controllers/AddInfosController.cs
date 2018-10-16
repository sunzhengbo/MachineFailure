using BLL;
using System;
using System.Web.Mvc;
using Tools;

namespace UI.Controllers
{
    public class AddInfosController : Controller
    {
        private AddInfosBLL addInfosBLL = new AddInfosBLL();
        /// <summary>
        /// 跳转添加信息首页
        /// </summary>
        /// <returns></returns>
        public ActionResult AddInformations()
        {
            return View(addInfosBLL.GetCustomersByDAL());
        }

        /// <summary>
        /// 获取合同主体列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetCustomer()
        {
            return Utils.ConvertListToJson(addInfosBLL.GetCustomersByDAL());
        }


        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSite(Guid id)
        {
            return Json(Utils.ConvertListToJson(addInfosBLL.GetSite(id)));
        }
        
        /// <summary>
        /// 获取机组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMachine(Guid id)
        {
            return Json(Utils.ConvertListToJson(addInfosBLL.GetMachine(id)));
        }

        /// <summary>
        /// 获取机组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetFaultType(Guid id)
        {
            return Json(Utils.ConvertListToJson(addInfosBLL.GetFaultType(id)));
        }


        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="site"></param>
        /// <param name="macs"></param>
        /// <param name="sens"></param>
        /// <param name="date"></param>
        /// <param name="pictures"></param>
        /// <param name="conclusion"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddRecord(string customer,string site,string macs,string sens,string date, string[] pictures, string conclusion)
        {
           return addInfosBLL.AddRecordBLL(customer, site, macs, sens, date, pictures, conclusion);
        }

        /// <summary>
        /// 添加大区
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddCustomer(string name)
        {
            return addInfosBLL.AddRegionalBLL(name);
        }

        /// <summary>
        /// 添加站点
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddSite(string customerID, string name)
        {
            return addInfosBLL.AddSiteBLL(Guid.Parse(customerID), name.Trim());
        }

        /// <summary>
        /// 添加机组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddMachine(string siteID,string customerID, string name)
        {
            return addInfosBLL.AddMachineBLL(Guid.Parse(siteID), Guid.Parse(customerID), name.Trim());
        }

        /// <summary>
        /// 添加故障类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddFaultType(string macID,string siteID, string customerID, string name)
        {
            return addInfosBLL.AddFaultTypeBLL(Guid.Parse(macID), Guid.Parse(siteID), Guid.Parse(customerID), name);
        }
    }
}