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
        private MouseMode mode= MouseMode.LeftDown;
        /// <summary>
        /// 鼠标模式
        /// </summary>
        public MouseMode Mode
        {
            get { return mode; }
            set { mode = value; OnPropertyChanged(); }
        }

           
        private Point? point=new Point();
        /// <summary>
        ///鼠标移动的相对位置
        /// </summary>
        public Point? Postion
        {
            get { return point; }
            set { point = value; OnPropertyChanged(); }
        }

       
        private int moveTimeSpan = 200;
        /// <summary>
        /// 鼠标移动所用时间
        /// </summary>
        public int MoveTimeSpan
        {
            get { return moveTimeSpan; }
            set { moveTimeSpan = value; OnPropertyChanged(); }
        }


        private bool isSimulatesSliding=true;
        /// <summary>
        ///  是否模拟滑动
        /// </summary>
        public bool IsSimulatesSliding
        {
            get { return isSimulatesSliding; }
            set { isSimulatesSliding = value; }
        }
    }

    /// <summary>
    /// 鼠标操作模式
    /// </summary>
    public enum MouseMode
    {
        /// <summary>
        /// 左键按下
        /// </summary>
        LeftDown = 0,
        /// <summary>
        /// 左键弹起
        /// </summary>
        LeftUp = 1,
        /// <summary>
        /// 左键单击
        /// </summary>
        LeftClick = 2,
        /// <summary>
        /// 左键双击
        /// </summary>
        LeftDoubleClick = 3,
        /// <summary>
        /// 右键按下
        /// </summary>
        RightDown = 4,
        /// <summary>
        /// 右键弹起
        /// </summary>
        RightUp = 5,
        /// <summary>
        /// 右键单击
        /// </summary>
        RightClick = 6,
        /// <summary>
        /// 右键双击
        /// </summary>
        RightDoubleClick = 7,
        /// <summary>
        /// 鼠标移动 相对坐标
        /// </summary>
        Move = 8,
        /// <summary>
        /// 鼠标移动 绝对坐标
        /// </summary>
        MoveTo=9,

    }
}
