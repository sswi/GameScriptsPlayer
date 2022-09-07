

using XE.Commands;

namespace NZ_Auto8.Models
{

    /// <summary>
    /// 屏幕点坐标 或 鼠标移动偏移量
    /// </summary>
    public class Point : BindableBase
    {
        public Point(int _x = 0, int _y = 0)
        {
            x = _x;
            y = _y;
        }

        private int x;
        /// <summary>
        /// 屏幕点X轴
        /// </summary>
        public int X
        {
            get { return x; }
            set 
            { 
                x = value; 
                OnPropertyChanged(); 
            }
        }
        private int y;

        /// <summary>
        /// 屏幕点Y轴
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; OnPropertyChanged(); }
        }
    }
}
