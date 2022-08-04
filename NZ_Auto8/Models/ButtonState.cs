using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{

    /// <summary>
    /// 按钮状态
    /// </summary>
    public class ButtonState:BindableBase
    {

        public ButtonState(string text,string icon)
        {
            this.text = text;
            this.icon = icon;
        }

        private string? text;
        public string? Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged(); }
        }

        private string? icon;
        public string? Icon
        {
            get { return icon; }
            set { icon = value; OnPropertyChanged(); }
        }


        /// <summary>
        /// 更新按钮状态
        /// </summary>
        /// <param name="state"></param>
        public void SetRunButtonState(bool state)
        {
            if (state)
            {
               Text = "停止";
               Icon = "Stop20";
            }
            else
            {
                Text = "调试";
                Icon = "Play32";
            }
           
        }

    }
}
