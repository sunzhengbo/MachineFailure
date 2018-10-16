using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tools;

namespace UI.Controllers
{
    public class FaultTypeController : Controller
    {
        FTBLL fTBLL = new FTBLL();
        /// <summary>
        /// home page controller
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetFaultTypes()
        {
            return Utils.ConvertListToJson(fTBLL.GetFaultTypes());
        }

        [HttpPost]
        public string AjaxGetFaultTypes(string ID)
        {
            Guid id = Guid.Empty;
            Guid.TryParse(ID, out id);
            return Utils.ConvertListToJson(fTBLL.GetFaultTypes(id));
        }
    }
}