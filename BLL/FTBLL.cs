using DAL;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class FTBLL
    {
        FTDAL fTDAL = new FTDAL();

        /// <summary>
        /// 获取故障类型
        /// </summary>
        /// <returns>返回故障类型</returns>
        public List<string> GetFaultTypes()
        {
            return fTDAL.ReadFaultTypes();
        }

        /// <summary>
        /// 根据机组id获取故障类型
        /// </summary>
        /// <param name="MachineID"></param>
        /// <returns></returns>
        public List<StructuralInformation> GetFaultTypes(Guid MachineID)
        {
            return fTDAL.ReadFaultTypes(MachineID);
        }
    }
}
