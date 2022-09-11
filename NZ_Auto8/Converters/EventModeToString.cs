using NZ_Auto8.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NZ_Auto8.Converters
{
    public class EventModeToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mode = (EventMode)value;
            return mode switch
            {
                EventMode.Keyboard => "键盘操作",
                EventMode.Mouse => "鼠标操作",
                EventMode.Sleep => "延迟等待",
                EventMode.FindPicture => "找图跳转",
                EventMode.Jump => "跳转循环",
                EventMode.FindPictureClick => "找图点击",
                EventMode.FindColor => "找色跳转",
                EventMode.Input => "文本输入",
                EventMode.RandomDelay => "随机延迟等待",
                EventMode.KeyboardReverted=>"按键复归",
                EventMode.ShutDown => "关机",
                EventMode.KillApp => "结束进程",
                EventMode.RandomJump => "随机跳转",
                _ => "未知步骤",
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
