using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{
    public class Step:BindableBase
    {

        //索引
        private int index = 0;
        public int Index
        {
            get { return index; }
            set { index = value; OnPropertyChanged(); }
        }

        //操作类型
        private EventMode mode=0;
        public EventMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;
                OnPropertyChanged(); 
            }
        }



        //键盘事件
        private KeyboardEvent keyboard =new();
        public KeyboardEvent Keyboard
        {
            get { return keyboard; }
            set { keyboard = value;OnPropertyChanged(); }
        }


        //鼠标事件
        private MouseEvent mouse = new();
        public MouseEvent Mouse
        {
            get { return mouse; }
            set { mouse = value; OnPropertyChanged(); }
        }


        //结束后等待时间
        private int endWaitTime=65;
        public int EndWaitTime
        {
            get { return endWaitTime; }
            set { endWaitTime = value;OnPropertyChanged(); }
        }

        //跳转
        private JumpEvent jump = new();
        public JumpEvent Jump
        {
            get { return jump; }
            set { jump = value;OnPropertyChanged();}
        }


        //找图
        private PictureEvent picture = new();
        public PictureEvent Picture
        {
            get { return picture; }
            set { picture = value;OnPropertyChanged(); }
        }


        //找色
        private ColorEvent? color = new();
        public ColorEvent? Color
        {
            get { return color; }
            set { color = value;OnPropertyChanged(); }
        }


        //备注
        private string remark;
        public string Remark
        {
            get { return remark; }
            set { remark = value;OnPropertyChanged(); }
        }


        //输入文本
        private string inputText;
        public string InputText
        {
            get { return inputText; }
            set { inputText = value; OnPropertyChanged(); }
        }


    }


    public enum EventMode
    {
        Keyboard = 0,
        Mouse = 1,
        Sleep = 2,
        FindPicture = 3,
        Jump = 4,
        FindPictureClick = 5,
        FindColor = 6,
        Input=7
    }
}
