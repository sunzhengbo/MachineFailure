using Model;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class MachinesDAL
    {
        /// <summary>
        /// 获取机组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ViewInfo> GetMachies(Guid id,string searchText)
        {
            using (var db = new MachineFailureEntities())
            {
                string sql = string.Format(@"select mac.[MachineID] Id,mac.[Name],CONVERT(nvarchar(19),mac.[Updated],20) Updated from [FaultTypes] ft
                                            left join [Machines] mac on mac.[MachineID] = ft.[MachineID]
                                            where ft.[Name] = N'{0}' and mac.[SiteID] = '{1}'
                                            order by mac.[Name] asc", searchText,id);
                var macs = db.Database.SqlQuery<ViewInfo>(sql);

                int totalRecord = macs.Count();
                if (totalRecord != 0)
                {
                    return macs.ToList();
                }
                else
                {
                    return new List<ViewInfo>();
                }
            }
        }

        /// <summary>
        /// 分页获取机组
        /// </summary>
        /// <param name="id"></param>
        /// <param name="searchText"></param>
        /// <param name="currentPage"></param>
        /// <param name="totalPage"></param>
        /// <returns></returns>
        public List<ViewInfo> LimitPageGetMachies(Guid id, string searchText, int currentPage, out int totalPage)
        {
            var macs = GetMachies(id,searchText);

            int totalRecord = macs.Count();
            if (totalRecord != 0)
            {
                //获取所有页
                totalPage = (int)Math.Ceiling(Convert.ToDecimal(totalRecord) / 30);
                macs = macs.Skip((currentPage - 1) * 30).Take(30).ToList();
            }
            else
            {
                totalPage = 0;
                macs = new List<ViewInfo>();
            }
            return macs;
        }

        /// <summary>
        /// 根据站点id获取机组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<StructuralInformation> AjaxGetMachines(Guid id)
        {
            using (var db = new MachineFailureEntities())
            {
                var machines = db.Machines;
                if(machines != null)
                {
                    return machines.ToList()
                        .Where(e => e.SiteID.Equals(id))
                        .Select(e => new StructuralInformation() { ID = e.MachineID, Name = e.Name })
                        .OrderBy(e=>e.Name)
                        .ToList();
                }
                else
                {
                    return new List<StructuralInformation>();
                }
            }
        }
    }
}
