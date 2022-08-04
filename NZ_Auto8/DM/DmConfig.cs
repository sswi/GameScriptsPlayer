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
        /// 大漠插件免注册 DmReg.dll 路径
        /// </summary>
        public const string DmRegDllPath = @"./Libs/QQPCRTPReg.dll";

        /// <summary>
        /// 大漠插件 dm.dll 路径
        /// </summary>
        public const string DmClassDllPath = @"./Libs/QQPCRTP.dll";

        /// <summary>
        /// 大漠插件注册码
        /// </summary>
        public const string DmRegCode = "jv965720b239b8396b1b7df8b768c919e86e10f";

        /// <summary>
        /// 大漠插件版本附加信息
        /// </summary>
        public const string DmVerInfo = "j2y1k46l6odi700";

        /// <summary>
        /// 大漠插件全局路径,设置了此路径后,所有接口调用中,相关的文件都相对于此路径. 比如图片,字库等.
        /// </summary>
        public const string DmGlobalPath = @"./Resources";
    }
}
