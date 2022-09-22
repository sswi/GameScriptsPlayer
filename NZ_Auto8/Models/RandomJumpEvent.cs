using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XE.Commands;

namespace NZ_Auto8.Models
{


    /// <summary>
    /// 随机跳转
    /// </summary>
    public class RandomJumpEvent:BindableBase   
    {
        public ObservableCollection<RandomJumpTarget> RandomJumpTargets { get; set; } = new ();

        private int numberOfCycles = 0;
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

        /// <summary>
        /// 已循环次数
        /// </summary>
        public int CyclesCount { get; set; }


        private string targetTags=null!;
        /// <summary>
        /// 跳转标记，多个用|隔开
        /// </summary>
        public string TargetTags
        {
            get { return targetTags; }
            set 
            { 
                targetTags = value;
                OnPropertyChanged();
            }
        }



    }
}
