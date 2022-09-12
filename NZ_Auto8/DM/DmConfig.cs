using NZ_Auto8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZ_Auto8.DM
{
    /// <summary>
    /// 大漠插件配置
    /// </summary>
    public class DmConfig
    {

        /// <summary>
        /// 窗口绑定模式,默认模式，适合逆战
        /// </summary>
        public static BindMode WindowBindMode = new BindMode()
        {
            Display="dx2",Mouse="dx2", Keypad="dx",Mode=11,Name="逆战"
        };


        /// <summary>
        /// 大漠插件免注册 DmReg.dll 路径
        /// </summary>
        public const string DmRegDllPath = @"./Libs/QQPcrtpReg.dll";

        /// <summary>
        /// 大漠插件 dm.dll 路径
        /// </summary>
        public const string DmClassDllPath = @"./Libs/QQPcrtp.dll";

        /// <summary>
        /// 大漠插件注册码
        /// </summary>
        public const string DmRegCode = "w30413033bf71a848c9b8c3d6a3963cab952745aa";

        /// <summary>
        /// 大漠插件版本附加信息
        /// </summary>
        public const string DmVerInfo = "1234";

        /// <summary>
        /// 大漠插件全局路径,设置了此路径后,所有接口调用中,相关的文件都相对于此路径. 比如图片,字库等.
        /// </summary>
        public const string DmGlobalPath = @"./Resources";
    }
}
