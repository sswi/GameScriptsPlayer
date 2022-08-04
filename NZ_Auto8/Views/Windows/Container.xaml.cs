using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Appearance;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace NZ_Auto8.Views.Windows
{
    /// <summary>
    /// Container.xaml 的交互逻辑
    /// </summary>
    public partial class Container 
    {

        private readonly IThemeService _themeService;
        public Container(IThemeService themeService, INavigationService navigationService, IPageService pageService)
        {
            InitializeComponent();
            _themeService = themeService;
            navigationService.SetNavigationControl(RootNavigation);
            SetPageService(pageService);
        }


        //明暗主题切换
        private void NavigationButtonTheme_OnClick(object sender, RoutedEventArgs e)
        {
            _themeService.SetTheme(_themeService.GetTheme() == ThemeType.Dark ? ThemeType.Light : ThemeType.Dark);
        }

        public void SetPageService(IPageService pageService)
    => RootNavigation.PageService = pageService;
    }
}
