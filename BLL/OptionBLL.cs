using System;
using System.Collections.Generic;
using DAL;
using Model.Entity;
using System.IO;
using Tools;
using System.Diagnostics;
using Model;

namespace BLL
{
    public class OptionBLL
    {
        private OptionDAL oDAL = new OptionDAL();
        private HomePageDAL homePageDAL = new HomePageDAL();

        /// <summary>
        /// 拼接图片地址,读取图片，并转化成Base64String
        /// </summary>
        /// <returns>返回图片的地址</returns>
        public List<PictureBase64String> PictureBase64()
        {

            List<PictureBase64String> p64 = new List<PictureBase64String>();
            Option option = oDAL.FilePathDAL();

            if (option == null)  //获取BinaryBase的根目录
            {
                p64.Add(new PictureBase64String() { Picture = "noPath" });  //表示binarybase路有问题
            }
            else
            {
                p64 = Utils.PictureBase64ByTools(option.Value, homePageDAL.ReadFailureInfo());
            }
            return p64;
        }
    }
}
