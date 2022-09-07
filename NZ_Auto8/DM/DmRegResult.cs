using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZ_Auto8.DM
{
    internal class DmRegResult
    {
        public int ReturnCode { get; set; }

        public string? ReturnMsg { get; set; }


        internal static List<DmRegResult> RegResults = new List<DmRegResult>()
        {

        new DmRegResult(){ ReturnCode=-1,ReturnMsg="无法连接网络,(可能防火墙拦截,如果可以正常访问大漠插件网站，那就可以肯定是被防火墙拦截)" },
        new DmRegResult(){ ReturnCode=-2,ReturnMsg="进程没有以管理员方式运行. (出现在win7 win8 vista 2008.建议关闭uac)" },
        new DmRegResult(){ ReturnCode=0,ReturnMsg="失败 (未知错误)" },
        new DmRegResult(){ ReturnCode=1,ReturnMsg="成功" },
        new DmRegResult(){ ReturnCode=2,ReturnMsg="余额不足" },
        new DmRegResult(){ ReturnCode=3,ReturnMsg="绑定了本机器，但是账户余额不足50元" },
        new DmRegResult(){ ReturnCode=4,ReturnMsg="插件注册码错误" },
        new DmRegResult(){ ReturnCode=5,ReturnMsg="你的机器或者IP在黑名单列表中或者不在白名单列表中" },
        new DmRegResult(){ ReturnCode=6,ReturnMsg="非法使用插件" },
        new DmRegResult(){ ReturnCode=7,ReturnMsg="你的帐号因为非法使用被封禁. （如果是在虚拟机中使用插件，必须使用Reg或者RegEx，不能使用RegNoMac或者RegExNoMac,否则可能会造成封号，或者封禁机器）" },
        new DmRegResult(){ ReturnCode=8,ReturnMsg="附加码不在你设置的附加白名单中" },
        new DmRegResult(){ ReturnCode=77,ReturnMsg="机器码或者IP因为非法使用，而被封禁. （如果是在虚拟机中使用插件，必须使用Reg或者RegEx，不能使用RegNoMac或者RegExNoMac,否则可能会造成封号，或者封禁机器）     封禁是全局的，如果使用了别人的软件导致77，也一样会导致所有注册码均无法注册。解决办法是更换IP，更换MAC" },
        new DmRegResult(){ ReturnCode=-8,ReturnMsg="版本附加信息长度超过了20" },
        new DmRegResult(){ ReturnCode=-9,ReturnMsg="版本附加信息里包含了非法字母" },
         new DmRegResult(){ ReturnCode=781,ReturnMsg="未知？请注意！本工具的作者提醒：可能使用了破解版，超过10注册可能会被封IP" },
         new DmRegResult(){ ReturnCode=778,ReturnMsg="未知？请注意！本工具的作者提醒：可能使用了破解版，超过10注册可能会被封IP" },
         new DmRegResult(){ ReturnCode=791,ReturnMsg="未知？请注意！本工具的作者提醒：可能使用了破解版，超过10注册可能会被封IP" },
        };
    }
}
