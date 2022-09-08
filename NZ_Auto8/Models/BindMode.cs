using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{

    /// <summary>
    /// 大漠插件窗口绑定模式
    /// </summary>
    public class BindMode:BindableBase
    {

        private string name=null!;
        public string Name
        {
            get { return name; }
            set 
            { 
                name = value; OnPropertyChanged();
            }
        }



        private string display=null!;
        /// <summary>
        /// 屏幕颜色获取方式
        /// </summary>
        public string Display
        {
            get { return display; }
            set { display = value; OnPropertyChanged(); }
        }


        private string mouse=null!;
        /// <summary>
        /// 鼠标仿真模式
        /// </summary>
        public string Mouse
        {
            get { return mouse; }
            set { mouse = value;OnPropertyChanged(); }
        }


        private string keypad=null!;
        /// <summary>
        /// 键盘仿真模式
        /// </summary>
        public string Keypad
        {
            get { return keypad; }
            set { keypad = value;OnPropertyChanged(); }
        }


        private int mode;
        /// <summary>
        /// 模式
        /// </summary>
        public int Mode
        {
            get { return mode; }
            set { mode = value;OnPropertyChanged(); }
        }


    }
}
