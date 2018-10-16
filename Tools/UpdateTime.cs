using Model;
using System;
using System.Linq;

namespace Tools
{
    public class UpdateTime
    {
        /// <summary>
        /// 更新故障类型的时间
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <param name="inserted"></param>
        public static void UpdateFaultTypes(MachineFailureEntities db, Guid id ,DateTime inserted)
        {
            var ft = db.FaultTypes.FirstOrDefault(e => id.Equals(e.FaultTypeID));
            if (ft != null)
            {
                ft.Updated = inserted;
            }
        }
        /// <summary>
        /// 更新机组时间
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <param name="inserted"></param>
        public static void UpdateMachines(MachineFailureEntities db, Guid id, DateTime inserted)
        {
            var mac = db.Machines.FirstOrDefault(e => id.Equals(e.MachineID));
            if (mac != null)
            {
                mac.Updated = inserted;
            }
        }
        /// <summary>
        /// 更新站点时间
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <param name="inserted"></param>
        public static void UpdateSites(MachineFailureEntities db, Guid id, DateTime inserted)
        {
            var sit = db.Sites.FirstOrDefault(e => id.Equals(e.SiteID));
            if (sit != null)
            {
                sit.Updated = inserted;
            }
        }
        /// <summary>
        /// 更新大区的时间
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <param name="inserted"></param>
        public static void UpdateRegionals(MachineFailureEntities db, Guid id, DateTime inserted)
        {
            var reg = db.Customers.FirstOrDefault(e => id.Equals(e.CustomerID));
            if (reg != null)
            {
                reg.Updated = inserted;
            }
        }
    }
}
