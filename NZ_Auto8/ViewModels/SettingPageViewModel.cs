using Newtonsoft.Json;
using NZ_Auto8.DM;
using NZ_Auto8.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.ViewModels
{

    /// <summary>
    /// 设置页面的 视图模型
    /// </summary>
    public class SettingPageViewModel:BindableBase
    {
        //插件实例
        private readonly DmSoft _dm;
        public SettingPageViewModel(DmSoft dm)
        {
            _dm = dm;
            Displays.ForEach(d =>
            {
                if (d.Name == DmConfig.WindowBindMode.Display)
                {
                    d.IsChecked = true;
                }
            });
            Mouses.ForEach(m =>
            {
                if (m.Name == DmConfig.WindowBindMode.Mouse)
                {
                    m.IsChecked = true;
                }
            });
            Keypads.ForEach(k =>
            {
                if (k.Name == DmConfig.WindowBindMode.Keypad)
                {
                    k.IsChecked = true;
                }
            });
            Mods.ForEach(m =>
            {
                if (m.index == DmConfig.WindowBindMode.Mode)
                {
                    m.IsChecked = true;
                }
            });

            //判断是否有本地验证码
            var fileName = Directory.GetCurrentDirectory() + "\\RegKey.txt";
            if (File.Exists(fileName))
            {
                var keyconfig = File.ReadAllText(Directory.GetCurrentDirectory() + "\\RegKey.txt");
                if (keyconfig != null && keyconfig.Contains("\r\n"))
                {
                    var ks = keyconfig.Split("\r\n");
                    if (ks.Length >= 2)
                    {
                        RegCode = ks[0];
                        VerInfo = ks[1];
                    }
                }
            }           
        }



        /// <summary>
        /// 屏幕颜色获取方式集合
        /// </summary>
        public List<Display> Displays { get; set; } = new List<Display>()
        {
         new Display(){ Name="normal",Details="正常模式,平常我们用的前台截屏模式" },
         new Display(){ Name="gdi",Details="gdi模式,用于窗口采用GDI方式刷新时. 此模式占用CPU较大. 参考SetAero  win10以上系统使用此模式，如果截图失败，尝试把目标程序重新开启再试试。" },
         new Display(){ Name="gdi2",Details="gdi2模式,此模式兼容性较强,但是速度比gdi模式要慢许多,如果gdi模式发现后台不刷新时,可以考虑用gdi2模式." },
         new Display(){ Name="dx2",Details="dx2模式,用于窗口采用dx模式刷新,如果dx方式会出现窗口所在进程崩溃的状况,可以考虑采用这种.采用这种方式要保证窗口有一部分在屏幕外.win7 win8或者vista不需要移动也可后台.此模式占用CPU较大. 参考SetAero.   win10以上系统使用此模式，如果截图失败，尝试把目标程序重新开启再试试。\r\n" },
         new Display(){ Name="dx3",Details="dx3模式,同dx2模式,但是如果发现有些窗口后台不刷新时,可以考虑用dx3模式,此模式比dx2模式慢许多. 此模式占用CPU较大. 参考SetAero. win10以上系统使用此模式，如果截图失败，尝试把目标程序重新开启再试试。" },
         new Display(){ Name="dx",Details="dx模式,等同于BindWindowEx中，display设置的\"dx.graphic.2d|dx.graphic.3d\"" }
        };

        /// <summary>
        /// 鼠标仿真模式集合
        /// </summary>
        public List<Mouse> Mouses { get; set; } = new List<Mouse>()
        {
         new Mouse(){ Name="normal",Details="正常模式,平常我们用的前台鼠标模式" },
         new Mouse(){ Name="windows",Details="Windows模式,采取模拟windows消息方式 同按键自带后台插件" },
         new Mouse(){ Name="windows2",Details="Windows2 模式,采取模拟windows消息方式(锁定鼠标位置) 此模式等同于BindWindowEx中的mouse为以下组合\r\n\"dx.mouse.position.lock.api|dx.mouse.position.lock.message|dx.mouse.state.message" },
         new Mouse(){ Name="windows3",Details="Windows3模式，采取模拟windows消息方式,可以支持有多个子窗口的窗口后台." },
         new Mouse(){ Name="dx",Details="正常模式,平常我们用的前台鼠标模式" },
         new Mouse(){ Name="dx2",Details="dx2模式,这种方式类似于dx模式,但是不会锁定外部鼠标输入.\r\n有些窗口在此模式下绑定时，需要先激活窗口再绑定(或者绑定以后手动激活)，否则可能会出现绑定后鼠标无效的情况. 此模式等同于BindWindowEx中的mouse为以下组合dx.public.active.api|dx.public.active.message|dx.mouse.position.lock.api|dx.mouse.state.api|dx.mouse.api|dx.mouse.focus.input.api|dx.mouse.focus.input.message|dx.mouse.clip.lock.api|dx.mouse.input.lock.api| dx.mouse.cursor" },
        };

        /// <summary>
        /// 键盘仿真模式集合
        /// </summary>
        public List<Keypad> Keypads { get; set; } = new List<Keypad>()
        {
         new Keypad (){ Name="normal",Details="正常模式,平常我们用的前台键盘模式" },
         new Keypad (){ Name="windows",Details="Windows模式,采取模拟windows消息方式 同按键的后台插件." },
         new Keypad (){ Name="dx",Details="dx模式,采用模拟dx后台键盘模式。有些窗口在此模式下绑定时，需要先激活窗口再绑定(或者绑定以后激活)，否则可能会出现绑定后键盘无效的情况. 此模式等同于BindWindowEx中的keypad为以下组合\r\n\"dx.public.active.api|dx.public.active.message| dx.keypad.state.api|dx.keypad.api|dx.keypad.input.lock.api\"\r\n" },
        };

        /// <summary>
        /// 绑定模式集合
        /// </summary>
        public List<Mod> Mods { get; set; } = new List<Mod>()
        { 
            new Mod(){ index=0, Details="推荐模式此模式比较通用，而且后台效果是最好的" },
            new Mod(){ index=2, Details="同模式0,如果模式0有崩溃问题，可以尝试此模式. 注意0和2模式，当主绑定(第一个绑定同个窗口的对象)绑定成功后，那么调用主绑定的线程必须一致维持,否则线程一旦推出,对应的绑定也会消失." },
            new Mod(){ index=11, Details="需要加载驱动,适合一些特殊的窗口,如果前面的无法绑定，可以尝试此模式. 此模式不支持32位系统" },
            new Mod(){ index=13, Details="需要加载驱动,适合一些特殊的窗口,如果前面的无法绑定，可以尝试此模式. 此模式不支持32位系统" },
            new Mod(){ index=101, Details="超级绑定模式. 可隐藏目标进程中的dm.dll.避免被恶意检测.效果要比dx.public.hide.dll好. 推荐使用." },
            new Mod(){ index=103, Details="同模式101，如果模式101有崩溃问题，可以尝试此模式. " }
        };

        public Command DisplayCheckedCommand => new((object obj) =>
        {
            var d = obj as Display;
            DmConfig.WindowBindMode.Display = d.Name;
            SaveWindowBindMod();
        });

        public Command MouseCheckedCommand => new((object obj) =>
        {
            var m = obj as Mouse;
            DmConfig.WindowBindMode.Mouse = m.Name;
            SaveWindowBindMod();
        });

        public Command KeypadCheckedCommand => new((object obj) =>
        {
            var k = obj as Keypad;
            DmConfig.WindowBindMode.Keypad = k.Name;
            SaveWindowBindMod();
        });

        public Command ModCheckedCommand => new((object obj) =>
        {
            var m = obj as Mod;
            DmConfig.WindowBindMode.Mode = m.index;
            SaveWindowBindMod();
        });

        /// <summary>
        /// 保存绑定模式
        /// </summary>
        private static void SaveWindowBindMod()
        {         
            var modeInfomation=JsonConvert.SerializeObject(DmConfig.WindowBindMode);
            try
            {
                File.WriteAllText("BindModeConfig", modeInfomation, Encoding.UTF8);
            }
            catch
            {              
            }
            
        }





        private string regCode=null!;
        /// <summary>
        /// 注册码
        /// </summary>
        public string RegCode
        {
            get { return regCode; }
            set { regCode = value;OnPropertyChanged(); }
        }

        private string verInfo = null!;
        /// <summary>
        /// 附加吗
        /// </summary>
        public string VerInfo
        {
            get { return verInfo; }
            set { verInfo = value; OnPropertyChanged(); }
        }


        public AsyncCommand SaveRegCodeCommand => new(SaveRegCode);

        /// <summary>
        /// 应用、保存注册码
        /// </summary>
        private async  Task SaveRegCode()
        {
          await  Task.Run(() =>
            {
                //验证注册码是否有效
                var result = _dm.Reg(RegCode, VerInfo);
                //匹配返回值
                var msg = DmRegResult.RegResults.FindLast(r => result == r.ReturnCode);
                if (msg != null)
                {
                    System.Windows.MessageBox.Show($"插件注册:+ {msg.ReturnMsg}");
                }       
                

                //注册码有效才保存
                if (result==1)
                {
                    DmSoft.IsReg = true;
                    var regCodeInfomation = $"{RegCode}\r\n{VerInfo}";
                    try
                    {
                        File.WriteAllText("RegKey.txt", regCodeInfomation, Encoding.UTF8);
                    }
                    catch
                    {
                    }
                }

            });          
        }



    }
}
