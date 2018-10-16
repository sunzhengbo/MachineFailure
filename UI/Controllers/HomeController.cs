using BLL;
using System.Web.Mvc;
using Tools;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private HomePageBLL homePageBLL = new HomePageBLL();
        
        /// <summary>
        /// home page controller
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.senNames = new FTBLL().GetFaultTypes();  //故障类型
            var pictureInfos = homePageBLL.HandleFailureInfo();
            return View(pictureInfos);
        }

        /// <summary>
        /// 获取合同主体
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetCustomersByBLL()
        {
            return Utils.ConvertListToJson(new AddInfosBLL().GetCustomersByDAL());
        }
    }
}