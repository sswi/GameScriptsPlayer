using NZ_Auto8.Handlers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{

    /// <summary>
    /// 多任务线程
    /// </summary>
    public class ScriptTask : BindableBase
    {

        /// <summary>
        /// 全局，任务线程列表
        /// </summary>
        public static ObservableCollection<ScriptTask> ScriptTasks = new();


        private bool isChecked=false;
        /// <summary>
        /// 是否被勾中删除
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; OnPropertyChanged(); }
        }


        private string taskName=null!;
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName
        {
            get { return taskName; }
            set { taskName = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 线程运行状态
        /// </summary>
        public bool IsRun { get; private set; } = false;



        /// <summary>
        /// 线程是否在运行(是否允许线程继续运行)
        /// </summary>
        private bool _taskIsRuning=false;


        /// <summary>
        /// 步骤
        /// </summary>
        public ObservableCollection<Step> Steps { get; set; } = new ();


        /// <summary>
        /// 停止线程
        /// </summary>
        public void Stop()
        {
            _taskIsRuning = false;
        }

        /// <summary>
        /// 线程开始
        /// </summary>
        public void Start( int startIndex=0,int endIndex=-1)
        {
            Task.Run(() =>
            {
                _taskIsRuning= true;
                RunTask(startIndex,endIndex);
            });
        }



        private void RunTask(int startIndex=0, int endIndex=-1)
        {
            //result=-1  下一步，result >=0的为跳转
            int result = -1;
            for (int i = startIndex; i < Steps.Count; i++)
            {
                //是否运行，用于接受用户中途手动停止
                if (!_taskIsRuning) break;
                IsRun = true;

                //调试信息 创建一个线程线程去输出，保证脚本运行最低延迟
                //if (Steps[i].Remark != null && Steps[i].Remark.Length > 0)
                //{
                //    Task.Run(() =>
                //    {
                //        DebugInformation = $"[{Scripts[i].Index}].{Scripts[i].Remark}";
                //    });
                //}

     

                //“步”的执行，此处脚本正式开始运行，接受返回值，result=-1  下一步，result >=0的为跳转
                //判断是否为多线程
                if (Steps[i].Mode == EventMode.MultiTask)
                {
                    var s = ScriptTasks.FirstOrDefault(s => s.TaskName == Steps[i].Task.TaskName);
                    //开启新线程
                    if (Steps[i].Task.Mode == MultiTaskMode.Start)
                    {                                               
                      s?.Start();                       
                    }
                    //停止线程运行
                    else
                    { 
                     s?.Stop();
                    }
                }
                else
                {
                     result = Steps[i].Run();
                }
                



                //判断是否设置了停止步
                if (endIndex != -1 && i == endIndex)
                {
                    IsRun = true;
                    break;
                }

                //当前步数结束后等待延迟，如果等待时间超过250ms则分段执行，解决中途手动停止时候脚本迟迟无法终止运行问题
                if (Steps[i].EndWaitTime > 250)
                {
                    //注意！这步不能直接操作EndWaitTime，否则会改变 脚本的设定值
                    int span = Steps[i].EndWaitTime;
                    while (span >= 0)
                    {
                        if (!_taskIsRuning) break;
                        //如果时间大于250 时候，就减少250ms
                        if (span > 250)
                        {
                            span -= 250;
                            Thread.Sleep(250);
                        }
                        else
                        {
                            //不足250时候就直接执行剩下的延时时间
                            Thread.Sleep(span);
                            break;
                        }
                    }
                }
                //低于250ms则直接运行延时等待
                else
                {
                    Thread.Sleep(Steps[i].EndWaitTime);
                }

                //步数跳转
                if (result != -1)
                {
                    if (!_taskIsRuning)
                    {             
                        break;
                    }
                    if (result < Steps.Count)
                    {

                        i = result != 0 ? result - 1 : 0;
                    }
                    else
                    {
                        throw new Exception($"跳转出错，线程名{taskName}所跳转的 步数索引{result} 超出列表索引最大数{Steps.Count}");
                    }
                }

            }

            //标记为结束
            IsRun = false;
            _taskIsRuning=false;
        }




    }
}
