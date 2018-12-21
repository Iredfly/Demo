using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.EntityDto.Dto
{
    public class OperBoxInDto
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }
        ///// <summary>
        ///// 命令类型:
        ///// "1"盒子主人开箱  "2" 投递员开箱  "3"盒子主人关箱  "4" 投递员关箱 
        ///// </summary>
        //public int CommandType { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceNo { get; set; }
    }
}
