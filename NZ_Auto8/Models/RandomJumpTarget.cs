using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{
    /// <summary>
    /// 随机跳转目标
    /// </summary>
    public class RandomJumpTarget:BindableBase
    {

        private int index;
        /// <summary>
        /// 用于显示的编号·
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value;OnPropertyChanged(); }
        }



        /// <summary>
        /// 跳转目标索引/编号
        /// </summary>
        public int TargetIndex { get; set; }


        private string targetTag=null!;
        /// <summary>
        /// 跳转目标标记
        /// </summary>
        public string TargetTag
        {
            get { return targetTag; }
            set { targetTag = value; OnPropertyChanged(); }
        }

    }
}
