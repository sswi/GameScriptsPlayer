using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{

    /// <summary>
    /// 键盘操作事件
    /// </summary>
    public class KeyboardEvent:BindableBase
    {

        
        private string? keyChar;
        /// <summary>
        /// 按键名
        /// </summary>
        public string? KeyChar
        {
            get { return keyChar; }
            set { keyChar = value; OnPropertyChanged(); }
        }


       
        private KeyboardMode mode= KeyboardMode.KeyDown;
        /// <summary>
        /// 键盘操作模式
        /// </summary>
        public KeyboardMode Mode
        {
            get { return mode; }
            set { mode = value; OnPropertyChanged(); }
        }

    }


    /// <summary>
    /// 键盘操作模式
    /// </summary>
    public enum KeyboardMode
    {
        /// <summary>
        /// 按下
        /// </summary>
        KeyDown = 0,
        /// <summary>
        /// 弹起
        /// </summary>
        KeyUp = 1,
        /// <summary>
        /// 按键
        /// </summary>
        KeyPress = 2
    }

}
