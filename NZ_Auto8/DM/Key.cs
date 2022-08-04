using NZ_Auto8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZ_Auto8.DM
{

    /// <summary>
    /// 按键
    /// </summary>
    public class Key
    {
        public Key(string keyChar, int keyCode)
        {
            KeyCode = keyCode;
            KeyChar = keyChar;
        }

        public int KeyCode { get; set; }
        public string KeyChar { get; set; }

        public KeyboardMode KeyState { get; set; } = KeyboardMode.KeyUp;


    }
}
