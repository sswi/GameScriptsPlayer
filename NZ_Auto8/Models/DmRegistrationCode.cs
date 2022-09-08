using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XE.Commands;

namespace NZ_Auto8.Models
{
    /// <summary>
    /// 大漠插件注册码
    /// </summary>
    public class DmRegistrationCode:BindableBase
    {
        private string verInfo=null!;
        /// <summary>
        /// 附加吗
        /// </summary>
        public string VerInfo
        {
            get { return verInfo; }
            set { verInfo = value; OnPropertyChanged(); }
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

    }
}
