using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{
    /// <summary>
    /// 步骤
    /// </summary>
    public class Step:BindableBase
    {


        private int index = 0;
        /// <summary>
        /// 索引
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; OnPropertyChanged(); }
        }

        private EventMode mode=0;
        /// <summary>
        /// 操作类型
        /// </summary>
        public EventMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;
                OnPropertyChanged(); 
            }
        }



       
        private KeyboardEvent keyboard =new();
        /// <summary>
        /// 键盘事件
        /// </summary>
        public KeyboardEvent Keyboard
        {
            get { return keyboard; }
            set { keyboard = value;OnPropertyChanged(); }
        }


        private MouseEvent mouse = new();
        /// <summary>
        /// 鼠标事件
        /// </summary>
        public MouseEvent Mouse
        {
            get { return mouse; }
            set { mouse = value; OnPropertyChanged(); }
        }



        private int endWaitTime=65;
        /// <summary>
        /// 结束后等待时间
        /// </summary>
        public int EndWaitTime
        {
            get { return endWaitTime; }
            set { endWaitTime = value;OnPropertyChanged(); }
        }


        private JumpEvent jump = new();
        /// <summary>
        /// 跳转
        /// </summary>
        public JumpEvent Jump
        {
            get { return jump; }
            set { jump = value;OnPropertyChanged();}
        }



        private PictureEvent picture = new();
        /// <summary>
        /// 找图
        /// </summary>
        public PictureEvent Picture
        {
            get { return picture; }
            set { picture = value;OnPropertyChanged(); }
        }



        private ColorEvent? color = new();
        /// <summary>
        /// 找色
        /// </summary>
        public ColorEvent? Color
        {
            get { return color; }
            set { color = value;OnPropertyChanged(); }
        }


        private string remark = null!;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value;OnPropertyChanged(); }
        }


        
        private string inputText = null!;
        /// <summary>
        /// 输入文本
        /// </summary>
        public string InputText
        {
            get { return inputText; }
            set { inputText = value; OnPropertyChanged(); }
        }





        private string tag = null!;
        /// <summary>
        /// 跳转目标标记
        /// </summary>
        public string JumTargetTag
        {
            get { return tag; }
            set { tag = value; OnPropertyChanged(); }
        }



        private RandomDelay randomDelay=new ();
        /// <summary>
        /// 随机延迟
        /// </summary>
        public RandomDelay RandomDelay
        {
            get { return randomDelay; }
            set { randomDelay = value; }
        }


        private CloseAppEvent killApp=new ();
        /// <summary>
        /// 结束进程
        /// </summary>
        public CloseAppEvent KillApp
        {
            get { return killApp; }
            set { killApp = value; OnPropertyChanged(); }
        }


        private ShutdownEvent shutdown=new ();
        /// <summary>
        /// 关机事件
        /// </summary>
        public ShutdownEvent Shutdown
        {
            get { return shutdown; }
            set { shutdown = value; OnPropertyChanged(); }
        }



        private bool isExpanded=false;
        /// <summary>
        /// 展开脚本内容
        /// </summary>
        public bool IsExpanded
        {
            get { return isExpanded; }
            set { isExpanded = value; OnPropertyChanged(); }
        }


        private RandomJumpEvent randomJump = new ();
        /// <summary>
        /// 随机跳转
        /// </summary>
        public RandomJumpEvent RandomJump
        {
            get { return randomJump; }
            set { randomJump = value;
                OnPropertyChanged();
            }
        }



    }









    /// <summary>
    /// 此步的操作模式，操作类型
    /// </summary>
    public enum EventMode
    {
        /// <summary>
        /// 键盘操作
        /// </summary>
        Keyboard = 0,
        /// <summary>
        /// 鼠标操作
        /// </summary>
        Mouse = 1,
        /// <summary>
        /// 延迟等待
        /// </summary>
        Sleep = 2,
        /// <summary>
        /// 找图跳转操作
        /// </summary>
        FindPicture = 3,
        /// <summary>
        /// 跳转
        /// </summary>
        Jump = 4,
        /// <summary>
        /// 找图并点击
        /// </summary>
        FindPictureClick = 5,
        /// <summary>
        /// 找色跳转操作
        /// </summary>
        FindColor = 6,
        /// <summary>
        /// 文本输入
        /// </summary>
        Input=7,
        /// <summary>
        /// 随机延迟等待
        /// </summary>
        RandomDelay=8,
        /// <summary>
        /// 按键复归
        /// </summary>
        KeyboardReverted=9,
        /// <summary>
        /// 关机
        /// </summary>
        ShutDown=10,
        /// <summary>
        /// 结束进程
        /// </summary>
        KillApp=11,
        /// <summary>
        /// 随机跳转
        /// </summary>
        RandomJump=12,
    }
}
