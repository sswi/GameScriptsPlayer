
using System.Windows;
using XE.Commands;

namespace NZ_Auto8.Models
{

    /// <summary>
    /// 跳转操作 ，及相关参数
    /// </summary>
    public class JumpEvent : BindableBase
    {
        private int jumToIndex;

        /// <summary>
        /// 跳转到 序号
        /// </summary>
        public int JumToIndex
        {
            get { return jumToIndex; }
            set { jumToIndex = value; OnPropertyChanged(); }
        }




        private string? targetTag;
        /// <summary>
        /// 跳转到 目标标记
        /// </summary>
        public string TargetTag
        {
            get { return targetTag!; }
            set { targetTag = value; OnPropertyChanged(); }
        }







        private int numberOfCycles = 1;
        /// <summary>
        /// 循环次数，0表示无限循环
        /// </summary>
        public int NumberOfCycles
        {
            get { return numberOfCycles; }
            set 
            {
                if (value >= 0)
                {
                    numberOfCycles = value;
                    OnPropertyChanged();
                }
                else
                {
                    MessageBox.Show("循环次数不可小于0，0=无限循环，大于0则循环次数");
                    numberOfCycles = 0;
                }
            }
        }





        private int cyclesCount;
        /// <summary>
        /// 已循环计数，界面上不可见，程序运行时内部计算用
        /// </summary>
        public int CyclesCount
        {
            get { return cyclesCount; }
            set { cyclesCount = value; OnPropertyChanged(); }
        }




    }
}
