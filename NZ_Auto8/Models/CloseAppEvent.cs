using XE.Commands;

namespace NZ_Auto8.Models
{

    /// <summary>
    /// 关闭程序
    /// </summary>
    public class CloseAppEvent:BindableBase
    {
        private string processName=null!;
        /// <summary>
        /// 进程名
        /// </summary>
        public string ProcessName
        {
            get { return processName; }
            set { processName = value;OnPropertyChanged(); }
        }


        private bool isCheck=false;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsCheck
        {
            get { return isCheck; }
            set 
            {
                isCheck = value;
                OnPropertyChanged();
            }
        }



    }
}
