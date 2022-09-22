using Microsoft.Extensions.DependencyInjection;
using NZ_Auto8.DM;
using NZ_Auto8.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace NZ_Auto8.Handlers
{

    /// <summary>
    /// 脚本处理器
    /// </summary>
    public static class ScriptsHandler
    {


        /// <summary>
        /// 随机数方法
        /// </summary>
        private static readonly Random random = new ();

        /// <summary>
        /// 鼠标模拟滑动配置
        /// </summary>
        private static readonly MouseMoveConfig[] mouseMoveConfig= new MouseMoveConfig[] {         
         new MouseMoveConfig(){ Frequency=20, Span=50 },
         new MouseMoveConfig(){ Frequency=25, Span=40 },
        };



        private static readonly DmSoft _dm = App._host.Services.GetRequiredService<DmSoft>();
        /// <summary>
        /// 执行步骤
        /// </summary>
        /// <param name="step"></param>
        /// <param name="dm"></param>
        public static int Run(this Step step)
        {
            switch (step.Mode)
            {
                //键盘
                case EventMode.Keyboard:
                    KeyboardHandler(step);
                    return -1;
                    
                //鼠标
                case EventMode.Mouse:
                    MouseHandler(step);
                    return -1;
                //延迟等待
                case EventMode.Sleep:                
                    return -1;


                //找图跳转
                case EventMode.FindPicture:
                 var pic= step.FindPicture();
                 return pic == null ? step.Picture.NotFoundJumToIndex : step.Picture.HasFoundJumToIndex;  //找到则返回 “找到跳转的步索引”，找不到则返回 “找不到跳转步的索引”
                   

                //跳转循环
                case EventMode.Jump:
                  return  step.Jump.JumHandler();
                  

                //找图点击
                case EventMode.FindPictureClick:
                 return  step.FindPictureClick();
                    

                //找色跳转
                case EventMode.FindColor:
                    var color = step.Color?.FindColor();
                    return color == null ? step.Color.NotFoundJumToIndex : step.Color.HasFoundJumToIndex; //找到则返回 “找到跳转的步索引”，找不到则返回 “找不到跳转步的索引”


                //文本输入
                case EventMode.Input:
                    _dm.SendString(_dm.BindWindowHwnd,step.InputText);                   
                    return -1;

                //随机延迟等待
                case EventMode.RandomDelay:
                    var random = new Random();
                    Thread.Sleep(random.Next(step.RandomDelay.MinValue, step.RandomDelay.MaxValue));
                    return -1;

                //恢复键状态
                case EventMode.KeyboardReverted:
                    KeyCharManage.RestKeyState(_dm);
                    return -1;

                //关机  
                case EventMode.ShutDown:
                    if (step.Shutdown.IsChecked)
                    {
                        try
                        {
                            //执行光鸡命令
                            Process.Start("C:/Windows/System32/shutdown.exe", "-s -t 0 ");
                        }
                        catch 
                        {                          
                        }                    
                    }
                    return -1;

                    //结束进程
                case EventMode.KillApp:
                    if (step.KillApp.IsCheck )
                    {
                        KillApp(step);
                    }                   
                    return -1;

                    //随机跳转
                case EventMode.RandomJump:
                    return step.RandomJumpHandler();
                    

            }
            return -1;
        }






        /// <summary>
        /// 结束进程
        /// </summary>
        /// <param name="step"></param>
        private static void KillApp(Step step)
        {
            try
            {
                //系统进程列表
                var processes = Process.GetProcesses().ToList();

                //判断是否含多个进程名
                if (step.KillApp.ProcessName != null && step.KillApp.ProcessName.Contains("|"))
                {
                    //目标进程名列表
                    var targetProcessesList = step.KillApp.ProcessName.Split("|");

                    if (targetProcessesList != null && targetProcessesList.Length > 0)
                    {
                        foreach (var tpName in targetProcessesList)
                        {
                            //找出所有对应进程名的进程,并结束
                            processes.ForEach(p =>
                            {
                                if (p.ProcessName == tpName)
                                {
                                    p.Kill();
                                }                            
                            });

                        }
                    }
                }

                //结束单进程
                else if(step.KillApp.ProcessName!=null && step.KillApp.ProcessName.Length>0)
                {
                    //找到所有指定的进程名，结束进程
                    processes.ForEach(p =>
                    {
                        if (p.ProcessName== step.KillApp.ProcessName)
                        {
                            p.Kill();
                        }
                    });
                }
            }
            catch 
            {               

            }       
        
        }









        /// <summary>
        /// 跳转处理器
        /// </summary>
        /// <param name="jump"></param>
        /// <returns></returns>
        public static int JumHandler(this JumpEvent jump)
        {
            //如果循环次数为0，表示无限循环，直接返回跳转的步数
            if (jump.NumberOfCycles == 0)
            {
                return jump.JumToIndex;
            }
            else
            {
                //若已跳转次数大于或等于 设置的跳转次数 则清空跳转 次数，返回 -1，下一步,  否则 返回跳转 步数索引，已跳转次数+1
                if (jump.CyclesCount >=jump.NumberOfCycles-1)
                {
                    jump.CyclesCount = 0;
                    return -1;
                }
                else
                {                 
                    jump.CyclesCount++;        
                    return jump.JumToIndex;
                }            
            }
        }




        /// <summary>
        /// 键盘处理器
        /// </summary>
        public static void KeyboardHandler(Step step)
        {
            var keyChar = step.Keyboard.KeyChar;
            if (keyChar == null) return;

            switch (step.Keyboard.Mode)
            {
                case KeyboardMode.KeyDown:
                     _dm.KeyDownChar(keyChar);
                     KeyCharManage.SetKeyState(keyChar, KeyboardMode.KeyDown); //记录按键状态，用于停止后恢复，避免卡键
                    break;
                case KeyboardMode.KeyUp:
                    _dm.KeyUpChar(keyChar);
                    KeyCharManage.SetKeyState(keyChar, KeyboardMode.KeyUp); //记录按键状态，用于停止后恢复，避免卡键
                    break;
                case KeyboardMode.KeyPress:
                    _dm.KeyPressChar(keyChar);
                    KeyCharManage.SetKeyState(keyChar, KeyboardMode.KeyPress); //记录按键状态，用于停止后恢复，避免卡键
                    break;
            }
           
        }

        /// <summary>
        /// 鼠标处理器
        /// </summary>
        /// <param name="step"></param>
        public static void MouseHandler(Step step)
        {
            switch (step.Mouse.Mode)
            {
                case MouseMode.LeftDown:
                    _dm.LeftDown();
                    break;
                case MouseMode.LeftUp:
                    _dm.LeftUp();
                    break;
                case MouseMode.LeftClick:
                    _dm.LeftClick();
                    break;
                case MouseMode.LeftDoubleClick:
                    _dm.LeftDoubleClick();
                    break;
                case MouseMode.RightDown:
                    _dm.RightDown();
                    break;
                case MouseMode.RightUp:
                    _dm.RightUp();
                    break;
                case MouseMode.RightClick:
                    _dm.RightClick();
                    break;
                case MouseMode.RightDoubleClick:
                    _dm.RightClick();
                    Thread.Sleep(60);
                    _dm.RightClick();
                    break;
                case MouseMode.Move:

                    //模拟滑动
                    if (step.Mouse.IsSimulatesSliding)
                    {

                      // var mouseConfig = mouseMoveConfig[random.Next(0, mouseMoveConfig.Length)];
                        var mouseConfig = mouseMoveConfig[0];
                        //每移动1坐标所需的延迟，计算出需要移动的次数
                        var times = step.Mouse.MoveTimeSpan * 20 / 1000;

                        //取余数
                       var yu = step.Mouse.MoveTimeSpan * 20 % 1000;


            

                        if (yu != 0)
                        {
                            times++;
                        }
                        //每次增量                             
                        var everyX = step.Mouse.Postion.X / times;
                        var everyY = step.Mouse.Postion.Y / times;
                        int x = 0, y = 0;
                        for (int i = 0; i < times; i++)
                        {
                            //最后一次
                            if (yu != 0 && i == times - 1)
                            {
                                x += everyX * yu;
                                y += everyY * yu;
                                _dm.MoveR(everyX * yu, everyY * yu);
                            }
                            else
                            {
                                x += everyX;
                                y += everyY;
                                _dm.MoveR(everyX, everyY);
                            }
                         Thread.Sleep(50);              
                        }
                    }
                    //瞬间移动
                    else
                    {
                        
                        _dm.MoveR(step.Mouse.Postion.X, step.Mouse.Postion.Y);
                    }
                    break;
            }            
        }



        /// <summary>
        /// 找图点击
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public static int FindPictureClick(this Step step)
        {
          var result= step.FindPicture();
            //找到则返回 “找到则点击，然后返回下一步的标记  -1 ”，找不到则返回 “找不到跳转步的索引”
            if (result!=null)
            {
                //鼠标移动到找到的坐标 并点击
                _dm.MoveTo(result.X, result.Y);
                Thread.Sleep(100);
                _dm.LeftClick();
                Thread.Sleep(100);
                //返回-1，让执行器跑下一步脚本
                return -1;
            }
            return step.Picture.NotFoundJumToIndex;
        }



        /// <summary>
        /// 找图
        /// </summary>
        public static Point FindPicture(this Step step)
        {
            var oldDateTime = DateTime.Now;
            while (DateTime.Now.Subtract(oldDateTime).TotalMilliseconds <step.Picture.TimeOut)
            {
                _dm.FindPic(step.Picture.StartPoint.X, step.Picture.StartPoint.Y, step.Picture.EndPoint.X, step.Picture.EndPoint.Y, step.Picture.Path!, "000000", step.Picture.Similarity, 0, out int X, out int Y);
                if (X >= 0 && Y >= 0)
                {
                    return new Point(X, Y);
                }
                // 按  1000/1=1000帧计算
                Thread.Sleep(1);
            }
            return null!;
        }


        /// <summary>
        /// 找色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Point FindColor(this ColorEvent color)
        {
            var oldDateTime = DateTime.Now;

            while (DateTime.Now.Subtract(oldDateTime).TotalMilliseconds < color.TimeOut)
            {
                var result = _dm.FindColor(color.StartPoint.X, color.StartPoint.Y, color.EndPoint.X, color.EndPoint.Y, color.Color, color.Similarity, 0, out int X, out int Y);
                if (result==1)
                {
                    var p = new Point(X, Y);
                    return p;
                }
                // 按  1000/1=1000帧计算
                Thread.Sleep(1);
            }
            return null!;
        }









        /// <summary>
        /// 获取随机跳转 中的跳转编号
        /// </summary>
        /// <param name="randomJump"></param>
        /// <returns></returns>
        public static int RandomJumpHandler(this Step step)
        {
            var randomJump=step.RandomJump;
            var max = randomJump.RandomJumpTargets.Count;
            //如果循环次数为0，表示无限循环，直接返回跳转的步数
            if (randomJump.NumberOfCycles == 0)
            {                
                var index = random.Next(0, max);
                Debug.WriteLine($"随机1={index}");
                return randomJump.RandomJumpTargets[index].TargetIndex;
            }
            else
            {
                //若已跳转次数大于或等于 设置的跳转次数 则清空跳转 次数，返回 -1，下一步,  否则 返回跳转 步数索引，已跳转次数+1
                if (randomJump.CyclesCount >= randomJump.NumberOfCycles - 1)
                {
                    randomJump.CyclesCount = 0;
                    return -1;
                }
                    randomJump.CyclesCount++;              
                    var index = random.Next(0, max);
                Debug.WriteLine($"随机2={index}");
                return randomJump.RandomJumpTargets[index].TargetIndex;
         
            }




        }








        //
    }
}
