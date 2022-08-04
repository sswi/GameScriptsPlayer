
using XE.Commands;

namespace NZ_Auto8.Models
{
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




        private int numberOfCycles = 1;
        /// <summary>
        /// 循环次数
        /// </summary>
        public int NumberOfCycles
        {
            get { return numberOfCycles; }
            set { numberOfCycles = value; OnPropertyChanged(); }
        }






        private int cyclesCount;
        /// <summary>
        /// 已循环计数
        /// </summary>
        public int CyclesCount
        {
            get { return cyclesCount; }
            set { cyclesCount = value; OnPropertyChanged(); }
        }




    }
}
