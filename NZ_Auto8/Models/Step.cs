﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{
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


        private string remark;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value;OnPropertyChanged(); }
        }


        
        private string inputText;
        /// <summary>
        /// 输入文本
        /// </summary>
        public string InputText
        {
            get { return inputText; }
            set { inputText = value; OnPropertyChanged(); }
        }





        private string tag;
        /// <summary>
        /// 跳转目标标记
        /// </summary>
        public string JumTargetTag
        {
            get { return tag; }
            set { tag = value; OnPropertyChanged(); }
        }






        private RandomDelay randomDelay=new RandomDelay();
        /// <summary>
        /// 随机延迟
        /// </summary>
        public RandomDelay RandomDelay
        {
            get { return randomDelay; }
            set { randomDelay = value; }
        }




    }










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
        KeyboardReverted=9
    }
}
