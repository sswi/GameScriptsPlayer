using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZ_Auto8.Models
{
    /// <summary>
    /// 鼠标移动间隔设置
    /// </summary>
    public class MouseMoveConfig
    {

        /// <summary>
        /// 移动时间间隔
        /// </summary>
        public int Span { get; set; }


        /// <summary>
        /// 每秒移动的频率
        /// </summary>
        public int Frequency { get; set; }


    }
}
