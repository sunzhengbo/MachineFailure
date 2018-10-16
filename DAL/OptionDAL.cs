using System.Linq;
using Model;

namespace DAL
{
    public partial class OptionDAL
    {
        /// <summary>
        /// 获取binarybase的根目录
        /// </summary>
        /// <returns></returns>
        public Option FilePathDAL()
        {
            using (var db = new MachineFailureEntities())
            {
                return db.Option.FirstOrDefault(elem => elem.ID.Equals("BinaryBase"));
            }
        }
    }
}
