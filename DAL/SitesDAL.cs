using Model;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools;

namespace DAL
{
    public class SitesDAL
    {
        /// <summary>
        /// 获取站点
        /// </summary>
        /// <param name="name">故障类型</param>
        /// <param index="index">页码</param>
        /// <param totalRecord="totalRecord">总的记录数</param>
        /// <returns></returns>
        public List<ViewInfo> LimitPageGetSites(string name,int currentPage, out int totalPage)
        {
            var sites = GetSites(name);
            int totalRecord = sites.Count();
            if (totalRecord != 0)
            {
                //获取所有页
                totalPage = (int)Math.Ceiling(Convert.ToDecimal(totalRecord) / 30);
                sites = sites.Skip((currentPage - 1) * 30).Take(30).ToList();
            }
            else
            {
                totalPage = 0;
            }
            return sites;
        }


        public List<ViewInfo> GetSites(string name)
        {
            using (var db = new MachineFailureEntities())
            {
                string sql = string.Format(@"select last_site.[SiteID] Id, last_site.[Name],Convert(nvarchar(19),new_site.[maxTime],20) Updated from 
								(select MAX(site.[Updated]) maxTime from
									(
										select sit.* from [FaultTypes] ft
										left join Machines mac on ft.[MachineID] = mac.MachineID
										left join [Sites] sit on mac.SiteID = sit.SiteID
										where ft.Name = '{0}'
									) site 
									group by site.[Name]
								) new_site
								left join [Sites] last_site on last_site.[Updated] = new_site.[maxTime]", name);
                var sites = db.Database.SqlQuery<ViewInfo>(sql);
                int totalRecord = sites.Count();
                if (totalRecord != 0)
                {
                    //获取所有页
                    return sites.ToList();
                }
                else
                {
                    return new List<ViewInfo>();
                }
            }
        }

        /// <summary>
        /// ajax读取数据
        /// </summary>
        /// <param name="coutomerID"></param>
        /// <returns></returns>
        public List<StructuralInformation> AjaxGetSites(Guid coutomerID)
        {
            using(var db = new MachineFailureEntities())
            {
                var dbSetSites = db.Sites;
                if(dbSetSites != null)
                {
                    var sites = dbSetSites.ToList()
                        .Where(e => e.CustomerID.Equals(coutomerID))
                        .Select(e => new StructuralInformation() { ID = e.SiteID, Name = e.Name })
                        .OrderBy(e=>e.Name)
                        .ToList();
                    return sites;
                }
                else
                {
                    return new List<StructuralInformation>();
                }
            }
        }
    }
}
