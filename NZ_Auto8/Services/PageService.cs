using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Mvvm.Contracts;

namespace NZ_Auto8.Services
{

    /// <summary>
    /// 2此处于脚本操作无瓜，不需任何改动，界面导航相关用的
    /// </summary>
    public class PageService : IPageService
    {
        /// <summary>
        /// Service which provides the instances of pages.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates new instance and attaches the <see cref="IServiceProvider"/>.
        /// </summary>
        public PageService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public T? GetPage<T>() where T : class
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(typeof(T)))
                throw new InvalidOperationException("The page should be a WPF control.");

            return (T?)_serviceProvider.GetService(typeof(T));
        }

        /// <inheritdoc />
        public FrameworkElement? GetPage(Type pageType)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
                throw new InvalidOperationException("The page should be a WPF control.");

            return _serviceProvider.GetService(pageType) as FrameworkElement;
        }
    }
}
