using NZ_Auto8.DM;
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
    /// KeyboardView.xaml 的交互逻辑
    /// </summary>
    public partial class KeyboardView : UserControl
    {
        public KeyboardView()
        {
            InitializeComponent();
            comboBox.ItemsSource = new string[3] {"按下","弹起","按键"};
           // autoSuggestBox.ItemsSource = KeyChar.KeyChars;
        }
        //KeyDown = 0,
        //KeyUp = 1,
        //KeyPress = 2
    }
}
