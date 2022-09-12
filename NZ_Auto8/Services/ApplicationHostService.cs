using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Mvvm.Contracts;

namespace NZ_Auto8.Services
{

    /// <summary>
    /// 此处于脚本操作无瓜，不需任何改动，界面导航相关用的
    /// </summary>
    public class ApplicationHostService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationService _navigationService;
        private readonly IPageService _pageService;
        //private readonly IThemeService _themeService;
        //private readonly ITaskBarService _taskBarService;

        private INavigationWindow _navigationWindow;

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public ApplicationHostService(IServiceProvider serviceProvider, INavigationService navigationService,
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
            IPageService pageService)
        {
            // If you want, you can do something with these services at the beginning of loading the application.
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;
            _pageService = pageService;
            //_themeService = themeService;
            //_taskBarService = taskBarService;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            PrepareNavigation();

            await HandleActivationAsync();
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Creates main window during activation.
        /// </summary>
        private async Task HandleActivationAsync()
        {
            await Task.CompletedTask;

            if (!Application.Current.Windows.OfType<Container>().Any())
            {
#pragma warning disable CS8601 // 引用类型赋值可能为 null。
                _navigationWindow = _serviceProvider.GetService(typeof(INavigationWindow)) as INavigationWindow;
#pragma warning restore CS8601 // 引用类型赋值可能为 null。
                _navigationWindow!.ShowWindow();

                // NOTICE: You can set this service directly in the window 
                // _navigationWindow.SetPageService(_pageService);

                // NOTICE: In the case of this window, we navigate to the Dashboard after loading with Container.InitializeUi()
                // _navigationWindow.Navigate(typeof(Views.Pages.Dashboard));
            }

            var notifyIconManager = _serviceProvider.GetService(typeof(INotifyIconService)) as INotifyIconService;

            if (!notifyIconManager!.IsRegistered)
            {
                notifyIconManager!.SetParentWindow(_navigationWindow as Window);
                notifyIconManager.Register();
            }

            await Task.CompletedTask;
        }

        private void PrepareNavigation()
        {
            _navigationService.SetPageService(_pageService);
        }
    }
}
