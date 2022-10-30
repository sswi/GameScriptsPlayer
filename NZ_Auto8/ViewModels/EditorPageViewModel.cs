using NZ_Auto8.DM;
using NZ_Auto8.Handlers;
using NZ_Auto8.Models;
using NZ_Auto8.Services;
using NZ_Auto8.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using XE.Commands;

namespace NZ_Auto8.ViewModels
{
    /// <summary>
    /// 编辑器页面视图模型
    /// </summary>
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
        /// <summary>
        /// 起始步
        /// </summary>
        public int StartIndex
        {
            get { return startIndex; }
            set { startIndex = value; OnPropertyChanged(); }
        }

        //停止步
        private int endIndex = -1;
        /// <summary>
        /// 停止步
        /// </summary>
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





        /// <summary>
        /// 脚本 "步"列表
        /// </summary>
        public ObservableCollection<Step> Scripts { get; set; } = new();


        /// <summary>
        /// 当前支持的 操作的名称列表
        /// </summary>
        public List<string> EventNames { get; set; } = new() { "键盘事件", "鼠标事件", "延迟事件", "限时找图", "跳转语句", "找图点击", "限时找色", "文本输入", "随机延迟等待", "按键复归", "关机", "结束进程", "随机跳转" };


        //调试按钮状态，及图标
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

            //判断大漠插件是否通过注册码验证，没验证成功无法使用，收费插件，莫得办法
            if (!DmSoft.IsReg)
            {
                MessageBox.Show("插件注册失败，无法运行脚本，请根据开始的错误提示通过注册\r\n温馨提醒：十分抱歉，大漠插件是收费的，本工具是基于大漠插件为底层开发的，本人也是买激活码，好在很便宜50块700天（7分钱1台电脑/天），某宝/大漠官网有卖\r\n或者去贴吧论坛找别人分享的注册码");
                return;
            }



            //运行状态 取反
            IsRun = !IsRun;

            //跳转步骤重定向，用于验证脚本各标签和跳转是否有效，不执行这步的话脚本有错误跳转的话程序将直接崩了
            if (!RestJump())
            {
                return;
            }

            //开始 运行
            if (IsRun)
            {
                //更新界面 显示为运行状态
                RunButtonState.SetRunButtonState(IsRun);

                //开启线程，不用Task任务
                Thread thread = new(() =>
                {

                    //关闭鼠标精度
                    _dm.EnableMouseAccuracy(0);
                    //设置鼠标速度=10
                    _dm.SetMouseSpeed(10);

                   
                    //脚本运行
                    for (int i = startIndex; i < Scripts.Count; i++)
                    {
                        //是否运行，用于接受用户中途手动停止
                        if (!IsRun) break;

                        //调试信息 创建一个线程线程去输出，保证脚本运行最低延迟
                        if (Scripts[i].Remark != null && Scripts[i].Remark.Length > 0)
                        {
                            Task.Run(() =>
                            {
                                DebugInformation = $"[{Scripts[i].Index}].{Scripts[i].Remark}";
                            });
                        }


                        //“步”的执行，此处脚本正式开始运行，接受返回值，result=-1  下一步，result >=0的为跳转                        
                        var result = Scripts[i].Run();



                        //判断是否设置了停止步
                        if (endIndex != -1 && i == endIndex)
                        {
                            break;
                        }

                        //当前步数结束后等待延迟，如果等待时间超过1000ms则分段执行，解决中途手动停止时候脚本迟迟无法终止运行问题
                        if (Scripts[i].EndWaitTime > 1000)
                        {
                            //注意！这步不能直接操作EndWaitTime，否则会改变 脚本的设定值
                            int span = Scripts[i].EndWaitTime;
                            while (span >= 0)
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
                                    //不足1000时候就直接执行剩下的延时时间
                                    Thread.Sleep(span);
                                    break;
                                }
                            }
                        }
                        //低于1000ms则直接运行延时等待
                        else
                        {
                            Thread.Sleep(Scripts[i].EndWaitTime);
                        }

                        //步数跳转
                        if ( result != -1)
                        {
                            if (!IsRun)
	                        {
                                break;
	                        }
                            if (result < Scripts.Count )
                            {
                              
                                i = result != 0 ? result - 1 : 0;
                            }
                            else
                            {
                                throw new Exception($"跳转出错，说跳转的 步数索引{result} 超出列表索引最大数{Scripts.Count}");
                            }
                        }

                    }

                    //更新界面 显示为停止状态
                    RunButtonState.SetRunButtonState(false);
                    //脚本运行结束 
                    IsRun = false;
                    //恢复鼠标键盘设置
                    RestMouseAndKeyConfig();
                });
                //开始线程
                thread.Start();
            }
            //脚本到此运行结束
            //恢复鼠标键盘设置
            RestMouseAndKeyConfig();
        });



        /// <summary>
        /// 回复鼠标、键盘原有设置
        /// </summary>
        private void RestMouseAndKeyConfig()
        {
            //恢复鼠标速度
            _dm.SetMouseSpeed(App.mouseSpeed);
            //开启鼠标高精度 
            _dm.EnableMouseAccuracy(1);
            //恢复键状态，避免卡键
            KeyCharManage.RestKeyState(_dm);
        }







        //游戏窗口数据
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
                if (_SelectIndex>=0)
                {
                    //插入到所选位置的前边
                    step.Index = _SelectIndex;
                    Scripts.Insert(_SelectIndex, DleteNotNeedProperty(step));
                }

            }
            else
            {
                //清空
                if (MessageBox.Show("是否要清空所有？", "清空脚本", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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
        public Command SetStartIndexComman => new((object o) =>
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
            //将脚本步数转换为列表，好用lamda操作
            var list = Scripts.ToList();


            foreach (var item in Scripts)
            {

                DleteNotNeedProperty(item);
                //Debug.WriteLine($"{item.Index},{item.Mode}");

                //找图跳转
                if (item.Mode == EventMode.FindPicture || item.Mode == EventMode.FindPictureClick)
                {
                    var s = list.FindLast(s => item.Picture.NofFoundTargetTag != null && s.JumTargetTag == item.Picture.NofFoundTargetTag);
                    if (s != null)
                    {
                        item.Picture.NotFoundJumToIndex = s.Index;
                    }
                    else if (item.Picture.NofFoundTargetTag != null && item.Picture.NofFoundTargetTag.Length > 0)
                    {
                        MessageBox.Show($"脚本初始化失败，第 {item.Index} 步，未找到跳转目标标签： {item.JumTargetTag}  不存在，请重新确认");
                        return false;
                    }
                    s = list.FindLast(s => !string.IsNullOrEmpty(item.Picture.HasFoundTargetTag) && s.JumTargetTag == item.Picture.HasFoundTargetTag);
                    if (s != null)
                    {
                        item.Picture.HasFoundJumToIndex = s.Index;
                    }
                    else if (!string.IsNullOrEmpty(item.Picture.HasFoundTargetTag)  )
                    {
                        MessageBox.Show($"脚本初始化失败，第 {item.Index} 步，找到跳转目标标签： {item.JumTargetTag}  不存在，请重新确认");
                        return false;
                    }



                }

                //找色跳转
                else if (item.Mode == EventMode.FindColor)
                {
                    var s = list.FindLast(s => item.Color.NofFoundTargetTag != null && s.JumTargetTag == item.Color.NofFoundTargetTag);
                    if (s != null)
                    {
                        item.Color.NotFoundJumToIndex = s.Index;
                    }
                    else if (item.Color.NofFoundTargetTag != null && item.Color.NofFoundTargetTag.Length > 0)
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
                else if (item.Mode == EventMode.Jump)
                {

                    var s = list.FindLast(s => item.Jump.TargetTag != null && s.JumTargetTag == item.Jump.TargetTag);
                    if (s != null)
                    {
                        item.Jump.JumToIndex = s.Index;
                    }
                    else if (item.Jump.TargetTag != null && item.Jump.TargetTag.Length > 0)
                    {
                        MessageBox.Show($"脚本初始化失败，第 {item.Index} 步，跳转目标标签： {item.JumTargetTag}  不存在，请重新确认");
                        return false;
                    }
                    item.Jump.CyclesCount = 0;
                }

                //随机跳转
                else if (item.Mode == EventMode.RandomJump)
                {
                    //先将跳转标记转换成跳转步数
                    //判断是否为空或者目标少于2个
                    if (!string.IsNullOrEmpty(item.RandomJump.TargetTags) && item.RandomJump.TargetTags.Contains('|'))
                    {                   

                        var tags = item.RandomJump.TargetTags.Split('|');
                        item.RandomJump.RandomJumpTargets.Clear();
                        foreach (var tag in tags)
                        {
                            //找出对应标签的序号
                            var tagIndex = list.Find(s => s.JumTargetTag == tag);
         
                            if (tagIndex != null)
                            {
                                var target = new RandomJumpTarget() { TargetTag = tag, TargetIndex = tagIndex.Index };
                                item.RandomJump.RandomJumpTargets.Add(target);
                            }
                            else
                            {
                                MessageBox.Show($"第{item.Index}步，随机跳转目标标记 {tag} 无法找到");
                                return false;
                            }
                        }                        
                    }
                    else
                    {
                        MessageBox.Show($"第{item.Index}步，随机跳转目标少于2个，无法随机跳转");
                        return false;
                    }
                }
            }
            return true;
        }




        /// <summary>
        /// 编号重新排序
        /// </summary>
        private void RestStartIndexList()
        {
            for (int i = 0; i < Scripts.Count; i++)
            {
                Scripts[i].Index = i;
            }
        }




        private string debuginfo;
        /// <summary>
        /// 调试信息显示
        /// </summary>
        public string DebugInformation
        {
            get { return debuginfo; }
            set
            {
                debuginfo = value;
                if (messageWindow != null)
                {
                    Application.Current.Dispatcher.BeginInvoke(() =>
                    {
                        messageWindow.Messages.Insert(0, value);
                    });
                }
                OnPropertyChanged();
            }
        }




        private MessageWindow messageWindow;
        //弹出调试窗口
        public Command ShowMessageWindowCommand => new(() =>
        {
            messageWindow ??= new MessageWindow();
            messageWindow.Show();
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
                    if (s.JumTargetTag?.Length > 0)
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
            
            _fileService.SaveScript(o.ToString()!, Scripts);
        });




        /// <summary>
        /// 删除不需要的属性值，减少脚本输出长度，和减少解析计算量，啊~~有点冗余，开始没想到这步， 暂时就这样吧，后期再用反射
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        private static Step DleteNotNeedProperty(Step step)
        {
            switch (step.Mode)
            {
                case EventMode.Keyboard:
                    //step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.InputText = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    break;
                case EventMode.Mouse:
                    step.Keyboard = null!;
                    // step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.InputText = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    break;
                case EventMode.Sleep:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.InputText = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    break;
                case EventMode.FindPicture:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    // step.Picture = null!;
                    step.Color = null!;
                    step.InputText = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    break;
                case EventMode.Jump:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    // step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.InputText = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    break;
                case EventMode.FindPictureClick:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    // step.Picture = null!;
                    step.Color = null!;
                    step.InputText = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    break;
                case EventMode.FindColor:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    // step.Color = null!;
                    step.InputText = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    break;
                case EventMode.Input:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    //step.InputText = null!;

                    break;
                case EventMode.RandomDelay:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    //step.InputText = null!;
                    break;
                case EventMode.KeyboardReverted:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;                    
                    step.RandomJump = null!;
                    //step.InputText = null!;
                    break;
                case EventMode.KillApp:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.Shutdown = null!;
                    step.RandomDelay = null!;
                    step.RandomJump = null!;
                    // step.KillApp = null!;
                    //step.InputText = null!;
                    break;
                case EventMode.ShutDown:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.RandomJump = null!;
                    //step.InputText = null!;
                    break;
                case EventMode.RandomJump:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;                    
                    //step.InputText = null!;
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
                if (item.JumTargetTag == tag)
                {
                    return true;
                }
            }
            return false;
        }



    }//
}
