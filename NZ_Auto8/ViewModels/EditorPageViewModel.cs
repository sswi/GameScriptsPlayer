﻿using NZ_Auto8.DM;
using NZ_Auto8.Handlers;
using NZ_Auto8.Models;
using NZ_Auto8.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using XE.Commands;

namespace NZ_Auto8.ViewModels
{
    public class EditorPageViewModel : BindableBase
    {
        //插件实例
        private readonly DmSoft _dm;

        //文件处理
        private readonly FileService _fileService;

        //脚本运行
        private bool IsRun = false;

        //跳转标记列表
        public ObservableCollection<string> Tags { get; set; } = new ObservableCollection<string>();


        //选中行
        private int _SelectIndex = 0;

        public EditorPageViewModel(DmSoft dm, GameHandle gameHandle, FileService fileService)
        {
            _dm = dm;
            GameHandle = gameHandle;
            _fileService = fileService;
        }

        //起始步
        private int startIndex = 0;
        public int StartIndex
        {
            get { return startIndex; }
            set { startIndex = value; OnPropertyChanged(); }
        }

        //停止步
        private int endIndex = -1;
        public int EndIndex
        {
            get { return endIndex; }
            set { endIndex = value; OnPropertyChanged(); }
        }






        //选中的脚本 
        public int SelectedIndex
        {
            get { return _SelectIndex; }
            set { _SelectIndex = value; OnPropertyChanged(); }
        }



        //用于新建的步骤
        private Step step = new() { Mode = 0 };
        public Step Step
        {
            get { return step; }
            set { step = value; OnPropertyChanged(); }
        }






        //脚本列表
        public ObservableCollection<Step> Scripts { get; set; } = new();


        public List<string> EventNames { get; set; } = new() { "键盘事件", "鼠标事件", "延迟事件", "限时找图", "跳转语句", "找图点击", "限时找色", "文本输入","随机延迟等待", "按键复归" };


        //调试按钮状态
        public ButtonState RunButtonState { get; set; } = new ButtonState("调试", "Play32");


        // 开始/停止调试运行
        public Command RunAndStopCommand => new(() =>
        {

            //判断窗口是否已绑定,未绑定需要询问是否继续以前台模式运行
            if (!GameHandle.IsBind && !IsRun)
            {
                if (System.Windows.MessageBox.Show("尚未绑定窗口，是否以前台方式继续运行？", "脚本运行", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.No)
                {
                    return;
                }
            }

            //取反
            IsRun = !IsRun;

            //跳转步骤重定向
            if (!RestJump())
            {
                return;
            }

            //运行
            if (IsRun)
            {
                RunButtonState.SetRunButtonState(IsRun);
                Thread thread = new(() =>
                {

                    //关闭鼠标精度
                    _dm.EnableMouseAccuracy(0);
                    //设置鼠标速度=10
                    _dm.SetMouseSpeed(10);


                    //脚本运行
                    for (int i = startIndex; i < Scripts.Count; i++)
                    {
                        //是否运行，用于接受中途停止
                        if (!IsRun) break;

                        //步数跳转
                        //接受返回值，result=-1  下一步，result >=0的为跳转
                        var result = Scripts[i].Run();
                        if (result != -1)
                        {
                            if (result < Scripts.Count)
                            {

                                i = result != 0 ? result - 1 : 0;
                            }
                            else
                            {
                                throw new Exception($"跳转出错，说跳转的 步数索引{result} 超出列表索引最大数{Scripts.Count}");
                            }
                        }




                        //判断是否设置了停止步
                        if (endIndex != -1 && i == endIndex)
                        {
                            break;
                        }
#if DEBUG
                        Debug.WriteLine(Scripts[i].Remark);
#endif

                        //当前步数结束后等待延迟
                        if (Scripts[i].EndWaitTime>1000)
                        {
                            //注意！这步不能直接操作EndWaitTime，否则会改变 脚本的设定值
                            int span = Scripts[i].EndWaitTime;
                            while (span>=0)
                            {
                                if (!IsRun) break;
               
                                //如果时间大于1000 时候，就减少1s
                                if (span > 1000)
                                {
                                    span -= 1000;
                                    Thread.Sleep(1000);
                                }
                                else
                                {
                                    //不足1000时候就直接自行剩下的延时时间
                                    Thread.Sleep(span);
                                    break;
                                }
                                
                            }
                        }
                        else
                        {
                            Thread.Sleep(Scripts[i].EndWaitTime);
                        }
                        
                    }

                    //脚本运行结束 
                    IsRun = false;
                    RunButtonState.SetRunButtonState(IsRun);
                });
                thread.Start();
            }



            //停止脚本
            //恢复鼠标速度
            _dm.SetMouseSpeed(App.mouseSpeed);
            //开启鼠标高精度 
            _dm.EnableMouseAccuracy(1);
            //恢复键状态
            KeyCharManage.RestKeyState(_dm);

        });

        //窗口数据
        public GameHandle GameHandle { get; set; }

        //窗体绑定
        public Command BindWindowCommand => new(GameHandle.BindWindow);

        //置窗口于左上角
        public Command WindowToLeftTopCommand => new(GameHandle.SetWinowToLeftTop);

        //添加行
        public Command AddStepCommand => new((object o) =>
        {
            //检测跳转目标标记是否已使用
            if (step.JumTargetTag != null && ExistsJumTag(step.JumTargetTag))
            {
                MessageBox.Show($"跳转标签名称：{step.JumTargetTag} 已被使用，请重新设置另一个名称");
                return;
            }

            if (o.ToString() == "add")
            {
                //添加到末尾
                Scripts.Add(DleteNotNeedProperty(step));
            }
            else if (o.ToString() == "insert")
            {         
                //插入到所选位置的前边
                step.Index = _SelectIndex;

                Scripts.Insert(_SelectIndex, DleteNotNeedProperty(step));
            }
            else
            {
                //清空
                if (System.Windows.MessageBox.Show("是否要清空所有？", "清空脚本", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                {
                    Scripts.Clear();
                }
                return;
            }
            //新建一个步骤
            Step = new Step() { Mode = EventMode.Keyboard };
            RestStartIndexList();
        });

        //设置启动行
        public Command SetStartIndexCommand => new((object o) =>
        {
            StartIndex = ((Step)o).Index;
        });

        //删除指定行
        public Command DeleteStepCommand => new((object o) =>
        {
            var s = (Step)o;
            //删除步
            Scripts.Remove(s);
            RestStartIndexList();
        });




          /// <summary>
          /// 重定向跳转
          /// </summary>
          /// <param name="mode"></param>
          /// <param name="s"></param>
        private bool RestJump()
        {
            foreach (var item in Scripts)           
            {

                    //找图跳转
                    if (item.Mode == EventMode.FindPicture || item.Mode == EventMode.FindPictureClick)
                    {
                        var list = Scripts.ToList();
                        var s = list.FindLast(s => item.Picture.NofFoundTargetTag!=null && s.JumTargetTag == item.Picture.NofFoundTargetTag  );
                        if (s != null)
                        {
                            item.Picture.NotFoundJumToIndex = s.Index;
                        }
                        else if (item.Picture.NofFoundTargetTag != null && item.Picture.NofFoundTargetTag.Length>0)
                        {
                            MessageBox.Show($"脚本初始化失败，第 {item.Index} 步，未找到跳转目标标签： {item.JumTargetTag}  不存在，请重新确认");
                            return false;
                        }

                        s = list.FindLast(s => item.Picture.HasFoundTargetTag != null && s.JumTargetTag == item.Picture.HasFoundTargetTag);
                        if (s != null)
                        {
                            item.Picture.HasFoundJumToIndex = s.Index;
                        }
                        else if (item.Picture.HasFoundTargetTag != null && item.Picture.HasFoundTargetTag.Length>0)
                    {
                            MessageBox.Show($"脚本初始化失败，第 {item.Index} 步，找到跳转目标标签： {item.JumTargetTag}  不存在，请重新确认");
                            return false;
                        }

                    }


                    //找色跳转
                    else if (item.Mode == EventMode.FindColor)
                    {

                        var list = Scripts.ToList();
                        var s = list.FindLast(s => item.Color.NofFoundTargetTag != null && s.JumTargetTag == item.Color.NofFoundTargetTag);
                        if (s != null)
                        {
                            item.Color.NotFoundJumToIndex = s.Index;
                        }
                        else if (item.Color.NofFoundTargetTag != null && item.Color.NofFoundTargetTag.Length>0)
                    {
                            MessageBox.Show($"脚本初始化失败，第 {item.Index} 步，未找到跳转目标标签： {item.JumTargetTag}  不存在，请重新确认");
                            return false;
                        }
                        s = list.FindLast(s => item.Color.HasFoundTargetTag != null && s.JumTargetTag == item.Color.HasFoundTargetTag);
                        if (s != null)
                        {
                            item.Color.HasFoundJumToIndex = s.Index;
                        }
                        else if (item.Color.HasFoundTargetTag != null && item.Color.HasFoundTargetTag.Length > 0)
                    {
                            MessageBox.Show($"脚本初始化失败，第 {item.Index} 步，找到跳转目标标签： {item.JumTargetTag}  不存在，请重新确认");
                            return false;
                        }

                    }

                    //纯跳转
                    else if(item.Mode== EventMode.Jump)
                    {
                        var list = Scripts.ToList();
                        var s = list.FindLast(s => item.Jump.TargetTag != null && s.JumTargetTag == item.Jump.TargetTag);
                        if (s != null)
                        {
                            item.Jump.JumToIndex = s.Index;
                        }
                        else if (item.Jump.TargetTag != null && item.Jump.TargetTag.Length > 0 )
                        {
                            MessageBox.Show($"脚本初始化失败，第 {item.Index} 步，跳转目标标签： {item.JumTargetTag}  不存在，请重新确认");
                            return false;
                        }
                        item.Jump.CyclesCount = 0;
                    }
  
            }
            return true;
        }





        //编号重新排序
        private void RestStartIndexList()
        {
            for (int i = 0; i < Scripts.Count; i++)
            {
                Scripts[i].Index = i;
            }

        }

        //弹出调试窗口

        public Command ShowMessageWindowCommand => new(() =>
        {



        });




        //导入本地脚本文件
        public Command LoadScriptsCommand => new(() =>
        {
            var scriptsList = _fileService.LoadScriptText();
            if (scriptsList != null)
            {
                Scripts.Clear();
                Tags.Clear();
                scriptsList.ForEach(s =>
                {
                    if (s.JumTargetTag?.Length>0)
                    {
                        Tags.Add(s.JumTargetTag);
                    }
                    Scripts.Add(s);
                });
            }
        });


        //保存/另存为
        public Command SaveAsScriptCommand => new((object o) =>
        {
            //重定向跳转
            if (!RestJump())
            {
                return;
            }
            _fileService.SaveScript(o.ToString(), Scripts);
        });



        //删除不需要的属性值，减少实例构造
        private static Step DleteNotNeedProperty(Step step)
        {
            switch (step.Mode)
            {
                case EventMode.Keyboard:
                    //step.Keyboard = null;
                    step.Mouse = null;
                    step.Jump = null;
                    step.Picture = null;
                    step.Color = null;
                    step.InputText = null;
                    step.RandomDelay = null;                    
                    break;
                case EventMode.Mouse:
                    step.Keyboard = null;
                    // step.Mouse = null;
                    step.Jump = null;
                    step.Picture = null;
                    step.Color = null;
                    step.InputText = null;
                    step.RandomDelay = null;
                    break;
                case EventMode.Sleep:
                    step.Keyboard = null;
                    step.Mouse = null;
                    step.Jump = null;
                    step.Picture = null;
                    step.Color = null;
                    step.InputText = null;
                    step.RandomDelay = null;
                    break;
                case EventMode.FindPicture:
                    step.Keyboard = null;
                    step.Mouse = null;
                    step.Jump = null;
                    // step.Picture = null;
                    step.Color = null;
                    step.InputText = null;
                    step.RandomDelay = null;
                    break;
                case EventMode.Jump:
                    step.Keyboard = null;
                    step.Mouse = null;
                    // step.Jump = null;
                    step.Picture = null;
                    step.Color = null;
                    step.InputText = null;
                    step.RandomDelay = null;
                    break;
                case EventMode.FindPictureClick:
                    step.Keyboard = null;
                    step.Mouse = null;
                    step.Jump = null;
                    // step.Picture = null;
                    step.Color = null;
                    step.InputText = null;
                    step.RandomDelay = null;
                    break;
                case EventMode.FindColor:
                    step.Keyboard = null;
                    step.Mouse = null;
                    step.Jump = null;
                    step.Picture = null;
                    // step.Color = null;
                    step.InputText = null;
                    step.RandomDelay = null;
                    break;
                case EventMode.Input:
                    step.Keyboard = null;
                    step.Mouse = null;
                    step.Jump = null;
                    step.Picture = null;
                    step.Color = null;
                    step.RandomDelay = null;
                    //step.InputText = null;
                    break;
                case EventMode.RandomDelay:
                    step.Keyboard = null;
                    step.Mouse = null;
                    step.Jump = null;
                    step.Picture = null;
                    step.Color = null;
                    //step.InputText = null;
                    break;
                case EventMode.KeyboardReverted:
                    step.Keyboard = null;
                    step.Mouse = null;
                    step.Jump = null;
                    step.Picture = null;
                    step.Color = null;
                    step.RandomDelay = null;                    
                    //step.InputText = null;
                    break;


            }
            return step;
        }


        /// <summary>
        /// 检测 跳转目标是否已存在
        /// </summary>
        /// <returns></returns>
        private bool ExistsJumTag(string tag)
        {
            foreach (var item in Scripts)
            {
                if (item.JumTargetTag==tag)
                {
                    return true;
                }
            }
            return false;
        }



    }//
}
