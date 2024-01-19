using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.DesignTokens;
using System.Windows;

namespace GameScriptsPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var serviceCollection = new ServiceCollection();
            UseComponents(serviceCollection);  
            Register(serviceCollection);
            Resources.Add("services", serviceCollection.BuildServiceProvider());
        }


        /// <summary>
        /// 注册组件
        /// </summary>
        /// <param name="services"></param>
        private static void UseComponents( ServiceCollection services)
        {
            services.AddFluentUIComponents(); //注册FluentUI  
            services.AddBlazorWebViewDeveloperTools(); //启用调试工具




            services.AddWpfBlazorWebView();//启用bazlor组件，要放在最后
        }


        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="services"></param>
        private static void Register(ServiceCollection services)
        {
            
            services.AddTransient<BaseLayerLuminance>();
       
            

            //注入


             
        }
    }
}