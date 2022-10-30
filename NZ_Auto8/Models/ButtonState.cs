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
        private static readonly ButtonState[] ButtonStates = new ButtonState[3]
        { 
         new ButtonState("调试","Play32"),
         new ButtonState("待停止","Pulse20"),
         new ButtonState("停止","Stop20"),
        };
        public ButtonState(string text,string icon)
        {
            this.text = text;
            this.icon = icon;
        }


        public buttonState State { get; private set; }
        public ButtonState(buttonState state)
        {
            State = state;
            SetRunButtonState(state);

        }


        private string? text;
        /// <summary>
        /// 按钮文本
        /// </summary>
        public string? Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged(); }
        }

        private string? icon;
        /// <summary>
        /// 按钮图标
        /// </summary>
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

        public void SetRunButtonState(buttonState state)
        {

            switch (state)
            {
                case buttonState.Run:
                    Text = ButtonStates[0].Text;
                    Icon = ButtonStates[0].Icon;
                    break;
                case buttonState.Stoping:
                    Text = ButtonStates[1].Text;
                    Icon = ButtonStates[1].Icon;
                    break;
                case buttonState.Stop:
                    Text = ButtonStates[2].Text;
                    Icon = ButtonStates[2].Icon;
                    break;
            }
            State = state;
        }
    }


    public enum buttonState
    { 
        Run=0,
        Stoping=1,
        Stop=2   
    }
}
