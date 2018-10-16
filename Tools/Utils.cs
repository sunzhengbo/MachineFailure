using Model.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Tools
{
    public class Utils
    {
        /// <summary>
        /// 拼接目录
        /// </summary>
        /// <param name="banaryBase"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string StitchingPath(string banaryBase, string date)
        {
            if (date[date.IndexOf("-") + 1] == '0')  //将2018-08转化成2018-8
            {
                date = date.Remove(date.IndexOf("-") + 1, 1);
            }
            if (date.Contains("-"))  //将2018-8转化成2018/8
            {
                date = date.Replace("-", "/");
            }

            //获取到图片的绝对路径
            return banaryBase.Replace(@"\", "/") + "/" + date + "/";
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DeleteDirecetor(string path)
        {
            try
            {
                for (int i = 1; i <= 2; i++)
                {
                    path = path.Substring(0, path.LastIndexOf("/"));
                    if (i == 1)  //删除最外一层目录
                    {
                        if (Directory.GetFiles(path).Length == 0)
                        {
                            Directory.Delete(path);
                        }
                        else  //如果最外层目录有文件就跳出循环
                        {
                            break;
                        }
                    }
                    else  //删除倒数第二层目录
                    {
                        if (Directory.GetDirectories(path).Length == 0)
                        {
                            Directory.Delete(path);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
            return true;
        }


        /// <summary>
        /// 把list集合转化成json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ConvertListToJson<T>(List<T> list)
        {
            string userJSON = "";
            if (list != null)
            {
                //创建json对象
                DataContractJsonSerializer json = new DataContractJsonSerializer(list.GetType());
                //序列化
                using (MemoryStream ms = new MemoryStream())
                {
                    json.WriteObject(ms, list);
                    userJSON = Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            return userJSON;
        }



        /// <summary>
        /// 拼接图片地址,读取图片，并转化成Base64String
        /// </summary>
        /// <returns>返回图片的地址</returns>
        public static List<PictureBase64String> PictureBase64ByTools(string path,List<Fault> fInfos)
        {
            List<PictureBase64String> p64 = new List<PictureBase64String>();
            string picture64 = null;
            foreach (var f in fInfos)
            {
                //拼接目录
                string binarybasePath = Utils.StitchingPath(path, f.Timestamp) + f.PictureID;
                //开始读取图片
                try
                {
                    using (FileStream fsr = new FileStream(binarybasePath, FileMode.Open))
                    {
                        byte[] buffer = new byte[fsr.Length]; //一次性读取整个流的长度
                        fsr.Read(buffer, 0, buffer.Length);
                        if (buffer != null)
                        {
                            picture64 = "data:image/png;base64," + Convert.ToBase64String(buffer);
                            p64.Add(new PictureBase64String() { Picture = picture64, PictureID = f.PictureID });
                        }
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    Debug.WriteLine("DirectoryNotFoundException");
                    p64.Add(new PictureBase64String() { Picture = "noFile", PictureID = f.PictureID });  //表示文件没有找到
                }
                catch (FileNotFoundException)
                {
                    Debug.WriteLine("FileNotFoundException");
                    p64.Add(new PictureBase64String() { Picture = "noFile", PictureID = f.PictureID });  //表示文件没有找到
                }
            }
            if (fInfos == null || fInfos.Count == 0)  //表示没有记录
            {
                p64.Add(new PictureBase64String() { Picture = "noRecord" });
            }
            return p64;
        }


        /// <summary>
        /// 将图片Base64String转化成集合
        /// </summary>
        /// <returns>返回故障的记录</returns>
        public static List<FaultInfo> ConvertBase64StringToList(List<PictureBase64String> p64, List<Fault> faultRecords)
        {
            List<FaultInfo> list = new List<FaultInfo>();
            FaultInfo info = new FaultInfo();
            List<string> listPics = null;

            //join图片和测点信息
            var querys = from p in p64
                         join f in faultRecords on p.PictureID equals f.PictureID
                         select new
                         {
                             Picture = p.Picture,
                             RecordID = f.RecordID,
                             FaultTypeName = f.FaultTypeName,
                             FaultInfo = f.FaultInfo
                         };
            //合并每条记录的信息
            var keys = from query in querys
                       group query by query.RecordID;
            var ks = keys.Count();
            foreach (var key in keys)
            {
                int index = 0;
                listPics = new List<string>();
                foreach (var k in key)
                {
                    if (index++ == 0)
                    {
                        info.FaultTypeName = k.FaultTypeName;
                        info.FaultInfos = k.FaultInfo;
                    }
                    listPics.Add(k.Picture);
                }
                list.Add(new FaultInfo()
                {
                    listPic = listPics,
                    FaultTypeName = info.FaultTypeName,
                    FaultInfos = info.FaultInfos
                });
            }
            return list;
        }
    }
}
