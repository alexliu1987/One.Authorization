using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Common
{
    public static class SerialNumber
    {
        private const string CSTR = "0000000000000000000000000000000000000000000000000000";
        /// <summary>
        /// 返回序号号
        /// </summary>
        /// <param name="FirstPart">开头字母部分</param>
        /// <param name="LastSerialNumber">最后一个序列号</param>
        /// <param name="NumberBit">数字位数</param>
        /// <returns>返回序号号</returns>
        public static string Get(string FirstPart, string LastSerialNumber, int NumberBit)
        {
            if (string.IsNullOrEmpty(LastSerialNumber))
                return FirstPart + (1).ToString(CSTR.Substring(0, NumberBit));
            int index = int.Parse(LastSerialNumber.Substring(FirstPart.Length, NumberBit));
            index++;
            return "M" + index.ToString(CSTR.Substring(0, NumberBit));
        }
    }
}
