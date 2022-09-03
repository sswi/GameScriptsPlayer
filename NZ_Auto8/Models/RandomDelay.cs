using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XE.Commands;

namespace NZ_Auto8.Models
{
    /// <summary>
    /// 随机延迟等待
    /// </summary>
  public  class RandomDelay:BindableBase
    {

        private int minValue;
        /// <summary>
        /// 最小值
        /// </summary>
        public int MinValue
        {
            get { return minValue; }
            set
            {
                if (value >= maxValue && value<0 && maxValue!>int.MaxValue)
                {
                    MessageBox.Show($"最小值不可以大于或等于最大值,,取值范围为(0~{int.MaxValue})");
                    return;
                }
                minValue = value;
                OnPropertyChanged();
            }
        }


        private int maxValue=100;
        public int MaxValue
        {
            get { return maxValue; }
            set
            {

                if (value<=minValue && value<0 && maxValue! > int.MaxValue)
                {
                    MessageBox.Show($"最大值不可以小于或等于最小值,取值范围为(0~{int.MaxValue})");
                    return;
                }

                maxValue = value;
                OnPropertyChanged();
            }
        }






    }
}
