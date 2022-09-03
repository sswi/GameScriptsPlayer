using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{
    public class MouseEvent:BindableBase
    {
        //鼠标模式
        private MouseMode mode= MouseMode.LeftDown;
        public MouseMode Mode
        {
            get { return mode; }
            set { mode = value; OnPropertyChanged(); }
        }


        //鼠标移动的相对位置
        private Point? point=new Point();
        public Point? Postion
        {
            get { return point; }
            set { point = value; OnPropertyChanged(); }
        }

        //鼠标移动所用时间
        private int moveTimeSpan = 200;
        public int MoveTimeSpan
        {
            get { return moveTimeSpan; }
            set { moveTimeSpan = value; OnPropertyChanged(); }
        }


        /// <summary>
        /// 模拟滑动
        /// </summary>
        private bool isSimulatesSliding=true;
        public bool IsSimulatesSliding
        {
            get { return isSimulatesSliding; }
            set { isSimulatesSliding = value; }
        }
    }

    public enum MouseMode
    {
        LeftDown = 0,
        LeftUp = 1,
        LeftClick = 2,
        LeftDoubleClick = 3,

        RightDown = 4,
        RightUp = 5,
        RightClick = 6,
        RightDoubleClick = 7,
        Move = 8
    }
}
