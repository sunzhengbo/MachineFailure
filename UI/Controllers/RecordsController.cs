using BLL;
using System;
using System.Diagnostics;
using System.Web.Mvc;
using Tools;

namespace UI.Controllers
{
    public class RecordsController : Controller
    {
        private RecordsBLL recordsBLL = new RecordsBLL();

        [HttpGet]
        public ActionResult ShowRecords(string id, string searchText,string siteName,string machineName,string siteID)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(id, out result);
            DateTime currentDate = DateTime.Now;
            ViewBag.id = id;
            ViewBag.searchText = searchText;
            ViewBag.siteName = siteName;
            ViewBag.machineName = machineName;
            ViewBag.siteID = siteID;
            string[] times = new string[] { currentDate.AddYears(-1).ToString("yyyy-MM"), currentDate.ToString("yyyy-MM") };
            return View(recordsBLL.GetRecordsByDAL(result, searchText, times));
        }


        [HttpPost]
        public string GetRecordsByBLL(string id, string searchText, string time)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(id, out result);
            string[] times = time.Replace(" ~ ","~").Split('~');
            ViewBag.searchText = searchText;
            var recs = recordsBLL.GetRecordsByDAL(result, searchText, times);
            return Utils.ConvertListToJson(recs);
        }

        [HttpPost]
        public string GetRecordsDetial(string id)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(id, out result);
            return Utils.ConvertListToJson(recordsBLL.GetRecordsFaultByDAL(result));
        }
    }
}