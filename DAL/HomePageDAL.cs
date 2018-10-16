using Model;
using Model.Entity;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class HomePageDAL
    {
        /// <summary>
        /// 获取每个测点下的最新一条记录
        /// </summary>
        /// <returns>返回每个测点下的最新一条记录的信息</returns>
        public List<Fault> ReadFailureInfo()
        {
            using (var db = new MachineFailureEntities())
            {

                string sql = @"select pic.[PictureID],rec.[RecordID],MaxTime.[Name] as FaultTypeName,rec.[Conclusion] as FaultInfo,rec.[Timestamp] from
                            (
	                            select MAX(rec.[Inserted]) lastInsered,ft.[Name] from [Records] rec
	                            left join [FaultTypes] ft on ft.[FaultTypeID] = rec.[FaultTypeID]
	                            group by ft.[Name]
                            ) MaxTime
                            left join [Records] rec on rec.[Inserted] = MaxTime.[lastInsered]
                            left join [Pictures] pic on pic.[RecordID] = rec.[RecordID]";

                return db.Database.SqlQuery<Fault>(sql).ToList();
            }
        }
    }
}
