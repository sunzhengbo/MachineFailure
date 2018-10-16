using DAL;
using Model.Entity;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class MachinesBLL
    {
        private MachinesDAL machinesDAL = new MachinesDAL();

        public List<ViewInfo> GetMachinesByDAL(Guid id, string searchText)
        {
            return machinesDAL.GetMachies(id, searchText);
        }

        public List<ViewInfo> LimitPageGetMachiesByDAL(Guid id, string searchText, int currentPage, out int totalPage)
        {
            return machinesDAL.LimitPageGetMachies(id, searchText, currentPage, out totalPage); 
        }

        public List<StructuralInformation> AjaxGetMachinesByDAL(Guid id)
        {
            return machinesDAL.AjaxGetMachines(id);
        }
    }
}
