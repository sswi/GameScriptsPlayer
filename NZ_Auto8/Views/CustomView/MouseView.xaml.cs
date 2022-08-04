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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NZ_Auto8.Views.CustomView
{
    /// <summary>
    /// MouseView.xaml 的交互逻辑
    /// </summary>
    public partial class MouseView : UserControl
    {
        public MouseView()
        {
            InitializeComponent();
            comboBox.ItemsSource = new string[9]{ "左键按下", "左键弹起", "左键单击", "左键双击", "右键按下", "右键弹起", "右键单击", "右键双击", "移动鼠标" };
        }
    }
}
