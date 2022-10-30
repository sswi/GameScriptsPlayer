using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{
    /// <summary>
    /// 多任务事件
    /// </summary>
    public class MultiTaskEvent:BindableBase
    {

        private string taskName=null!;
        /// <summary>
        /// 线程名
        /// </summary>
        public string TaskName
        {
            get { return taskName; }
            set { taskName = value; OnPropertyChanged(); }
        }



        private MultiTaskMode mode= MultiTaskMode.Start;
        /// <summary>
        /// 模式- 启动、停止线程
        /// </summary>
        public MultiTaskMode Mode
        {
            get { return mode; }
            set { mode = value; OnPropertyChanged(); }
        }




    }

    /// <summary>
    /// 多线程操作模式
    /// </summary>
    public enum MultiTaskMode
    { 
        /// <summary>
        /// 启动线程
        /// </summary>
        Start=0,
        /// <summary>
        /// 停止线程
        /// </summary>
        Stop=1,
    
    }

}
