using NZ_Auto8.DM;
using NZ_Auto8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.ViewModels
{
    public class ScriptEditorPageViewModel:BindableBase
    {
       // private readonly DmSoft _dm;


        //脚本运行
        private bool IsRun = false;

        public ScriptEditorPageViewModel()
        {
         //   _dm = dm;
         //   GameHandle = gameHandle;
        }

        //窗口绑定
        public GameHandle GameHandle { get; set; }


        //调试按钮状态
        public ButtonState RunButtonState { get; set; } = new ButtonState("调试", "Play32");


        public Command RunAndStopCommand => new(() => 
        {
            IsRun = !IsRun;
            RunButtonState.SetRunButtonState(IsRun);

        });


    }
}
