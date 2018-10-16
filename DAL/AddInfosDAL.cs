using Model;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tools;

namespace DAL
{
    public class AddInfosDAL
    {
        /// <summary>
        /// 读取数据库中的信息
        /// </summary>
        /// <param name="sql">查出的sql语句</param>
        /// <param name="id">查询的条件</param>
        /// <returns></returns>
        public List<StructuralInformation> GetStruckInfos(string sql)
        {
            List<StructuralInformation> list = null;
            using (var db = new MachineFailureEntities())
            {
                var results = db.Database.SqlQuery<StructuralInformation>(sql);
                if (results.Count() != 0)
                {
                    list = results.OrderBy(e => e.Name).ToList();
                }
            }
            return list;
        }


        /// <summary>
        /// 获取大区列表
        /// </summary>
        /// <returns></returns>
        public List<StructuralInformation> GetCustomers()
        {
            string sql = @"select CustomerID ID,Name from [Customers]";
            return GetStruckInfos(sql);
        }

        /// <summary>
        /// 获取站点列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<StructuralInformation> GetSites(Guid id)
        {
            string sql = string.Format(@"select SiteID ID,Name from [Sites] where CustomerID = '{0}'", id);
            return GetStruckInfos(sql);
        }

        /// <summary>
        /// 获取机组列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<StructuralInformation> GetMachines(Guid id)
        {
            string sql = string.Format(@"select MachineID ID,Name from [Machines] where SiteID = '{0}'", id);
            return GetStruckInfos(sql);
        }

        /// <summary>
        /// 获取故障列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<StructuralInformation> GetFaultTypes(Guid id)
        {
            string sql = string.Format(@"select FaultTypeID ID,Name from [FaultTypes] where MachineID = '{0}'", id);
            return GetStruckInfos(sql);
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="regs"></param>
        /// <param name="site"></param>
        /// <param name="macs"></param>
        /// <param name="sens"></param>
        /// <param name="date"></param>
        /// <param name="conclusion"></param>
        /// <param name="inserted"></param>
        /// <returns></returns>
        public string AddRecordDAL(string regs, string site, string macs, string sens, string date, string conclusion, List<Guid> gList)
        {
            DateTime inserted = DateTime.Now;  //获取当前系统的时间
            List<Pictures> picList = new List<Pictures>();  //存放图片信息

            using (var db = new MachineFailureEntities())
            {
                using (var tran = db.Database.BeginTransaction()) //开启事物
                {
                    try
                    {
                        //查询记录
                        var record = db.Records.FirstOrDefault(e => sens.Equals(e.FaultTypeID.ToString()) && date.Equals(e.Timestamp) && conclusion.Equals(e.Conclusion));
                        if (record != null)  //判断插入的记录是否已经存在
                        {
                            return "exsit";
                        }
                        //插入记录
                        var recordID = Guid.NewGuid();  //生成记录的ID
                        db.Records.Add(new Records() {
                            RecordID = recordID,
                            FaultTypeID = Guid.Parse(sens),
                            Conclusion = conclusion,
                            Timestamp = date,
                            Inserted = inserted
                        });

                        //插入图片
                        foreach(var g in gList)
                        {
                            picList.Add(new Pictures() { PictureID = g, RecordID = recordID });
                        }
                        db.Pictures.AddRange(picList);

                        //更新故障类型的时间
                        UpdateTime.UpdateFaultTypes(db, Guid.Parse(sens), inserted);
                        //更新机组时间
                        UpdateTime.UpdateMachines(db, Guid.Parse(macs), inserted);
                        //更新站点时间
                        UpdateTime.UpdateSites(db, Guid.Parse(site), inserted);
                        //更新大区的时间
                        UpdateTime.UpdateRegionals(db, Guid.Parse(regs), inserted);

                        db.SaveChanges();
                        tran.Commit();
                    }
                    catch(Exception e)
                    {
                        tran.Rollback();
                        Debug.WriteLine(e);
                        return "false";
                    }
                }
            }
            return "true";
        }


        /// <summary>
        /// 添加大区
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string AddRegionalDAL(string name)
        {
            using (var db = new MachineFailureEntities())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        //插入之前先查询是否存在
                        var reg = db.Customers.FirstOrDefault(e=>name.Equals(e.Name));
                        if(reg != null)
                        {
                            return "This name already exist";
                        }
                        //开始插入
                        db.Customers.Add(new Customers() { CustomerID = Guid.NewGuid(), Name = name, Updated = DateTime.Now });
                        db.SaveChanges();
                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        return "Add failed";
                    }
                }
            }
            return "Add success";  
        }

        /// <summary>
        /// 插入站点
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="addr"></param>
        /// <returns></returns>
        public string AddSiteDAL(Guid regsID,string name)
        {
            using (var db = new MachineFailureEntities())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        //插入之前先查询是否存在
                        var site = db.Sites.FirstOrDefault(e => name.Equals(e.Name) && regsID.Equals(e.CustomerID));
                        if (site != null)
                        {
                            return "This name already exsit";
                        }
                        //开始插入
                        var date = DateTime.Now;
                        db.Sites.Add(new Sites() { SiteID = Guid.NewGuid(),Name = name, CustomerID = regsID, Updated = date });
                        UpdateTime.UpdateRegionals(db, regsID, date);
                        db.SaveChanges();
                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        return "Add filded";
                    }
                }
            }
            return "Add success";
        }

        /// <summary>
        /// 插入机组
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="addr"></param>
        /// <returns></returns>
        public string AddMachineDAL(Guid siteID,Guid regsID, string name)
        {
            using (var db = new MachineFailureEntities())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        //插入之前先查询是否存在
                        var mac = db.Machines.FirstOrDefault(e => name.Equals(e.Name) && siteID.Equals(e.SiteID));
                        if (mac != null)
                        {
                            return "This name already exsit";
                        }
                        //开始插入
                        var date = DateTime.Now;
                        db.Machines.Add(new Machines() { MachineID = Guid.NewGuid(), Name = name, SiteID = siteID, Updated = date });
                        UpdateTime.UpdateSites(db, siteID, date);
                        UpdateTime.UpdateRegionals(db, regsID, date);
                        db.SaveChanges();
                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        return "Add failed";
                    }
                }
            }
            return "Add success";
        }

        /// <summary>
        /// 插入故障类型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="addr"></param>
        /// <returns></returns>
        public string AddFaultTypeDAL(Guid macID, Guid siteID, Guid regsID,string name)
        {
            using (var db = new MachineFailureEntities())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        //插入之前先查询是否存在
                        var ft = db.FaultTypes.FirstOrDefault(e => name.Equals(e.Name) && macID.Equals(e.MachineID));
                        if (ft != null)
                        {
                            return "This name already exsit";
                        }
                        //开始插入
                        var date = DateTime.Now;
                        db.FaultTypes.Add(new FaultTypes() { FaultTypeID = Guid.NewGuid(),Name = name,MachineID = macID,Updated = date});
                        UpdateTime.UpdateMachines(db, macID, date);
                        UpdateTime.UpdateSites(db, siteID, date);
                        UpdateTime.UpdateRegionals(db, regsID, date);
                        db.SaveChanges();
                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        return "Add failed";
                    }
                }
            }
            return "Add success";
        }
    }
}
