using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NZ_Auto8.DM;
using NZ_Auto8.Models;
using NZ_Auto8.Services;
using NZ_Auto8.ViewModels;
using NZ_Auto8.Views.Pages;
using NZ_Auto8.Views.Windows;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace NZ_Auto8
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static readonly IHost _host = Host.CreateDefaultBuilder() .ConfigureServices((context, services) =>
        {
            ConfigureServices(services);

        })
        .Build();



        //依赖注入
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            //文件管理
            services.AddSingleton<FileService>();

            //主题注入
            services.AddSingleton<IThemeService, ThemeService>();

            //主窗口容器页面注入
            services.AddSingleton<Container>();

            //注入大漠
            services.AddSingleton<DmSoft>();
             services.AddSingleton<GameHandle>();

            //页面
            services.AddScoped<EditorPage>();
            services.AddScoped<EditorPageViewModel>();
        }



        private async void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {

            await _host.StartAsync();
            _host.Services.GetRequiredService<Container>().Show();

            //远程注册大漠，大漠插件的收费验证，莫得办法，要么缴费，要么找破解版
            _ = Task.Run(() =>
            {
                var x = RegisterDmSoft.RegisterDmSoftDll();
                var _dm = _host.Services.GetRequiredService<DmSoft>();
                var result= _dm.Reg(DmConfig.DmRegCode, DmConfig.DmVerInfo);

                //判断联网注册是否成功 不等于1则注册失败，弹出失败信息
                if (result!=1)
                {
                  var msg=  DmRegResult.RegResults.FindLast(r => result == r.ReturnCode);
                  System.Windows.MessageBox.Show(msg.ReturnMsg);
                }

                Debug.WriteLine($"注册码返回信息：{result}");
                Debug.WriteLine($"当前使用大漠的版本：{_dm.Ver()}");
                //记录鼠标速度
                mouseSpeed = _dm.GetMouseSpeed();
            });
        }

    
     //系统鼠标速度记录
     public static  int mouseSpeed = 10;



        private async void OnExit(object sender, System.Windows.ExitEventArgs e)
        {
            var _dm = _host.Services.GetRequiredService<DmSoft>();


            //还原鼠标速度
            _dm.SetMouseSpeed(mouseSpeed);
            //开启鼠标精度
            _dm.EnableMouseAccuracy(1);

            //解除窗口绑定
            _dm.UnBindWindow();

            //清理掉插件对象
            _dm.Dispose();

            await _host.StopAsync();
            _host.Dispose();
        }


        public static T? GetService<T>()
          where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }
    }
}
