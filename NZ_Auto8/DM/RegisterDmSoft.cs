using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NZ_Auto8.DM
{
    /// <summary>
    /// 免注册调用大漠插件
    /// </summary>
    public static class RegisterDmSoft
    {
        // 不注册调用大漠插件，实际上是使用 dmreg.dll 来配合实现，这个文件有 2 个导出接口 SetDllPathW 和 SetDllPathA。 SetDllPathW 对应 unicode，SetDllPathA 对应 ascii 接口。


        [DllImport(DmConfig.DmRegDllPath)]
        private static extern int SetDllPathA(string path, int mode);




        /// <summary>
        /// 免注册调用大漠插件
        /// </summary>
        /// <returns></returns>
        public static bool RegisterDmSoftDll()
        {
            var setDllPathResult = SetDllPathA(DmConfig.DmClassDllPath, 1);      
            if (setDllPathResult == 0)
            {
                // 加载 dm.dll 失败
                MessageBox.Show("加载改名后的大漠插件  QQPCRTP.dll 失败");
                return false;
            }
            return true;
        }



    }
}
