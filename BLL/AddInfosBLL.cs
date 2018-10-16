using DAL;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Tools;

namespace BLL
{
    public class AddInfosBLL
    {
        private AddInfosDAL addInfosDAL = new AddInfosDAL();
        private OptionDAL optionDAL = new OptionDAL();
        

        /// <summary>
        /// 获取大区列表
        /// </summary>
        /// <returns></returns>
        public List<StructuralInformation> GetCustomersByDAL() {
            
            return addInfosDAL.GetCustomers();
        }

        /// <summary>
        /// 获取站点列表
        /// </summary>
        /// <returns></returns>
        public List<StructuralInformation> GetSite(Guid id)
        {
            return addInfosDAL.GetSites(id);
        }

        /// <summary>
        /// 获取机组列表
        /// </summary>
        /// <returns></returns>
        public List<StructuralInformation> GetMachine(Guid id)
        {
            return addInfosDAL.GetMachines(id);
        }

        /// <summary>
        /// 获取故障类型列表
        /// </summary>
        /// <returns></returns>
        public List<StructuralInformation> GetFaultType(Guid id)
        {
            return addInfosDAL.GetFaultTypes(id);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="regs"></param>
        /// <param name="site"></param>
        /// <param name="macs"></param>
        /// <param name="sens"></param>
        /// <param name="date"></param>
        /// <param name="pictures"></param>
        /// <param name="conclusion"></param>
        /// <returns></returns>
        public string AddRecordBLL(string regs,string site, string macs, string sens, string date, string[] pictures, string conclusion)
        {
            List<Guid> gList = new List<Guid>();  //存放生成的PictureID
            Guid id = Guid.Empty;
            
            try
            {
                string binarybasePath = optionDAL.FilePathDAL().Value;  //获取目录
                foreach (var picture in pictures)
                {
                    id = Guid.NewGuid();
                    string director = Utils.StitchingPath(binarybasePath, date);
                    if (!Directory.Exists(director))  //判断目录是否存在
                    {
                        Directory.CreateDirectory(director);  //不存在就创建目录
                    }
                    //将base64String图片转化成图片存入本地
                    File.WriteAllBytes(director + id, Convert.FromBase64String(picture.Substring(picture.IndexOf(",") + 1)));
                    gList.Add(id);  //遍历生成图片的ID
                }

                //调用插入的方法
                string flag = addInfosDAL.AddRecordDAL(regs,site, macs, sens, date, conclusion, gList);

                if ("true".Equals(flag))
                {
                    return "add success";
                }
                else if("exsit".Equals(flag))
                {
                    return "Record already exsit";
                }
                else
                {
                    foreach (var g in gList)
                    {
                        string file = Utils.StitchingPath(binarybasePath, date) + g;
                        if (File.Exists(file))  //判断文件是否存在
                        {
                            File.Delete(file);  //存在就删除
                            //判断目录是否为空，为空则删除目录
                            Utils.DeleteDirecetor(file);
                        }
                    }
                }
            }
            catch(FormatException e)
            {
                Debug.WriteLine(e);
                return "Not effective base64";
            }
            catch(NullReferenceException e)
            {
                Debug.WriteLine(e);
                return "No binarybase path";
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return "An unknown error";
            }
            return "Add failed";
        }

        /// <summary>
        /// 添加大区
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string AddRegionalBLL(string name)
        {
            return addInfosDAL.AddRegionalDAL(name);
        }

        /// <summary>
        /// 添加站点
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string AddSiteBLL(Guid regsID, string name)
        {
            return addInfosDAL.AddSiteDAL(regsID, name);
        }

        /// <summary>
        /// 添加机组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string AddMachineBLL(Guid siteID, Guid regsID, string name)
        {
            return addInfosDAL.AddMachineDAL(siteID,regsID,name);
        }


        /// <summary>
        /// 添加故障类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string AddFaultTypeBLL(Guid macID, Guid siteID, Guid regsID, string name)
        {
            return addInfosDAL.AddFaultTypeDAL(macID, siteID, regsID, name);
        }
    }
}
