using NZ_Auto8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NZ_Auto8.DM
{

    /// <summary>
    /// 按键管理，用于中途手动停止脚本运行后恢复按键状态的
    /// </summary>
    public class KeyCharManage
    {
        /// <summary>
        /// 按键名列表
        /// </summary>
        public static readonly List< Key> Keys = new()
        {
            new Key("1",49),new Key("A",65),new Key("O",79),new Key("SHIFT",16),new Key("DELETE",46),
            new Key("2",50),new Key("B",66),new Key("P",80),new Key("WIN",91),new Key("HOME",36),
            new Key("3",51),new Key("C",67),new Key("Q",81),new Key("SPACE",32),new Key("END",35),
            new Key("4",52),new Key("D",68),new Key("R",82),new Key("CAP",20),new Key("PGUP",33),
            new Key("5",53),new Key("E",69),new Key("S",83),new Key("TAB",9),new Key("PUDN",34),
            new Key("6",54),new Key("F",70),new Key("T",84),new Key("~",192),
            new Key("7",55),new Key("G",71),new Key("U",85),new Key("ESC",27),
            new Key("8",56),new Key("H",72),new Key("V",86),new Key("ENTER",13),
            new Key("9",57),new Key("I",73),new Key("W",87),new Key("UP",38),
            new Key("0",48),new Key("J",74),new Key("X",88),new Key("DOWN",40),
            new Key("-",189),new Key("K",75),new Key("Y",89),new Key("LEFT",37),
            new Key("=",187),new Key("L",76),new Key("Z",90),new Key("RIGHT",39),
            new Key("black",8),new Key("M",77),new Key("CTRL",17),new Key("OPTION",93),
            new Key("7",55),new Key("N",78),new Key("ALT",18),new Key("PRINT",44),
        };





        /// <summary>
        /// 设置按键状态
        /// </summary>
        /// <param name="keyChar"></param>
        public static void SetKeyState(string keyChar, KeyboardMode mode)
        {

            //启动一个异步线程来记录按键状态。避免使用 脚本线程造成延迟过大
            Task.Run(() =>
            {
                var key = Keys.FindLast(k => k.KeyChar == keyChar);
                if (key != null)
                    key.KeyState = mode;
            });
        }




        
        /// <summary>
        /// 恢复按键状态
        /// </summary>
        public static void RestKeyState(DmSoft dm)
        {
            Keys.ForEach(key =>
            {
                if (key.KeyState== KeyboardMode.KeyDown)
                {
                    dm.KeyUp(key.KeyCode);
                    key.KeyState = KeyboardMode.KeyUp;
                    Thread.Sleep(60);
                }
            });
        }


    }
}
