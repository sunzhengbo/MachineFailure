using DAL;
using Model;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools;

namespace BLL
{
    public class RecordsBLL
    {
        private RecordsDAL recordsDAL = new RecordsDAL();
        private OptionBLL optionBLL = new OptionBLL();

        public List<ViewRecords> GetRecordsByDAL(Guid id, string searchText, string[] time)
        {
            return recordsDAL.GetRecords(id,searchText,time);
        }

        /// <summary>
        /// 合并字符串图片
        /// </summary>
        /// <returns>返回图片的地址</returns>
        public List<FaultInfo> GetRecordsFaultByDAL(Guid id)
        {
            List<PictureBase64String> p64 = new List<PictureBase64String>();
            List<FaultInfo> listFailure = new List<FaultInfo>();
            List<Fault> faultRecords = recordsDAL.GetRecordsFault(id);
            Option option = new OptionDAL().FilePathDAL();
            
            //拼接图片地址,读取图片，并转化成Base64String
            if (option == null)  //获取BinaryBase的根目录
            {
                p64.Add(new PictureBase64String() { Picture = "noPath" });  //表示binarybase路有问题
            }
            else
            {
                p64 = Utils.PictureBase64ByTools(option.Value, faultRecords);
            }

            //合并字符串图片
            if (p64[0].Picture == "noPath")
            {
                listFailure.Add(new FaultInfo() { FaultInfos = "noPath" });
            }
            else if (p64[0].Picture == "noRecord")
            {
                listFailure.Add(new FaultInfo() { FaultInfos = "noRecord" });
            }
            else
            {
                listFailure = Utils.ConvertBase64StringToList(p64, faultRecords);
            }
            return listFailure;
        }
    }
}
