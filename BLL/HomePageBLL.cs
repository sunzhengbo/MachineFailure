using DAL;
using Model.Entity;
using System.Collections.Generic;
using Tools;

namespace BLL
{
    public class HomePageBLL
    {
        private HomePageDAL homePageDAL = new HomePageDAL();
        private OptionBLL optionBLL = new OptionBLL();

        /// <summary>
        /// 获取测点故障信息
        /// </summary>
        /// <returns>返回测点的故障信息</returns>
        public List<FaultInfo> HandleFailureInfo()
        {
            List<FaultInfo> listFailure = new List<FaultInfo>();
            var pictureBase64Strings = optionBLL.PictureBase64();
            var failureInfos = homePageDAL.ReadFailureInfo();

            if (pictureBase64Strings[0].Picture == "noPath")
            {
                listFailure.Add(new FaultInfo() { FaultInfos = "noPath" });
            }
            else if (pictureBase64Strings[0].Picture == "noRecord")
            {
                listFailure.Add(new FaultInfo() { FaultInfos = "noRecord" });
            }
            else
            {
                listFailure = Utils.ConvertBase64StringToList(pictureBase64Strings, failureInfos);
            }
            return listFailure;
        }
    }
}
