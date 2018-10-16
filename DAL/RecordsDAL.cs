using Model;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class RecordsDAL
    {
        /// <summary>
        /// 获取记录列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="searchText"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<ViewRecords> GetRecords(Guid id,string searchText,string[] time)
        {
            using(var db = new MachineFailureEntities())
            {
                string sql = string.Format(@"select rec.[RecordID] Id,rec.[Timestamp] from [Records] rec 
                                            left join[FaultTypes] ft on ft.[FaultTypeID] = rec.[FaultTypeID]
                                            left join[Machines] mac on mac.[MachineID] = ft.[MachineID]
                                            where mac.[MachineID] = '{0}' and ft.[Name] = N'{1}' and [Timestamp] between '{2}' and '{3}'
                                            order by [rec].[Timestamp] desc", id, searchText, time[0],time[1]);
                var records = db.Database.SqlQuery<ViewRecords>(sql);
                int count = records.Count();
                if (count > 0)
                {
                    return records.ToList();
                }
                else
                {
                    return new List<ViewRecords>();
                }
            }
        }

        public List<Fault> GetRecordsFault(Guid id)
        {
            using (var db = new MachineFailureEntities())
            {
                string sql = string.Format(@"select pic.[PictureID],rec.[RecordID],rec.[Conclusion] FaultInfo,rec.[Timestamp] from [Pictures] pic
                                            left join [Records] rec on rec.[RecordID] = pic.[RecordID]
                                            where rec.[RecordID] = '{0}'", id);
                var records = db.Database.SqlQuery<Fault>(sql);
                int count = records.Count();
                if (count > 0)
                {
                    return records.ToList();
                }
                else
                {
                    return new List<Fault>();
                }
            }
        }
    }
}
