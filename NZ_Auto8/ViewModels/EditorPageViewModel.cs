using NZ_Auto8.DM;
using NZ_Auto8.Handlers;
using NZ_Auto8.Models;
using NZ_Auto8.Services;
using NZ_Auto8.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using XE.Commands;

namespace NZ_Auto8.ViewModels
{
    public class EditorPageViewModel:BindableBase
    {
        //插件实例
        private readonly DmSoft _dm;

        //文件处理
        private readonly FileService _fileService;

        //脚本运行
        private bool IsRun = false;

        //选中行
        private int _SelectIndex=0;

        public EditorPageViewModel(DmSoft dm, GameHandle gameHandle, FileService fileService)
        {
            _dm = dm;
            GameHandle = gameHandle;
            _fileService = fileService;
        }


        //起始步
        private int startIndex=0;
        public int StartIndex
        {
            get { return startIndex; }
            set { startIndex = value; OnPropertyChanged(); }
        }



        //停止步
        private int endIndex=-1;
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
        private Step step = new() { Mode=0 };        
        public Step Step
        {
            get { return step; }
            set { step = value; OnPropertyChanged(); }
        }






        //脚本列表
        public ObservableCollection<Step> Scripts { get; set; } = new();


        public List<string> EventNames { get; set; } = new() { "键盘事件", "鼠标事件", "延迟事件", "限时找图", "跳转语句", "找图点击", "限时找色","文本输入" }; 


        //调试按钮状态
        public ButtonState RunButtonState { get; set; } = new ButtonState("调试", "Play32");


        // 开始/停止调试运行
        public Command RunAndStopCommand => new(() =>
        {
            //判断是否已绑定,未绑定需要询问是否继续运行
            if (!GameHandle.IsBind)
            {
                if (System.Windows.MessageBox.Show("尚未绑定窗口，是否以前台方式继续运行？", "脚本运行", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question)== System.Windows.MessageBoxResult.No)
                {
                    return;
                }               
            }

            IsRun = !IsRun;
            
            //运行
            if (IsRun)
            {
                RunButtonState.SetRunButtonState(IsRun);
                Thread thread = new (() =>
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
                        
                        //接受返回值，result=-1  下一步，result >=0的为跳转
                        var result =Scripts[i].Run();                        
                        if (result != -1)
                        {
                            if (result < Scripts.Count)
                            {
                                i = result - 1;
                            }
                            else
                            {
                                throw new Exception($"跳转出错，说跳转的 步数索引{result} 超出列表索引最大数{Scripts.Count}");
                            }
                        }

                        //判断是否设置了停止步
                        if (endIndex!=-1 && i==endIndex)
                        {
                            break;
                        }

                        //当前步数结束后等待延迟
                        Thread.Sleep(Scripts[i].EndWaitTime);
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
        public Command BindWindowCommand => new (GameHandle.BindWindow);

        //置窗口于左上角
        public Command WindowToLeftTopCommand => new (GameHandle.SetWinowToLeftTop);

        //添加行
        public Command AddStepCommand => new ((object o) =>
        {

            if (o.ToString() == "add")
            {
                //清除掉无用的属性               

                //添加到末尾
                Scripts.Add(DleteNotNeedProperty(step));
            }
            else if (o.ToString() == "insert")
            {
                //清除掉无用的属性               
                //插入到所选位置的前边
                step.Index = _SelectIndex;
                RestJumpIndex("insert", step);
                Scripts.Insert(_SelectIndex, DleteNotNeedProperty(step));
            }
            else
            {
                //清空
                if (System.Windows.MessageBox.Show("是否要清空所有？","清空脚本", System.Windows.MessageBoxButton.YesNo,  System.Windows.MessageBoxImage.Question)== System.Windows.MessageBoxResult.Yes)
                {
                    Scripts.Clear();
                }  
                return;
            }
            //新建一个步骤
            Step = new Step() {Mode= EventMode.Keyboard};
            RestStartIndexList();
        });

        //设置启动行
        public Command SetStartIndexCommand => new((object o) => 
        {
            StartIndex = ((Step)o).Index;               
        });

        //删除指定行
        public Command DeleteStepCommand => new ((object o) => 
        {
            var s = (Step)o;
            RestJumpIndex("Remove", s);
            //删除步
            Scripts.Remove(s);
            RestStartIndexList();
        });




        /// <summary>
        /// 删除、插入行后跳转重新指向
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="step"></param>
        private void RestJumpIndex(string mode,Step s)
        {
            if (mode == "Remove")
            {
                //更新跳转步
                for (int i = s.Index; i < Scripts.Count; i++)
                {
                    if (Scripts[i].Mode == EventMode.Jump)
                    {
                        if (Scripts[i].Jump.JumToIndex != -1 && Scripts[i].Jump.JumToIndex > s.Index)
                        {
                            Scripts[i].Jump.JumToIndex--;
                        }
                    }
                    else if (Scripts[i].Mode == EventMode.FindPicture || Scripts[i].Mode == EventMode.FindPictureClick)
                    {
                        if (Scripts[i].Picture.HasFoundJumToIndex != -1 && Scripts[i].Picture.HasFoundJumToIndex > s.Index)
                        {
                            Scripts[i].Picture.HasFoundJumToIndex--;
                        }
                        if (Scripts[i].Picture.NotFoundJumToIndex != -1 && Scripts[i].Picture.NotFoundJumToIndex > s.Index)
                        {
                            Scripts[i].Picture.NotFoundJumToIndex--;
                        }
                    }
                    else if (Scripts[i].Mode == EventMode.FindColor)
                    {
                        if (Scripts[i].Color.NotFoundJumToIndex != -1 && Scripts[i].Color.NotFoundJumToIndex > s.Index)
                        {
                            Scripts[i].Color.NotFoundJumToIndex--;
                        }
                        if (Scripts[i].Color.HasFoundJumToIndex != -1 && Scripts[i].Color.HasFoundJumToIndex > s.Index)
                        {
                            Scripts[i].Color.HasFoundJumToIndex--;
                        }
                    }
                }
            }
            else
            {
                for (int i = s.Index; i < Scripts.Count; i++)
                {
                    if (Scripts[i].Mode == EventMode.Jump)
                    {
                        if (Scripts[i].Jump.JumToIndex != -1 && Scripts[i].Jump.JumToIndex > s.Index)
                        {
                            Scripts[i].Jump.JumToIndex++;
                        }
                    }
                    else if (Scripts[i].Mode == EventMode.FindPicture || Scripts[i].Mode == EventMode.FindPictureClick)
                    {
                        if (Scripts[i].Picture.HasFoundJumToIndex != -1 && Scripts[i].Picture.HasFoundJumToIndex > s.Index)
                        {
                            Scripts[i].Picture.HasFoundJumToIndex++;
                        }
                        if (Scripts[i].Picture.NotFoundJumToIndex != -1 && Scripts[i].Picture.NotFoundJumToIndex > s.Index)
                        {
                            Scripts[i].Picture.NotFoundJumToIndex++;
                        }
                    }
                    else if (Scripts[i].Mode == EventMode.FindColor)
                    {
                        if (Scripts[i].Color.NotFoundJumToIndex != -1 && Scripts[i].Color.NotFoundJumToIndex > s.Index)
                        {
                            Scripts[i].Color.NotFoundJumToIndex++;
                        }
                        if (Scripts[i].Color.HasFoundJumToIndex != -1 && Scripts[i].Color.HasFoundJumToIndex > s.Index)
                        {
                            Scripts[i].Color.HasFoundJumToIndex++;
                        }
                    }
                }

            }
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

        public Command ShowMessageWindowCommand => new (() => 
        {
        
        
        
        });




        //导入本地脚本文件
        public Command LoadScriptsCommand => new (() =>
        {           
            var scriptsList=  _fileService.LoadScriptText();
            if (scriptsList!=null)
            {
                Scripts.Clear();
                scriptsList.ForEach(s =>
                {
                    Scripts.Add(s);
                });
            }
        });


        //保存/另存为
        public Command SaveAsScriptCommand => new((object o) =>
        {
                _fileService.SaveScript(o.ToString(), Scripts);      
        });



        //删除不需要的属性值，减少实例构造
        private Step DleteNotNeedProperty(Step step)
        {
            switch (step.Mode)
            {
                case EventMode.Keyboard:
                    //step.Keyboard = null;
                    step.Mouse =null;
                    step.Jump = null;
                    step.Picture = null;
                    step.Color = null;
                    step.InputText = null;
                    break;
                case EventMode.Mouse:
                    step.Keyboard = null;
                   // step.Mouse = null;
                    step.Jump = null;
                    step.Picture = null;
                    step.Color = null;
                    step.InputText = null;
                    break;
                case EventMode.Sleep:
                    step.Keyboard = null;
                    step.Mouse = null;
                    step.Jump = null;
                    step.Picture = null;
                    step.Color = null;
                    step.InputText = null;
                    break;
                case EventMode.FindPicture:
                    step.Keyboard = null;
                    step.Mouse = null;
                    step.Jump = null;
                   // step.Picture = null;
                    step.Color = null;
                    step.InputText = null;
                    break;
                case EventMode.Jump:
                    step.Keyboard = null;
                    step.Mouse = null;
                   // step.Jump = null;
                    step.Picture = null;
                    step.Color = null;
                    step.InputText = null;
                    break;
                case EventMode.FindPictureClick:
                    step.Keyboard = null;
                    step.Mouse = null;
                    step.Jump = null;
                   // step.Picture = null;
                    step.Color = null;
                    step.InputText = null;
                    break;
                case EventMode.FindColor:
                    step.Keyboard = null;
                    step.Mouse = null;
                    step.Jump = null;
                    step.Picture = null;
                   // step.Color = null;
                    step.InputText = null;
                    break;
                case EventMode.Input:
                    step.Keyboard = null;
                    step.Mouse = null;
                    step.Jump = null;
                    step.Picture = null;
                    step.Color = null;
                    //step.InputText = null;
                    break;
            }
            return step;
        }



        }//
}
