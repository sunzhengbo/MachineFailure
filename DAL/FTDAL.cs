using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Entity;

namespace DAL
{
    public class FTDAL
    {
        /// <summary>
        /// 按故障类型名称分组获取故障类型
        /// </summary>
        /// <returns故障类型的集合</returns>
        public List<string> ReadFaultTypes()
        {
            List<string> senNames = new List<string>();
            using (var db = new MachineFailureEntities())
            {
                var sensorQuery = from sensor in db.FaultTypes
                                  group sensor by sensor.Name;

                foreach (var sensor in sensorQuery)
                {
                    senNames.Add(sensor.Key);
                }
            }
            return senNames;
        }

        /// <summary>
        /// 按机组id获取故障类型
        /// </summary>
        /// <returns故障类型的集合</returns>
        public List<StructuralInformation> ReadFaultTypes(Guid MachineID)
        {
            using (var db = new MachineFailureEntities())
            {
                var fts = db.FaultTypes;
                if(fts != null)
                {
                    return fts.ToList()
                        .Where(e => e.MachineID.Equals(MachineID))
                        .Select(e => new StructuralInformation() { ID = e.FaultTypeID, Name = e.Name })
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
