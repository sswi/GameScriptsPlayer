﻿using NZ_Auto8.DM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{

    /// <summary>
    /// 游戏句柄
    /// </summary>
    public class GameHandle:BindableBase
    {
        private readonly DmSoft _dm;
        public GameHandle(DmSoft dm)
        {
            _dm = dm;
            MousePosition = new Point(0, 0);
        }



        //锁定按键
        private bool isEnabled=true;
        /// <summary>
        /// 锁定绑定 按钮
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled= value;OnPropertyChanged(); }
        }









        /// <summary>
        /// 鼠标所指的窗口句柄
        /// </summary>
        private int selectHandle=0;


        /// <summary>
        /// 选择的窗口句柄
        /// </summary>
        private int handle;
        public int Handle
        {
            get { return handle; }
            set { handle = value; OnPropertyChanged(); }
        }


        //停止鼠标句柄获取跟踪
        private bool _getCursorPosStop;
        public bool IsChecked
        {
            get { return _getCursorPosStop; }
            set
            { 
                _getCursorPosStop = value;
                if (value)
                {
                    LoopGetCursorPos(100);
                    WaitHotKey(50);
                }
                OnPropertyChanged(); 
            }
        }
        public Point MousePosition { get; set; }




        /// <summary>
        /// 循环获取鼠标坐标,和窗口句柄
        /// </summary>
        /// <param name="timeSpan"></param>
        private void LoopGetCursorPos(int timeSpan)
        {
            Task.Run(() =>
            {
                while (_getCursorPosStop)
                {
                    int dm_ret = _dm.GetCursorPos(out int x, out int y);
                    if (dm_ret == 1)
                    {
                        //显示鼠标坐标
                        MousePosition.X = x;
                        MousePosition.Y = y;
                    }
                    else
                    {
                        //坐标获取失败显示-1
                        MousePosition.X = -1;
                        MousePosition.Y =-1;
                    }
                    //获取鼠标所指窗口的句柄
                    selectHandle = _dm.GetMousePointWindow();

                    //线程休眠，避免CPU超高占用
                    Thread.Sleep(timeSpan);
                }
            });
        }


        /// <summary>
        /// 等待快捷键 ALT+A
        /// </summary>
        private void WaitHotKey(int timeSpan)
        {
            Task.Run(() =>
            {
                while (_getCursorPosStop)
                {
                    if (_dm.WaitKey(18, 1) + _dm.WaitKey(65, 1) == 2)
                    {
                        Handle = selectHandle;
                    }
                    //线程休眠，避免CPU超高占用
                    Thread.Sleep(timeSpan);
                }
            });
        }


        private bool isBind=false;

        /// <summary>
        /// 窗口是否已绑定
        /// </summary>
        public bool IsBind
        {
            get { return isBind; }
            set
            { 
                isBind = value;
                BindButtonText = value ? "解除绑定" : "窗口绑定";
                OnPropertyChanged();
            }
        }




        private string bindButtonText="绑定窗口";
        public string BindButtonText
        {
            get { return bindButtonText; }
            set { bindButtonText = value; OnPropertyChanged(); }
        }



        /// <summary>
        /// 窗口绑定
        /// </summary>
        public void BindWindow()
        {
            //如果已绑定则解除绑定，否则则绑定
            if (!IsBind)
            {
                if (Handle > 0)
                {
                    Task.Run(() =>
                    {
                        IsEnabled = false;
                        int dm_ret = _dm.BindWindow(Handle, "dx2", "dx2", "dx", 0);
                        if (dm_ret == 1)
                        {
                            IsChecked = false;  //停止鼠标句柄获取
                            IsBind = true;
                        }
                        System.Windows.MessageBox.Show(dm_ret == 1 ? "绑定成功!" : "绑定失败");
                        IsEnabled = true;
                    });
                }
            }
            else
            {

                Task.Run(() =>
                {
                    IsEnabled = false;
                    var result = _dm.UnBindWindow();
                    IsBind = !(result == 1);
                    IsEnabled = true;
                    System.Windows.MessageBox.Show(result == 1 ? "解绑成功!" : "解绑失败");
                });

            }

        }


        /// <summary>
        /// 将窗口移至左上角
        /// </summary>
        public void SetWinowToLeftTop() 
        {
          
            if (isBind && handle>0)
            {
                _dm.MoveWindow(handle, 0, 0);
            }       
        
        }


    }
}
