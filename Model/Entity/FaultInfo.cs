using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity
{
    public struct FaultInfo
    {
        public List<string> listPic { get; set; }

        public string FaultTypeName { get; set; }

        public string FaultInfos { get; set; }
    }
}
