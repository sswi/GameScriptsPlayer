using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{

    /// <summary>
    /// 键盘事件
    /// </summary>
    public class KeyboardEvent:BindableBase
    {

        //按键名
        private string? keyChar;
        public string? KeyChar
        {
            get { return keyChar; }
            set { keyChar = value; OnPropertyChanged(); }
        }


        //键盘操作模式
        private KeyboardMode mode= KeyboardMode.KeyDown;
        public KeyboardMode Mode
        {
            get { return mode; }
            set { mode = value; OnPropertyChanged(); }
        }


    }

    public enum KeyboardMode
    {
        KeyDown = 0,
        KeyUp = 1,
        KeyPress = 2
    }

}
