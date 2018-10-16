using DAL;
using Model.Entity;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class SitesBLL
    {
        SitesDAL sitesDAL = new SitesDAL();

        public List<ViewInfo> LimitPageGetSiteByDAL(string name,int currentPage, out int totalPage)
        {
            return sitesDAL.LimitPageGetSites(name, currentPage, out totalPage);
        }

        public List<ViewInfo> GetSiteByDAL(string name)
        {
            return sitesDAL.GetSites(name);
        }

        public List<StructuralInformation> AjaxGetSiteByDAL(Guid customerID)
        {
            return sitesDAL.AjaxGetSites(customerID);
        }
    }
}
